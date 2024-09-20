using Everglow.Yggdrasil.YggdrasilTown.VFXs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

internal class FeatheredStaff : ModProjectile
{
	private const int MaxCollisionCount = 2;

	private const int Phase1Duration = 90;
	private const float Phase1FOVDistance = 500;
	private const float Phase1FOVAngle = MathF.PI / 2.0f;
	private const float Phase1RotationSpeed = 0.02f;

	private const float Phase2VelocityLimitY = 10f;
	private const float Phase2Gravity = 0.2f;

	private int TargetWhoAmI { get; set; } = -1;

	private bool HasNotBursted { get; set; } = true;

	private int CollisionCounter
	{
		get
		{
			return (int)Projectile.ai[0];
		}

		set
		{
			Projectile.ai[0] = value;
		}
	}

	private int LifeTimer
	{
		get
		{
			return (int)Projectile.ai[1];
		}

		set
		{
			Projectile.ai[1] = value;
		}
	}

	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 8;
	}

	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = 0;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = 3;
		Projectile.timeLeft = 300;
		Projectile.tileCollide = true;

		CollisionCounter = 0;
		LifeTimer = 0;
	}

	public override void AI()
	{
		if (Projectile.velocity.Y > 10f)
		{
			Projectile.velocity.Y = 10f;
		}

		if (LifeTimer < Phase1Duration)
		{
			Phase1AI();
		}
		else
		{
			Phase2AI();
		}

		LifeTimer++;
	}

	// Phase 1: No gravity, track enemy within the field of view (a sector)
	// --------------------------------------------------------------------
	private void Phase1AI()
	{
		CheckTargetActive();

		if (TargetWhoAmI == -1)
		{
			FindEnemy();
			return;
		}
		else // track enemy
		{
			NPC npc = Main.npc[TargetWhoAmI];

			Vector2 directionToTarget = npc.Center - Projectile.Center;
			directionToTarget.Normalize();

			var angleToTarget = MathHelper.WrapAngle(
				directionToTarget.ToRotation() -
				Projectile.velocity.ToRotation());

			if (Math.Abs(angleToTarget) > Phase1RotationSpeed)
			{
				// Rotate by the rotation speed
				if (angleToTarget > 0)
				{
					Projectile.velocity = Projectile.velocity.RotatedBy(Phase1RotationSpeed);
				}
				else
				{
					Projectile.velocity = Projectile.velocity.RotatedBy(-Phase1RotationSpeed);
				}
			}
			else
			{
				// Rotate directly to the target direction
				Projectile.velocity.RotatedBy(angleToTarget);
			}
		}
	}

	public void GenerateParticles(int duplicateTimes = 1)
	{
		for (int i = 0; i < duplicateTimes; i++)
		{
			Vector2 newVelocity = Projectile.velocity.NormalizeSafe() * 1.5f;
			var somg = new DarkGlimmeringParticleDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + Projectile.velocity * i / (float)duplicateTimes,
				maxTime = Main.rand.Next(27, 66),
				scale = 1,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 7, (float)(Main.time + i / (float)duplicateTimes) * 1.8f },
			};
			Ins.VFXManager.Add(somg);
		}
	}

	public void GenerateParticlesExposion(int duplicateTimes = 1)
	{
		for (int i = 0; i < duplicateTimes; i++)
		{
			float speed = MathF.Pow(Main.rand.NextFloat(1f), 0.15f) * 5f;
			Vector2 newVelocity = new Vector2(0, speed).RotatedByRandom(MathHelper.TwoPi);
			var somg = new DarkGlimmeringParticleDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(27, 36),
				scale = 1,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 5, (float)Main.time * 1.8f - speed * 2.4f },
			};
			Ins.VFXManager.Add(somg);
		}
	}

	// Phase 2: Gravity gradually increase to max value
	// ------------------------------------------------
	private void Phase2AI()
	{
		if (HasNotBursted)
		{
			GenerateParticlesExposion(50);

			Gore.NewGore(Projectile.Center, Projectile.velocity, ModContent.Find<ModGore>("Everglow/FeatheredStaff_gore0").Type, Projectile.scale);
			Gore.NewGore(Projectile.Center, Projectile.velocity, ModContent.Find<ModGore>("Everglow/FeatheredStaff_gore1").Type, Projectile.scale);

			HasNotBursted = false;
		}
		GenerateParticles(5);
		Projectile.velocity *= 0.99f;

		// Gravity
		Projectile.velocity.Y =
			Projectile.velocity.Y + Phase2Gravity < Phase2VelocityLimitY ?
			Projectile.velocity.Y + Phase2Gravity :
			Phase2VelocityLimitY;
	}

	private void FindEnemy()
	{
		float minDistance = Phase1FOVDistance + 1;

		foreach (NPC npc in Main.npc)
		{
			if (npc.active)
			{
				// Outside the field of view
				if (!CheckTargetWithinFOV(npc))
				{
					continue;
				}

				if (npc.dontTakeDamage || npc.friendly || !npc.CanBeChasedBy() || !Collision.CanHit(Projectile, npc))
				{
					continue;
				}

				if (Projectile.Center.Distance(npc.Center) >= minDistance)
				{
					continue;
				}

				TargetWhoAmI = npc.whoAmI;
				minDistance = Projectile.Center.Distance(npc.Center);
			}
		}
	}

	private void CheckTargetActive()
	{
		if (TargetWhoAmI == -1)
		{
			return;
		}

		NPC npc = Main.npc[TargetWhoAmI];

		if (!npc.active || npc.dontTakeDamage)
		{
			TargetWhoAmI = -1;
			return;
		}

		if (!CheckTargetWithinFOV(npc))
		{
			TargetWhoAmI = -1;
			return;
		}
	}

	private bool CheckTargetWithinFOV(NPC npc)
	{
		Vector2 directionToTarget = npc.Center - Projectile.Center;

		Vector2 projectileDirection = Projectile.velocity;

		float angleToTarget = MathHelper.WrapAngle(projectileDirection.ToRotation() - directionToTarget.ToRotation());

		bool angleCheck = MathF.Abs(angleToTarget) <= Phase1FOVAngle / 2;

		bool distanceCheck = Projectile.Center.Distance(npc.Center) <= Phase1FOVDistance;

		return angleCheck && distanceCheck;
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		CollisionCounter++;
		if (CollisionCounter >= MaxCollisionCount)
		{
			GenerateParticles(5);
			Projectile.Kill();
			return true;
		}
		if (LifeTimer >= Phase1Duration)
		{
			Projectile.Kill();
			return true;
		}
		if (Projectile.velocity.X != oldVelocity.X)
		{
			Projectile.velocity.X = -oldVelocity.X * 0.8f;
		}
		if (Projectile.velocity.Y != oldVelocity.Y)
		{
			Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
		}
		return false;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		if (LifeTimer < Phase1Duration)
		{
			Texture2D texture = ModAsset.Projectiles_FeatheredStaff.Value;

			Color drawColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());

			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			int frameY = frameHeight * Projectile.frame;
			Rectangle sourceRect = new Rectangle(0, frameY, texture.Width, frameHeight);

			var rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
			SpriteEffects effects = SpriteEffects.None;

			// If the projectile is facing left
			if (Projectile.velocity.X < 0 &&
				Projectile.oldVelocity.X < 0)
			{
				effects = SpriteEffects.FlipHorizontally;
				rotation = rotation - MathF.PI;
			}

			Main.spriteBatch.Draw(
				texture,
				Projectile.Center - Main.screenPosition,
				sourceRect,
				drawColor,
				rotation,
				origin: new Vector2(texture.Width, texture.Width) / 2.0f,
				scale: 1,
				effects: effects,
				0);

			Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
		}
		else
		{
			Texture2D texture = ModContent.Request<Texture2D>(BrokenTexture(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			Color color = Lighting.GetColor(Projectile.Center.ToTileCoordinates());

			var rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);

			SpriteEffects effects = SpriteEffects.None;

			// If the projectile is facing left
			if (Projectile.velocity.X < 0 &&
				Projectile.oldVelocity.X < 0)
			{
				effects = SpriteEffects.FlipHorizontally;
				rotation = rotation - MathF.PI;
			}

			Main.spriteBatch.Draw(
				texture,
				Projectile.Center - Main.screenPosition,
				null,
				color,
				rotation,
				origin: new Vector2(texture.Width, texture.Width) / 2.0f,
				scale: 1,
				effects: effects,
				0);
		}

		return false;
	}

	private static string BrokenTexture() => "Everglow/Yggdrasil/YggdrasilTown/Projectiles/FeatheredStaff_broken";

	public override void OnKill(int timeLeft)
	{
		for (int i = 0; i < 20; i++)
		{
			Dust.NewDust(Projectile.Center, 3, 3, DustID.Cloud, newColor: Color.SandyBrown);
		}
	}
}