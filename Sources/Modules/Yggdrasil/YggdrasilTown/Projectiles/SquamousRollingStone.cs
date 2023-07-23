namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class SquamousRollingStone : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.width = 100;
		Projectile.height = 100;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 1500;
		Projectile.aiStyle = -1;
	}
	public override void AI()
	{
		Projectile.velocity.X += Projectile.direction * 0.04f;
		Projectile.velocity.Y += 0.2f;
		Projectile.rotation += Projectile.velocity.X * 0.04f;
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if(Collision.SolidCollision(Projectile.Center + new Vector2(60 * Projectile.direction, 0), 0, 0))
		{
			Projectile.velocity.Y -= Main.rand.NextFloat(3f, 9f);
			return false;
		}
		if((Projectile.velocity - oldVelocity).Length() > 8)
		{
			return true;
		}
		return false;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return true;
	}
	public override void Kill(int timeLeft)
	{

	}
}

