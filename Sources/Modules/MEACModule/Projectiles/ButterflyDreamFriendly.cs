using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Terraria.ID;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{
   
    public class ButterflyDreamFriendly : ModProjectile
    {
        public override string Texture => "Everglow/Sources/Modules/MythModule/Bosses/CorruptMoth/Projectiles/ButterflyDream";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("蓝蝶幻梦");
            Main.projFrames[Projectile.type] = 4;
            //ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            //ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 34;
            Projectile.netImportant = true;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.timeLeft);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.timeLeft = reader.ReadInt32();
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.velocity.X > 0 ? -1 : 1;
            if (Projectile.timeLeft < 260)
            {
                Projectile.friendly = true;
                NPC target = Main.npc[(int)Projectile.ai[0]];
                if (!target.active&&Projectile.timeLeft>10)
                {
                    Projectile.timeLeft = 10;
                }
                else//追踪目标
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * 15, 0.05f);
                }
            }
            else
            {
                Projectile.velocity *= 0.98f;
            }
            if (Projectile.timeLeft < 10)
                Projectile.scale -= 0.1f;
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }
            if (Projectile.timeLeft % 10 == 0)
            {
                Projectile.frame++;
            }
            if (Projectile.timeLeft % 3 == 0 )
            {
                int index = Dust.NewDust(Projectile.position - new Vector2(8), Projectile.width, Projectile.height, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.7f, 1.9f));
                Main.dust[index].velocity = Projectile.velocity * 0.5f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default, 0.6f);
                }
                for (int i = 0; i < 6; i++)
                {
                    int index = Dust.NewDust(Projectile.position - new Vector2(8), Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Blue, Main.rand.NextFloat(0.7f, 1.2f));
                    Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(5f, 10f)).RotatedByRandom(6.283);
                    Main.dust[index].noGravity = true;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0.2f, 0.5f, 1f, 0) * (1 - Projectile.alpha / 255f);
        }

    }
}
