using Everglow.Yggdrasil.YggdrasilTown.Buffs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class NehemothBullet : ModProjectile
{
	public const int BuffDuration = 4 * 60;

	public bool HasNotShot { get; private set; } = true;

	public override void SetDefaults()
	{
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = 1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 600;
		Projectile.alpha = 255;
		Projectile.light = 0.5f;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = true;
		Projectile.extraUpdates = 1;

		AIType = ProjectileID.Bullet;
	}

	public override void AI()
	{
		// TODO: Init speed
		if (HasNotShot)
		{
			Projectile.velocity = Projectile.velocity.NormalizeSafe() * 30f;
			HasNotShot = false;
		}

		base.AI();
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (Main.rand.NextFloat() > 0.34)
		{
			target.AddBuff(ModContent.BuffType<Plague>(), BuffDuration);
		}
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		if (Main.rand.NextFloat() > 0.34)
		{
			target.AddBuff(ModContent.BuffType<Plague>(), BuffDuration);
		}
	}
}