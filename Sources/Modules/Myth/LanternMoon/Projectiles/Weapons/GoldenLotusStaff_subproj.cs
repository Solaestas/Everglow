using Everglow.Myth.LanternMoon.VFX;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.Weapons;

public class GoldenLotusStaff_subproj : ModProjectile
{
	public Player Owner;

	public Projectile ParentProj;

	public int MaxTime;

	public int DashTimer = 0;

	public int Timer;

	public Queue<Vector3> OldPosAndRot = new Queue<Vector3>();

	public override void SetDefaults()
	{
		Projectile.aiStyle = -1;
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 6000;
		Projectile.hostile = false;
		Projectile.friendly = true;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 10;
		Timer = 0;
	}

	public override void OnSpawn(IEntitySource source)
	{
		base.OnSpawn(source);
	}

	public override void AI()
	{
		OldPosAndRot.Enqueue(new Vector3(Projectile.Center, Projectile.rotation));
		if (OldPosAndRot.Count > 6)
		{
			OldPosAndRot.Dequeue();
		}
		Timer++;
		if (Owner is null)
		{
			if (Projectile.owner >= 0 && Projectile.owner < Main.player.Length)
			{
				Owner = Main.player[Projectile.owner];
			}
			else
			{
				Projectile.Kill();
				return;
			}
		}
		bool canChaseTarget = true;
		if (Timer < 60)
		{
			canChaseTarget = false;
			Projectile.velocity *= 0;
			float index = AllocateIndex();
			Projectile.rotation = index / 7f * MathHelper.TwoPi + MathF.Pow(Timer / 60f, 2) * 4;
		}
		if (Timer == 60)
		{
			canChaseTarget = false;
			Projectile.velocity = new Vector2(20, 0).RotatedBy(Projectile.rotation);
		}
		if (Timer is > 60 and < 120)
		{
			canChaseTarget = false;
			float rotValue = (120 - Timer) / 120f * 0.05f;
			Projectile.velocity = Projectile.velocity.RotatedBy(rotValue) * 0.96f;
			Projectile.rotation = Projectile.velocity.ToRotationSafe();
		}
		if (Timer > MaxTime - 120 && Timer < MaxTime)
		{
			canChaseTarget = false;
			Vector2 targetPos = ParentProj.Center;
			Vector2 toTarget = targetPos - Projectile.Center - Projectile.velocity;
			float index = AllocateIndex();
			if (toTarget.Length() < 300)
			{
				toTarget = toTarget / 10f;
			}
			else
			{
				toTarget = toTarget.NormalizeSafe() * 30;
			}
			if (toTarget.Length() > 6)
			{
				Projectile.velocity = Projectile.velocity * 0.95f + toTarget * 0.05f;
				Projectile.rotation = Projectile.velocity.ToRotationSafe();
			}
			else
			{
				Projectile.Center = ParentProj.Center;
				Projectile.velocity *= 0;
				Projectile.rotation = index / 7f * MathHelper.TwoPi + MathF.Pow(Timer / 60f, 2) * 20;
			}
		}
		if (Timer >= MaxTime)
		{
			Projectile.Kill();
		}
		if (canChaseTarget)
		{
			NPC target = Projectile.FindTargetWithinRange(800);
			if (target is null)
			{
				IdleMove();
			}
			else
			{
				ChaseTarget(target);
			}
		}
	}

	public void ChaseTarget(NPC target)
	{
		var targetPos = target.Center;
		Vector2 toNPCTarget = targetPos - Projectile.Center - Projectile.velocity;
		if (toNPCTarget.Length() > 100 + Projectile.whoAmI * 2)
		{
			Vector2 targetVel = toNPCTarget.NormalizeSafe() * 32f;
			Projectile.velocity = targetVel * 0.05f + Projectile.velocity * 0.95f;
		}
		else
		{
			DashTimer++;
			if (DashTimer == 1)
			{
				Projectile.velocity = toNPCTarget.NormalizeSafe() * (60f + Projectile.whoAmI);
				var star = new FlashTrajectory
				{
					Active = true,
					Visible = true,
					Position = Projectile.Center - Projectile.velocity.NormalizeSafe() * 10f,
					Rotation = Projectile.velocity.ToRotationSafe() - MathHelper.PiOver2,
					MaxTime = Main.rand.Next(20, 30),
					Distance = 320 + Projectile.whoAmI * 7,
				};
				Ins.VFXManager.Add(star);
			}
			if (DashTimer < 15)
			{
				Vector2 targetVel = toNPCTarget.NormalizeSafe() * 1f;
				Projectile.velocity = targetVel * 0.4f + Projectile.velocity * 0.6f;
			}
			else
			{
				DashTimer = 0;
			}
		}
		Projectile.rotation = Projectile.velocity.ToRotationSafe();
	}

	public void IdleMove()
	{
		DashTimer = 0;
		Vector2 twistedRingTrajecory = new Vector2(240 * MathF.Sin(Timer * 0.05f + Projectile.whoAmI), 90 * MathF.Sin(Timer * 0.025f) - 160) + Owner.Center;
		Vector2 toTarget = twistedRingTrajecory - Projectile.Center - Projectile.velocity;
		if (toTarget.Length() > 12)
		{
			toTarget.Normalize();
			toTarget *= 12;
		}
		Projectile.velocity = toTarget * 0.05f + Projectile.velocity * 0.95f;
		Projectile.rotation = Projectile.velocity.ToRotationSafe();
	}

	public int AllocateIndex()
	{
		int index = 0;
		foreach (var proj in Main.projectile)
		{
			if (proj is not null && proj.active && proj.type == Type && proj.whoAmI < Projectile.whoAmI)
			{
				GoldenLotusStaff_subproj gLSsp = proj.ModProjectile as GoldenLotusStaff_subproj;
				if (gLSsp is not null && gLSsp.ParentProj == ParentProj)
				{
					index++;
				}
			}
		}
		return index;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		for (int g = 0; g < 8; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(4f, 10f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new HitEffectSpark
			{
				Velocity = newVelocity,
				Active = true,
				Visible = true,
				Position = Projectile.Center,
				MaxTime = Main.rand.Next(12, 16),
				DrawColor = new Color(0.8f, 0.8f, 0, 0),
				LightFlat = 0f,
				SpeedDecay = 0.9f,
				GravityAcc = 0.0f,
				SelfLight = false,
				Scale = Main.rand.NextFloat(10f, 20f),
			};
			Ins.VFXManager.Add(spark);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
		Color drawColor;
		for (int i = 0; i < OldPosAndRot.Count; i++)
		{
			Vector3 pos = OldPosAndRot.ToArray()[i];
			drawColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
			Main.EntitySpriteDraw(tex, new Vector2(pos.X, pos.Y) - Main.screenPosition, null, drawColor * (i / (float)OldPosAndRot.Count) * 0.25f, pos.Z, new Vector2(4, tex.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
		}
		drawColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
		Texture2D bloom = ModAsset.GoldenLotusStaff_subproj_bloom.Value;
		if (DashTimer > 0 && DashTimer < 12)
		{
			float value = 1 - DashTimer / 12f;
			Main.EntitySpriteDraw(bloom, Projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0) * value, Projectile.rotation, new Vector2(11, bloom.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
		}
		Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation, new Vector2(4, tex.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}