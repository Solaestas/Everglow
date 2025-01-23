using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EvilMusicRemnant_Minion : ModProjectile
{
	public enum Minion_MainState
	{
		Spawn,
		Action,
		Kill,
	}

	public enum Minion_ActionState
	{
		Patrol,
		Chase,
		Attack,
	}

	public const int SpawnDuration = 120;
	public const int TimeLeftMax = 300;
	public const int SearchDistance = 500;
	public const int VelocityMax = 10;

	public Minion_MainState MainState { get; set; } = Minion_MainState.Spawn;

	public Minion_ActionState ActionState { get; set; } = Minion_ActionState.Patrol;

	public Player Owner => Main.player[Projectile.owner];

	public int TargetWhoAmI
	{
		get => (int)Projectile.ai[0];
		set => Projectile.ai[0] = value;
	}

	public float SpawnProgress => MathF.Min(1f, (TimeLeftMax - Projectile.timeLeft) / (float)SpawnDuration);

	public override void SetDefaults()
	{
		Projectile.width = 48;
		Projectile.height = 56;

		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.timeLeft = TimeLeftMax;

		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.minionSlots = 1;

		TargetWhoAmI = -1;
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.velocity.Y = -0.5f;
	}

	public override void AI()
	{
		if(Projectile.timeLeft < 60)
		{
			Projectile.timeLeft = 60;
		}

		if (MainState == Minion_MainState.Spawn)
		{
			GenerateSpawnMask();
			if (SpawnProgress > 0.6f)
			{
				Projectile.velocity = Vector2.Zero;
			}

			if (TimeLeftMax - Projectile.timeLeft > SpawnDuration)
			{
				if (Owner.slotsMinions <= Owner.maxMinions)
				{
					MainState = Minion_MainState.Action;
				}
				else
				{
					MainState = Minion_MainState.Kill;
				}
			}
		}
		else if (MainState == Minion_MainState.Action)
		{
			// Action
			Action();

			if (Projectile.timeLeft < 60)
			{
				MainState = Minion_MainState.Kill;
			}
		}
		else
		{
			// Kill
			for (int i = 0; i < 30; i++)
			{
				Dust.NewDust(Projectile.Center, 1, 1, DustID.FlameBurst, 0, 0);
			}
			Projectile.Kill();
		}
	}

	private void GenerateSpawnMask()
	{
		for (int i = 0; i < 4; i++)
		{
			float size = Main.rand.NextFloat(0.1f, 0.96f);
			var lotusFlame = new CyanLotusFlameDust
			{
				Velocity = new Vector2(0, Main.rand.NextFloat(0.3f, 1f)).RotatedByRandom(MathHelper.TwoPi) * 0.8f,
				Active = true,
				Visible = true,
				Position = Projectile.Center,
				MaxTime = Main.rand.Next(24, 36) * 6 * (1 - SpawnProgress),
				Scale = 14f * size,
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
				Frame = Main.rand.Next(3),
				ai = new float[] { Main.rand.NextFloat(-0.8f, 0.8f) },
			};
			Ins.VFXManager.Add(lotusFlame);
		}
	}

	private void Action()
	{
		if (TargetWhoAmI < 0)
		{
			var targetWhoAmI = ProjectileUtils.FindTarget(Projectile.Center, SearchDistance);
			if (targetWhoAmI >= 0)
			{
				TargetWhoAmI = targetWhoAmI;
				ActionState = Minion_ActionState.Chase;
				Projectile.netUpdate = true;
			}
		}

		if (!ProjectileUtils.MinionCheckTargetActive(TargetWhoAmI))
		{
			TargetWhoAmI = -1;
			ActionState = Minion_ActionState.Patrol;
			Projectile.netUpdate = true;
		}

		if (ActionState == Minion_ActionState.Chase)
		{
			var target = Main.npc[TargetWhoAmI];
			var directionToTarget = (target.Center - Projectile.Center).NormalizeSafe();
			Projectile.velocity = MathUtils.Lerp(0.1f, Projectile.velocity, directionToTarget * VelocityMax);
		}
		else if (ActionState == Minion_ActionState.Attack)
		{
			var target = Main.npc[TargetWhoAmI];
			var directionToTarget = (target.Center - Projectile.Center).NormalizeSafe();
			Projectile.velocity = MathUtils.Lerp(0.1f, Projectile.velocity, directionToTarget * VelocityMax);
		}
		else
		{
			Vector2 aim;
			const float NotMovingVelocity = 1E-05f;
			if (Owner.velocity.Length() > NotMovingVelocity) // Player is moving
			{
				aim = Owner.MountedCenter
					+ new Vector2(
						x: (10 - 1 * 30) * Owner.direction,
						y: -40 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f - 1) * 35f);
			}
			else
			{
				aim = Owner.MountedCenter
					+ new Vector2(
						x: Owner.direction * (MathF.Sin((float)Main.timeForVisualEffects * 0.02f) * 40f - 1 * 30),
						y: -50 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f) * 20f);
			}

			float timeValue = (float)(Main.timeForVisualEffects * 0.014f);

			Projectile.velocity *= 0.97f;

			var newRotation = MathF.Log(MathF.Abs(Projectile.velocity.X) + 1) * 0.2f * Projectile.direction;
			Projectile.rotation = Projectile.rotation * 0.95f + newRotation * 0.05f;
			int dirY = Projectile.velocity.Y >= 0 ? 1 : -1;
			Vector2 aimPosition =
				aim +
				new Vector2(
					210f * MathF.Sin(timeValue * 2 + Projectile.whoAmI) * Projectile.direction + 1,
					(-50 + 1 + 30f * MathF.Sin(timeValue * 0.15f + Projectile.whoAmI)) * dirY)
				* Projectile.scale;
			Vector2 toAim = aimPosition - Projectile.Center - Projectile.velocity;
			if (toAim.Length() > 50)
			{
				Projectile.velocity += Vector2.Normalize(toAim) * 0.15f * Projectile.scale;
			}
			Projectile.rotation = Projectile.rotation * 0.95f + Projectile.velocity.X * 0.002f;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var drawColor = Color.White * SpawnProgress * 0.8f;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, drawColor, 0, texture.Size() / 2, Projectile.scale * 0.6f, SpriteEffects.None, 0);
		return false;
	}
}