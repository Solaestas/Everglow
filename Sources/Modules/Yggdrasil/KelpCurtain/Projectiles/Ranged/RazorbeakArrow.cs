using Everglow.Yggdrasil.KelpCurtain.Items.Weapons;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Ranged;

public class RazorbeakArrow : ModProjectile
{
	public Player Owner => Main.player[Projectile.owner];

	public int GroupIndex => (int)Projectile.ai[0];

	public override string Texture => ModAsset.CyatheaArrow_Mod;

	public override void SetDefaults()
	{
		Projectile.width = 18;
		Projectile.height = 18;

		Projectile.arrow = true;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;

		Projectile.penetrate = 1;

		Projectile.timeLeft = 1200;
	}

	public override void AI()
	{
		Projectile.velocity.Y += 0.1f;

		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

		if (Projectile.velocity.Y > 16f)
		{
			Projectile.velocity.Y = 16f;
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		var modPlayer = Owner.GetModPlayer<RazorbeakBowPlayer>();

		// Reset hit effect timer.
		modPlayer.RazorbeakBowTimer = RazorbeakBowPlayer.RazorbeakBowEffectDuration;

		// Process effect counter.
		if (modPlayer.RazorbeakHitInfo.TryGetValue(GroupIndex, out var value))
		{
			var count = value.Count + 1;
			if (count < RazorbeakBowPlayer.RazorbeakBowEffectHitRequirement)
			{
				modPlayer.RazorbeakHitInfo[GroupIndex] = (count, value.Done);
			}
			else
			{
				modPlayer.RazorbeakBowEffectStack = Math.Min(modPlayer.RazorbeakBowEffectStack + 1, RazorbeakBowPlayer.RazorbeakBowEffectMaxStack);
				modPlayer.RazorbeakHitInfo.Remove(GroupIndex); // Remove element immediately ensuring stack won't triggered twice by a single group.
			}
		}
	}

	public override void OnKill(int timeLeft)
	{
		// Remove the group index from the player's RazorbeakHitInfo if no other arrows of the same group are active.
		var modPlayer = Main.player[Projectile.owner].GetModPlayer<RazorbeakBowPlayer>();
		if (!Main.projectile.Where(proj =>
				proj.active
				&& proj.owner == Projectile.owner
				&& proj.type == Projectile.type
				&& proj.whoAmI != Projectile.whoAmI)
			.Any(p => (p.ModProjectile as RazorbeakArrow).GroupIndex == GroupIndex)
				&& modPlayer.RazorbeakHitInfo.TryGetValue(GroupIndex, out var hitInfo)
				&& hitInfo.Done)
		{
			modPlayer.RazorbeakHitInfo.Remove(GroupIndex);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, 1f, SpriteEffects.FlipVertically, 0);
		return false;
	}
}