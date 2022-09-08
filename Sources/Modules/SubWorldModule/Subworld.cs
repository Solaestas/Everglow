using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.SubWorldModule
{
    public abstract class Subworld : ModType
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract List<GenPass> Tasks { get; }
        public virtual WorldGenConfiguration Config
        {
            get
            {
                return null;
            }
        }
        public virtual bool ShouldSave
        {
            get
            {
                return false;
            }
        }
        public virtual bool NoPlayerSaving
        {
            get
            {
                return false;
            }
        }
        public virtual bool NormalUpdates
        {
            get
            {
                return false;
            }
        }
        public virtual string SpecailPath => null;
        public virtual void OnEnter()
        {
        }
        public virtual void OnExit()
        {
        }
        public virtual void OnLoad()
        {
        }
        public virtual void OnUnload()
        {
        }
        public virtual void DrawSetup(GameTime gameTime)
        {
            PlayerInput.SetZoom_Unscaled();
            Main.instance.GraphicsDevice.Clear(Color.Black);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            DrawMenu(gameTime);
            Main.DrawCursor(Main.DrawThickCursor(false), false);
            Main.spriteBatch.End();
        }
        public virtual void DrawMenu(GameTime gameTime)
        {
            Main.spriteBatch.DrawString(FontAssets.DeathText.Value, Main.statusText, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f - FontAssets.DeathText.Value.MeasureString(Main.statusText) / 2f, Color.White);
        }
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