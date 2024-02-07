namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class OrichalcumPedal : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = false;
		Projectile.DamageType = DamageClass.Magic;

		Projectile.timeLeft = 360;
	}
	public override void AI()
	{
		if(TileCollisionUtils.PlatformCollision(Projectile.Center))
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.alpha += 10;
			return;
		}
		Projectile.frameCounter++;
		if(Projectile.frameCounter > 5)
		{
			Projectile.frame++;
			Projectile.frameCounter = 0;
		}
		if(Projectile.frame > 2)
		{
			Projectile.frame = 0;
		}
		Projectile.rotation += Projectile.ai[0];
		Projectile.ai[0] *= 0.98f;
		Projectile.velocity += new Vector2(Main.windSpeedCurrent * 0.2f, 0.25f * Projectile.scale);
		Projectile.velocity *= MathF.Pow(0.98f, Projectile.velocity.Length() * 0.54f);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		SpriteEffects spriteEffects = SpriteEffects.None;
		if (Projectile.spriteDirection == -1)
			spriteEffects = SpriteEffects.FlipHorizontally;
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(Projectile.frame * 20, (int)(Projectile.ai[1] * 20), 20, 20), lightColor * ((255 - Projectile.alpha) / 255f), Projectile.rotation, new Vector2(5), Projectile.scale, spriteEffects, 0);
		return false;
	}
}