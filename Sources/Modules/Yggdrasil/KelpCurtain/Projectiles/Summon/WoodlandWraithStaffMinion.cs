using Everglow.Yggdrasil.KelpCurtain.Buffs;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;

public class WoodlandWraithStaffMinion : ModProjectile
{
	public override string Texture => Commons.ModAsset.White_Mod;

	private Player Owner => Main.player[Projectile.owner];

	public override void SetStaticDefaults()
	{
		Main.projPet[Projectile.type] = true;
		Main.projFrames[Projectile.type] = 8;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
	}

	public override void SetDefaults()
	{
		Projectile.width = 24;
		Projectile.height = 24;

		Projectile.DamageType = DamageClass.Summon;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;

		Projectile.minion = true;
		Projectile.minionSlots = 1f;
	}

	public override void AI()
	{
		if (Owner.active)
		{
			Owner.AddBuff(ModContent.BuffType<WoodlandWraithStaffBuff>(), 2);
			Projectile.timeLeft = 2;
		}

		var movement = Owner.Center - Projectile.Center + new Vector2(Owner.direction * -30, -40);
		if (movement.Length() >= 8)
		{
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, movement.NormalizeSafe() * 8f, 0.2f);
		}
		else
		{
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, movement, 0.6f);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var scale = 0.2f;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, 0, texture.Size() / 2, scale, SpriteEffects.None, 0);
		return false;
	}
}