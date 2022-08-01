using Everglow.Sources.Modules.ZYModule.Commons.Function;

using MonoMod.Cil;

namespace Everglow.Sources.Commons.Core
{
    /// <summary>
    /// 对一个方法的管理，可以用来控制钩子是否启用
    /// </summary>
    [DebuggerDisplay("{name} : {enable ? \"Enable\" : \"Disable\"}")]
    public class ActionHandler
    {
        public Action Action
        {
            get => action;
            set
            {
                enable = value is not null;
                action = value;
            }
        }
        private Action action;
        private string name;
        private bool enable;
        public bool Enable
        {
            get => enable;
            set => enable = value;
        }
        public string Name => name;
        public ActionHandler(Action action)
        {
            this.action = action;
            name = action.ToString();
            enable = true;
        }
        public ActionHandler(Action action, string name)
        {
            this.action = action;
            this.name = name;
            enable = true;
        }

        public ActionHandler()
        {
            action = null;
            name = string.Empty;
            enable = false;
        }

        public void Invoke() => action.Invoke();
        public override string ToString() => name;
    }
    public enum CallOpportunity
    {
        None,
        //绘制
        PostDrawFilter,
        PostDrawTiles,
        PostDrawProjectiles,
        PostDrawDusts,
        PostDrawNPCs,
        PostDrawPlayers,
        PostDrawMapIcons,
        PostDrawBG,
        //加载
        PostUpdateEverything,
        PostUpdateProjectiles,
        PostUpdatePlayers,
        PostUpdateNPCs,
        PostUpdateDusts,
        PostUpdateInvasions,
        PostEnterWorld_Single,
        PostEnterWorld_Server,
        PostExitWorld_Single,
        ResolutionChanged
    }

    /// <summary>
    /// 钩子统一管理，不用继承一大堆ModSystem或者加很多的On，一些常见的加载时刻都写了
    /// </summary>
    public class HookSystem : ModSystem
    {
        public HookSystem()
        {
            waitToRemove = new List<(CallOpportunity op, ActionHandler handler)>();
            methods = new Dictionary<CallOpportunity, List<ActionHandler>>();
            foreach (var op in validOpportunity)
            {
                methods.Add(op, new List<ActionHandler>());
            }
        }
        private List<(CallOpportunity op, ActionHandler handler)> waitToRemove;
        public static readonly CallOpportunity[] validOpportunity = new CallOpportunity[]
        {
            //Draw
            CallOpportunity.PostDrawFilter,
            CallOpportunity.PostDrawTiles,
            CallOpportunity.PostDrawProjectiles,
            CallOpportunity.PostDrawDusts,
            CallOpportunity.PostDrawNPCs,
            CallOpportunity.PostDrawPlayers,
            CallOpportunity.PostDrawMapIcons,
            CallOpportunity.PostDrawBG,
            //Update
            CallOpportunity.PostUpdateEverything,
            CallOpportunity.PostUpdateProjectiles,
            CallOpportunity.PostUpdatePlayers,
            CallOpportunity.PostUpdateNPCs,
            CallOpportunity.PostUpdateDusts,
            CallOpportunity.PostUpdateInvasions,
            //Misc
            CallOpportunity.PostEnterWorld_Single,
            CallOpportunity.PostExitWorld_Single,
            CallOpportunity.PostEnterWorld_Server,
            CallOpportunity.ResolutionChanged
        };
        internal bool DisableDrawNPCs { get; set; } = false;
        internal bool DisableDrawSkyAndHell { get; set; } = false;
        internal bool DisableDrawBackground { get; set; } = false;
        internal Dictionary<CallOpportunity, List<ActionHandler>> methods;
        /// <summary>
        /// 现在存在的问题就是，这里的method都是无参数的Action，但是如DrawMapIcon这样的方法就需要传参了，只好用这种这种定义字段的方法
        /// </summary>
        public (Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale) MapIconInfomation
        {
            get; internal set;
        }
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
            methods[op].Add(handler);
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
        /// <summary>
        /// 根据Handler移除一个方法
        /// </summary>
        /// <param name="handler"></param>
        /// <returns>是否成功移除</returns>
        public bool Remove(ActionHandler handler)
        {
            foreach(var op in validOpportunity)
            {
                var handlers = methods[op];
                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handler == handlers[i])
                    {
                        waitToRemove.Add((op, handler));
                        handler.Enable = false;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 移除所有被Disable的Action
        /// </summary>
        public void RemoveDisabledAction()
        {
            foreach(var op in validOpportunity)
            {
                methods[op].RemoveAll(handler => !handler.Enable);
            }
        }
        public void HookLoad()
        {
            IL.Terraria.Main.DoDraw += Main_DoDraw;
            On.Terraria.Main.DrawDust += Main_DrawDust;
            On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
            On.Terraria.Main.DrawNPCs += Main_DrawNPCs;
            On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.DrawPlayers += LegacyPlayerRenderer_DrawPlayers;
            On.Terraria.WorldGen.playWorldCallBack += WorldGen_playWorldCallBack; ;
            On.Terraria.WorldGen.SaveAndQuit += WorldGen_SaveAndQuit;
            On.Terraria.Main.DrawMiscMapIcons += Main_DrawMiscMapIcons;
            On.Terraria.WorldGen.serverLoadWorldCallBack += WorldGen_serverLoadWorldCallBack;
            On.Terraria.Main.DrawBG += Main_DrawBG;
            On.Terraria.Main.DrawBackground += Main_DrawBackground;
            On.Terraria.Main.DoDraw_WallsTilesNPCs += Main_DoDraw_WallsTilesNPCs;
            Main.OnResolutionChanged += Main_OnResolutionChanged;
        }


        public void HookUnload()
        {
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
                        Debug.Fail($"{handler} 抛出了异常 {ex}");
                        Everglow.Instance.Logger.Error($"{handler} 抛出了异常 {ex}");
                        handler.Enable = false;
                    }
                }
            }
        }
        public override void PostUpdateInvasions()
        {
            Invoke(CallOpportunity.PostUpdateInvasions);
        }
        public override void PostUpdateEverything()
        {
            Invoke(CallOpportunity.PostUpdateEverything);
            UpdateTimer++;
            foreach(var (op, handler) in waitToRemove)
            {
                methods[op].Remove(handler);
            }
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
        private void Main_DrawBackground(On.Terraria.Main.orig_DrawBackground orig, Main self)
        {
            if (DisableDrawBackground)
            {
                return;
            }
            orig(self);
        }
        private void Main_DrawBG(On.Terraria.Main.orig_DrawBG orig, Main self)
        {
            if (DisableDrawSkyAndHell)
            {
                return;
            }
            orig(self);
        }

        private void Main_DoDraw_WallsTilesNPCs(On.Terraria.Main.orig_DoDraw_WallsTilesNPCs orig, Main self)
        {
            Invoke(CallOpportunity.PostDrawBG);
            orig(self);
        }

        private void WorldGen_serverLoadWorldCallBack(On.Terraria.WorldGen.orig_serverLoadWorldCallBack orig)
        {
            orig();
            Invoke(CallOpportunity.PostEnterWorld_Server);
        }

        private void Main_DrawMiscMapIcons(On.Terraria.Main.orig_DrawMiscMapIcons orig, Main self, SpriteBatch spriteBatch, Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale, float drawScale, ref string mouseTextString)
        {
            orig(self, spriteBatch, mapTopLeft, mapX2Y2AndOff, mapRect, mapScale, drawScale, ref mouseTextString);
            MapIconInfomation = (mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
            Invoke(CallOpportunity.PostDrawMapIcons);
        }

        private void WorldGen_SaveAndQuit(On.Terraria.WorldGen.orig_SaveAndQuit orig, Action callback)
        {
            orig(callback);
            Invoke(CallOpportunity.PostExitWorld_Single);
        }

        private void WorldGen_playWorldCallBack(On.Terraria.WorldGen.orig_playWorldCallBack orig, object threadContext)
        {
            orig(threadContext);
            Invoke(CallOpportunity.PostEnterWorld_Single);
        }

        private void Main_OnResolutionChanged(Vector2 obj)
        {
            Invoke(CallOpportunity.ResolutionChanged);
        }

        private void LegacyPlayerRenderer_DrawPlayers(On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.orig_DrawPlayers orig, Terraria.Graphics.Renderers.LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, IEnumerable<Player> players)
        {
            orig.Invoke(self, camera, players);
            Invoke(CallOpportunity.PostDrawPlayers);
        }

        private void Main_DrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
        {
            orig.Invoke(self, behindTiles);
            if (!behindTiles)
            {
                Invoke(CallOpportunity.PostDrawNPCs);
            }
        }

        private void Main_DrawDust(On.Terraria.Main.orig_DrawDust orig, Main self)
        {
            orig.Invoke(self);
            Invoke(CallOpportunity.PostDrawDusts);
        }

        private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            orig.Invoke(self);
            Invoke(CallOpportunity.PostDrawProjectiles);
        }

        private void Main_DoDraw(ILContext il)
        {
            var cursor = new ILCursor(il);
            if (!cursor.TryGotoNext(MoveType.Before, ins => ins.MatchLdcI4(36)))
            {
                //HookException.Throw("Main_DoDraw_NotFound_1");
            }
            cursor.EmitDelegate(() => Invoke(CallOpportunity.PostDrawFilter));
        }
    }

}

