using Terraria.Localization;

namespace Everglow.Myth.Acytaea.Projectiles;

public class playerHit : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("");
			}

	public override void SetDefaults()
	{
		Projectile.width = 50;
		Projectile.height = 50;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 5;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1;
	}

	public override void AI()
	{
	}
}