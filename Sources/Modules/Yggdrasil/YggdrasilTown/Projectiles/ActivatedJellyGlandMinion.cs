using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class ActivatedJellyGlandMinion : ModProjectile
{
	private enum AttackState
	{
		Dash,
		Rest,
		Explode,
	}

	private const int NoTarget = -1;
	private const int SearchDistance = 1200;
	private const int ExplosionDuration = 80;
	private const float ExplosionScaleFrequencyLimit = 0.4f;

	private int targetWhoAmI = NoTarget;

	private int TargetWhoAmI { get => targetWhoAmI; set => targetWhoAmI = value; }

	private Vector3 BloomLightColor { get; set; } = new Vector3(0f, 0.5f, 1f);

	private AttackState MinionAttackState { get; set; } = AttackState.Dash;

	private int ExplosionTimer { get; set; } = 0;

	private float ExplosionScaleFrequency { get; set; } = 0.2f;

	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 5;
	}

	public override void SetDefaults()
	{
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
		Projectile.timeLeft = 600;

		Projectile.tileCollide = true;
		Projectile.penetrate = -1;
		Projectile.aiStyle = -1;

		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.scale = 0.5f;

		Projectile.DamageType = DamageClass.Summon;
		Projectile.minion = true;
		Projectile.minionSlots = 0;

		ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;

		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

		Main.projPet[Projectile.type] = true;
	}

	public override bool? CanCutTiles() => true;

	public override bool MinionContactDamage() => false;

	public override void DrawBehind(
		int index,
		List<int> behindNPCsAndTiles,
		List<int> behindNPCs,
		List<int> behindProjectiles,
		List<int> overPlayers,
		List<int> overWiresUI) => behindNPCsAndTiles.Add(index);

	public override void AI()
	{
		if (Main.time % 10 == 0)
		{
			Projectile.frameCounter = (Projectile.frameCounter + 1) % Main.projFrames[Projectile.type];
		}

		Player owner = Main.player[Projectile.owner];
		if (CheckPlayerNotActive(owner))
		{
			Projectile.Kill();
			return;
		}

		Bloom();
		if (CheckTargetNotActive())
		{
			TargetWhoAmI = NoTarget;
		}

		if (HasNoTarget)
		{
			GeneralBehavior();
			SearchTarget();
		}
		else
		{
			Attack();
		}
	}

	private bool CheckPlayerNotActive(Player owner) => owner.dead || !owner.active ? true : false;

	private bool CheckTargetNotActive()
	{
		if (HasNoTarget)
		{
			return true;
		}

		NPC target = Main.npc[TargetWhoAmI];
		if (!target.active || target.dontTakeDamage)
		{
			return true;
		}

		return false;
	}

	private void SearchTarget()
	{
		Projectile.Minion_FindTargetInRange(SearchDistance, ref targetWhoAmI, false, (entiy, npcid) =>
		{
			return true;
		});
	}

	private bool HasNoTarget => TargetWhoAmI == NoTarget;

	private bool HasTarget => TargetWhoAmI != NoTarget;

	private void Bloom()
	{
		float glowStrength = 0.4f;
		if (HasTarget)
		{
			if (Projectile.frameCounter == 1)
			{
				BloomLightColor = Vector3.Lerp(BloomLightColor, new Vector3(0.5f, 2f, 4f) * glowStrength, 0.3f);
			}
			else if (Projectile.frameCounter == 2)
			{
				BloomLightColor = Vector3.Lerp(BloomLightColor, new Vector3(0f, 1f, 2f) * glowStrength, 0.3f);
			}
			else
			{
				BloomLightColor = Vector3.Lerp(BloomLightColor, new Vector3(0f, 0.3f, 0.6f) * glowStrength, 0.3f);
			}
		}
		else
		{
			BloomLightColor = Vector3.Lerp(BloomLightColor, new Vector3(0f, 0.5f, 1f) * glowStrength, 0.3f);
		}
		Lighting.AddLight(Projectile.Center, BloomLightColor);
	}

	private void Attack()
	{
		NPC target = Main.npc[TargetWhoAmI];
		var movement = target.Center - Projectile.Center;
		float ExplodeStateDistance = 100f;

		switch (MinionAttackState)
		{
			case AttackState.Dash:
				{
					var DashSpeed = 3f;
					Projectile.velocity = (movement + new Vector2(0, MathF.Sin((float)Main.time * 0.0005f + Projectile.whoAmI * 7 + 2.1f))).NormalizeSafe() * DashSpeed;
					Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

					if (Projectile.frameCounter == 0)
					{
						MinionAttackState = AttackState.Rest;
					}
					if (movement.Length() <= ExplodeStateDistance)
					{
						MinionAttackState = AttackState.Explode;
						Projectile.velocity = Vector2.Zero;
					}
					break;
				}
			case AttackState.Rest:
				{
					Projectile.velocity *= 0.92f;
					if (Projectile.velocity.Length() <= 0.5f)
					{
						if (Projectile.frameCounter == 0)
						{
							MinionAttackState = AttackState.Dash;
						}
						if (movement.Length() <= ExplodeStateDistance)
						{
							MinionAttackState = AttackState.Explode;
							Projectile.velocity = Vector2.Zero;
						}
					}
					break;
				}
			case AttackState.Explode:
				{
					if (ExplosionTimer < ExplosionDuration &&
						Projectile.timeLeft > ExplosionDuration - ExplosionTimer)
					{
						ExplosionScaleFrequency += ExplosionScaleFrequency < ExplosionScaleFrequencyLimit ? 0.005f : 0f;
						Projectile.scale = 0.5f + 0.1f * MathF.Sin((float)Main.time * 0.8f * ExplosionScaleFrequency);
					}
					else
					{
						// TODO: Replace the test projectile with a lightning projectile (blue & white)
						Projectile.NewProjectile(
							Terraria.Entity.InheritSource(Projectile),
							Projectile.Center.X,
							Projectile.Center.Y,
							movement.NormalizeSafe().X * 2f,
							movement.NormalizeSafe().Y * 2f,
							ProjectileID.Bullet,
							Projectile.damage,
							Projectile.knockBack,
							Projectile.owner);
						Projectile.Kill();
					}
					ExplosionTimer++;
					break;
				}
		}
	}

	public void GeneralBehavior()
	{
		// TODO: Complete code
		Projectile.velocity = Vector2.Zero;
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		modifiers.FinalDamage *= 0.15f;

		base.ModifyHitNPC(target, ref modifiers);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		float glowStrength = HasTarget ? 1f : 0.4f;

		Texture2D texture = ModAsset.JellyBall.Value;
		Texture2D textureG = ModAsset.JellyBall_glow.Value;
		Texture2D textureB = ModAsset.JellyBall_bloom.Value;

		Color drawColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
		int frameWidth = texture.Width / 2;
		int frameHeight = texture.Height / 5;
		int frameX = HasTarget ? 0 : frameWidth;
		int frameY = frameHeight * (Projectile.frameCounter % 5);

		Rectangle sourceRect = new Rectangle(frameX, frameY, frameWidth, frameHeight);

		int bloomFrameWidth = textureB.Width / 2;
		int bloomFrameHeight = textureB.Height / 5;
		int bloomFrameX = HasTarget ? 0 : bloomFrameWidth;
		int bloomFrameY = bloomFrameHeight * (Projectile.frameCounter % 5);
		Rectangle bloomSourceRect = new Rectangle(bloomFrameX, bloomFrameY, bloomFrameWidth, bloomFrameHeight);

		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, sourceRect, Color.Lerp(drawColor * 0.7f, new Color(0.6f, 1f, 1f, 1f), 0.4f), Projectile.rotation, new Vector2(texture.Width / 4f, texture.Height / 10f), Projectile.scale, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(textureB, Projectile.Center - Main.screenPosition, bloomSourceRect, new Color(1f, 1f, 1f, 0f) * glowStrength, Projectile.rotation, new Vector2(textureB.Width / 4f, textureB.Height / 10f), Projectile.scale, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(textureG, Projectile.Center - Main.screenPosition, sourceRect, new Color(1f, 1f, 1f, 0f) * glowStrength, Projectile.rotation, new Vector2(textureG.Width / 4f, textureG.Height / 10f), Projectile.scale, SpriteEffects.None, 0);

		return false;
	}

	public override void OnKill(int timeLeft)
	{
		if (Projectile.timeLeft > 200)
		{
			for (int i = 0; i < 12; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<JellyBallGel>());
				d.scale *= Main.rand.NextFloat(0.7f, 1.4f);
				d.velocity = new Vector2(Main.rand.NextFloat(2, 6f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
			for (int i = 0; i < 4; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<JellyBallSpark>());
				d.velocity = new Vector2(Main.rand.NextFloat(2, 6f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
			for (int g = 0; g < 32; g++)
			{
				Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(2, 24)).RotatedByRandom(MathHelper.TwoPi);
				float mulScale = Main.rand.NextFloat(6f, 12f);
				var blood = new JellyBallGelDrop
				{
					velocity = afterVelocity / mulScale,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
					maxTime = Main.rand.Next(42, 84),
					scale = mulScale,
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
				};
				Ins.VFXManager.Add(blood);
			}
			for (int g = 0; g < 8; g++)
			{
				Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(2, 4)).RotatedByRandom(MathHelper.TwoPi);
				var blood = new JellyBallGelSplash
				{
					velocity = afterVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) - afterVelocity,
					maxTime = Main.rand.Next(32, 94),
					scale = Main.rand.NextFloat(6f, 24f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.4f), 0 },
				};
				Ins.VFXManager.Add(blood);
			}
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<JellyBallGel>());
				d.scale *= Main.rand.NextFloat(0.6f, 1.3f);
				d.velocity = new Vector2(Main.rand.NextFloat(1, 3f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
			for (int i = 0; i < 1; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<JellyBallSpark>());
				d.velocity = new Vector2(Main.rand.NextFloat(2, 4f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
		}
	}
}