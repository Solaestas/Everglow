using Everglow.Commons.Utilities;
using Terraria.DataStructures;

namespace Everglow.Commons.MEAC;

public abstract partial class MeleeProj_3D : ModProjectile, IWarpProjectile_warpStyle2, IBloomProjectile
{
	public int WeaponItemType;

	public int MaxTrail = 30;

	public float CenterZ = 200;

	/// <summary>
	/// MainAxis will spin around this.<br/>
	/// Suggest to set this perpendicular to the MainAxis.
	/// </summary>
	public Vector3 RotatedAxis = new Vector3(0, -1, 0);

	public Vector3 SphericalCoordPos => CartesianToSpherical(MainAxis);

	public Vector3 MainAxisWithDepth => MainAxis + new Vector3(0, 0, CenterZ);

	public Vector3 MainAxis = new Vector3(150, 0, 0);

	public Vector3 WeaponAxis = new Vector3(150, 0, 0);

	public Vector3 MainAxisDirection => Vector3.Normalize(MainAxis);

	public float RadialDistance => SphericalCoordPos.X;

	public float PolarAngle => SphericalCoordPos.Y;

	public float AzimuthalAngle => SphericalCoordPos.Z;

	public Vector2 ScreenPositionOffset => Projectile.Center - Main.screenPosition;

	public Queue<Vector3> OldArrowTips = new Queue<Vector3>();

	public List<Vector3> OldArrowTips_Smoothed = new List<Vector3>();

	public int CurrentAttackType = 0;

	public float AttackTimer;

	public float CurrentAttackSpeed;

	public float RotateSpeed = 0;

	public override void OnSpawn(IEntitySource source)
	{
		EnableSphereCoordDraw = false;
	}

	public override void SetDefaults()
	{
		Projectile.hide = true;
		Projectile.aiStyle = -1;
		Projectile.height = Projectile.width = 120;
	}

	public override void AI()
	{
		AttachWithOwner();
		CheckHeldItem();
		CenterZ = 1620;
		DevelopersAdjust();
		Trail();
		AmendCoord();
	}

	public void Trail()
	{
		if(RotateSpeed <= 0.005f)
		{
			OldTrailFade.Clear();
			OldArrowTips.Clear();
		}
		OldTrailFade.Enqueue(CurrentTrailFade);
		if (OldTrailFade.Count > MaxTrail)
		{
			OldTrailFade.Dequeue();
		}
		OldArrowTips.Enqueue(WeaponAxis);
		if (OldArrowTips.Count > MaxTrail)
		{
			OldArrowTips.Dequeue();
		}
		OldArrowTips_Smoothed = GraphicsUtils.Smooth(OldArrowTips);
	}

	public void DevelopersAdjust()
	{
		//float rotSpeed = 0.03f;
		//if (Owner.controlUp)
		//{
		//	SphericalCoordPos.Y += rotSpeed;
		//}
		//if (Owner.controlDown)
		//{
		//	SphericalCoordPos.Y -= rotSpeed;
		//}
		//if (Owner.controlLeft)
		//{
		//	SphericalCoordPos.Z += rotSpeed;
		//}
		//if (Owner.controlRight)
		//{
		//	SphericalCoordPos.Z -= rotSpeed;
		//}
		// RotatedByMouseAxis();
		if(Main.mouseLeft && Main.mouseLeftRelease)
		{
			RotatedAxis = new Vector3(Main.rand.NextFloat(-1, 1), -1 / 3f, Main.rand.NextFloat(-1, 1));
			RotatedAxis = Vector3.Normalize(RotatedAxis);
			RotateToPerpendicular(RotatedAxis, ref MainAxis);
			RotateSpeed = 2.5f;
			CurrentTrailFade = 0;
		}

		CurrentTrailFade = RotateSpeed * 3;
		RotateMainAxis(RotateSpeed * 0.35f);
		BindWeaponAxis();
		RotateSpeed *= 0.88f;
	}

	public void FollowMouse()
	{
		Vector3 toMouse = new Vector3(Main.MouseScreen - new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f, 0) - new Vector3(0, 0, CenterZ * 0.1f);
		MainAxis = Vector3.Normalize(toMouse) * 240f;
	}

	public void RotatedByMouseAxis()
	{
		float value = (MathF.Sin((float)Main.time * 0.13f) + 1) / 2f;
		value = MathF.Pow(value, 3) + 0.5f;

		Vector3 toMouse = new Vector3(Main.MouseScreen - new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f, 0) - new Vector3(0, 0, CenterZ * 0.1f);
		RotatedAxis = Vector3.Normalize(toMouse) * 120f;
		RotateMainAxis(value * 0.35f);
	}

	public virtual void CheckHeldItem()
	{
		if (Owner.HeldItem.type == WeaponItemType)
		{
			if (Projectile.timeLeft < 5)
			{
				Projectile.timeLeft = 5;
			}
		}
	}

	public virtual void AttachWithOwner()
	{
		if (Owner is null || !Owner.active || Owner.dead)
		{
			Projectile.Kill();
		}
		Projectile.Center = Owner.Center;
		Projectile.velocity *= 0;
	}

	public virtual void AmendCoord()
	{
		//if(SphericalCoordPos.Y < 0)
		//{
		//	SphericalCoordPos.Y = MathF.Abs(SphericalCoordPos.Y);
		//}
		//if(SphericalCoordPos.Y > MathHelper.Pi)
		//{
		//	SphericalCoordPos.Y = MathHelper.TwoPi - SphericalCoordPos.Y;
		//}
		//if(SphericalCoordPos.Z > MathHelper.Pi)
		//{
		//	SphericalCoordPos.Z -= MathHelper.TwoPi;
		//}
		//if (SphericalCoordPos.Z < -MathHelper.Pi)
		//{
		//	SphericalCoordPos.Z += MathHelper.TwoPi;
		//}
	}

	public virtual void BindWeaponAxis()
	{
		WeaponAxis = MainAxis + MainAxisDirection * 120f;
	}
}