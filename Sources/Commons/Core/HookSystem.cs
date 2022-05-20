using Everglow.Sources.Commons.Core.Profiler.Fody;

namespace Everglow.Sources.Commons.Core
{
    /// <summary>
    /// 对一个方法的管理，可以用来控制钩子是否启用
    /// </summary>
    public class ActionHandler
    {
        internal Action action;
        public string Name { get; internal set; }
        public bool Enable { get; set; } = true;
        public ActionHandler(Action action)
        {
            this.action = action;
            Name = action.ToString();
        }
        public ActionHandler(Action action, string name)
        {
            this.action = action;
            Name = name;
        }
        public void Invoke() => action.Invoke();
    }
    public enum CallOpportunity
    {
        None,
        //绘制
        PostDrawEverything,
        PostDrawTiles,
        PostDrawProjectiles,
        PostDrawDusts,
        PostDrawNPCs,
        PostDrawPlayers,
        PostDrawMapIcons,
        //加载
        PostUpdateEverything,
        PostUpdateProjectiles,
        PostUpdatePlayers,
        PostUpdateNPCs,
        PostUpdateDusts,
        PostEnterWorld_Single,
        PostEnterWorld_Server,
        PostExitWorld_Single,
        ResolutionChanged
    }

    [ProfilerMeasure]
    /// <summary>
    /// 钩子统一管理，不用继承一大堆ModSystem或者加很多的On，一些常见的加载时刻都写了
    /// </summary>
    public class HookSystem : ModSystem
    {
        public static readonly CallOpportunity[] validOpportunity = new CallOpportunity[]
        {
            //Draw
            CallOpportunity.PostDrawTiles,
            CallOpportunity.PostDrawProjectiles,
            CallOpportunity.PostDrawDusts,
            CallOpportunity.PostDrawNPCs,
            CallOpportunity.PostDrawPlayers,
            CallOpportunity.PostDrawEverything,
            CallOpportunity.PostDrawMapIcons,
            //Update
            CallOpportunity.PostUpdateEverything,
            CallOpportunity.PostUpdateProjectiles,
            CallOpportunity.PostUpdatePlayers,
            CallOpportunity.PostUpdateNPCs,
            CallOpportunity.PostUpdateDusts,
            //Misc
            CallOpportunity.PostEnterWorld_Single,
            CallOpportunity.PostExitWorld_Single,
            CallOpportunity.PostEnterWorld_Server,
            CallOpportunity.ResolutionChanged
        };
        internal Dictionary<CallOpportunity, List<ActionHandler>> methods = new Dictionary<CallOpportunity, List<ActionHandler>>();
        /// <summary>
        /// 现在存在的问题就是，这里的method都是无参数的Action，但是如DrawMapIcon这样的方法就需要传参了，只好用这种这种定义字段的方法
        /// </summary>
        public (Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale) MapIconInfomation { get; internal set; }
        /// <summary>
        /// 更新的计时器，PostUpateEverything后加一
        /// </summary>
        public static int UpdateTimer
        {
            get; internal set;
        }
        /// <summary>
        /// 绘制的计时器，PostDrawEverything后加一
        /// </summary>
        public static int DrawTimer
        {
            get; internal set;
        }
        /// <summary>
        /// 在<paramref name="op"/>时执行<paramref name="action"/>
        /// </summary>
        /// <param name="action"></param>
        /// <param name="op"></param>
        /// <param name="name">Handler的名字，方便查找</param>
        /// <exception cref="ArgumentException"></exception>
        public ActionHandler AddMethod(Action action, CallOpportunity op, string name = null)
        {
            if (!validOpportunity.Contains(op))
            {
                //除非搞事不然应该不会执行这行代码
                throw new ArgumentException("Invaild Opportunity");
            }
            var handler = new ActionHandler(action, name ?? action.ToString());
            methods[op].Add(new ActionHandler(action));
            return handler;
        }
        public void AddMethod(ActionHandler handler, CallOpportunity op)
        {
            if (!validOpportunity.Contains(op))
            {
                //除非搞事不然应该不会执行这行代码
                throw new ArgumentException("Invaild Opportunity");
            }
            methods[op].Add(handler);
        }
        /// <summary>
        /// 根据Name寻找方法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionHandler Find(string name, CallOpportunity op = CallOpportunity.None)
        {
            if (op == CallOpportunity.None)
            {
                foreach (var vs in methods.Values)
                {
                    foreach (var handler in vs)
                    {
                        if (handler.Name == name)
                        {
                            return handler;
                        }
                    }
                }
            }
            else
            {
                foreach (var handler in methods[op])
                {
                    if (handler.Name == name)
                    {
                        return handler;
                    }
                }
            }
            return null;
        }

        public override void Load()
        {
            foreach (var op in validOpportunity)
            {
                methods.Add(op, new List<ActionHandler>());
            }
            On.Terraria.Main.DrawDust += Main_DrawDust;
            On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
            On.Terraria.Main.DrawNPCs += Main_DrawNPCs;
            On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.DrawPlayers += LegacyPlayerRenderer_DrawPlayers;
            On.Terraria.Main.DoDraw += Main_DoDraw;
            On.Terraria.WorldGen.playWorld += WorldGen_playWorld;
            On.Terraria.WorldGen.SaveAndQuit += WorldGen_SaveAndQuit;
            On.Terraria.Main.DrawMiscMapIcons += Main_DrawMiscMapIcons;
            On.Terraria.WorldGen.serverLoadWorldCallBack += WorldGen_serverLoadWorldCallBack;
            Main.OnResolutionChanged += Main_OnResolutionChanged;
        }

        public override void Unload()
        {
            methods = null;
            Main.OnResolutionChanged -= Main_OnResolutionChanged;
        }
        internal void Invoke(CallOpportunity op)
        {
            foreach (var handler in methods[op])
            {
                if (handler.Enable)
                {
                    try
                    {
                        handler.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Everglow.Instance.Logger.Error($"{handler.Name} 抛出了异常 {ex}");
                        handler.Enable = false;
                        if (Function.FeatureFlags.EverglowConfig.DebugMode)
                        {
                            //自动暂停的，方便监视
                            Debug.Assert(false);
                        }
                    }
                }
            }
        }

        internal void WorldGen_serverLoadWorldCallBack(On.Terraria.WorldGen.orig_serverLoadWorldCallBack orig)
        {
            orig();
            Invoke(CallOpportunity.PostEnterWorld_Server);
        }

        internal void Main_DrawMiscMapIcons(On.Terraria.Main.orig_DrawMiscMapIcons orig, Main self, SpriteBatch spriteBatch, Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale, float drawScale, ref string mouseTextString)
        {
            orig(self, spriteBatch, mapTopLeft, mapX2Y2AndOff, mapRect, mapScale, drawScale, ref mouseTextString);
            MapIconInfomation = (mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
            Invoke(CallOpportunity.PostDrawMapIcons);
        }

        public override void PostUpdateEverything()
        {
            Invoke(CallOpportunity.PostUpdateEverything);
            UpdateTimer++;
        }

        public override void PostUpdateNPCs()
        {
            Invoke(CallOpportunity.PostUpdateNPCs);
        }

        public override void PostUpdateProjectiles()
        {
            Invoke(CallOpportunity.PostUpdateProjectiles);
        }

        public override void PostUpdateDusts()
        {
            Invoke(CallOpportunity.PostUpdateDusts);
        }

        public override void PostUpdatePlayers()
        {
            Invoke(CallOpportunity.PostUpdatePlayers);
        }

        public override void PostDrawTiles()
        {
            Invoke(CallOpportunity.PostDrawTiles);
        }

        internal void WorldGen_SaveAndQuit(On.Terraria.WorldGen.orig_SaveAndQuit orig, Action callback)
        {
            orig(callback);
            Invoke(CallOpportunity.PostExitWorld_Single);
        }

        internal void WorldGen_playWorld(On.Terraria.WorldGen.orig_playWorld orig)
        {
            orig();
            Invoke(CallOpportunity.PostEnterWorld_Single);
        }

        internal void Main_OnResolutionChanged(Vector2 obj)
        {
            Invoke(CallOpportunity.ResolutionChanged);
        }

        internal void LegacyPlayerRenderer_DrawPlayers(On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.orig_DrawPlayers orig, Terraria.Graphics.Renderers.LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, IEnumerable<Player> players)
        {
            orig.Invoke(self, camera, players);
            Invoke(CallOpportunity.PostDrawPlayers);
        }

        internal void Main_DrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
        {
            orig.Invoke(self, behindTiles);
            if (!behindTiles)
            {
                Invoke(CallOpportunity.PostDrawNPCs);
            }
        }

        internal void Main_DrawDust(On.Terraria.Main.orig_DrawDust orig, Main self)
        {
            orig.Invoke(self);
            Invoke(CallOpportunity.PostDrawDusts);
        }

        internal void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            orig.Invoke(self);
            Invoke(CallOpportunity.PostDrawProjectiles);
        }

        private void Main_DoDraw(On.Terraria.Main.orig_DoDraw orig, Main self, GameTime gameTime)
        {
            orig(self, gameTime);
            Invoke(CallOpportunity.PostDrawEverything);
            DrawTimer++;
        }

        internal void EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
            Invoke(CallOpportunity.PostDrawEverything);
            DrawTimer++;
        }
    }

}

