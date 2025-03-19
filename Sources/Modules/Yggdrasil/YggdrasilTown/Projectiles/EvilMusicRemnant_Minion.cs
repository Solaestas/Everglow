using Everglow.Commons.Mechanics.ElementalDebuff;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EvilMusicRemnant_Minion : ModProjectile
{
	public enum Minion_MainState
	{
		Spawn,
		Action,
	}

	public enum Minion_ActionState
	{
		Patrol,
		Chase,
		Attack,
	}

	private const int TimeLeftMax = 300;
	private const float MaxDistanceToOwner = 1000f;
	private const int MaxTeleportCooldown = 60;
	private const int SpawnDuration = 120;

	private const int SearchDistance = 500;
	private const int KillTime = 30;
	private const int DashDistance = 200;
	private const int DashCooldown = 60;

	private Vector2 dashStartPos;
	private Vector2 dashEndPos;

	public Minion_MainState MainState { get; set; } = Minion_MainState.Spawn;

	public Minion_ActionState ActionState { get; set; } = Minion_ActionState.Patrol;

	public Player Owner => Main.player[Projectile.owner];

	private int MinionIndex => (int)Projectile.ai[0];

	public int TargetWhoAmI
	{
		get => (int)Projectile.ai[1];
		set => Projectile.ai[1] = value;
	}

	private int Timer
	{
		get { return (int)Projectile.ai[2]; }
		set { Projectile.ai[2] = value; }
	}

	private int TeleportCooldown { get; set; }

	public NPC Target => Main.npc[TargetWhoAmI];

	public float SpawnProgress => MathF.Min(1f, (TimeLeftMax - Projectile.timeLeft) / (float)SpawnDuration);

	public override void SetStaticDefaults()
	{
		Main.projPet[Projectile.type] = true;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
	}

	public override void SetDefaults()
	{
		Projectile.width = 48;
		Projectile.height = 56;

		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = TimeLeftMax;

		Projectile.DamageType = DamageClass.Summon;
		Projectile.minion = true;
		Projectile.minionSlots = 1;

		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 30;

		Projectile.netImportant = true;

		TargetWhoAmI = -1;
	}

	public override bool MinionContactDamage() => true;

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.velocity.Y = -0.5f;
	}

	public override void AI()
	{
		UpdateLifeCycle();

		LimitDistanceFromOwner();

		if (MainState == Minion_MainState.Spawn)
		{
			GenerateSpawnMask();
			if (SpawnProgress > 0.6f)
			{
				Projectile.velocity = Vector2.Zero;
			}

			if (TimeLeftMax - Projectile.timeLeft > SpawnDuration)
			{
				MainState = Minion_MainState.Action;
				ActionState = Minion_ActionState.Patrol;
				TargetWhoAmI = -1;
			}
		}
		else if (MainState == Minion_MainState.Action)
		{
			Action();
		}
	}

	public override void OnKill(int timeLeft)
	{
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<EvilMusicRemnant_Explosion>(), Projectile.damage * 4, Projectile.knockBack, Projectile.owner);
		for (int i = 0; i < 30; i++)
		{
			Dust.NewDust(Projectile.Center, 1, 1, DustID.Shadowflame, 0, 0);
		}
	}

	#region AI

	private void UpdateLifeCycle()
	{
		if (CheckOwnerActive())
		{
			Owner.AddBuff(ModContent.BuffType<Buffs.EvilMusicRemnant>(), 30);
			if (Projectile.timeLeft <= KillTime)
			{
				Projectile.timeLeft = KillTime;
			}
		}
	}

	/// <summary>
	/// Keep the distance between minion and owner within a certain amount
	/// </summary>
	private void LimitDistanceFromOwner()
	{
		if (TeleportCooldown > 0)
		{
			TeleportCooldown--;
		}
		else if (Projectile.Center.Distance(Owner.Center) > MaxDistanceToOwner)
		{
			TargetWhoAmI = -1;
			ActionState = Minion_ActionState.Patrol;

			// Teleport to
			TeleportCooldown = MaxTeleportCooldown;
			Projectile.position = Owner.MountedCenter + new Vector2((10 - MinionIndex * 30) * Owner.direction, -40 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f - MinionIndex) * 35f);
		}
	}

	/// <summary>
	/// Check if owner is active
	/// </summary>
	/// <returns>
	/// active: true | inactive: false
	/// </returns>
	private bool CheckOwnerActive()
	{
		if (Owner.dead || Owner.active is false)
		{
			Owner.ClearBuff(ModContent.BuffType<Buffs.EvilMusicRemnant>());
			return false;
		}

		if (!Owner.HasBuff(ModContent.BuffType<Buffs.EvilMusicRemnant>()))
		{
			return false;
		}

		return true;
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
				ai = [Main.rand.NextFloat(-0.8f, 0.8f)],
			};
			Ins.VFXManager.Add(lotusFlame);
		}
	}

	private void Action()
	{
		// If has target, check target active
		if (TargetWhoAmI >= 0 && !ProjectileUtils.MinionCheckTargetActive(TargetWhoAmI))
		{
			TargetWhoAmI = -1;
			ActionState = Minion_ActionState.Patrol;
			Projectile.netUpdate = true;
		}

		// If has no target, search target
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

		// Switch action state
		if (ActionState == Minion_ActionState.Chase) // Phase: Chase
		{
			MoveTo(Target.Center);

			Timer++;
			Vector2 distanceToTarget = Target.Center - Projectile.Center;
			if (distanceToTarget.Length() <= DashDistance && Timer >= DashCooldown)
			{
				Timer = 0;
				dashStartPos = Projectile.Center;
				dashEndPos = Target.Center + distanceToTarget.NormalizeSafe() * (DashDistance - distanceToTarget.Length() + MinionIndex);
				ActionState = Minion_ActionState.Attack;
			}
		}
		else if (ActionState == Minion_ActionState.Attack) // Phase: Attack
		{
			var dashProgress = Timer / 30f;
			var pos = dashStartPos + (dashEndPos - dashStartPos) * dashProgress;
			Projectile.velocity = pos - Projectile.Center;

			Timer++;
			if (Timer == 30)
			{
				Timer = 0;
				ActionState = Minion_ActionState.Chase;
			}
		}
		else // Phase: Patrol
		{
			Vector2 aim;
			const float NotMovingVelocity = 1E-05f;
			if (Owner.velocity.Length() > NotMovingVelocity) // Player is moving
			{
				aim = Owner.MountedCenter
					+ new Vector2(
						x: (10 - MinionIndex * 30 + MinionIndex * 10) * Owner.direction,
						y: -50 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f - MinionIndex) * 35f);
			}
			else
			{
				aim = Owner.MountedCenter
					+ new Vector2(
						x: Owner.direction * (MathF.Cos((float)Main.timeForVisualEffects * 0.02f) * 60f - MinionIndex * 30),
						y: Owner.height + MathF.Sin((float)Main.timeForVisualEffects * 0.04f) * 30f);
			}

			MoveTo(aim);
		}
	}

	private void MoveTo(Vector2 aim)
	{
		Projectile.velocity *= 0.97f;

		Vector2 toAim = aim - Projectile.Center - Projectile.velocity;

		float timeValue = (float)(Main.timeForVisualEffects * 0.012f);
		Vector2 aimPosition = aim +
			new Vector2(
				80f * MathF.Sin(timeValue * 2f + Projectile.whoAmI) * Math.Clamp(Projectile.velocity.X, 1, MinionIndex + 1),
				(-10 + MinionIndex + 30f * MathF.Sin(timeValue * 0.18f + Projectile.whoAmI)) * 1)
			* Projectile.scale;
		if (toAim.Length() > 50)
		{
			Projectile.velocity += Vector2.Normalize(aimPosition - Projectile.Center - Projectile.velocity) * 0.18f * Projectile.scale;
		}
	}

	#endregion

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddElementalDebuffBuildUp(Main.player[Projectile.owner], ElementalDebuffType.NervousImpairment, 125);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var skullTexture = ModAsset.EvilMusicRemnant_Minion_Skull.Value;
		var chinTexture = ModAsset.EvilMusicRemnant_Minion_Chin.Value;
		var glowTexture = ModAsset.EvilMusicRemnant_Minion_Glow.Value;
		var drawColor = Color.White * SpawnProgress * 0.7f;
		var scaleFactor = 0.3f;
		var scale = Projectile.scale * scaleFactor;

		float skullRotation;
		SpriteEffects skullSpriteEffect;
		if (Projectile.direction < 0)
		{
			skullRotation = Projectile.velocity.ToRotation() - MathF.PI;
			skullSpriteEffect = SpriteEffects.FlipHorizontally;
		}
		else
		{
			skullRotation = Projectile.velocity.ToRotation();
			skullSpriteEffect = SpriteEffects.None;
		}

		// Skull
		var skullPosition = Projectile.Center - Main.screenPosition;
		var skullOrigin = skullTexture.Size() / 2;
		Main.spriteBatch.Draw(skullTexture, Projectile.Center - Main.screenPosition, null, drawColor, skullRotation, skullOrigin, scale, skullSpriteEffect, 0);

		// Glow
		var glowPositionOffset = new Vector2(25 * Projectile.direction, 6).RotatedBy(skullRotation) * scaleFactor;
		var glowPosition = skullPosition + glowPositionOffset;
		var glowColor = drawColor * 1.2f * MathF.Sin((float)Main.timeForVisualEffects * 0.06f);
		Main.spriteBatch.Draw(glowTexture, glowPosition, null, glowColor, skullRotation, glowTexture.Size() / 2, scale, skullSpriteEffect, 0);

		// Chin
		var chinPositionOffset = new Vector2(-12 * Projectile.direction, 26).RotatedBy(skullRotation) * scaleFactor;
		var chinPosition = skullPosition + chinPositionOffset;
		var chinOrigin = new Vector2(0, 0);
		var chinRotation = skullRotation + 0.2f * MathF.Sin((float)Main.timeForVisualEffects * 0.6f) * Projectile.direction;
		var chinSpriteEffect = SpriteEffects.None;
		if (Projectile.direction < 0)
		{
			chinOrigin = new Vector2(chinTexture.Width, 0);
			chinSpriteEffect = SpriteEffects.FlipHorizontally;
		}
		Main.spriteBatch.Draw(chinTexture, chinPosition, null, drawColor, chinRotation, chinOrigin, scale, chinSpriteEffect, 0);

		DrawShield(Main.spriteBatch);
		return false;
	}

	private void DrawShield(SpriteBatch spriteBatch)
	{
		//var shieldTexture = ModAsset.EvilMusicRemnant_Shield_Color.Value;
		//var colorProgress = MathF.Pow(SpawnProgress, 4);
		//var shieldColor = new Color(colorProgress, colorProgress, colorProgress, 0);
		//var scaleFactor = 0.9f + 0.1f * MathF.Sin((float)Main.timeForVisualEffects * 0.05f);

		//var sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		//Main.spriteBatch.End();
		//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		//var effect = ModAsset.EvilMusicRemnant_Shield.Value;
		//effect.Parameters["uHeatMap"].SetValue(ModAsset.EvilMusicRemnant_Shield_Heat.Value);
		//effect.Parameters["uNoiseMap"].SetValue(Commons.ModAsset.Noise_burn.Value);
		//effect.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects * 0.005f);
		//effect.CurrentTechnique.Passes[0].Apply();

		//spriteBatch.Draw(shieldTexture, Projectile.Center - Main.screenPosition, null, shieldColor, 0, shieldTexture.Size() / 2, 0.05f * scaleFactor, SpriteEffects.None, 0);
		//Main.spriteBatch.End();
		//Main.spriteBatch.Begin(sBS);
	}
}