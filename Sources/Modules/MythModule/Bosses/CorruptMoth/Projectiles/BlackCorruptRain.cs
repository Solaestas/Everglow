using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Projectiles
{
    public class BlackCorruptRain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Corrupt Rain");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "黑蚀雨");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 1000;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 0));
        }
        private bool initialization = true;
        private double X;
        private float Y;
        private float b;
        private float Stre2 = 1;
        public override void AI()
        {
            if (initialization)
            {
                X = (float)Math.Sqrt((double)Projectile.velocity.X * (double)Projectile.velocity.X + (double)Projectile.velocity.Y * (double)Projectile.velocity.Y);
                b = Main.rand.Next(-50, 50);
                initialization = false;
                if (Main.rand.Next(0, 2) == 1)
                {
                    Y = (float)Math.Sin((double)X / 5f * 3.1415926535f / 1f) / 1000f + 1;
                }
                else
                {
                    Y = (float)Math.Sin(-(double)X / 5f * 3.1415926535f / 1f) / 1000f + 1;
                }
            }
            if (Stre2 > 0.2)
            {
                Stre2 -= 0.005f;
            }
            Projectile.velocity *= 0.995f;
            if (Projectile.timeLeft < 995)
            {
                Vector2 vector = base.Projectile.position + new Vector2(2);
                int num = Dust.NewDust(vector, 2, 2, 191, 0f, 0f, 0, default(Color), (float)Projectile.scale * 0.8f);
                Main.dust[num].velocity *= 0.0f;
                Main.dust[num].noGravity = true;
                Main.dust[num].scale *= 1.2f;
                Main.dust[num].alpha = 200;
            }
            if (Projectile.timeLeft < 600 && Projectile.timeLeft >= 585)
            {
                if (Y < 1)
                {
                    Projectile.scale *= (float)Y / (Projectile.timeLeft / 585f);
                }
                else
                {
                    Projectile.scale *= (float)Y * Projectile.timeLeft / 585f;
                }
            }
            if (Projectile.timeLeft < 580 && Projectile.timeLeft >= 100 + (float)b)
            {
                Projectile.scale *= (float)Y;
            }
            if (Projectile.timeLeft < 100 + (float)b)
            {
                Projectile.scale *= 0.95f;
            }
            Projectile.velocity.Y += 0.01f;
            Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0f / 255f * Projectile.scale, (float)(255 - base.Projectile.alpha) * 0.01f / 255f, (float)(255 - base.Projectile.alpha) * 0.6f / 255f * Projectile.scale);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Light = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/FixCoinLight3");
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(255 * Stre2), (int)(255 * Stre2), (int)(255 * Stre2), 0), Projectile.rotation, new Vector2(56f, 56f), Projectile.scale, SpriteEffects.None, 0);
            return true;
        }
    }
}