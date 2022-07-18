using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Projectiles
{
    public class BlackCorruptRain3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Corrupt Ball");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "黑萤球");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 700;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 0));
        }
        private bool initialization = true;
        private float X;
        private float E;
        private float Y;
        private float b;
        private float Stre2 = 1;
        private float sc = 0;
        public override void AI()
        {
            if (initialization)
            {
                X = Projectile.velocity.Length();
                b = Main.rand.Next(-50, 50);
                initialization = false;
                if (Main.rand.Next(0, 2) == 1)
                {
                    Y = (float)Math.Sin(X / 5 * Math.PI) / 1000f + 1;
                }
                else
                {
                    Y = (float)Math.Sin(-X / 5 * Math.PI) / 1000f + 1;
                }
            }
            if (Projectile.timeLeft < 995)
            {
            }
            if (sc < 1 && Projectile.timeLeft < 900)
            {
                sc += 0.01f;
            }
            if (Projectile.timeLeft < 600 && Projectile.timeLeft >= 585)
            {
                if (Y < 1)
                {
                    Projectile.scale *= Y / (Projectile.timeLeft / 585f);
                }
                else
                {
                    Projectile.scale *= Y * Projectile.timeLeft / 585f;
                }
            }
            if (Projectile.timeLeft < 580 && Projectile.timeLeft >= 100 + b)
            {
                Projectile.scale *= Y;
            }
            if (Projectile.timeLeft < 100 + b)
            {
                Projectile.scale *= 0.95f;
            }
            if (Projectile.velocity.Length() < 5f)
            {
                Projectile.velocity *= 1.018f;
            }
            if (Stre2 > 0)
            {
                Stre2 -= 0.005f;
            }
            if (E < 1)
            {
                E += 0.01f;
            }
            Lighting.AddLight(base.Projectile.Center, (255 - base.Projectile.alpha) * 0f / 255f * Projectile.scale * E, (255 - base.Projectile.alpha) * 0.01f / 255f * E, (255 - base.Projectile.alpha) * 0.6f / 255f * Projectile.scale * E);
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/Lightball");
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(10, 83, 110, 0), Projectile.rotation, new Vector2(250f, 250f), (float)(Projectile.scale * (1.4 + Math.Sin(Projectile.timeLeft / 15d + Projectile.position.X / 36d)) / 1.25 * E), SpriteEffects.None, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Light = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/FixCoinLight3");
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(255 * Stre2), (int)(255 * Stre2), (int)(255 * Stre2), 0), Projectile.rotation, new Vector2(56f, 56f), Projectile.scale, SpriteEffects.None, 0);
            return true;
        }
    }
}