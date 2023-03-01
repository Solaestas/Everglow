namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles
{
    internal class AcytaeaMagicBook : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("魔法书");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 56;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
        }

        private float K = 10;
        private int AIMNpc = -1;
        private float ka = 1;

        public override void AI()
        {
            if (Projectile.timeLeft % 5 == 0)
            {
                if (Projectile.frame < 4)
                {
                    Projectile.frame++;
                }
                else
                {
                    Projectile.frame = 0;
                    Projectile.NewProjectile(null, Projectile.Center, new Vector2(Projectile.spriteDirection * 5, -3), ModContent.ProjectileType<MeteroFri>(), 40, 3, Main.myPlayer);
                }
            }
            ka = 1;
            for (int f = 0; f < 200; f++)
            {
                if (Main.npc[f].type == ModContent.NPCType<NPCs.Acytaea>())
                {
                    AIMNpc = f;
                    break;
                }
            }
            if (AIMNpc != -1)
            {
                Projectile.Center = Main.npc[AIMNpc].Center;
                Projectile.spriteDirection = Main.npc[AIMNpc].spriteDirection;
            }
        }
    }
}