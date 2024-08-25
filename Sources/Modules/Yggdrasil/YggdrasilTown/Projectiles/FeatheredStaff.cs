namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

internal class FeatheredStaff : ModProjectile
{
	private const int MaxCollideCount = 2;
	private const int Phase1Duration = 120;

	// Phase 1
	private const float FOVDistance = 500;
	private const float FOVAngle = MathF.PI / 2.0f;
	private const float RotationSpeed = 0.02f;

	private int TargetWhoAmI { get; set; } = -1;

	// Phase 2
	private const float VelocityLimitY = 10f;
	private const float Gravity = 0.2f;

	private bool Bursted { get; set; } = false;

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

		Projectile.ai[0] = 0f; // CollideCounter
		Projectile.ai[1] = 0f; // Timer
	}

	public override void AI()
	{
		if (Projectile.velocity.Y > 10f)
		{
			Projectile.velocity.Y = 10f;
		}

		if (Projectile.ai[1] < Phase1Duration)
		{
			Phase1AI();
		}
		else
		{
			Phase2AI();
		}

		// Increase timer
		Projectile.ai[1]++;
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
		else
		{
			// track enemy
			NPC npc = Main.npc[TargetWhoAmI];

			Vector2 directionToTarget = npc.Center - Projectile.Center;
			directionToTarget.Normalize();

			var angleToTarget = MathHelper.WrapAngle(
				directionToTarget.ToRotation() -
				Projectile.velocity.ToRotation());

			if (Math.Abs(angleToTarget) > RotationSpeed)
			{
				// Rotate by the rotation speed
				if (angleToTarget > 0)
				{
					Projectile.velocity = Projectile.velocity.RotatedBy(RotationSpeed);
				}
				else
				{
					Projectile.velocity = Projectile.velocity.RotatedBy(-RotationSpeed);
				}
			}
			else
			{
				// Rotate directly to the target direction
				Projectile.velocity.RotatedBy(angleToTarget);
			}
		}
	}

	// Phase 2: Gravity gradually increase to max value
	// ---------------------------------------------------------------------
	private void Phase2AI()
	{
		if (!Bursted)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(Projectile.Center, 2, 2, DustID.Cloud, newColor: Color.SandyBrown);
			}

			// Draw gores
			Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0, 6f)).RotatedByRandom(MathHelper.TwoPi);
			int type0 = ModContent.Find<ModGore>("Everglow/FeatheredStaff_gore1").Type;
			int type1 = ModContent.Find<ModGore>("Everglow/FeatheredStaff_gore0").Type;

			Gore.NewGore(Projectile.Center, v0, type0, Projectile.scale);
			Gore.NewGore(Projectile.Center, v0, type1, Projectile.scale);

			Bursted = true;
		}

		// Gravity
		Projectile.velocity.Y =
			Projectile.velocity.Y + Gravity < VelocityLimitY ?
			Projectile.velocity.Y + Gravity :
			VelocityLimitY;
	}

	private void FindEnemy()
	{
		Vector2 detectCenter = Projectile.Center;

		float minDistance = FOVDistance + 1;

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

		bool angleCheck = MathF.Abs(angleToTarget) <= FOVAngle / 2;

		bool distanceCheck = Projectile.Center.Distance(npc.Center) <= FOVDistance;

		return angleCheck && distanceCheck;
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		// Check collision count
		Projectile.ai[0]++;
		if (Projectile.ai[0] >= MaxCollideCount)
		{
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Dirt);
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
		if (Projectile.ai[1] < Phase1Duration)
		{
			Texture2D texture = ModAsset.Projectiles_FeatheredStaff.Value;

			Color drawColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());

			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			int frameY = frameHeight * Projectile.frame;
			Rectangle sourceRect = new Rectangle(0, frameY, texture.Width, frameHeight);

			var rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
			SpriteEffects effects = SpriteEffects.None;

			// Check if the projectile is facing left
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

			// Loop frame count.
			Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
		}
		else
		{
			Texture2D texture = ModContent.Request<Texture2D>(BrokenTexture(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			Color color = Lighting.GetColor(Projectile.Center.ToTileCoordinates());

			var rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);

			SpriteEffects effects = SpriteEffects.None;

			// Check if the projectile is facing left
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

			Dust.NewDust(Projectile.position - Projectile.velocity.NormalizeSafe() * 6, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, new Color(184, 134, 11, 0), 1.0f);
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