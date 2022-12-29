using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class FruitBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //TODO: FruitBomb Localization Text
            //DisplayName.SetDefault("FruitBomb");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "萤火爆裂");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1000;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 0f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 0));
        }
        private bool initialization = true;
        private double X;
        private float Y;
        private float b = 0;
        public override void AI()
        {
            b += 0.015f;
            b *= 0.99f;
            Projectile.scale = b / 2f;
            if (Projectile.scale > 0.5f)
            {
                Projectile.Kill();
            }
            Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0f / 255f * Projectile.scale, (float)(255 - base.Projectile.alpha) * 0.01f / 255f, (float)(255 - base.Projectile.alpha) * 0.6f / 255f * Projectile.scale);
        }
        public override void PostDraw(Color lightColor)
        {
            /*Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);*/
        }
        int frequency = 30;
        int Acount = 1;
        private Effect ef;
        private float Rota = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Rota = (float)(Math.Sqrt(Projectile.scale * 2));
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/BlueWave").Value;
            Texture2D tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/FruitBomb").Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            //ef.Parameters["ValB"].SetValue(Rota);
            //ef.Parameters["Col"].SetValue((float)(fade * fade));
            //ef.CurrentTechnique.Passes["Test"].Apply();
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), 0, new Vector2(600), Projectile.scale, SpriteEffects.None, 0f);
            if (!Main.gamePaused)
            {
            }
            return false;
        }
    }
}