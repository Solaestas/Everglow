namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles
{
    internal class AcytaeaTornadoHit : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 1500;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        private float ka = 0;
        private int AIMNpc = -1;

        public override void AI()
        {
            if (AIMNpc < 0)
            {
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].type == ModContent.NPCType<NPCs.Acytaea>())
                    {
                        AIMNpc = i;
                        break;
                    }
                }
            }
            if (AIMNpc >= 0)
            {
                Projectile.Center = Main.npc[AIMNpc].Center;
            }
            Timer = Projectile.timeLeft / 15f;
            WHOAMI = Projectile.whoAmI;
            Typ = Projectile.type;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public static float Timer = 0;
        public static int WHOAMI = -1;
        public static int Typ = -1;
    }
}