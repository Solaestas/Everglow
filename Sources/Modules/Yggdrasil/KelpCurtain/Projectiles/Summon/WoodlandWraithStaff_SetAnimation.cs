using Everglow.Yggdrasil.KelpCurtain.Items.Armors.Ruin;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;

public class WoodlandWraithStaff_SetAnimation : ModProjectile
{
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
		Projectile.timeLeft = 9999;
	}

	public override void AI()
	{
		if (!Owner.active || Owner.GetModPlayer<RuinMask.RuinSetPlayer>().RuinSetBuffTimer <= 0)
		{
			Projectile.Kill();
			return;
		}

		Projectile.Center = Owner.Center;
		Owner.heldProj = Projectile.whoAmI;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var timer = Owner.GetModPlayer<RuinMask.RuinSetPlayer>().RuinSetBuffTimer;

		var armRot = Owner.direction * MathF.Cos(timer / (float)RuinMask.RuinSetPlayer.AnimationSwingInterval * MathHelper.TwoPi) + MathHelper.Pi;
		Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, armRot);
		armPosition.Y += Owner.gfxOffY;

		var gravDirAdaption = armPosition - Owner.Center;
		gravDirAdaption.Y *= Owner.gravDir;
		armPosition = Owner.Center + gravDirAdaption;

		var texture = ModAsset.WoodlandWraithStaff.Value;
		var weaponSourceRect = new Rectangle(0, 0, texture.Width, (int)(texture.Height * 0.8f)); // Cut texture to hide behind player's arm.
		var weaponColor = Lighting.GetColor(armPosition.ToTileCoordinates());
		var weaponRot = Owner.gravDir * armRot + MathHelper.Pi + MathHelper.PiOver4 - Owner.gravDir * MathHelper.PiOver2;
		var weaponOrigin = new Vector2(texture.Width * 0.1f, texture.Height * 0.9f);
		var weaponScale = Owner.GetAdjustedItemScale(Owner.HeldItem) * Owner.HeldItem.scale;

		Main.spriteBatch.Draw(texture, armPosition - Main.screenPosition, weaponSourceRect, weaponColor, weaponRot, weaponOrigin, weaponScale, SpriteEffects.None, 0);

		var handGlow = Commons.ModAsset.Point.Value;
		var glowScale = 0.3f + 0.05f * MathF.Sin((float)Main.timeForVisualEffects * 0.4f);
		Main.spriteBatch.Draw(handGlow, armPosition - Main.screenPosition, null, new Color(1f, 0f, 0f, 0f), 0, handGlow.Size() / 2, glowScale, SpriteEffects.None, 0);
		return false;
	}
}