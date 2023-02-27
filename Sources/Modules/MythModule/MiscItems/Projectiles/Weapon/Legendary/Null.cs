using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class Null : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Null");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "错误代码C:\\user");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
        }
        public override void AI()
        {
        }
    }
}