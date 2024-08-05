using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class AuburnBellSummon : ModProjectile
{
	private const float MaxSpeed = 25f;
	private const float Acceleration = 0.05f;
	private const float Deceleration = 0.95f;
	private const float HitDistance = 50f;
	private const float MinSpeed = 0.1f;
	private const float RotationSpeed = 0.1f;

	private const float MinInitialDashSpeed = 8f;
	private const int DashTimeLimit = 60;

	private float ZigzagAmplitude = 10f; // Amplitude of the zigzag motion
	private float ZigzagFrequency = 1f; // Frequency of the zigzag motion
	private float ZigzagTime; // To track the elapsed time for the zigzag motion

	private float DashTime { get; set; }

	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 8;
	}

	public override void SetDefaults()
	{
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 6;
		Projectile.timeLeft = 720;
		Projectile.minionSlots = 1;
		Projectile.penetrate = -1;
		Projectile.aiStyle = -1;
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.DamageType = DamageClass.Summon;
		Projectile.minion = true;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
	}

	/// <summary>
	/// Target
	/// </summary>
	internal int TargetWhoAmI { get; set; } = -1;

	/// <summary>
	/// Attack Cooldown
	/// </summary>
	internal int AttackPhase { get; set; } = 0;

	/// <summary>
	/// Teleport Cooldown
	/// </summary>
	internal int TeleportCooldown { get; set; } = 0;

	private void UpdateProjectileLifecycle()
	{
		// Get player who owns this projectile
		Player player = Main.player[Projectile.owner];

		// Check if player is dead or not active in the game
		if (player.dead || player.active is not true)
		{
			player.ClearBuff(ModContent.BuffType<AuburnBell>());
			Projectile.Kill();
			return;
		}

		// Check if the player has the AuburnBell buff
		if (player.HasBuff(ModContent.BuffType<AuburnBell>()))
		{
			// If player has the [Auburn Bell] buff,
			// keep the projectile alive by resetting its time left
			Projectile.timeLeft = 2;
		}
		else
		{
			// If player does not have the [Auburn Bell] buff,
			// kill the projectile
			Projectile.Kill();
		}
	}

	private void TelePortTo(Vector2 aim)
	{
		TeleportCooldown = 60;
		Projectile.Center = aim;
		for (int f = 0; f < 15; f++)
		{
			var g = Gore.NewGoreDirect(
				null,
				aim,
				new Vector2(0, Main.rand.NextFloat(10f)).RotatedByRandom(6.283),
				0,
				Main.rand.NextFloat(0.65f, Main.rand.NextFloat(2.5f, 3.75f)));
			g.timeLeft = Main.rand.Next(250, 500);
		}
	}

	private void MoveTo(Vector2 aim, float speedValue = 0.1f)
	{
		Vector2 v = aim - Projectile.velocity - Projectile.Center;
		float vVal = v.Length();
		if (vVal > 100f)
		{
			vVal = 100f;
		}

		Projectile.velocity = v.SafeNormalize(Vector2.Zero) * vVal * speedValue;
	}

	private void FindEnemies()
	{
		Player player = Main.player[Projectile.owner];
		Vector2 detectCenter = player.Center;
		if (Projectile.ai[0] > 5)
		{
			detectCenter = Projectile.Center;
		}

		float minDistance = 1600;
		foreach (NPC npc in Main.npc)
		{
			if (npc.active)
			{
				if (npc.whoAmI == player.MinionAttackTargetNPC)
				{
					TargetWhoAmI = npc.whoAmI;
					return;
				}
				if (!npc.dontTakeDamage && !npc.friendly && npc.CanBeChasedBy() && Collision.CanHit(Projectile, npc))
				{
					if ((npc.Center - detectCenter).Length() < minDistance)
					{
						TargetWhoAmI = npc.whoAmI;
						return;
					}
				}
			}
		}
	}

	private void CheckTargetActive()
	{
		if (TargetWhoAmI > -1)
		{
			if (Main.npc[TargetWhoAmI].active is false || Main.npc[TargetWhoAmI].dontTakeDamage)
			{
				TargetWhoAmI = -1;
				AttackPhase = 0;
			}
		}
	}

	private void Attack()
	{
		NPC target;

		// Valid Target
		if (TargetWhoAmI >= 0)
		{
			target = Main.npc[TargetWhoAmI];
		}
		else
		{
			TargetWhoAmI = -1;
			return;
		}

		Vector2 directionToTarget = target.Center - Projectile.Center;
		float distanceToTarget = directionToTarget.Length();
		float angleToTarget = MathF.Atan2(directionToTarget.Y, directionToTarget.X);

		if (AttackPhase == 0) // Phase 1: Aiming at the target
		{
			if (Math.Abs(directionToTarget.Y / directionToTarget.X) > MathF.Tan(MathF.PI / 4)) // Check if the target angle exceeds 45° pitch angle; if so, move in the Y direction towards the target
			{
				// Apply zigzag motion while moving towards the target's Y position
				ZigzagTime += ZigzagFrequency;
				float zigzagOffset = MathF.Sin(ZigzagTime) * ZigzagAmplitude;

				if (directionToTarget.Y >= 0)
				{
					MoveTo(new Vector2(Projectile.Center.X + zigzagOffset, target.position.Y));
				}
				else
				{
					MoveTo(new Vector2(Projectile.Center.X + zigzagOffset, target.position.Y));
				}

				Projectile.rotation += MathF.Sign(0 - Projectile.rotation) * RotationSpeed;
			}
			else if (Math.Abs(MathF.Tan(Projectile.rotation - angleToTarget)) > 0.01f) // Adjust to the direction of the target
			{
				Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);

				float currentRotation = Projectile.rotation;

				float rotationDifference;
				if (directionToTarget.X >= 0)
				{
					// Calculate the shortest rotation difference to the target
					rotationDifference = MathHelper.WrapAngle(angleToTarget - currentRotation);
				}
				else
				{
					// Calculate the shortest rotation difference to the target, adjusted for left side
					rotationDifference = MathHelper.WrapAngle(angleToTarget - currentRotation - MathF.PI);
				}

				// Rotate towards the target by RotationSpeed
				Projectile.rotation += MathF.Sign(rotationDifference) * RotationSpeed;

				// If the rotation difference is less than RotationSpeed, set the rotation directly to avoid overshooting
				if (Math.Abs(rotationDifference) < RotationSpeed)
				{
					if (directionToTarget.X >= 0)
					{
						Projectile.rotation = angleToTarget;
					}
					else
					{
						Projectile.rotation = MathHelper.WrapAngle(angleToTarget - MathF.PI);
					}
				}
			}
			else
			{
				// Reset dash timer
				DashTime = 0;

				// Transition to the next phase
				AttackPhase = 1;
			}
		}
		else if (AttackPhase == 1) // Phase 2: Lock on to the target and gradually accelerate
		{
			DashTime++;

			if (Projectile.velocity.Length() < MinInitialDashSpeed)
			{
				Projectile.velocity = directionToTarget.SafeNormalize(Vector2.Zero) * MinInitialDashSpeed;
			}

			// Gradually accelerate
			if (Projectile.velocity.Length() < MaxSpeed)
			{
				// Increase the velocity by Acceleration factor while maintaining the direction towards the target
				Projectile.velocity += directionToTarget.SafeNormalize(Vector2.Zero) * Acceleration;
				if (Projectile.velocity.Length() > MaxSpeed)
				{
					Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * MaxSpeed;
				}
			}

			// Check if the projectile hits the target, is close to the target, or has been dashing for a certain duration
			if ((target.Center - Projectile.Center).Length() < HitDistance || DashTime > DashTimeLimit)
			{
				// Reset dash timer
				DashTime = 0;

				// Transition to the next phase
				AttackPhase = 2;
			}
		}
		else if (AttackPhase == 2) // Phase 3: Gradually decelerate after hitting the target
		{
			// Gradually decelerate
			Projectile.velocity *= Deceleration;

			// If the projectile has come to a complete stop, transition back to the aiming phase
			if (Projectile.velocity.Length() < MinSpeed)
			{
				// Transition to the next phase
				AttackPhase = 0;
			}
		}
		else // Reset Attack Phase
		{
			AttackPhase = 0;
		}
	}

	private void FlyToPlayer()
	{
		Player player = Main.player[Projectile.owner];
		float speed;
		Vector2 aim;

		if ((Projectile.Center - player.MountedCenter).Length() > 5
			&& player.velocity.Length() > 1E-05f)
		{
			speed = 0.5f;
			aim = player.MountedCenter
				+ new Vector2(
					x: (10 - Projectile.ai[0] * 30) * player.direction,
					y: -40 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f - Projectile.ai[0]) * 35f);

			if ((Projectile.Center - aim).Length() > 500f)
			{
				speed = (Projectile.Center - player.Center).Length() / 1000f;
			}
		}
		else
		{
			speed = 0.1f;
			aim = player.MountedCenter
				+ new Vector2(
					x: player.direction * (MathF.Sin((float)Main.timeForVisualEffects * 0.02f) * 40f - Projectile.ai[0] * 30),
					y: -50 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f) * 20f);
		}

		MoveTo(aim, speed);
		Projectile.rotation = Projectile.rotation * 0.95f + Projectile.velocity.X * 0.002f;
	}

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Projectile.hide = true;
		UpdateProjectileLifecycle();

		// Manage distance to owner
		if (TeleportCooldown > 0)
		{
			TeleportCooldown--;
		}
		else
		{
			if ((player.Center - Projectile.Center).Length() > 2700f)
			{
				TelePortTo(player.MountedCenter + new Vector2((10 - Projectile.ai[0] * 30) * player.direction, -40 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f - Projectile.ai[0]) * 35f));
			}
		}

		// Attack actions
		if (TargetWhoAmI <= -1)
		{
			FlyToPlayer();
			FindEnemies();
		}
		else // Has Target
		{
			CheckTargetActive();
			Attack();
		}
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCsAndTiles.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		int BodyLength = 12;

		SpriteBatch spriteBatch = Main.spriteBatch;

		Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(
			SpriteSortMode.Deferred,
			BlendState.NonPremultiplied,
			Main.DefaultSamplerState,
			DepthStencilState.None,
			RasterizerState.CullNone,
			null,
			Main.GameViewMatrix.TransformationMatrix);

		List<Vertex2D> bars = new List<Vertex2D>();
		Vector2 drawCenter = Projectile.Center - Main.screenPosition + new Vector2(BodyLength * 5f * 0.75f, 0).RotatedBy(Projectile.rotation) * Projectile.scale;
		for (int i = 0; i < BodyLength; i++)
		{
			Color drawColor = lightColor;
			float jointIndex = i / (float)BodyLength;
			int frameY = (int)(Projectile.frame + i) % Main.projFrames[Projectile.type];
			float frameYValue = frameY / (float)Main.projFrames[Projectile.type];
			float deltaYValue = 1 / (float)Main.projFrames[Projectile.type];
			float jointScale = 0.5f + 0.5f * MathF.Sin(jointIndex * MathHelper.Pi);

			bars.Add(
				drawCenter + new Vector2(-20, -20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(0, frameYValue, 0));
			bars.Add(
				drawCenter + new Vector2(-20, 20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(0, frameYValue + deltaYValue, 0));
			bars.Add(
				drawCenter + new Vector2(20, -20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(1, frameYValue, 0));
			bars.Add(
				drawCenter + new Vector2(20, -20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(1, frameYValue, 0));
			bars.Add(
				drawCenter + new Vector2(-20, 20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(0, frameYValue + deltaYValue, 0));
			bars.Add(
				drawCenter + new Vector2(20, 20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(1, frameYValue + deltaYValue, 0));

			drawCenter -= new Vector2(10, 0).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale;
		}

		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = texture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count / 3);
		}

		spriteBatch.End();
		spriteBatch.Begin(sBS);

		// Loop frame count.
		Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];

		return false;
	}
}