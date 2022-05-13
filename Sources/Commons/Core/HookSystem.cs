namespace Everglow.Sources.Commons.Core
{
    /// <summary>
    /// 对一个方法的管理，可以用来控制钩子是否启用
    /// Debug暂时用于异常处理，以后可能会改成调用专门的方法来解决
    /// </summary>
    public class ActionHandler
    {
        internal Action action;
        public string Name { get; internal set; }
        public bool Enable { get; set; } = true;
        public bool Debug { get; set; } = false;
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
        LoadWorlds,
        UnloadWorlds,
        ResolutionChanged
    }
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
            CallOpportunity.LoadWorlds,
            CallOpportunity.UnloadWorlds,
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
            On.Terraria.Main.LoadWorlds += Main_LoadWorlds;
            On.Terraria.WorldGen.SaveAndQuit += WorldGen_SaveAndQuit;
            On.Terraria.Main.DrawMiscMapIcons += Main_DrawMiscMapIcons;
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
                        if (handler.Debug)
                        {
                            //自动暂停的，方便监视
                            Debug.Assert(false);
                        }
                    }
                }
            }
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
            Invoke(CallOpportunity.UnloadWorlds);
            orig(callback);
        }
        internal void Main_LoadWorlds(On.Terraria.Main.orig_LoadWorlds orig)
        {
            orig();
            Invoke(CallOpportunity.LoadWorlds);
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
    ////TimeSlow
    //internal int timeSlowTarget;
    //internal int[] timeSlowTimer;
    //internal int[] timeSlowTimeLeft;
    //internal int[] timeSlowScale;
    //internal readonly HashSet<int> heldProjIndex = new HashSet<int>();
    //internal readonly (Vector2 oldpos, float oldrot, Vector2 pos, float rot, bool active)[][] timeSlowOldValue
    //    = new (Vector2 oldpos, float oldrot, Vector2 pos, float rot, bool active)[TARGET_COUNT][];
    //public const int TARGET_DUST = 0b100000;
    //public const int TARGET_GORE = 0b010000;
    //public const int TARGET_ITEM = 0b001000;
    //public const int TARGET_NPC = 0b000100;
    //public const int TARTET_PLAYER = 0b000010;
    //public const int TARGET_PROJ = 0b000001;
    //public const int TARGET_ALL = 0b111111;
    //public const int TARGET_NOPLAYER = 0b111101;
    //public const int TARGET_DUST_INDEX = 5;
    //public const int TARGET_GORE_INDEX = 4;
    //public const int TARGET_ITEM_INDEX = 3;
    //public const int TARGET_NPC_INDEX = 2;
    //public const int TARGET_PLAYER_INDEX = 1;
    //public const int TARGET_PROJ_INDEX = 0;
    //public const int TARGET_COUNT = 6;

    //public void ApplyTimeSlow(int target, int time, int scale)
    //{
    //    timeSlowTimer ??= new int[TARGET_COUNT];
    //    timeSlowTimeLeft ??= new int[TARGET_COUNT];
    //    timeSlowScale ??= new int[TARGET_COUNT];
    //    for (int index = 0; target != 0; index++, target >>= 1)
    //    {
    //        if (BitUtils.IsTrue(target, 0))
    //        {
    //            timeSlowTimeLeft[index] = time;
    //            timeSlowScale[index] = scale;
    //            timeSlowOldValue[index] = index switch
    //            {
    //                TARGET_DUST_INDEX => new (Vector2 oldpos, float oldrot, Vector2 pos, float rot, bool active)[Main.dust.Length],
    //                TARGET_GORE_INDEX => new (Vector2 oldpos, float oldrot, Vector2 pos, float rot, bool active)[Main.gore.Length],
    //                TARGET_ITEM_INDEX => new (Vector2 oldpos, float oldrot, Vector2 pos, float rot, bool active)[Main.item.Length],
    //                TARGET_NPC_INDEX => new (Vector2 oldpos, float oldrot, Vector2 pos, float rot, bool active)[Main.npc.Length],
    //                TARGET_PLAYER_INDEX => new (Vector2 oldpos, float oldrot, Vector2 pos, float rot, bool active)[Main.player.Length],
    //                TARGET_PROJ_INDEX => new (Vector2 oldpos, float oldrot, Vector2 pos, float rot, bool active)[Main.projectile.Length],
    //                _ => throw new Exception("Noknown error"),
    //            };
    //            timeSlowTarget |= 1 << index;
    //        }
    //    }
    //}
    //public void UpdateTimeSlow()
    //{
    //    if (timeSlowTarget != 0)
    //    {
    //        for (int i = 0; i < TARGET_COUNT; i++)
    //        {
    //            if (timeSlowTimeLeft[i] != 0)
    //            {
    //                --timeSlowTimeLeft[i];
    //                ++timeSlowTimer[i];
    //            }
    //            else
    //            {
    //                timeSlowTimer[i] = 0;
    //                timeSlowScale[i] = 0;
    //                timeSlowOldValue[i] = null;
    //                timeSlowTarget = BitUtils.SetFalse(timeSlowTarget, i);
    //            }
    //        }
    //    }
    //}
    //public override void PreUpdateDusts()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_DUST_INDEX) && timeSlowTimer[TARGET_DUST_INDEX] % timeSlowScale[TARGET_DUST_INDEX] == 0)
    //    {
    //        for (int i = 0; i < Main.dust.Length; i++)
    //        {
    //            var dust = Main.dust[i];
    //            if (dust.active)
    //            {
    //                timeSlowOldValue[TARGET_DUST_INDEX][i].oldpos = dust.position;
    //                timeSlowOldValue[TARGET_DUST_INDEX][i].oldrot = dust.rotation;
    //                timeSlowOldValue[TARGET_DUST_INDEX][i].active = true;
    //            }
    //            else
    //            {
    //                timeSlowOldValue[TARGET_DUST_INDEX][i].active = false;
    //            }
    //        }
    //    }
    //}
    //public override void PreUpdateGores()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_GORE_INDEX) && timeSlowTimer[TARGET_GORE_INDEX] % timeSlowScale[TARGET_GORE_INDEX] == 0)
    //    {
    //        for (int i = 0; i < Main.gore.Length; i++)
    //        {
    //            var gore = Main.gore[i];
    //            if (gore.active)
    //            {
    //                timeSlowOldValue[TARGET_GORE_INDEX][i].oldpos = gore.position;
    //                timeSlowOldValue[TARGET_GORE_INDEX][i].oldrot = gore.rotation;
    //                timeSlowOldValue[TARGET_GORE_INDEX][i].active = true;
    //            }
    //            else
    //            {
    //                timeSlowOldValue[TARGET_GORE_INDEX][i].active = false;
    //            }
    //        }
    //    }
    //}
    //public override void PreUpdateItems()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_ITEM_INDEX) && timeSlowTimer[TARGET_ITEM_INDEX] % timeSlowScale[TARGET_ITEM_INDEX] == 0)
    //    {
    //        for (int i = 0; i < Main.item.Length; i++)
    //        {
    //            var item = Main.item[i];
    //            if (item.active)
    //            {
    //                timeSlowOldValue[TARGET_ITEM_INDEX][i].oldpos = item.position;
    //                timeSlowOldValue[TARGET_ITEM_INDEX][i].active = true;
    //            }
    //            else
    //            {
    //                timeSlowOldValue[TARGET_ITEM_INDEX][i].active = false;
    //            }
    //        }
    //    }
    //}
    //public override void PreUpdateNPCs()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_NPC_INDEX) && timeSlowTimer[TARGET_NPC_INDEX] % timeSlowScale[TARGET_NPC_INDEX] == 0)
    //    {
    //        for (int i = 0; i < Main.npc.Length; i++)
    //        {
    //            var npc = Main.npc[i];
    //            if (npc.active)
    //            {
    //                timeSlowOldValue[TARGET_NPC_INDEX][i].oldpos = npc.position;
    //                timeSlowOldValue[TARGET_NPC_INDEX][i].oldrot = npc.rotation;
    //                timeSlowOldValue[TARGET_NPC_INDEX][i].active = true;
    //            }
    //            else
    //            {
    //                timeSlowOldValue[TARGET_NPC_INDEX][i].active = false;
    //            }
    //        }
    //    }
    //}
    //public override void PreUpdatePlayers()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_PLAYER_INDEX) && timeSlowTimer[TARGET_PLAYER_INDEX] % timeSlowScale[TARGET_PLAYER_INDEX] == 0)
    //    {
    //        for (int i = 0; i < Main.player.Length; i++)
    //        {
    //            var player = Main.player[i];
    //            if (player.active)
    //            {
    //                timeSlowOldValue[TARGET_PLAYER_INDEX][i].oldpos = player.position;
    //                timeSlowOldValue[TARGET_PLAYER_INDEX][i].active = true;
    //            }
    //            else
    //            {
    //                timeSlowOldValue[TARGET_PLAYER_INDEX][i].active = false;
    //            }
    //        }
    //    }
    //}
    //public override void PreUpdateProjectiles()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_PROJ_INDEX) && timeSlowTimer[TARGET_PROJ_INDEX] % timeSlowScale[TARGET_PROJ_INDEX] == 0)
    //    {
    //        for (int i = 0; i < Main.projectile.Length; i++)
    //        {
    //            var projectile = Main.projectile[i];
    //            if (projectile.active && !heldProjIndex.Contains(i))
    //            {
    //                timeSlowOldValue[TARGET_PROJ_INDEX][i].oldpos = projectile.position;
    //                timeSlowOldValue[TARGET_PROJ_INDEX][i].oldrot = projectile.rotation;
    //                timeSlowOldValue[TARGET_PROJ_INDEX][i].active = true;
    //            }
    //            else
    //            {
    //                timeSlowOldValue[TARGET_PROJ_INDEX][i].active = false;
    //            }
    //        }
    //    }
    //}
    //public override void PostUpdateDusts()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_DUST_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_DUST_INDEX] % timeSlowScale[TARGET_DUST_INDEX] == 0)
    //        {
    //            for (int i = 0; i < Main.dust.Length; i++)
    //            {
    //                var dust = Main.dust[i];
    //                timeSlowOldValue[TARGET_DUST_INDEX][i].pos = dust.position;
    //                timeSlowOldValue[TARGET_DUST_INDEX][i].rot = dust.rotation;
    //            }
    //        }
    //        for (int i = 0; i < Main.dust.Length; i++)
    //        {
    //            var dust = Main.dust[i];
    //            var (oldpos, oldrot, pos, rot, active) = timeSlowOldValue[TARGET_DUST_INDEX][i];
    //            if (active)
    //            {
    //                float factor = (float)(timeSlowTimer[TARGET_DUST_INDEX] % timeSlowScale[TARGET_DUST_INDEX]) / (timeSlowScale[TARGET_DUST_INDEX] + 1);
    //                dust.position = MathUtils.Lerp(oldpos, pos, factor);
    //                dust.rotation = MathUtils.RadianLerp(oldrot, rot, factor);
    //            }
    //        }
    //    }
    //}
    //public override void PostUpdateGores()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_DUST_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_GORE_INDEX] % timeSlowScale[TARGET_GORE_INDEX] == 0)
    //        {
    //            for (int i = 0; i < Main.gore.Length; i++)
    //            {
    //                var gore = Main.gore[i];
    //                timeSlowOldValue[TARGET_GORE_INDEX][i].pos = gore.position;
    //                timeSlowOldValue[TARGET_GORE_INDEX][i].rot = gore.rotation;
    //            }
    //        }
    //        for (int i = 0; i < Main.gore.Length; i++)
    //        {
    //            var gore = Main.gore[i];
    //            var (oldpos, oldrot, pos, rot, active) = timeSlowOldValue[TARGET_GORE_INDEX][i];
    //            if (active)
    //            {
    //                float factor = (float)(timeSlowTimer[TARGET_GORE_INDEX] % timeSlowScale[TARGET_GORE_INDEX]) / (timeSlowScale[TARGET_GORE_INDEX] + 1);
    //                gore.position = MathUtils.Lerp(oldpos, pos, factor);
    //                gore.rotation = MathUtils.RadianLerp(oldrot, rot, factor);
    //            }
    //        }
    //    }
    //}
    //public override void PostUpdateItems()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_DUST_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_ITEM_INDEX] % timeSlowScale[TARGET_ITEM_INDEX] == 0)
    //        {
    //            for (int i = 0; i < Main.item.Length; i++)
    //            {
    //                var item = Main.item[i];
    //                timeSlowOldValue[TARGET_ITEM_INDEX][i].pos = item.position;
    //            }
    //        }
    //        for (int i = 0; i < Main.item.Length; i++)
    //        {
    //            var item = Main.item[i];
    //            var (oldpos, _, pos, _, active) = timeSlowOldValue[TARGET_ITEM_INDEX][i];
    //            if (active)
    //            {
    //                float factor = (float)(timeSlowTimer[TARGET_ITEM_INDEX] % timeSlowScale[TARGET_ITEM_INDEX]) / (timeSlowScale[TARGET_ITEM_INDEX] + 1);
    //                item.position = MathUtils.Lerp(oldpos, pos, factor);
    //            }
    //        }
    //    }
    //}
    //public override void PostUpdateNPCs()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_NPC_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_NPC_INDEX] % timeSlowScale[TARGET_NPC_INDEX] == 0)
    //        {
    //            for (int i = 0; i < Main.npc.Length; i++)
    //            {
    //                var npc = Main.npc[i];
    //                timeSlowOldValue[TARGET_NPC_INDEX][i].pos = npc.position;
    //                timeSlowOldValue[TARGET_NPC_INDEX][i].rot = npc.rotation;
    //            }
    //        }
    //        for (int i = 0; i < Main.npc.Length; i++)
    //        {
    //            var npc = Main.npc[i];
    //            var (oldpos, oldrot, pos, rot, active) = timeSlowOldValue[TARGET_NPC_INDEX][i];
    //            if (active)
    //            {
    //                float factor = (float)(timeSlowTimer[TARGET_NPC_INDEX] % timeSlowScale[TARGET_NPC_INDEX]) / (timeSlowScale[TARGET_NPC_INDEX] + 1);
    //                npc.position = MathUtils.Lerp(oldpos, pos, factor);
    //                npc.rotation = MathUtils.RadianLerp(oldrot, rot, factor);
    //            }
    //        }
    //    }
    //}
    //public override void PostUpdatePlayers()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_PLAYER_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_PLAYER_INDEX] % timeSlowScale[TARGET_PLAYER_INDEX] == 0)
    //        {
    //            for (int i = 0; i < Main.player.Length; i++)
    //            {
    //                var player = Main.player[i];
    //                timeSlowOldValue[TARGET_PLAYER_INDEX][i].pos = player.position;
    //            }
    //        }
    //        for (int i = 0; i < Main.player.Length; i++)
    //        {
    //            var player = Main.player[i];
    //            var (oldpos, _, pos, _, active) = timeSlowOldValue[TARGET_PLAYER_INDEX][i];
    //            if (active)
    //            {
    //                float factor = (float)(timeSlowTimer[TARGET_PLAYER_INDEX] % timeSlowScale[TARGET_PLAYER_INDEX]) / (timeSlowScale[TARGET_PLAYER_INDEX] + 1);
    //                player.position = MathUtils.Lerp(oldpos, pos, factor);
    //            }
    //        }
    //    }
    //}
    //public override void PostUpdateProjectiles()
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_PROJ_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_PROJ_INDEX] % timeSlowScale[TARGET_PROJ_INDEX] == 0)
    //        {
    //            for (int i = 0; i < Main.projectile.Length; i++)
    //            {
    //                var projectile = Main.projectile[i];
    //                timeSlowOldValue[TARGET_PROJ_INDEX][i].pos = projectile.position;
    //                timeSlowOldValue[TARGET_PROJ_INDEX][i].rot = projectile.rotation;
    //            }
    //            if (!BitUtils.IsTrue(timeSlowTarget, TARGET_PLAYER_INDEX))
    //            {
    //                heldProjIndex.Clear();
    //                foreach (var player in Main.player)
    //                {
    //                    heldProjIndex.Add(player.heldProj);
    //                }
    //            }
    //        }
    //        for (int i = 0; i < Main.projectile.Length; i++)
    //        {
    //            var projectile = Main.projectile[i];
    //            var (oldpos, oldrot, pos, rot, active) = timeSlowOldValue[TARGET_PROJ_INDEX][i];
    //            if (active && !heldProjIndex.Contains(i))
    //            {
    //                float factor = (float)(timeSlowTimer[TARGET_PROJ_INDEX] % timeSlowScale[TARGET_PROJ_INDEX]) / (timeSlowScale[TARGET_PROJ_INDEX] + 1);
    //                projectile.position = MathUtils.Lerp(oldpos, pos, factor);
    //                projectile.rotation = MathUtils.RadianLerp(oldrot, rot, factor);
    //            }
    //        }
    //    }
    //}
    //public void Hook_UpdateDusts(On.Terraria.Dust.orig_UpdateDust orig)
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_DUST_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_DUST_INDEX] % timeSlowScale[TARGET_DUST_INDEX] != 0)
    //        {
    //            return;
    //        }
    //    }
    //    orig.Invoke();
    //}
    //public void Hook_UpdateGores(On.Terraria.Gore.orig_Update orig, Gore self)
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_GORE_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_GORE_INDEX] % timeSlowScale[TARGET_GORE_INDEX] != 0)
    //        {
    //            return;
    //        }
    //    }
    //    orig.Invoke(self);
    //}
    //public void Hook_UpdateItems(On.Terraria.Item.orig_UpdateItem orig, Terraria.Item self, int i)
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_ITEM_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_ITEM_INDEX] % timeSlowScale[TARGET_ITEM_INDEX] != 0)
    //        {
    //            return;
    //        }
    //    }
    //    orig.Invoke(self, i);
    //}

    //public void Hook_UpdatePlayers(On.Terraria.Player.orig_Update orig, Player self, int i)
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_PLAYER_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_PLAYER_INDEX] % timeSlowScale[TARGET_PLAYER_INDEX] != 0)
    //        {
    //            return;
    //        }
    //    }
    //    orig.Invoke(self, i);
    //}

    //public void Hook_UpdateProjectiles(On.Terraria.Projectile.orig_Update orig, Projectile self, int i)
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_PROJ_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_PROJ_INDEX] % timeSlowScale[TARGET_PROJ_INDEX] != 0)
    //        {
    //            if (!heldProjIndex.Contains(i))
    //            {
    //                return;
    //            }
    //        }
    //    }
    //    orig.Invoke(self, i);
    //}

    //public void Hook_UpdateNPCs(On.Terraria.NPC.orig_UpdateNPC orig, NPC self, int i)
    //{
    //    if (BitUtils.IsTrue(timeSlowTarget, TARGET_NPC_INDEX))
    //    {
    //        if (timeSlowTimer[TARGET_NPC_INDEX] % timeSlowScale[TARGET_NPC_INDEX] != 0)
    //        {
    //            return;
    //        }
    //    }
    //    orig.Invoke(self, i);
    //}

}

