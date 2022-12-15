namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    class Tusk0 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 39;
            Projectile.height = 39;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 720;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }

        private float Vd = 0;
        private float Sc = 0;
        public override void AI()
        {
            float Rot1 = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
            float Rot2 = 1.8f;
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
            if (Projectile.timeLeft > 640)
            {
                Vd = (720 - Projectile.timeLeft) / 80f;
                Sc = (720 - Projectile.timeLeft) / 80f;
            }
            if (Projectile.timeLeft < 60)
            {
                Vd = Projectile.timeLeft / 60f;
                Sc = Projectile.timeLeft / 60f;
            }
            float num2 = Projectile.Center.X;
            float num3 = Projectile.Center.Y;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num5) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num6);
                    if (num7 < 200)
                    {
                        Vector2 v = Main.npc[j].Center - Projectile.Center;
                        v = v / v.Length() * 36f;
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v, ModContent.ProjectileType<Projectiles.Weapon.Tusk>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                        Projectile.Kill();
                    }
                }
            }
            if (Main.rand.Next(3) == 1)
            {
                int num91 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, 183, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 9f) * Sc * 0.4f);
                Main.dust[num91].noGravity = true;
                Main.dust[num91].velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 2.5f)).RotatedByRandom(Math.PI * 2d) * Vd * 0.5f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}
