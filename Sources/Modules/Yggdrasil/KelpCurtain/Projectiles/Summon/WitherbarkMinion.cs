using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Everglow.Yggdrasil.KelpCurtain.VFXs;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;

public class WitherbarkMinion : ModProjectile
{
	private const int NoTarget = -1;
	private const int TeleportDistance = 1000;
	private const int SearchDistance = 800;
	private const int BlinkDistance = 400;
	private const int BlinkTimeMax = 60;
	private const int AccelerateTimeMax = 4;
	private const int ChargeTimeMax = 40;
	private const int MoveDiffMax = 5;

	public enum AttackState
	{
		None,
		Blink,
		NormalAttack,
		ProjectileAttack,
	}

	public enum NormalAttackState
	{
		Preparing,
		Accelerating,
		Charging,
		Decelerating,
	}

	private Vector2 dashStartPos;
	private Vector2 dashEndPos;

	private Player Owner => Main.player[Projectile.owner];

	private int TargetWhoAmI { get; set; } = NoTarget;

	private Vector2 NextDashStart
	{
		get => new Vector2(Projectile.ai[0], Projectile.ai[1]);
		set
		{
			Projectile.ai[0] = value.X;
			Projectile.ai[1] = value.Y;
		}
	}

	private ref float Timer => ref Projectile.localAI[0];

	public AttackState MinionAttackState { get; private set; } = AttackState.None;

	public NormalAttackState MinionNormalAttackState { get; private set; } = NormalAttackState.Preparing;

	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 3;
	}

	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 42;

		Projectile.DamageType = DamageClass.Summon;
		Projectile.netImportant = true;

		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;

		Projectile.minion = true;
		Projectile.minionSlots = 0;

		Projectile.timeLeft = 2;

		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
	}

	public override bool? CanDamage() => true;

	public override bool? CanHitNPC(NPC target) =>
		MinionAttackState is AttackState.NormalAttack
		&& MinionNormalAttackState is NormalAttackState.Charging or NormalAttackState.Decelerating;

	public override void AI()
	{
		UpdtateLifeCycle();
		Console.WriteLine($"===================={MinionAttackState.ToString()} : {MinionNormalAttackState}===================");
		Console.WriteLine(TargetWhoAmI);
		// If the minion is too far from the player, reset state and teleport back to owner.
		if (Vector2.Distance(Projectile.Center, Owner.Center) > TeleportDistance)
		{
			ResetTarget();
			Projectile.Center = Owner.Center; // Teleport back to owner
		}

		// Check if the target npc is active.
		if (!ProjectileUtils.MinionCheckTargetActive(TargetWhoAmI))
		{
			ResetTarget();
		}

		if (TargetWhoAmI < 0)
		{
			ChasePlayer(Owner);

			// Search target npc around the owner
			TargetWhoAmI = ProjectileUtils.FindTarget(Owner.Center, SearchDistance);
		}
		else
		{
			Attack();
		}
	}

	private void ResetTarget()
	{
		TargetWhoAmI = NoTarget; // Reset target
		MinionAttackState = AttackState.None; // Reset attack state
		MinionNormalAttackState = NormalAttackState.Preparing; // Reset normal attack state
	}

	private void UpdtateLifeCycle()
	{
		// Update timeleft
		if (Owner.HasBuff<WitherbarkSetBuff>())
		{
			Projectile.timeLeft = 2; // Reset time left if the buff is active
		}
		else
		{
			Projectile.Kill(); // Kill the projectile if the buff is not active
		}

		// Update framed
		if (Main.timeForVisualEffects % 15 == 0)
		{
			Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
		}
	}

	private void MoveTo(Vector2 destination, float velocityMax = 5f)
	{
		var movement = destination - Projectile.Center;
		var targetVelo = movement.NormalizeSafe() * MathF.Min(movement.Length(), velocityMax);
		Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelo, 0.2f);

		if (movement.Length() < 1)
		{
			Projectile.velocity = movement;
		}
	}

	private void ChasePlayer(Player player)
	{
		var targetPos = player.Center - new Vector2(40 * player.direction, 30);
		var movement = targetPos - Projectile.Center;
		if (movement.Length() >= 160f)
		{
			MoveTo(targetPos, MathF.Max(8f, Owner.velocity.Length()));
		}
		else
		{
			MoveTo(targetPos);
		}
	}

	private void Attack()
	{
		var target = Main.npc[TargetWhoAmI];
		if (Vector2.Distance(Projectile.Center, target.Center) > BlinkDistance)
		{
			MinionAttackState = AttackState.Blink;
			MinionNormalAttackState = NormalAttackState.Preparing;
		}

		if (MinionAttackState is AttackState.None)
		{
			MinionAttackState = AttackState.NormalAttack;
			MinionNormalAttackState = NormalAttackState.Preparing;
			// Randomly choose to dash or shoot.
			//if (Main.rand.NextBool())
			//{
			//	MinionAttackState = AttackState.NormalAttack;
			//}
			//else
			//{
			//	MinionAttackState = AttackState.ProjectileAttack;
			//}
		}

		// Manage attack state
		switch (MinionAttackState)
		{
			case AttackState.Blink:
				Blink(target);
				break;
			case AttackState.NormalAttack:
				NormalAttack(target);
				break;
			case AttackState.ProjectileAttack:
				ProjectileAttack(target);
				break;
			default:
				break;
		}
	}

	private void Blink(NPC target)
	{
		Projectile.velocity *= 0.91f;
		Timer++;
		Console.WriteLine($"Flashing: {Timer} / 60");

		if (Timer >= BlinkTimeMax)
		{
			Timer = 0;
			Projectile.Center = target.Center;
			MinionAttackState = AttackState.None;
		}
	}

	private void NormalAttack(NPC target)
	{
		switch (MinionNormalAttackState)
		{
			case NormalAttackState.Preparing:
				GenerateBurstDusts();

				// Set base dash info
				var movementToTarget = target.Center - Projectile.Center + target.velocity;
				var dashLength = Math.Max(movementToTarget.Length(), 64f);
				dashStartPos = Projectile.Center;
				dashEndPos = Projectile.Center + movementToTarget.NormalizeSafe() * dashLength;

				MinionNormalAttackState = NormalAttackState.Accelerating;
				Timer = 0;
				break;
			case NormalAttackState.Accelerating:
				Timer++;
				var movement = dashEndPos - dashStartPos;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, movement.NormalizeSafe() * 16f, Math.Clamp(MathF.Pow(Timer / AccelerateTimeMax, 2), 0f, 1f));
				if (Projectile.velocity.Length() >= 8f)
				{
					Timer = 0;
					MinionNormalAttackState = NormalAttackState.Charging;
				}
				break;
			case NormalAttackState.Charging:
				Timer++;
				Projectile.velocity *= 0.99f;
				if (Vector2.Distance(Projectile.Center, dashEndPos) <= MoveDiffMax || Timer >= ChargeTimeMax)
				{
					Timer = 0;
					MinionNormalAttackState = NormalAttackState.Decelerating;
				}
				break;
			case NormalAttackState.Decelerating:
				Projectile.velocity *= 0.92f;
				if (Projectile.velocity.Length() < 0.1f)
				{
					Projectile.velocity = Vector2.Zero;
					MinionNormalAttackState = NormalAttackState.Preparing;
					MinionAttackState = AttackState.None;
				}
				break;
		}
	}

	private void GenerateBurstDusts()
	{
		for (int t = 0; t < 25; t++)
		{
			Vector2 phi = new Vector2(0, Main.rand.Next(14, 22)).RotatedBy(t / 25f * MathHelper.TwoPi);
			var dustVFX4 = new MossBlossomDustSide
			{
				velocity = phi.RotatedBy(0.7f) * 0.2f,
				Active = true,
				Visible = true,
				position = Projectile.Center + phi * 0.4f,
				maxTime = Main.rand.Next(15, 40),
				scale = Main.rand.NextFloat(12, 16),
				rotation = phi.RotatedBy(0.7f).ToRotationSafe() - MathHelper.PiOver4 * 3,
				ai = new float[] { 0, 0, 0 },
			};
			Ins.VFXManager.Add(dustVFX4);
		}
	}

	// TODO: Implement projectile attack logic
	private void ProjectileAttack(NPC target)
	{
		Timer++;
		Console.WriteLine("Shooting: " + Timer + " / 60");
		if (Timer >= 60)
		{
			Timer = 0;
			MinionAttackState = AttackState.None;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
		var spriteEffect = Projectile.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, frame.Size() / 2f, Projectile.scale, spriteEffect, 0f);

		// Draw Visual Effects
		{
			DrawPoint(dashStartPos - Main.screenPosition, new Color(1f, 1f, 1f, 0f));
			DrawPoint(dashEndPos - Main.screenPosition, new Color(1f, 1f, 1f, 0f));

			if (ProjectileUtils.MinionCheckTargetActive(TargetWhoAmI))
			{
				DrawPoint(Projectile.Center - Main.screenPosition, new Color(0f, 1f, 0f, 0f));
				DrawPoint(Main.npc[TargetWhoAmI].Center - Main.screenPosition, new Color(1f, 0f, 0f, 0f));
			}

			// Draw dash shield
			if (MinionNormalAttackState is NormalAttackState.Charging)
			{
				DrawDashShield();
			}
		}

		return false;
	}

	private void DrawDashShield()
	{
		var texture = Commons.ModAsset.Trail_0.Value;
		var drawPos = Projectile.Center - Main.screenPosition + Projectile.velocity;
		var drawColor = new Color(0.2f, 1f, 0.2f, 0f);
		Main.spriteBatch.Draw(texture, drawPos, drawColor);

		var vertices = new List<Vertex2D>();
		vertices.Add(drawPos + new Vector2(0, -30), drawColor, new(0f, 0f, 0f));
	}

	// TODO: delete test code.
	private void DrawPoint(Vector2 pos, Color color)
	{
		var point = Commons.ModAsset.Point.Value;
		Main.spriteBatch.Draw(point, pos, null, color, 0f, point.Size() / 2f, 0.1f, SpriteEffects.None, 0);
	}
}