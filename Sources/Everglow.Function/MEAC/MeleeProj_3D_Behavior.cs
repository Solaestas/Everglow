using Everglow.Commons.Utilities;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Shaders;

namespace Everglow.Commons.MEAC;

public abstract partial class MeleeProj_3D : ModProjectile, IWarpProjectile_warpStyle2, IBloomProjectile
{
	public struct SlashEffect
	{
		public Queue<Vector3> SlashTrail;

		public List<Vector3> SlashTrail_Smoothed;

		public Queue<float> SlashFade;

		public float Timer;

		public int MaxTime;

		public int TrailMax;

		public int Direction;

		public bool Active;

		public float RotateSpeed;

		public float TrailFade;

		public Vector3 MainAxis;

		public Vector3 WeaponAxis;

		public Vector3 RotationAxis;
	}

	public List<SlashEffect> SlashEffects = new List<SlashEffect>();

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

	public Vector2 ScreenPositionOffset => Projectile.Center - Main.screenPosition;

	public float RadialDistance => SphericalCoordPos.X;

	public float PolarAngle => SphericalCoordPos.Y;

	public float AzimuthalAngle => SphericalCoordPos.Z;

	public float WeaponLength = 60;

	public int CurrentAttackType = 0;

	public float AttackTimer;

	public float MaxAttackTime = 60;

	public float RotateSpeed = 0;

	public float GetMeleeSpeed()
	{
		if (Owner is null || !Owner.active || Owner.dead)
		{
			return 1;
		}
		return Owner.GetAttackSpeed(DamageClass.Melee);
	}

	public override void OnSpawn(IEntitySource source)
	{
		EnableSphereCoordDraw = false;
	}

	public override void SetDefaults()
	{
		Projectile.hide = true;
		Projectile.aiStyle = -1;
		Projectile.height = Projectile.width = 120;
		Projectile.penetrate = -1;
	}

	public override void AI()
	{
		AttachWithOwner();
		CheckHeldItem();
		CenterZ = 1620;
		DevelopersAdjust();
		Update();
		HoldWeapon();
		ProduceWaterRipples(new Vector2(CurrentWeaponTipPosition().Length(), 30));
	}

	public void Update()
	{
		float meleeSpeed = GetMeleeSpeed();
		TrailSlashEffects();
		CurrentTrailFade = RotateSpeed * 3;
		RotateMainAxis(RotateSpeed * 0.35f, RotatedAxis, ref MainAxis);
		RotateSpeed *= SolveB(BaseMeleeSpeed * Owner.meleeSpeed);
		BindWeaponAxis();
		if (Owner.controlUseItem)
		{
			AttackTimer += meleeSpeed;
			if (AttackTimer > MaxAttackTime * 0.6f)
			{
				NewAttack();
			}
		}
	}

	public void NewAttack()
	{
		if(Main.MouseWorld.X > Owner.Center.X)
		{
			Owner.direction = 1;
		}
		else
		{
			Owner.direction = -1;
		}
		AttackTimer = 0;
		float meleeSpeed = Owner.meleeSpeed;
		Vector2 mouseDir = Main.MouseWorld - Owner.Center;
		mouseDir = mouseDir.SafeNormalize(Vector2.zeroVector).RotatedBy(MathHelper.PiOver2);
		RotatedAxis = new Vector3(mouseDir, Main.rand.NextFloat(0.2f, 1f));
		RotatedAxis = Vector3.Normalize(RotatedAxis);
		MainAxis = new Vector3((60 + WeaponLength) * Owner.direction, 0, 0);
		RotateToPerpendicular(RotatedAxis, ref MainAxis);
		RotateSpeed = (float)BaseMeleeSpeed * meleeSpeed * Owner.direction;
		CurrentTrailFade = 0;
		AddSlashEffect();
	}

	public void AddSlashEffect(int maxTime = 60, int trailMax = 60)
	{
		SlashEffect sEffect = new SlashEffect() { };
		sEffect.Active = true;
		sEffect.Timer = 0;
		sEffect.MaxTime = maxTime;
		sEffect.TrailMax = trailMax;
		sEffect.SlashFade = new Queue<float>();
		sEffect.SlashTrail = new Queue<Vector3>();
		sEffect.SlashTrail_Smoothed = new List<Vector3>();
		sEffect.TrailFade = 0;
		sEffect.MainAxis = MainAxis;
		sEffect.WeaponAxis = WeaponAxis;
		sEffect.RotateSpeed = RotateSpeed;
		sEffect.RotationAxis = RotatedAxis;
		sEffect.Direction = Owner.direction;
		SlashEffects.Add(sEffect);
	}

	public void TrailSlashEffects()
	{
		float meleeSpeed = GetMeleeSpeed();
		for (int k = SlashEffects.Count - 1; k >= 0; k--)
		{
			SlashEffect sEffect = SlashEffects[k];
			sEffect.Timer += meleeSpeed;
			if (sEffect.Timer > sEffect.MaxTime)
			{
				sEffect.Active = false;
			}
			sEffect.TrailFade = Math.Abs(sEffect.RotateSpeed * 3);
			RotateMainAxis(sEffect.RotateSpeed * 0.35f, sEffect.RotationAxis, ref sEffect.MainAxis);
			sEffect.RotateSpeed *= SolveB(BaseMeleeSpeed * meleeSpeed);
			sEffect.WeaponAxis = sEffect.MainAxis + Vector3.Normalize(sEffect.MainAxis) * WeaponLength;
			if (sEffect.SlashFade is null)
			{
				sEffect.SlashFade = new Queue<float>();
			}
			if (sEffect.SlashTrail is null)
			{
				sEffect.SlashTrail = new Queue<Vector3>();
			}
			if (sEffect.SlashTrail_Smoothed is null)
			{
				sEffect.SlashTrail_Smoothed = new List<Vector3>();
			}
			if (sEffect.Timer < sEffect.MaxTime)
			{
				sEffect.SlashFade.Enqueue(sEffect.TrailFade);
				sEffect.SlashTrail.Enqueue(sEffect.WeaponAxis);
			}
			if (sEffect.SlashFade.Count > sEffect.TrailMax)
			{
				sEffect.SlashFade.Dequeue();
			}
			if (sEffect.SlashTrail.Count > sEffect.TrailMax)
			{
				sEffect.SlashTrail.Dequeue();
			}
			if (sEffect.SlashTrail.Count > 3)
			{
				sEffect.SlashTrail_Smoothed = GraphicsUtils.Smooth(sEffect.SlashTrail);
			}
			SlashEffects[k] = sEffect;
			if (!sEffect.Active)
			{
				SlashEffects.RemoveAt(k);
			}
		}
	}

	public void DevelopersAdjust()
	{
		if (Main.mouseLeft && Main.mouseLeftRelease)
		{
			NewAttack();
		}
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
		RotateMainAxis(value * 0.35f, RotatedAxis, ref MainAxis);
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

	public virtual void BindWeaponAxis()
	{
		WeaponAxis = MainAxis + MainAxisDirection * WeaponLength;
	}
}