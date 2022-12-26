using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Tusk
{
    public class TuskSummon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
        }
        float Sca = 0;
        public override void AI()
        {
            if (Projectile.timeLeft > 170)
            {
                Sca = (200 - Projectile.timeLeft) / 10f;
            }
            else if (Projectile.timeLeft < 60)
            {
                Sca = Projectile.timeLeft / 20f;
            }
            if (Projectile.timeLeft == 160)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(0, -0.3f).RotatedBy(Projectile.ai[0]), ModContent.ProjectileType<Projectiles.Tusk.TuskSpiceBlack>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
            if (Projectile.timeLeft % 4 == 0)
            {
                int k = Dust.NewDust(Projectile.Center - new Vector2(4), 0, 0, DustID.VampireHeal, 0, 0, 0, default, Sca);
                Main.dust[k].noGravity = true;
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            /*Color colorz = Lighting.GetColor((int)(Projectile.Center.X / 16d), (int)(Projectile.Center.Y / 16d));
            colorz = Projectile.GetAlpha(colorz) * ((255 - Projectile.alpha) / 255f);
            Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Projectiles/Tusk/TuskSpice" + Fra.ToString()).Value;*/
            float Sca2 = (Sca - 1) / 9f;
            if (Sca2 > 0)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Projectiles/Tusk/BloodVortexCore").Value;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(56f, 56f), Sca2, SpriteEffects.None, 0);
            }
        }
    }
}