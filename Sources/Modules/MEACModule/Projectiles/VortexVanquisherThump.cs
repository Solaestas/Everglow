using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Terraria.ID;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{
    public class VortexVanquisherThump : ModProjectile
    {
        public override string Texture => "Everglow/Sources/Modules/MEACModule/Projectiles/VortexVanquisher";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.netImportant = true;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 45;
            Projectile.tileCollide = false;
        }
        bool Crash = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft % 6 == 0)
            {
                StrikeDown();
            }
            player.immune = true;
            player.immuneTime = 8;
            Projectile.position += new Vector2(12 * Math.Sign(Projectile.velocity.X), 0);
            if(Projectile.timeLeft > 20)
            {
                player.velocity = Projectile.velocity * 16f;
            }
            else
            {
                player.velocity *= 0.9f;
            }
        }
        public void StrikeDown()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 CheckPoint = Projectile.Center + new Vector2(0, -100);
            for (int y = 0; y < 60; y++)
            {
                if (Collision.SolidCollision(CheckPoint, 1, 1))
                {
                    break;
                }
                else
                {
                    CheckPoint += new Vector2(0, 5) * player.gravDir;
                }
                if (y == 59)
                {
                    CheckPoint = Projectile.Center + new Vector2(0, -100);
                }
            }

            Vector2 TotalVector = Vector2.Zero;//合向量
            if((Projectile.Center + new Vector2(0, -100)).Y < CheckPoint.Y)
            {
                int TCount = 0;
                for (int a = 0; a < 12; a++)
                {
                    Vector2 v0 = new Vector2(10, 0).RotatedBy(a / 6d * Math.PI);
                    if (Collision.SolidCollision(CheckPoint + v0, 1, 1))
                    {
                        TotalVector -= v0;
                        TCount++;
                    }
                    else
                    {
                        TotalVector += v0;
                    }
                }
                for (int a = 0; a < 24; a++)
                {
                    Vector2 v0 = new Vector2(20, 0).RotatedBy(a / 12d * Math.PI);
                    if (Collision.SolidCollision(CheckPoint + v0, 1, 1))
                    {
                        TotalVector -= v0 * 0.5f;
                        TCount++;
                    }
                    else
                    {
                        TotalVector += v0 * 0.5f;
                    }
                }
            }

            if(TotalVector == Vector2.Zero)
            {
                TotalVector = new Vector2(0, -player.gravDir);
            }
            else
            {
                TotalVector = Utils.SafeNormalize(TotalVector, new Vector2(0, -player.gravDir));
            }
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), CheckPoint + TotalVector * 180, -TotalVector * 0.9f, ModContent.ProjectileType<VortexVanquisher3>(), Projectile.damage, 0, player.whoAmI, 1);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
