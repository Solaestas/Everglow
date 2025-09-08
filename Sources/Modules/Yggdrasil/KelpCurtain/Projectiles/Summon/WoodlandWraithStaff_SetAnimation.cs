using Terraria.Audio;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;

public class WoodlandWraithStaff_SetAnimation : ModProjectile
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.SummonProjectiles;

	// behind -> front -> behind -> front -> behind = 5 * 15 frames
	public const int AnimationDuration = 75;
	public const int AnimationSwingInterval = 30;

	public override string Texture => ModAsset.WoodlandWraithStaffBuff_Mod;

	public Player Owner => Main.player[Projectile.owner];

	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = AnimationDuration;
	}

	public override void AI()
	{
		if (!Owner.active)
		{
			Projectile.Kill();
			return;
		}

		Projectile.Center = Owner.Center;
		Owner.heldProj = Projectile.whoAmI;

		SetArmPosition();
	}

	private void SetArmPosition()
	{
		Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Owner.direction * MathF.Cos(Projectile.timeLeft / (float)AnimationSwingInterval * MathHelper.TwoPi) + MathHelper.Pi);

		if (Projectile.timeLeft % AnimationSwingInterval == AnimationSwingInterval / 2)
		{
			SoundEngine.PlaySound(SoundID.Item20, Owner.Center);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var timeProgress = Projectile.timeLeft / (float)AnimationSwingInterval;
		var armRot = Owner.direction * MathF.Cos(timeProgress * MathHelper.TwoPi) * 0.8f + MathHelper.Pi;
		Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, armRot);
		armPosition.Y += Owner.gfxOffY;

		var gravDirAdaption = armPosition - Owner.Center;
		gravDirAdaption.Y *= Owner.gravDir;
		armPosition = Owner.Center + gravDirAdaption;

		var texture = ModAsset.WoodlandWraithStaff.Value;
		var weaponColor = Lighting.GetColor(armPosition.ToTileCoordinates());
		var weaponRot = Owner.gravDir * armRot + MathHelper.Pi + MathHelper.PiOver4 - Owner.gravDir * MathHelper.PiOver2;
		var weaponOrigin = new Vector2(texture.Width * 0.1f, texture.Height * 0.9f);
		var perspectiveScale = MathF.Sin(timeProgress * MathHelper.TwoPi) / MathHelper.Pi * 0.4f + 1;
		var weaponScale = Owner.GetAdjustedItemScale(Owner.HeldItem) * Owner.HeldItem.scale * perspectiveScale;
		Main.spriteBatch.Draw(texture, armPosition - Main.screenPosition, null, weaponColor, weaponRot, weaponOrigin, weaponScale, SpriteEffects.None, 0);

		var handGlow = Commons.ModAsset.Point.Value;
		var glowScale = 0.3f + 0.05f * MathF.Sin((float)Main.timeForVisualEffects * 0.4f);
		Main.spriteBatch.Draw(handGlow, armPosition - Main.screenPosition, null, new Color(1f, 0f, 0f, 0f), 0, handGlow.Size() / 2, glowScale, SpriteEffects.None, 0);
		return false;
	}
}