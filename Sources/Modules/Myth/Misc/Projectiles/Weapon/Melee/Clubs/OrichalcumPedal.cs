namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class OrichalcumPedal : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 40;
		Projectile.height = 40;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = false;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 360;
	}
	public override void AI()
	{
		Projectile.frameCounter++;
		if(Projectile.frameCounter > 5)
		{
			Projectile.frame++;
		}
		if(Projectile.frame == 2)
		{
			Projectile.frame = 0;
		}
		Projectile.velocity += new Vector2(Main.windSpeedCurrent * 0.2f, 0.15f);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		SpriteEffects spriteEffects = SpriteEffects.None;
		if (Projectile.spriteDirection == -1)
			spriteEffects = SpriteEffects.FlipHorizontally;
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 20, 20, 20), lightColor, Projectile.rotation, new Vector2(5), Projectile.scale, spriteEffects, 0);
		return false;
	}
}