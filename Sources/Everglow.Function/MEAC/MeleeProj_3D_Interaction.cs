using Everglow.Commons.Utilities;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Shaders;

namespace Everglow.Commons.MEAC;

public abstract partial class MeleeProj_3D : ModProjectile, IWarpProjectile_warpStyle2, IBloomProjectile
{

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		for (int k = 0; k < SlashEffects.Count; k++)
		{
			SlashEffect sEffect = SlashEffects[k];
			if (sEffect.SlashTrail_Smoothed is null || Math.Abs(sEffect.RotateSpeed) < 0.1f || sEffect.SlashTrail_Smoothed.Count < 3 || !sEffect.Active)
			{
				continue;
			}

			// Using a polygon of the last several vertices of the slash trail to detect collision, which can better fit the actual slash area and avoid missing targets when the slash is fast.
			List<Vector2> attackPolygon = [];
			int start = sEffect.SlashTrail_Smoothed.Count - 16;
			start = Math.Max(start, 0);
			for (int i = start; i < sEffect.SlashTrail_Smoothed.Count; i++)
			{
				Vector3 currentPos3D = sEffect.SlashTrail_Smoothed[i] + new Vector3(0, 0, CenterZ);
				Vector2 currentPos = Project(currentPos3D, ProjectionMatrix);
				Vector2 worldPos = Projectile.Center + currentPos;
				attackPolygon.Add(worldPos);
			}
			for (int i = start; i < sEffect.SlashTrail_Smoothed.Count; i++)
			{
				Vector3 currentPos3D_Inner = sEffect.SlashTrail_Smoothed[i] * 0.2f + new Vector3(0, 0, CenterZ);
				Vector2 currentPos_Inner = Project(currentPos3D_Inner, ProjectionMatrix);
				Vector2 worldPos_Inner = Projectile.Center + currentPos_Inner;
				attackPolygon.Add(worldPos_Inner);
			}

			if (MathUtils.IntersectsPolygonAABB(attackPolygon, targetHitbox.TopLeft(), targetHitbox.BottomRight()))
			{
				return true;
			}
		}
		return false;
	}

	public virtual void ProduceWaterRipples(Vector2 beamDims)
	{
		var shaderData = (WaterShaderData)Terraria.Graphics.Effects.Filters.Scene["WaterDistortion"].GetShader();
		float waveSine = 1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
		Vector2 ripplePos = Projectile.Center + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(CurrentWeaponTipPosition().ToRotation());
		Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
		shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, CurrentWeaponTipPosition().ToRotation());
	}

	public override void CutTiles()
	{
		DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
		var cut = new Terraria.Utils.TileActionAttempt(DelegateMethods.CutTiles);
		Vector2 beamStartPos = Projectile.Center;
		Vector2 beamEndPos = beamStartPos + CurrentWeaponTipPosition();
		Utils.PlotTileLine(beamStartPos, beamEndPos, Projectile.width * Projectile.scale, cut);
	}

	public void ScreenShake()
	{
		ShakerManager.AddShaker(Owner.Center + CurrentWeaponTipPosition(), new Vector2(0, -1).RotatedByRandom(MathHelper.TwoPi), 6, 0.8f, 16, 0.9f, 0.8f, 30);
	}
}