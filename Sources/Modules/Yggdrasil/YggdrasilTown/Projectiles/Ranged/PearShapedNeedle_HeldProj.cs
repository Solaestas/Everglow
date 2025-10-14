using Everglow.Commons.Templates.Weapons;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Ranged;

public class PearShapedNeedle_HeldProj : HandholdProjectile
{
	public float Timer = 0;

	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.RangedProjectiles;

	public override void SetDef()
	{
		Projectile.friendly = false;
		DepartLength = 30;
	}

	public override void AI()
	{
		Timer++;
		base.AI();
	}

	public override void HeldProjectileAI()
	{
		Player player = Main.player[Projectile.owner];
		if (player.HeldItem.type != ModContent.ItemType<Items.Weapons.PearShapedNeedle>())
		{
			Projectile.Kill();
		}
		player.heldProj = Projectile.whoAmI;
		ArmRootPos = player.MountedCenter + new Vector2(-4 * player.direction, -2);

		Vector2 mouseToPlayer = Main.MouseWorld - ArmRootPos;
		mouseToPlayer = Vector2.Normalize(mouseToPlayer);
		Projectile.rotation = mouseToPlayer.ToRotation() + MathHelper.PiOver4;

		player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (Projectile.rotation - MathF.PI * 0.25f) * player.gravDir - MathF.PI * 0.5f);

		Projectile.Center = ArmRootPos + new Vector2(0, 1).RotatedBy(Projectile.rotation - Math.PI * 0.75) * DepartLength;
		if (player.controlUseItem)
		{
			Projectile.timeLeft = 10;
		}
		if (Projectile.Center.X < ArmRootPos.X)
		{
			player.direction = -1;
		}
		else
		{
			player.direction = 1;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawNeedles();
		return base.PreDraw(ref lightColor);
	}

	public void DrawNeedles()
	{
		for (int k = 0; k < 13; k++)
		{
			float distance = GetDistance(k);
			if (distance < 0)
			{
				break;
			}
			DrawNeedlePiece(k);
			if (k > 0)
			{
				DrawNeedlePiece(-k);
			}
		}
	}

	public float GetDistance(int k)
	{
		k = Math.Abs(k);
		float needlePhase = Timer - k * 12;
		if (needlePhase < 0)
		{
			return -1;
		}
		float distance = needlePhase * 6;
		float maxDis = 30 + 5 * MathF.Sin(Timer * 0.1f + (k % 2) * 0.1f + k);
		if (k % 2 == 0)
		{
			maxDis += 10;
		}
		if (distance > maxDis)
		{
			distance = maxDis;
		}
		return distance;
	}

	public float GetShineValue(int k)
	{
		k = Math.Abs(k);
		float shineValue = 0;
		if (Timer > 156)
		{
			shineValue = (Timer - 156) / 24f * 13f;
			shineValue = 3 - (k - shineValue);
			shineValue = Math.Max(shineValue, 0) / 3f;
		}
		if (shineValue > 1)
		{
			shineValue = MathF.Sin(Timer * 0.1f + (k % 2) * 0.1f + k) + 1;
			shineValue *= 0.1f;
		}
		return shineValue;
	}

	public void DrawNeedlePiece(int k)
	{
		Texture2D needle = ModAsset.PearShapedNeedle_Needle.Value;
		Texture2D needle_bloom = ModAsset.PearShapedNeedle_shine.Value;
		float kAngle = 0.14f;
		Vector2 drawPos = GetNeedleWorldPos(k);
		Color lightColor = Lighting.GetColor(drawPos.ToTileCoordinates());
		drawPos -= Main.screenPosition;
		Main.EntitySpriteDraw(needle, drawPos, null, lightColor, Projectile.rotation + k * kAngle, needle.Size() * 0.5f, 1f, SpriteEffects.None, 0);

		float shineValue = GetShineValue(k);
		if (shineValue > 0)
		{
			Color shineColor = lightColor;
			shineColor.A = 0;
			shineColor *= shineValue;
			Main.EntitySpriteDraw(needle_bloom, drawPos, null, shineColor, Projectile.rotation + k * kAngle, needle_bloom.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		}
	}

	public override void OnKill(int timeLeft)
	{
		ReleaseNeedles();
		base.OnKill(timeLeft);
	}

	public bool NeedleReady(int k)
	{
		float shineValue = 0;
		if (Timer > 156)
		{
			shineValue = (Timer - 156) / 24f * 13f;
			shineValue = 3 - (k - shineValue);
			shineValue = Math.Max(shineValue, 0) / 3f;
		}
		if (shineValue > 1)
		{
			return true;
		}
		return false;
	}

	public Vector2 GetNeedleWorldPos(int k)
	{
		return Projectile.Center + GetReleaseVel(k);
	}

	public Vector2 GetReleaseVel(int k)
	{
		float kAngle = 0.14f;
		return new Vector2(GetDistance(k), 0).RotatedBy(Projectile.rotation - MathHelper.PiOver4 + k * kAngle);
	}

	public void ReleaseNeedles()
	{
		for (int k = 0; k < 13; k++)
		{
			if (NeedleReady(k))
			{
				Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), GetNeedleWorldPos(k), GetReleaseVel(k).NormalizeSafe(), ModContent.ProjectileType<PearShapedNeedle_Needle>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				if (k > 0)
				{
					Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), GetNeedleWorldPos(-k), GetReleaseVel(-k).NormalizeSafe(), ModContent.ProjectileType<PearShapedNeedle_Needle>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				}
			}
		}
	}
}