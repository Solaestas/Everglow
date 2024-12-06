namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class MysteriousTablet : ModProjectile
{
	public int TileCollideCounter { get; set; }

	public override void SetDefaults()
	{
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.scale = 0.5f;
		Projectile.width = 64;
		Projectile.height = 64;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 600;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
	}

	public override void AI()
	{
		Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
		if (Projectile.velocity.Y > 16f)
		{
			Projectile.velocity.Y = 16f;
		}
		Projectile.rotation += 0.4f * Projectile.direction;
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (++TileCollideCounter >= 5)
		{
			return true;
		}

		Projectile.velocity *= -0.69f;
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.value *= 2;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}