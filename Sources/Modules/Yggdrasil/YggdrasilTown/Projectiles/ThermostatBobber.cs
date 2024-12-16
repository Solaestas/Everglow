namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class ThermostatBobber : ModProjectile
{
	public static Color FishingLineColor => new(255, 215, 0);

	public override void SetDefaults()
	{
		Projectile.width = 14;
		Projectile.height = 14;
		Projectile.aiStyle = 61;
		Projectile.bobber = true;
		Projectile.penetrate = -1;
		Projectile.CloneDefaults(ProjectileID.BobberWooden);
	}

	public override void AI()
	{
		if (!Main.dedServ) // Same as 'NetUtils.NotServer'
		{
			Lighting.AddLight(Projectile.Center, FishingLineColor.ToVector3());
		}
	}
}