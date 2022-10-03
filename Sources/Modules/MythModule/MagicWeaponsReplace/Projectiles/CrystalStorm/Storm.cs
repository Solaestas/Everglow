using Everglow.Sources.Commons.Core.VFX;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CrystalStorm
{
    internal class Storm : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 AimCenter = Main.MouseWorld;
            bool weakening = false;
            for(int x = -80;x < 808;x+=8)
            {
                if(Collision.SolidCollision(AimCenter + new Vector2(0, x), 1, 1))
                {
                    AimCenter += new Vector2(0, x);
                    if(x <= 0)
                    {
                        weakening = true;
                    }
                    break;
                }
            }

            Projectile.Center = AimCenter;
            Projectile.velocity *= 0;
            if (Main.mouseRight && player.HeldItem.type == ItemID.CrystalStorm && !weakening)
            {
                Projectile.timeLeft = Intensity + 60;
                if(Intensity < 450)
                {
                    Intensity+=3;
                }
            }
            else
            {
                Intensity-=5;
                if(Intensity <= 0)
                {
                    Projectile.Kill();
                }
            }
            for (int j = 0; j < 12; j++)
            {
                Vector2 v0 = Vector2.Zero;
                float k0 = Main.rand.NextFloat(0f, 1f);
                float k1 = k0 * k0 * k0 * k0;
                Vector2 v1 = new Vector2(Main.rand.NextFloat(-150f, 150f) / (k1 * 10f + 1f), -k1 * 200 + 10);
                if (Collision.SolidCollision(Projectile.Center + v1, 1, 1))
                {
                    continue;
                }
                int dust0 = Dust.NewDust(Projectile.Center + v1, 0, 0, ModContent.DustType<Dusts.CrystalAppearStoppedByTileInAStorm>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(0.3f, 1.6f) * Math.Min(Intensity, 300) / 450f);
                Main.dust[dust0].noGravity = true;
                Main.dust[dust0].color.B = (byte)(v1.Length() / 2f);
                Main.dust[dust0].color.A = (byte)(Intensity / 2);
            }

            for (int j = 0; j < 4; j++)
            {
                float k2 = Main.rand.NextFloat(0f, 1f);
                float k3 = k2 * k2 * k2 * k2;
                Vector2 v2 = new Vector2(Main.rand.NextFloat(-150f, 150f) / (k3 * 10f + 1f), -k3 * 200 + 10);
                Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + v2, Vector2.Zero, ModContent.ProjectileType<CrystalWind>(), 0, 0, Projectile.owner, Main.rand.NextFloat(6.283f), Intensity / 1400f * Main.rand.NextFloat(0.85f, 1.15f));
                p0.timeLeft = Math.Min(120, Intensity / 2);
            }

            for (int j = 0; j < 8; j++)
            {
                float k2 = Main.rand.NextFloat(0f, 1f);
                float k3 = k2 * k2 * k2 * k2;
                Vector2 v2 = new Vector2(Main.rand.NextFloat(-150f, 150f) / (k3 * 10f + 1f), -k3 * 200 + 10);

                CrystalParticleStorm cp = new CrystalParticleStorm()
                {
                    timeLeft = 120,
                    size = Main.rand.NextFloat(0.03f, 0.06f),
                    velocity = Vector2.Zero,
                    Active = true,
                    Visible = true,
                    AI0 = Main.rand.NextFloat(6.283f),
                    AI1 = Intensity / 1400f * Main.rand.NextFloat(0.85f, 1.15f),
                    AI2 = 1
                };
                cp.position = Projectile.Center;

                VFXManager.Add(cp);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        internal int Intensity = 0;
        internal Vector2 RingPos = Vector2.Zero;
    }
}
