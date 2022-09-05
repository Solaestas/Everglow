using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class CorruptDust : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1080;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
        }
        public override void AI()
        {
            if (Projectile.timeLeft % 3 == 0)
            {
                int index = Dust.NewDust(Projectile.position - new Vector2(8), Projectile.width, Projectile.height, ModContent.DustType<BlueGlowAppear>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.7f, 1.9f));
                Main.dust[index].velocity = Projectile.velocity * 0.5f;
            }
            int index2 = Dust.NewDust(Projectile.position - new Vector2(8), Projectile.width, Projectile.height, ModContent.DustType<BlueParticleDark2>(), 0f, 0f, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
            Main.dust[index2].velocity = Projectile.velocity * 0.5f;
            Main.dust[index2].alpha = (int)(Main.dust[index2].scale * 50);

        }
        public override void Kill(int timeLeft)
        {

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

        }

        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);
        }
    }
}