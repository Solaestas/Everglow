using Everglow.Commons.Modules;
using Everglow.Commons.ObjectPool;
using Everglow.IIID.Projectiles.NonIIIDProj.GoldenCrack;
using Everglow.IIID.Projectiles.PlanetBefall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;

namespace Everglow.Sources.Modules.IIIDModule
{
    /*internal class IIIDModule : EverglowModule
	{
        RenderTarget2D render;
        RenderTarget2D screen;
        RenderTarget2D bloom1;
        RenderTarget2D bloom2;
        Effect Bloom,GoldenCrackVFX;
		public override string Name => "IIID";
		public override void Load()
		{
            Bloom = ModContent.Request<Effect>("Everglow/Sources/Modules/IIIDModule/Effects/Bloom1").Value;
            GoldenCrackVFX = ModContent.Request<Effect>("Everglow/Sources/Modules/IIIDModule/Effects/GoldenCrack").Value;
			On_FilterManager.EndCapture += FilterManager_EndCapture;//原版绘制场景的最后部分——滤镜。在这里运用render保证不会与原版冲突
            Main.OnResolutionChanged += Main_OnResolutionChanged;
            On.Terraria.Main.LoadWorlds += Main_OnLoadWorlds;
        }        
        public override void Unload()
        {
            On.Terraria.Graphics.Effects.FilterManager.EndCapture -= FilterManager_EndCapture;
            Main.OnResolutionChanged -= Main_OnResolutionChanged;
            On.Terraria.Main.LoadWorlds -= Main_OnLoadWorlds;
        }

        private void Main_OnResolutionChanged(Vector2 obj)//在分辨率更改时，重建render防止某些bug
        {
            if (render != null)
            {
                CreateRender();
            }
        }

		private void FilterManager_EndCapture(On_FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
		{
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;

            if (render == null)
            {
                CreateRender();
            }
            if (gd == null)
            {
                return;
            }

            Bloom = ModContent.Request<Effect>("Everglow/Sources/Modules/IIIDModule/Effects/Bloom1").Value;
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();

            gd.SetRenderTarget(screen);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<GoldenCrack>())
                {
                    Color c3 = Color.Gold;
                    (proj.ModProjectile as GoldenCrack).PreDraw(ref c3);
                }
            }
            Main.spriteBatch.End();

            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
            Bloom.Parameters["uScreenResolution"].SetValue(new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 3f);
            Bloom.Parameters["uRange"].SetValue(1.5f);
            Bloom.Parameters["uIntensity"].SetValue(1.5f);
            Bloom.CurrentTechnique.Passes["GlurV"].Apply();
            /*CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y, Main.LocalPlayer.width, Main.LocalPlayer.height),
                   new Color(255, 0, 0),
                   node.num,
                   true, false);*/

           /* gd.SetRenderTarget(bloom1);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Draw(screen, Vector2.Zero, Color.White);
            Bloom.CurrentTechnique.Passes["GlurH"].Apply();

            gd.SetRenderTarget(bloom2);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Draw(bloom1, Vector2.Zero, Color.White);
            Main.spriteBatch.End();

            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.Additive);
            Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            Main.spriteBatch.Draw(bloom2, new Rectangle(0, 0, Main.screenWidth * 3, Main.screenHeight * 3), Color.White);
            Main.spriteBatch.End();

            // UseCosmic(gd);
            GoldenCrackVFX = ModContent.Request<Effect>("Everglow/Sources/Modules/IIIDModule/Effects/GoldenCrack").Value;
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(render);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<GoldenCrack>())
                {
                    Color c3 = Color.Gold;
                    (proj.ModProjectile as GoldenCrack).PreDraw(ref c3);
                }
            }
            sb.End();

            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            gd.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/IIIDModule/Projectiles/NonIIIDProj/GoldenCrack/GoldenCrack").Value;


            GoldenCrackVFX.CurrentTechnique.Passes["Tentacle"].Apply();
            GoldenCrackVFX.Parameters["m"].SetValue(0.0f);
            GoldenCrackVFX.Parameters["n"].SetValue(0.00f);
            sb.Draw(render, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.Additive);
            Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<PlanetBeFall>())
                {
                    (proj.ModProjectile as PlanetBeFall).DrawIIIDProj();
                }
            }
            Main.spriteBatch.End();

            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        private void Main_OnLoadWorlds(On.Terraria.Main.orig_LoadWorlds orig)
        {
            if (render != null)
            {
                CreateRender();
            }
            orig();
        }
        public void CreateRender()
        {
            render = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            screen = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            bloom1 = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 3, Main.screenHeight / 3);
            bloom2 = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 3, Main.screenHeight / 3);
        }

    }*/
}
