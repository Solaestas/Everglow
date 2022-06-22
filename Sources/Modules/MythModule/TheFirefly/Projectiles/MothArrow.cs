﻿using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;

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
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 0));
        }
        private int addi;
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
            addi++;
            if (addi % 3 == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<MothBlue2>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default(Color), Main.rand.NextFloat(0.6f, 1.8f));
            }
            if (Collision.SolidCollision(Projectile.Center, 1, 1))
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
        public override void Kill(int timeLeft)
        {
            /*for (int j = 0; j < 20; j++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(6.28);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + v * 2f, v, ModContent.ProjectileType<Projectiles.Typeless.BlueTriangle>(), 0, 1, Main.myPlayer, Projectile.ai[0], 0f);
            }*/
            for (int j = 0; j < 16; j++)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283);
                int num20 = Dust.NewDust(Projectile.Center + Vector2.Normalize(Projectile.velocity) * 16f, 0, 0, ModContent.DustType<MothBlue2>(), v0.X, v0.Y, 100, default(Color), 1f);
                Main.dust[num20].noGravity = true;
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 100 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[1]);
                    Player player = Main.player[Projectile.owner];
                    player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[1] / 100f) * 1.0f));
                }
            }
        }
    }
}
