﻿namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    internal class LittleTusk0 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color((int)(Sc * 255f), (int)(Sc * 255f), (int)(Sc * 255f), (int)(Sc * 255f)));
        }
        private float Vd = 0;
        private float Sc = 0;
        public override void AI()
        {
            float Rot1 = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + (Math.PI * 0.25));
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
            if (Projectile.timeLeft > 230)
            {
                Vd = (240 - Projectile.timeLeft) / 10f;
                Sc = (240 - Projectile.timeLeft) / 10f;
            }
            if (Projectile.timeLeft < 60)
            {
                Vd = Projectile.timeLeft / 60f;
                Sc = Projectile.timeLeft / 60f;
            }
            if (Projectile.timeLeft < 220)
            {
                Projectile.friendly = true;
                for (int j = 0; j < 200; j++)
                {
                    if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                    {
                        float num5 = Main.npc[j].position.X + Main.npc[j].width / 2;
                        float num6 = Main.npc[j].position.Y + Main.npc[j].height / 2;
                        float num7 = Math.Abs(Projectile.position.X + Projectile.width / 2 - num5) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num6);
                        if (num7 < 200)
                        {
                            Vector2 v = Main.npc[j].Center - Projectile.Center;
                            v = v / v.Length() * 20f;
                            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v, ModContent.ProjectileType<Projectiles.Weapon.Tusk>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                            Projectile.Kill();
                        }
                    }
                }
                if (Main.rand.NextBool(3))
                {
                    int num91 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, DustID.VampireHeal, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 3f) * Sc * 0.4f);
                    Main.dust[num91].noGravity = true;
                    Main.dust[num91].velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 2.5f)).RotatedByRandom(Math.PI * 2d) * Vd * 0.5f;
                }
            }
            if (Projectile.oldPosition != Vector2.Zero && Projectile.timeLeft < 238)
            {
                for (int g = 0; g < Projectile.velocity.Length() * 2.5f; g++)
                {
                    Vector2 a0 = new Vector2(Projectile.width, Projectile.height) / 2f;
                    Vector2 v3 = Projectile.oldPosition + a0;
                    Vector2 v4 = Vector2.Normalize(Projectile.velocity) * 0.6f;
                    int num92 = Dust.NewDust(v3 + (v4 * g) - new Vector2(4, 4), 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 2f) * Sc * 0.4f);
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
