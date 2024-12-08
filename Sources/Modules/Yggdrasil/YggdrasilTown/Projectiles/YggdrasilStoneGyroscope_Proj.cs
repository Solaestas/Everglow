using Everglow.Commons.Weapons.Whips;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

/// <summary>
/// Non-air summon projectile.
/// </summary>
public class YggdrasilStoneGyroscope_Proj : ModProjectile
{
	private Player Owner => Main.player[Projectile.owner];

	public int EnemyTarget;

	public float MaxSearchRange = 450;

	public float MaxPower = 600;

	public float Power = 600;

	/// <summary>
	/// Item1 : Projectile WhoAMI; Item2 : Cooling Timer
	/// </summary>
	public List<(int ProjectileWhoAmI, int CoolingTimer)> WhipCoolingsForProjectileWhoAmI = new List<(int, int)>();

	/// <summary>
	/// 0 : slowest;6 : fastest
	/// </summary>
	public int State => (int)(Power / 100);

	public override void SetDefaults()
	{
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Summon;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 12;
		Projectile.minionSlots = 1;
		Projectile.minion = true;
	}

	public float MaxSpeed()
	{
		float maxSpeed = 10;
		if (Power < 400)
		{
			maxSpeed = 7;
		}
		if (Power < 200)
		{
			maxSpeed = 5;
		}
		if (Power < 100)
		{
			maxSpeed = 0.5f;
		}
		return maxSpeed;
	}

	public override void OnSpawn(IEntitySource source)
	{
		EnemyTarget = -1;
		base.OnSpawn(source);
	}

	public override void AI()
	{
		CheckKill();
		if (Projectile.velocity.X > 1)
		{
			Projectile.spriteDirection = -1;
		}
		if (Projectile.velocity.X < -1)
		{
			Projectile.spriteDirection = 1;
		}
		FindFrame();

		if (EnemyTarget < 0)
		{
			EnemyTarget = FindEnemy();
		}
		if (EnemyTarget != -1)
		{
			AttackEnemies();
		}
		else
		{
			ApproachMyOwner();
		}
		CheckWhipHit();
		Power -= 0.5f;
		if (Power < 1)
		{
			Projectile.Kill();
		}
	}

	/// <summary>
	/// When there are no enemies, try go back to player.
	/// </summary>
	public void ApproachMyOwner()
	{
		Vector2 targetPos = FindTargetPosWhenNoEnemies();
		Vector2 toTarget = targetPos - Projectile.velocity - Projectile.Center;
		if (toTarget.Length() > 2400)
		{
			Projectile.active = false;
		}
		if (toTarget.Length() > 1)
		{
			toTarget = Vector2.Normalize(toTarget);
			Projectile.velocity.X += toTarget.X * 0.5f;
			if (Projectile.velocity.Length() > MaxSpeed())
			{
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MaxSpeed();
			}
			if (!TileCollisionUtils.PlatformCollision(Projectile.Bottom))
			{
				Projectile.velocity.Y += 0.5f;
			}
			else
			{
				for (int i = 0; i < 100; i++)
				{
					if (TileCollisionUtils.PlatformCollision(Projectile.Bottom))
					{
						Projectile.position.Y -= 1;
						Power -= 0.2f;
					}
					else
					{
						break;
					}
				}
				BottomSpark(2);
			}
			if (Power > 400)
			{
				if (Collision.SolidCollision(Projectile.position + new Vector2(Projectile.velocity.X, 0), Projectile.width, Projectile.height))
				{
					Projectile.velocity.Y += -2;
				}
			}
		}
	}

	/// <summary>
	/// Choose a point close to player, oscillated by time and Projectile.WhoAmI.
	/// </summary>
	/// <returns></returns>
	public Vector2 FindTargetPosWhenNoEnemies()
	{
		// A serrated function.
		Vector2 targetPos = Owner.Center + new Vector2(240 * MathF.Sin((float)Main.time * 0.04f + Projectile.whoAmI), 24);
		if (!TileCollisionUtils.PlatformCollision(targetPos))
		{
			int count = 0;
			while (!TileCollisionUtils.PlatformCollision(targetPos))
			{
				count++;
				targetPos.Y += 8;
				if (count > 40)
				{
					break;
				}
			}
		}
		if (TileCollisionUtils.PlatformCollision(targetPos))
		{
			int count = 0;
			while (TileCollisionUtils.PlatformCollision(targetPos))
			{
				count++;
				targetPos.Y -= 8;
				if (count > 40)
				{
					break;
				}
			}
		}
		return targetPos;
	}

	/// <summary>
	/// Attack.
	/// </summary>
	public void AttackEnemies()
	{
		NPC npc = Main.npc[EnemyTarget];

		// if target is invalid, discard.
		if (npc == null || !npc.active || npc.life <= 0)
		{
			EnemyTarget = -1;
			return;
		}
		if ((npc.Center - Projectile.Center).Y < -120)
		{
			EnemyTarget = -1;
			return;
		}

		// oscillate over target.
		Vector2 targetPos = npc.Center + new Vector2(120 * MathF.Sin((float)Main.time * 0.05f + Projectile.whoAmI), 0);
		if (!TileCollisionUtils.PlatformCollision(targetPos))
		{
			int count = 0;
			while (!TileCollisionUtils.PlatformCollision(targetPos))
			{
				count++;
				targetPos.Y += 8;
				if (count > 40)
				{
					break;
				}
			}
		}
		if (TileCollisionUtils.PlatformCollision(targetPos))
		{
			int count = 0;
			while (TileCollisionUtils.PlatformCollision(targetPos))
			{
				count++;
				targetPos.Y -= 8;
				if (count > 40)
				{
					break;
				}
			}
		}
		Vector2 toTarget = targetPos - Projectile.velocity - Projectile.Center;
		if (toTarget.Length() > 1)
		{
			toTarget = Vector2.Normalize(toTarget);
			Projectile.velocity.X += toTarget.X * 0.5f;
			if (Projectile.velocity.Length() > MaxSpeed())
			{
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MaxSpeed();
			}
			if (!TileCollisionUtils.PlatformCollision(Projectile.Bottom))
			{
				Projectile.velocity.Y += 0.5f;
			}
			else
			{
				for (int i = 0; i < 100; i++)
				{
					if (TileCollisionUtils.PlatformCollision(Projectile.Bottom))
					{
						Projectile.position.Y -= 1;
						Power -= 0.2f;
					}
					else
					{
						break;
					}
				}
				BottomSpark(2);
			}
			if (Power > 400)
			{
				if (Collision.SolidCollision(Projectile.position + new Vector2(toTarget.X * 16, 0), Projectile.width, Projectile.height))
				{
					Projectile.velocity.Y += -2;
				}
			}
		}
	}

	/// <summary>
	/// Generate spark VFX from bottom;
	/// </summary>
	/// <param name="count"></param>
	public void BottomSpark(int count = 1)
	{
		for (int i = 0; i < count; ++i)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 2f)).RotatedByRandom(MathHelper.TwoPi) - new Vector2(Projectile.velocity.X, 0);
			var spark = new Spark_MoonBladeDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Bottom,
				maxTime = Main.rand.Next(7, 20),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 27.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				noGravity = true,
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.03f, 0.03f) },
			};
			Ins.VFXManager.Add(spark);
		}
	}

	/// <summary>
	/// Whipping gyroscope can charge power.
	/// </summary>
	public void CheckWhipHit()
	{
		foreach (var proj in Main.projectile)
		{
			if (proj != null && proj.active)
			{
				bool isAWhip = proj.aiStyle == ProjAIStyleID.Whip;
				if (!isAWhip)
				{
					if (proj.ModProjectile != null && proj.ModProjectile is WhipProjectile)
					{
						isAWhip = true;
					}
				}
				if (isAWhip)
				{
					int index = WhipCoolingsForProjectileWhoAmI.FindIndex(p => p.ProjectileWhoAmI == proj.whoAmI);
					if (index < 0)
					{
						if (proj.Colliding(proj.Hitbox, Projectile.Hitbox))
						{
							int whipValue = 150;
							CombatText.NewText(Projectile.Hitbox, new Color(1f, 1f, 1f, 1f), whipValue);
							Power += whipValue;
							if (Power > MaxPower)
							{
								Power = MaxPower;
							}
							WhipCoolingsForProjectileWhoAmI.Add((proj.whoAmI, 10));
							KillingSpark(36);
							return;
						}
					}
				}
			}
		}
		for (int i = WhipCoolingsForProjectileWhoAmI.Count - 1; i > -1; i--)
		{
			if (WhipCoolingsForProjectileWhoAmI[i].CoolingTimer > 0)
			{
				WhipCoolingsForProjectileWhoAmI[i] = (WhipCoolingsForProjectileWhoAmI[i].ProjectileWhoAmI, WhipCoolingsForProjectileWhoAmI[i].CoolingTimer - 1);
			}
			else
			{
				WhipCoolingsForProjectileWhoAmI.RemoveAt(i);
			}
		}
	}

	/// <summary>
	/// Find a npc index which can be an enemy.
	/// </summary>
	/// <returns></returns>
	public int FindEnemy()
	{
		if (Owner.MinionAttackTargetNPC != -1)
		{
			return Owner.MinionAttackTargetNPC;
		}
		float minDetectionRange = MaxSearchRange;
		int targeWhoAmI = -1;
		foreach (NPC npc in Main.npc)
		{
			if (npc != null && npc.active && npc.life > 0 && !npc.friendly && npc.type != NPCID.TargetDummy && !npc.CountsAsACritter && npc.CanBeChasedBy() && !npc.dontTakeDamage)
			{
				float distance = (npc.Center - Owner.Center).Length();
				if (distance < MaxSearchRange)
				{
					if (Collision.CanHit(npc.Center - Vector2.One, 2, 2, Projectile.Center - Vector2.One, 2, 2) && (npc.Center - Projectile.Center).Y > -120)
					{
						if (npc.boss)
						{
							distance -= 600;
						}
						if (distance < minDetectionRange)
						{
							targeWhoAmI = npc.whoAmI;
							minDetectionRange = distance;
						}
					}
				}
			}
		}
		return targeWhoAmI;
	}

	/// <summary>
	/// Add power by 30;
	/// </summary>
	/// <param name="target"></param>
	/// <param name="hit"></param>
	/// <param name="damageDone"></param>
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		Power += 30;
		if (Power > MaxPower)
		{
			Power = MaxPower;
		}
	}

	/// <summary>
	/// Get a unique order by Projectile.whoAmI.
	/// </summary>
	/// <returns></returns>
	public int GetOrderFromOwner()
	{
		int count = 0;
		for (int i = 0; i < Main.projectile.Length; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (projectile.active && projectile.owner == Owner.whoAmI)
			{
				if (projectile == Projectile)
				{
					return count;
				}
				if (projectile.type == Type)
				{
					count++;
				}
			}
		}
		return 0;
	}

	/// <summary>
	/// Low speed and high speed behave differently.
	/// </summary>
	public void FindFrame()
	{
		Projectile.frameCounter += Math.Clamp((int)Power, 300, 600);
		if (Power < 400)
		{
			if (Projectile.frameCounter > 2400)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame >= 4)
			{
				Projectile.frame = 0;
			}
		}
		else
		{
			if (Projectile.frameCounter > 1200)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame >= 7)
			{
				Projectile.frame = 4;
			}
		}
		if (Power > 100)
		{
			Projectile.rotation = Math.Clamp(Projectile.velocity.X * 0.05f, -0.8f, 0.8f);
		}
		else
		{
			float targetRot = (1.5f - Power / 100f) * MathF.Sin((float)Main.time * 0.24f + Projectile.whoAmI);
			Projectile.rotation = targetRot * 0.1f + Projectile.rotation * 0.9f;
		}
	}

	/// <summary>
	/// If rightclick to cancel the summon buff, remove projectile.
	/// </summary>
	private void CheckKill()
	{
		Player player = Main.player[Projectile.owner];
		if (player.dead || !player.active)
		{
			player.ClearBuff(ModContent.BuffType<YggdrasilStoneGyroscopeBuff>());
			Projectile.Kill();
		}
		if (player.HasBuff(ModContent.BuffType<YggdrasilStoneGyroscopeBuff>()))
		{
			Projectile.timeLeft = 2;
		}
		else
		{
			Projectile.Kill();
		}
	}

	/// <summary>
	/// Killing effect: some smoke.
	/// </summary>
	/// <param name="timeLeft"></param>
	public override void OnKill(int timeLeft)
	{
		if (Owner.ownedProjectileCounts[Type] <= 1)
		{
			Owner.ClearBuff(ModContent.BuffType<YggdrasilStoneGyroscopeBuff>());
		}
		for (int i = 0; i < 8; i++)
		{
			int type;
			switch (Main.rand.Next(3))
			{
				case 0:
					type = GoreID.ChimneySmoke1;
					break;
				case 1:
					type = GoreID.ChimneySmoke2;
					break;
				case 2:
					type = GoreID.ChimneySmoke3;
					break;
				default:
					type = GoreID.ChimneySmoke1;
					break;
			}

			Gore gore = Gore.NewGorePerfect(Projectile.Center, new Vector2(0, 2).RotatedByRandom(MathHelper.TwoPi), type);
			gore.timeLeft = Main.rand.Next(60, 120);
		}
		KillingSpark();
	}

	public void KillingSpark(int count = 20)
	{
		for (int i = 0; i < count; ++i)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 14f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new Spark_MoonBladeDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(7, 45),
				scale = Main.rand.NextFloat(2f, Main.rand.NextFloat(4f, 27.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				noGravity = true,
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.03f, 0.03f) },
			};
			Ins.VFXManager.Add(spark);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = ModAsset.YggdrasilStoneGyroscope_Proj.Value;
		Texture2D textureGlow = ModAsset.YggdrasilStoneGyroscope_Proj_glow.Value;
		Rectangle frame = new Rectangle(32 * Projectile.frame, 0, 32, 32);
		if (Projectile.frame >= 4)
		{
			frame = new Rectangle(32 * (Projectile.frame - 4), 32, 32, 32);
		}
		int whipCooling = 0;
		foreach (var pC in WhipCoolingsForProjectileWhoAmI)
		{
			if (pC.CoolingTimer > whipCooling)
			{
				whipCooling = pC.CoolingTimer;
			}
		}
		if (whipCooling > 0)
		{
			float value = whipCooling / 10f;
			Main.spriteBatch.Draw(textureGlow, Projectile.Bottom - Main.screenPosition, null, new Color(value, value, value, 0), Projectile.rotation, new Vector2(64, 78), Projectile.scale, SpriteEffects.None, 0);
		}
		Main.spriteBatch.Draw(texture, Projectile.Bottom - Main.screenPosition, frame, lightColor, Projectile.rotation, new Vector2(frame.Width * 0.5f, frame.Height), Projectile.scale, SpriteEffects.None, 0);
		if (Power > 500)
		{
			Main.spriteBatch.Draw(texture, Projectile.Bottom - Main.screenPosition + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(MathHelper.TwoPi), frame, lightColor * 0.5f, Projectile.rotation, new Vector2(frame.Width * 0.5f, frame.Height), Projectile.scale, SpriteEffects.None, 0);
		}
		float powerProgress = Power / MaxPower;
		var progressTexture = Commons.ModAsset.White.Value;
		var progressPosition = Projectile.Center - Main.screenPosition + Owner.gravDir * new Vector2(0, 36);

		Color frameColor = new Color(0.05f, 0.05f, 0.08f, 0.9f);
		Color frameColor2 = new Color(0.15f, 0.25f, 0.38f, 0.4f);
		Vector2 frameScale = new Vector2(2f, 0.4f) * 0.05f;
		Vector2 frameScale2 = new Vector2(1.8f, 0.2f) * 0.05f;

		Color lineColor = Color.White;
		Color lineColorInner = Color.White * 0.8f;
		lineColorInner.A = 255;
		Vector2 lineScaleOuter = new Vector2(2.2f * powerProgress + 0.2f, 0.7f) * 0.05f;
		Vector2 lineScale = new Vector2(2.2f * powerProgress, 0.5f) * 0.05f;
		Vector2 lineScale2 = new Vector2(2.2f * powerProgress - 0.2f, 0.3f) * 0.05f;
		Vector2 linePositionOffset = new Vector2(-2.2f * (1 - powerProgress) * progressTexture.Width * 0.025f, 0);

		Main.spriteBatch.Draw(progressTexture, progressPosition, null, frameColor, 0, progressTexture.Size() / 2, frameScale, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(progressTexture, progressPosition, null, frameColor2, 0, progressTexture.Size() / 2f, frameScale2, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(progressTexture, progressPosition + linePositionOffset, null, frameColor, 0, progressTexture.Size() / 2, lineScaleOuter, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(progressTexture, progressPosition + linePositionOffset, null, lineColor, 0, progressTexture.Size() / 2, lineScale, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(progressTexture, progressPosition + linePositionOffset, null, lineColorInner, 0, progressTexture.Size() / 2, lineScale2, SpriteEffects.None, 0);

		// Debug code: visualize target NPC.
		// if (EnemyTarget >= 0)
		// {
		// Main.spriteBatch.Draw(textureGlow, Main.npc[EnemyTarget].Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0), 0, textureGlow.Size() / 2, 0.5f, SpriteEffects.None, 0);
		// }
		return false;
	}
}