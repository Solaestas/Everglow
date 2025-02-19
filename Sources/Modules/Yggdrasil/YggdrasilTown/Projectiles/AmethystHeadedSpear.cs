using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class AmethystHeadedSpear : ModProjectile
{
	protected virtual float HoldoutRangeMin => 24f;

	protected virtual float HoldoutRangeMax => 112f;

	public override void SetDefaults()
	{
		Projectile.CloneDefaults(ProjectileID.Spear);
	}

	public override bool PreAI()
	{
		Player player = Main.player[Projectile.owner];
		int duration = player.itemAnimationMax;

		player.heldProj = Projectile.whoAmI;

		if (Projectile.timeLeft > duration)
		{
			Projectile.timeLeft = duration;
		}

		Projectile.velocity = Vector2.Normalize(Projectile.velocity);

		float halfDuration = duration * 0.5f;
		float progress;

		if (Projectile.timeLeft < halfDuration)
		{
			progress = Projectile.timeLeft / halfDuration;
			Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<AmethystSpearDust>(), 0, 0);
			dust.noGravity = true;
			dust.velocity *= 0;
		}
		else
		{
			progress = (duration - Projectile.timeLeft) / halfDuration;
		}

		Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);

		return false;
	}

	public Vector2 oldPos;

	public override bool PreDraw(ref Color lightColor)
	{
		if (oldPos == default)
		{
			oldPos = Projectile.Center;
		}
		Texture2D flag = ModAsset.AmethystHeadedSpear_flag.Value;
		Vector2 normalVel = Vector2.Normalize(Projectile.velocity);
		float rotation = (Projectile.Center - oldPos).ToRotation() + MathHelper.PiOver2;
		Main.spriteBatch.Draw(flag, Projectile.Center - Main.screenPosition - normalVel * 28f, null, lightColor, rotation, new Vector2(3, 0), new Vector2(1f, 2f), SpriteEffects.None, 0);
		oldPos = Projectile.Center;
		return base.PreDraw(ref lightColor);
	}

	public override void PostDraw(Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		Texture2D head = ModAsset.AmethystHead.Value;
		Vector2 normalVel = Vector2.Normalize(Projectile.velocity);
		Main.spriteBatch.Draw(head, Projectile.Center - Main.screenPosition - normalVel * 14.142f, null, lightColor * 0.9f, Projectile.rotation, head.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		float duration = Projectile.timeLeft / (float)player.itemAnimationMax;
		if (duration is > 0.4f and < 0.6f)
		{
			float value = (0.1f - Math.Abs(0.5f - duration)) * 10f;
			Main.spriteBatch.Draw(head, Projectile.Center - Main.screenPosition - normalVel * 14.142f, null, new Color(value, value, value, 0), Projectile.rotation, head.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		}
	}
}