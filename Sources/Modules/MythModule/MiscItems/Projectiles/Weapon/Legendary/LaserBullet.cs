using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class LaserBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Laser Bullet");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "光弹");
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 120;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;
        }
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
        }
        public override void Kill(int timeLeft)
        {
            for (int y = 0; y < 4; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, 182, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.0f, 2.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(1.0f, 1.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 0));
        }
    }
}