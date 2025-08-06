namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Melee;

public class LampWoodYoyo : ModProjectile
{
	public override void SetStaticDefaults()
	{
		ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 3.5f;
		ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 200f;
		ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 10f;
	}

	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;

		Projectile.aiStyle = ProjAIStyleID.Yoyo;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.MeleeNoSpeed;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;
	}
}