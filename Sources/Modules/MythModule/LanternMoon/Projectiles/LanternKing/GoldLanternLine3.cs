using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles.LanternKing
{
    public class GoldLanternLine3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Lantern Line");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "灯笼须3");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
        }
        float Z = 0;
        public override void AI()
        {
            Projectile.velocity = Projectile.velocity * 0.95f;
            if (Projectile.timeLeft == 90)
            {
                x = Main.rand.NextFloat(0, (float)(Math.PI * 2));
            }
        }
        public override void PostDraw(Color lightColor)
        {
            x += 0.01f;
            float K = (float)(Math.Sin(x + Math.Sin(x) * 6) * (0.95 + Math.Sin(x + 0.24 + Math.Sin(x))) + 3) / 30f;
            float M = (float)(Math.Sin(x + Math.Tan(x) * 6) * (0.95 + Math.Cos(x + 0.24 + Math.Sin(x))) + 3) / 30f;
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.8f, 0f, 0) * 0.4f, 0, new Vector2(128f, 128f), K * 2.4f * Projectile.timeLeft / 90f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.8f, 0f, 0) * 0.4f, (float)(Math.PI * 0.5), new Vector2(128f, 128f), K * 2.4f * Projectile.timeLeft / 90f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.6f, 0f, 0) * 0.4f, (float)(Math.PI * 0.75), new Vector2(128f, 128f), M * 2.4f * Projectile.timeLeft / 90f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.6f, 0f, 0) * 0.4f, (float)(Math.PI * 0.25), new Vector2(128f, 128f), M * 2.4f * Projectile.timeLeft / 90f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.4f, 0f, 0) * 0.4f, x * 6f, new Vector2(128f, 128f), (M + K) * 2.4f * Projectile.timeLeft / 90f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.4f, 0f, 0) * 0.4f, -x * 6f, new Vector2(128f, 128f), (float)Math.Sqrt(M * M + K * K) * 2.4f * Projectile.timeLeft / 90f, SpriteEffects.None, 0f);
        }
        float x = 0;
    }
}
