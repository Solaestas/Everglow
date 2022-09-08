using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.SubWorldModule
{
    public abstract class Subworld : ModType
    {
        /// <summary>
        /// 世界宽度,必须为200的倍数且不小于800,不满足会自动修正
        /// </summary>
        public abstract int Width { get; }
        /// <summary>
        /// 世界高度,必须为150的倍数且不小于600,不满足会自动修正
        /// </summary>
        public abstract int Height { get; }
        /// <summary>
        /// 生成世界的过程任务
        /// </summary>
        public abstract List<GenPass> Tasks { get; }
        /// <summary>
        /// 我也不知道这是个什么玩意
        /// </summary>
        public virtual WorldGenConfiguration Config
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 是否保存世界
        /// </summary>
        public virtual bool ShouldSave
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 是否保存玩家
        /// </summary>
        public virtual bool NoPlayerSaving
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 正常更新,我也没搞明白指的什么更新
        /// </summary>
        public virtual bool NormalUpdates
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 进入世界时
        /// </summary>
        public virtual void OnEnter()
        {
        }
        /// <summary>
        /// 退出世界时
        /// </summary>
        public virtual void OnExit()
        {
        }
        /// <summary>
        /// 加载世界时
        /// </summary>
        public virtual void OnLoad()
        {
        }
        /// <summary>
        /// 卸载世界时
        /// </summary>
        public virtual void OnUnload()
        {
        }
        public virtual void DrawSetup(GameTime gameTime)
        {
            PlayerInput.SetZoom_Unscaled();
            Main.instance.GraphicsDevice.Clear(Color.Black);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            this.DrawMenu(gameTime);
            Main.DrawCursor(Main.DrawThickCursor(false), false);
            Main.spriteBatch.End();
        }
        public virtual void DrawMenu(GameTime gameTime)
        {
            Main.spriteBatch.DrawString(FontAssets.DeathText.Value, Main.statusText, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f - FontAssets.DeathText.Value.MeasureString(Main.statusText) / 2f, Color.White);
        }
        /// <summary>
        /// 光照
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rand"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public virtual bool GetLight(Tile tile, int x, int y, ref FastRandom rand, ref Vector3 color)
        {
            return false;
        }
        protected sealed override void Register()
        {
            if (SubworldSystem.subworlds is null)
            {
                SubworldSystem.waitregister.Enqueue(this);
            }
            else
            {
                SubworldSystem.subworlds.Add(this);
            }
        }
        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }
    }
}