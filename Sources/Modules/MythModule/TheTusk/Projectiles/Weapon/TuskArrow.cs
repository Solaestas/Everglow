namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    class TuskArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color((int)(Sc * 255f), (int)(Sc * 255f), (int)(Sc * 255f), (int)(Sc * 255f)));
        }
        private float Vd = 0;
        private float Sc = 0;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int y = 0; y < 4; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, 183, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d) * 0.5f;
            }
            for (int y = 0; y < 16; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, DustID.Blood, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 3f));
                Main.dust[num90].noGravity = false;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d) * 0.5f;
            }
        }
        public override void AI()
        {
            float Rot1 = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
            Projectile.rotation = Rot1;
            Player player = Main.player[Projectile.owner];
            Vector2 v0 = player.Center + new Vector2(-20 * player.direction, -40) - Projectile.Center;
            float vscale = (float)Math.Pow(v0.Length(), 0.2f);
            Vector2 v1 = v0 / v0.Length();
            Projectile.velocity += v1 * vscale / 10f;
            if (Projectile.velocity.Length() > 20)
            {
                Projectile.velocity *= 0.99f;
            }
            if (Projectile.velocity.Length() > 30)
            {
                Projectile.velocity *= 0.99f;
            }
            if (Projectile.velocity.Length() > 40)
            {
                Projectile.velocity *= 0.99f;
            }
            if (Projectile.timeLeft > 570)
            {
                Vd = (600 - Projectile.timeLeft) / 30f;
                Sc = (600 - Projectile.timeLeft) / 30f;
            }
            if (Projectile.timeLeft < 60)
            {
                Vd = Projectile.timeLeft / 60f;
                Sc = Projectile.timeLeft / 60f;
            }
            float num2 = Projectile.Center.X;
            float num3 = Projectile.Center.Y;
            if (Main.mouseRight)
            {
                Vector2 v0v = Main.MouseWorld - Projectile.Center;
                float vscale2 = (float)Math.Pow(v0v.Length(), 0.2f) * 20f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v0v / v0v.Length() * vscale2, ModContent.ProjectileType<Projectiles.Weapon.TuskArrow2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Projectile.Kill();
            }
            if (Main.rand.Next(3) == 1)
            {
                int num91 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, 183, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 9f) * Sc * 0.4f);
                Main.dust[num91].noGravity = true;
                Main.dust[num91].velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 2.5f)).RotatedByRandom(Math.PI * 2d) * Vd * 0.5f;
            }
            if (Projectile.oldPosition != Vector2.Zero)
            {
                for (int g = 0; g < Projectile.velocity.Length() * 2.5f; g++)
                {
                    Vector2 a0 = new Vector2(Projectile.width, Projectile.height) / 2f;
                    Vector2 v3 = Projectile.oldPosition + a0;
                    Vector2 v4 = Vector2.Normalize(Projectile.velocity) * 0.6f;
                    int num92 = Dust.NewDust(v3 + v4 * g - new Vector2(4, 4), 4, 4, DustID.Blood, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2f) * Sc * 0.4f);
                    Main.dust[num92].noGravity = true;
                    Main.dust[num92].velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 2.5f)).RotatedByRandom(Math.PI * 2d) * Vd * 0.5f;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}
