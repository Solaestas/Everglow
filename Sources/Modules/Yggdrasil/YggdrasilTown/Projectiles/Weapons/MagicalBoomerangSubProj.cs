using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Weapons;

public class MagicalBoomerangSubProj : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.timeLeft = 3600;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = true;
		Projectile.ignoreWater = true;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.width = 26;
		Projectile.height = 26;
		Timer = 0;
	}

	public bool Returning = false;

	public int Timer = 0;

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (Returning)
		{
			return false;
		}
		if (Projectile.velocity.X != oldVelocity.X)
		{
			Projectile.velocity.X = -oldVelocity.X;
		}

		if (Projectile.velocity.Y != oldVelocity.Y)
		{
			Projectile.velocity.Y = -oldVelocity.Y;
		}
		Projectile.tileCollide = false;
		Returning = true;
		return false;
	}

	public override void OnSpawn(IEntitySource source)
	{
	}

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Timer++;
		Projectile.rotation += 0.3f;
		if (!Returning && Timer > 20)
		{
			Returning = true;
		}
		if (Returning)
		{
			Projectile.tileCollide = false;
			Vector2 toPlayer = player.Center + player.velocity - Projectile.Center - Projectile.velocity;
			float speed = 13f;
			speed += (Timer - 120) / 10f;
			if(toPlayer.Length() < speed * 2)
			{
				Projectile.Kill();
			}
			Projectile.velocity = Projectile.velocity * 0.9f + toPlayer.NormalizeSafe() * speed * 0.1f;
		}
		Lighting.AddLight(Projectile.Center, new Vector3(0.3f, 0.7f, 1f) * 0.5f);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (!Returning)
		{
			Returning = true;
		}
		base.OnHitNPC(target, hit, damageDone);
	}

	public override void OnKill(int timeLeft) => base.OnKill(timeLeft);

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D boomerang = ModAsset.MagicalBoomerangSubProj.Value;
		Texture2D boomerangGlow = ModAsset.MagicalBoomerangSubProj_glow.Value;
		Main.EntitySpriteDraw(boomerang, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, boomerang.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		Main.EntitySpriteDraw(boomerangGlow, Projectile.Center - Main.screenPosition, null, new Color(0.3f, 0.7f, 1f, 0), Projectile.rotation, boomerangGlow.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}