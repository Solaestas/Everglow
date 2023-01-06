using Terraria.Localization;


namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Melee.Hepuyuan
{
    public class HepuyuanShake : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HepuyuanSpice");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "地桩枪");
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 180;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.extraUpdates = 2;
        }
        public override void AI()
        {

        }
        int CyanStrike = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}