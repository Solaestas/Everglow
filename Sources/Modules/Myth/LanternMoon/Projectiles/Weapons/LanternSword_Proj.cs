using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.Weapons;

public class LanternSword_Proj : MeleeProj_3D
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MeleeProjectiles;

	public override void OnSpawn(IEntitySource source)
	{
		EnableSphereCoordDraw = false;
		SlashColor = new Color(0.85f, 0.02f, 0.06f, 0);
	}

	public override void SetCustomDefaults()
	{
		Projectile.width = 82;
		Projectile.height = 82;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 5;
		WeaponLength = 84;
	}

	public override void HitNPCVFX(float hitRotation, Vector2 hitPos)
	{
		// DivineAscendHitStar dAHS = new DivineAscendHitStar();
		// dAHS.Active = true;
		// dAHS.Visible = true;
		// dAHS.Position = hitPos;
		// dAHS.Rotation = hitRotation;
		// dAHS.Scale = 1f;
		// dAHS.MaxTime = 12;
		// Ins.VFXManager.Add(dAHS);

		// for (int k = 0; k < 8; k++)
		// {
		// DivineAscendHitSpark dAHSp = new DivineAscendHitSpark();
		// dAHSp.Active = true;
		// dAHSp.Visible = true;
		// dAHSp.Position = hitPos;
		// dAHSp.Scale = Main.rand.NextFloat(0.1f, 0.3f);
		// dAHSp.MaxTime = Main.rand.NextFloat(30, 60);
		// dAHSp.Velocity = new Vector2(Main.rand.NextFloat(6, 48), 0).RotatedBy(hitRotation - MathHelper.PiOver2 + Main.rand.NextFloat(-0.5f, 0.5f));
		// dAHSp.ai = [Main.rand.NextFloat(MathHelper.TwoPi)];
		// Ins.VFXManager.Add(dAHSp);
		// }
	}

	public override Color GetTrailColor(int style, Vector2 worldPos, int index, ref float factor, float extraValue0 = 0, float extraValue1 = 0)
	{
		if(style == 3)
		{
			Color drawColor = new Color(1f, 0.7f, 0.1f, 0);
			drawColor *= factor * extraValue0;
			if (!SelfLuminous)
			{
				Color lightC = Lighting.GetColor(worldPos.ToTileCoordinates());
				drawColor.R = (byte)(lightC.R * drawColor.R / 255f);
				drawColor.G = (byte)(lightC.G * drawColor.G / 255f);
				drawColor.B = (byte)(lightC.B * drawColor.B / 255f);
			}
			drawColor *= 1.7f;
			float rot = (worldPos - Projectile.Center).ToRotation() - (float)Main.time * 0.01f;
			float factorHighlight = factor * 3;
			drawColor *= MathF.Max(MathF.Pow(Math.Max(0, 0.5f + 0.5f * MathF.Cos(rot * 2)), 16) * 0.4f * ReflectionSharpValue * MathF.Pow(extraValue0, 2), factorHighlight);
			drawColor.A = 0;
			return drawColor;
		}
		return base.GetTrailColor(style, worldPos, index, ref factor, extraValue0, extraValue1);
	}
}