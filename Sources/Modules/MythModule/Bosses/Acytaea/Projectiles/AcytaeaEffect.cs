using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles
{
    public class AcytaeaEffect : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("AcytaeaEffect");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "AcytaeaEffect");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
            Projectile.scale = 5;
        }
        public override void AI()
        {
            Projectile.velocity *= 0;
            v0 = Projectile.Center;
            float DusS = 0.25f;
            if (Projectile.timeLeft >= 200 && Projectile.timeLeft <= 210)
            {
                DusS = 1.2f;
            }
            if (Projectile.timeLeft <= 200)
            {
                DusS = 0.25f;
            }
            if (Projectile.timeLeft >= 210)
            {
                Pro = (240 - Projectile.timeLeft) * (240 - Projectile.timeLeft) / 3;
            }
            else
            {
                Pro = 300;
            }
            if (Projectile.timeLeft >= 90)
            {
                Scale = 1;
            }
            else
            {
                float k0 = Projectile.timeLeft / 90f;
                Scale = k0 * k0 * k0 * k0;
            }
            for (int x = 0; x < 5; x++)
            {
                Vector2 v1 = new Vector2(Main.rand.NextFloat(-300f, -300f + Pro * 2) * Scale, 0).RotatedBy(0.4 * Projectile.ai[0]);
                Vector2 v2 = new Vector2(0, Main.rand.NextFloat(0.02f, 0.3f) * Scale).RotatedBy(0.4 * Projectile.ai[0]);
                Dust d = Dust.NewDustDirect(Projectile.Center + v1 - Vector2.Normalize(v2) * 4, 0, 0, ModContent.DustType<Dusts.CosmicFlame>(), 0, 0, 0, default, Scale * (300 - v1.Length()) / 300f * DusS);
                d.velocity = v2 * 0;
            }
            if (Pro < 300)
            {
                for (int x = 0; x < 3; x++)
                {
                    Vector2 v1 = new Vector2((-300f + Pro * Main.rand.NextFloat(2 - Pro / 900f, 2f)) * Scale, 0).RotatedBy(0.4 * Projectile.ai[0]);
                    Vector2 v2 = new Vector2(0, Main.rand.NextFloat(0.02f, 5f) * Scale).RotatedByRandom(6.28);
                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0.02f, 0.3f) * Scale).RotatedBy(0.4 * Projectile.ai[0]);
                    Dust d = Dust.NewDustDirect(Projectile.Center + v1 + v2 - Vector2.Normalize(v3) * 4, 0, 0, ModContent.DustType<Dusts.CosmicFlame>(), 0, 0, 0, default, 0.7f * Main.rand.NextFloat(0.5f, 1.5f) * (1.05f - Math.Abs((150 - Pro) / 150f)) * (1.05f - Math.Abs((150 - Pro) / 150f)) + 0.25f);
                    d.velocity = Vector2.Zero;
                }
            }
            AI0 = Projectile.ai[0];
        }
        public static Vector2 v0;
        public static float Scale = 1;
        public static int Pro = 0;
        public static float AI0 = 0;
        public static void DrawAll(SpriteBatch sb)
        {
            Texture2D tex2 = ModContent.Request<Texture2D>("MythMod/Dusts/CosmicCrack").Value;
            if (AI0 == -1)
            {
            }
            sb.Draw(tex2, v0 - Main.screenPosition, new Rectangle(0, 0, Pro, 50), Color.White, 0.4f * AI0, tex2.Size() / 2, Scale * 2, SpriteEffects.None, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float Col = 0;
            if (Projectile.timeLeft > 180)
            {
                if (Projectile.timeLeft > 210)
                {
                    float f = 1 - (Projectile.timeLeft - 210) / 30f;
                    Col = f * f * f;
                }
                else
                {
                    float f = 1 - (210 - Projectile.timeLeft) / 30f;
                    Col = f * f * f;
                }
            }
            Texture2D tex2 = ModContent.Request<Texture2D>("MythMod/Dusts/CosmicCrack2").Value;
            Main.spriteBatch.Draw(tex2, v0 - Main.screenPosition, new Rectangle(0, 0, Pro, 50), new Color(Col, Col, Col, 0), 0.4f * AI0, tex2.Size() / 2, Scale * 2.1f, SpriteEffects.None, 0);
            return true;
        }
    }
}
