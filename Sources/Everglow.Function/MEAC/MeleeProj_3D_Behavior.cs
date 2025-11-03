using Everglow.Commons.Utilities;
using Terraria.DataStructures;

namespace Everglow.Commons.MEAC;

public abstract partial class MeleeProj_3D : ModProjectile, IWarpProjectile_warpStyle2, IBloomProjectile
{
	public int WeaponItemType;

	public int MaxTrail = 30;

	public override void OnSpawn(IEntitySource source)
	{
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
		OldArrowTips.Enqueue(SpacePosition);
		if (OldArrowTips.Count > MaxTrail)
		{
			OldArrowTips.Dequeue();
		}
		OldArrowTips_Smoothed = GraphicsUtils.Smooth(OldArrowTips);
	}

	public void DevelopersAdjust()
	{
		float rotSpeed = 0.03f;
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
		RotatedByMouseAxis();

		//FollowMouse();
	}

	public void FollowMouse()
	{
		Vector3 toMouse = new Vector3(Main.MouseScreen - new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f, 0) - new Vector3(0, 0, CenterZ * 0.1f);
		SpacePosition = Vector3.Normalize(toMouse) * 240f;
	}

	public void RotatedByMouseAxis()
	{
		float value = (MathF.Sin((float)Main.time * 0.13f) + 1) / 2f;
		value = MathF.Pow(value, 3) + 0.5f;

		Vector3 toMouse = new Vector3(Main.MouseScreen - new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f, 0) - new Vector3(0, 0, CenterZ * 0.1f);
		RotatedAxis = Vector3.Normalize(toMouse) * 120f;
		Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.Normalize(RotatedAxis), value * 0.35f);
		SpacePosition = Vector3.Transform(SpacePosition, rotation);
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
}