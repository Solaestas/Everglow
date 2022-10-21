using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    internal class MothArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 0));
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.timeLeft % 3 == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<BlueGlowAppear>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default(Color), Main.rand.NextFloat(0.6f, 1.8f));
            }
            /*
            if (Collision.SolidCollision(Projectile.Center, 1, 1))
            {
                Projectile.Kill();
            }*/
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public override void Kill(int timeLeft)
        {
            for (int j = 0; j < 16; j++)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283);
                int num20 = Dust.NewDust(Projectile.Center + Vector2.Normalize(Projectile.velocity) * 16f, 0, 0, ModContent.DustType<BlueGlowAppear>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(0.6f, 1.8f));
                Main.dust[num20].noGravity = true;
            }
            for (int j = 0; j < 32; j++)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283);
                int num21 = Dust.NewDust(Projectile.Center + Vector2.Normalize(Projectile.velocity) * 16f, 0, 0, ModContent.DustType<BlueParticleDark2>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(3.7f, 5.1f));
                Main.dust[num21].alpha = (int)(Main.dust[num21].scale * 50);
            }
            //写得不是很好的范围杀伤
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].active)
                {
                    if ((Main.npc[j].Center - Projectile.Center).Length() < 100 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly)
                    {
                        Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), true);
                        Player player = Main.player[Projectile.owner];
                        player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[1] / 100f) * 1.0f));
                    }
                }
            }
        }
    }
}