using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class RockElemental_Stonefragment : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = 0;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 600;
	}

	public override void AI()
	{
		if (Projectile.velocity.Y <= 12)
		{
			Projectile.velocity.Y += 0.8f;
		}
		if (Projectile.timeLeft <= 597)
		{
			Projectile.tileCollide = true;
		}
	}

	public override void OnSpawn(IEntitySource source)
	{
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}

	public override void OnKill(int timeLeft)
	{

	}
	public override bool PreDraw(ref Color lightColor)
	{
		return base.PreDraw(ref lightColor);
	}
}