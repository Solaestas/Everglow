using ReLogic.Graphics;
using System.Runtime.CompilerServices;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.SubWorldModule
{
    public abstract class Subworld : ModType
    {
        protected sealed override void Register()
        {
            SubworldSystem.Register(this);
        }
        public sealed override void SetupContent() => SetStaticDefaults();
        public abstract SaveSetting HowSaveWorld { get; init; }
        public abstract int Width { get; init; }
        public abstract int Height { get; init; }
        public abstract List<GenPass> Tasks { get; init; }
        public virtual bool NormalTime { get; }
        public virtual bool HideUnderworld { get; }
        public virtual bool SavePlayer { get => true; }
        public virtual bool SaveMap { get => true; }
        public virtual WorldGenConfiguration Config { get => null; }
        public virtual ModSystem WorldSystem { get => null; }
        public virtual void OnLoad() { }
        public virtual void OnUnload() { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual bool GetTileLight(Tile tile, int x, int y, ref FastRandom random, ref Color lightcolor) { return true; }
        public virtual void DrawSetUp(GameTime gameTime)
        {
            PlayerInput.SetZoom_Unscaled();

            Main.instance.GraphicsDevice.Clear(Color.Black);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            Main.spriteBatch.DrawString(FontAssets.DeathText.Value, Main.statusText, new Vector2(Main.screenWidth, Main.screenHeight) / 2 - FontAssets.DeathText.Value.MeasureString(Main.statusText) / 2, Color.White);
            Main.DrawCursor(Main.DrawThickCursor());

            Main.spriteBatch.End();
        }
        /// <summary>
        /// 注:此方法调用时已完成部分计算,详情转到定义查看注释
        /// </summary>
        /// <param name="player"></param>
        /// <param name="basicgravity"></param>
        /// <param name="maxFallSpeed"></param>
        public virtual void ModifyPlayerBasicGravity(Player player, ref float basicgravity, ref float maxFallSpeed)
        {
            //maxFallSpeed = 10f;重置默认最大掉落速度
            //gravity = Player.defaultGravity;重置默认重力
            //PortalPhysicsEnabled ? maxFallSpeed = 35f;传送门提高最大掉落速度
            //wet & honeywet ? gravity = 0.1f; maxFallSpeed = 3f;蜂蜜
            //wet & merman ? gravity = 0.3f; maxFallSpeed = 7f;水中鱼人
            //wet & trident & !lavaWet ? gravity = controlUp ? 0.1f : 0.25f; maxFallSpeed = controlUp ? 2f : 6f;水中持有三叉戟,控制向上移动
            //wet ? gravity = 0.2f; maxFallSpeed = 5f;水中
            //vortexDebuff ? gravity = 0f;漩涡柱失重debuff
            //maxFallSpeed += 0.01f;不知道干啥的
            //下方为计算太空对重力的影响
            //float num4 = (float)(Main.maxTilesX / 4200);
            //num4 *= num4;
            //float num5 = (float)((double)(position.Y / 16f - (60f + 10f * num4)) / (Main.worldSurface / 6.0));
            //if ((double)num5 < 0.25)
            //{
            //    num5 = 0.25f;
            //}
            //if (num5 > 1f)
            //{
            //    num5 = 1f;
            //}
            //gravity *= num5;
            //此方法在这里调用,修改玩家的gravity和maxFallSpeed;
        }
        /// <summary>
        /// 注:此方法调用时已完成部分计算,未计算液体影响,详情转到定义查看注释
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="basicgravity"></param>
        /// <param name="maxFallSpeed"></param>
        public virtual void ModifyNPCBasicGravity(NPC npc,ref float basicgravity, ref float maxFallSpeed)
        {
            //maxFallSpeed = 10f;默认最大掉落速度
            //NPC.gravity = 0.3f;默认重力
            //if (type == NPCID.MushiLadybug)
            //{
            //    NPC.gravity = 0.1f;
            //    if (velocity.Y > 3f)
            //    {
            //        velocity.Y = 3f;
            //    }
            //}
            //else if (type == NPCID.VortexRifleman && ai[2] == 1f)
            //{
            //    NPC.gravity = 0.1f;
            //    if (velocity.Y > 2f)
            //    {
            //        velocity.Y = 2f;
            //    }
            //}
            //else if ((type == NPCID.DD2OgreT2 || type == NPCID.DD2OgreT3) && ai[0] > 0f && ai[1] == 2f)
            //{
            //    NPC.gravity = 0.45f;
            //    if (velocity.Y > 32f)
            //    {
            //        velocity.Y = 32f;
            //    }
            //}
            //else if (type == NPCID.VortexHornet && ai[2] == 1f)
            //{
            //    NPC.gravity = 0.1f;
            //    if (velocity.Y > 4f)
            //    {
            //        velocity.Y = 4f;
            //    }
            //}
            //else if (type == NPCID.VortexHornetQueen)
            //{
            //    NPC.gravity = 0.1f;
            //    if (velocity.Y > 3f)
            //    {
            //        velocity.Y = 3f;
            //    }
            //}
            //else if (type == NPCID.SandElemental)
            //{
            //    NPC.gravity = 0f;
            //}
            //float num = (float)(Main.maxTilesX / 4200);
            //num *= num;
            //float num2 = (float)((double)(position.Y / 16f - (60f + 10f * num)) / (Main.worldSurface / 6.0));
            //if ((double)num2 < 0.25)
            //{
            //    num2 = 0.25f;
            //}
            //if (num2 > 1f)
            //{
            //    num2 = 1f;
            //}
            //此方法在这里调用,修改NPC的gravity和maxFallSpeed
            //if (wet)
            //{
            //    if (honeyWet)
            //    {
            //        NPC.gravity = 0.1f;
            //        maxFallSpeed = 4f;
            //        return;
            //    }
            //    NPC.gravity = 0.2f;
            //    maxFallSpeed = 7f;
            //}
        }
        public enum SaveSetting
        {
            NoSave,
            PerPlayer,
            PerWorld,
            Public
        }
    }
}