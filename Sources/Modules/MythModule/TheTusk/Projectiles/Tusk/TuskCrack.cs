using Terraria.Graphics.Effects;
using Terraria.Localization;


namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Tusk
{
    public class TuskCrack : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tusk Crack");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "血裂痕");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            //if (Projectile.timeLeft > 590)
            //{
                //if (!Filters.Scene["TusCra"].IsActive())
                //{
                //    Filters.Scene.Activate("TusCra");
                //}
            //}
            //if (Projectile.timeLeft < 25)
            //{
            //    if (Filters.Scene["TusCra"].IsActive())
            //    {
            //        Filters.Scene.Deactivate("TusCra");
            //        ef2.Parameters["Strds"].SetValue(0);
            //        ef2.Parameters["DMax"].SetValue(0.1f);
            //        Projectile.Kill();
            //    }
            //}
            //ef2 = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/TuskCrack").Value;
            //float k0 = Projectile.velocity.Y / (float)Projectile.velocity.X;
            //k0 *= (float)Main.screenWidth / (float)Main.screenHeight;
            //ef2.Parameters["k0"].SetValue(k0);
            //Vector2 v0 = Projectile.Center - Main.screenPosition;
            //float Correc = (float)Main.screenWidth / (float)Main.screenHeight;
            //float x0 = v0.X / (float)Main.screenWidth;
            //float y0 = v0.Y / (float)Main.screenHeight;
            //float b0 = y0 - k0 * x0;
            //ef2.Parameters["b0"].SetValue(b0);
            //ef2.Parameters["x1"].SetValue(x0);
            //ef2.Parameters["y1"].SetValue(y0);
            //ef2.Parameters["uTime"].SetValue((float)Main.time * 0.06f);
            //float DMax = 0f;
            //if (Projectile.timeLeft > 500)
            //{
            //    DMax = (600 - Projectile.timeLeft) / 2500f;
            //    Projectile.velocity *= 0.95f;
            //}
            //if (Projectile.timeLeft > 60 && Projectile.timeLeft <= 500)
            //{
            //    DMax = 0.04f;
            //}
            //if (Projectile.timeLeft <= 60)
            //{
            //    DMax = Projectile.timeLeft / 1500f;
            //}
            //ef2.Parameters["DMax"].SetValue(DMax);
            //ef2.Parameters["LeMax"].SetValue(0.05f);
            if (Projectile.timeLeft > 150 && Projectile.timeLeft <= 533)
            {
                int Frequ = 24;
                if (Main.expertMode && !Main.masterMode)
                {
                    Frequ = 18;
                }
                if (Main.masterMode)
                {
                    Frequ = 3;
                }
                if (Projectile.timeLeft % Frequ == 0)
                {
                    float RanX = Main.rand.NextFloat(-400, 400);
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(RanX, -RanX / 15f) + new Vector2(0, -5), new Vector2(0, 0.3f), ModContent.ProjectileType<Projectiles.Tusk.TuskSpiceBlack>(), (int)((double)Projectile.damage / 6), Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
            }
            if (Projectile.velocity.Length() > 46)
            {
                Projectile.velocity = Projectile.velocity / Projectile.velocity.Length() * 46;
            }
            //ef2.Parameters["Strds"].SetValue(100);
        }
    }
}
