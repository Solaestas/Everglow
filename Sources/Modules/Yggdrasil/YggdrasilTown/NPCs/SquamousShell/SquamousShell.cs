using Everglow.Commons.Coroutines;
using Everglow.Commons.Skeleton2D;
using Everglow.Commons.Skeleton2D.Reader;
using Everglow.Commons.Skeleton2D.Renderer;
using Everglow.Commons.Skeleton2D.Renderer.DrawCommands;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs.SquamousShell;

[AutoloadBossHead]
public class SquamousShell : ModNPC
{
	public override void SetDefaults()
	{
		NPC.aiStyle = -1;
		NPC.damage = 30;
		NPC.width = 140;
		NPC.height = 150;
		NPC.defense = 15;
		NPC.lifeMax = 6000;
		NPC.knockBackResist = 0;
		NPC.lavaImmune = true;
		NPC.noGravity = true;
		NPC.noTileCollide = false;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath1;
		NPC.value = 20000;
		NPC.localAI[0] = 0;
	}

	public bool Flying = false;
	public float PhantomValue = 0f;
	public Skeleton2D SquamousShellSkeleton;
	private SkeletonRenderer skeletonRenderer = new SkeletonRenderer();
	private SkeletonDebugRenderer skeletonDebugRenderer = new SkeletonDebugRenderer();
	public override void Load()
	{
	}

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 1;
	}

	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return 0f;
	}

	public override void OnSpawn(IEntitySource source)
	{
		NPC.velocity.Y = 2f;
		_coroutineManager.StartCoroutine(new Coroutine(Landing()));
		_coroutineManager.StartCoroutine(new Coroutine(FlyingFrame()));
		var data = Mod.GetFileBytes("Yggdrasil/YggdrasilTown/NPCs/SquamousShell/Skeletons/monster.json");
		if (SquamousShellSkeleton == null)
		{
			var json = Mod.GetFileBytes("Yggdrasil/YggdrasilTown/NPCs/SquamousShell/Skeletons/monsterj.json");
			var altas = Mod.GetFileBytes("Yggdrasil/YggdrasilTown/NPCs/SquamousShell/Skeletons/monstera.atlas");
			SquamousShellSkeleton = Skeleton2DReader.ReadSkeleton(altas, json, ModAsset.monster.Value);
			SquamousShellSkeleton.AnimationState.SetAnimation(0, "walk", true);
		}
	}
	public override void AI()
	{
		SquamousShellSkeleton.AnimationState.Update(0.016f);
		SquamousShellSkeleton.AnimationState.Apply(SquamousShellSkeleton.Skeleton);
		NPC.localAI[0] += 1;
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
		_coroutineManager.Update();
	}

	/// <summary>
	/// 落地
	/// </summary>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> Landing()
	{
		while (NPC.velocity.Y != 0)
		{
			NPC.velocity.Y += 0.2f;
			NPC.spriteDirection = 1;
			yield return new SkipThisFrame();
		}

		// 这里需要一个震屏
		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, NPC.Center);
		_coroutineManager.StartCoroutine(new Coroutine(Wake()));
	}

	/// <summary>
	/// 苏醒
	/// </summary>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> Wake()
	{
		for (int x = 0; x <= 180; x++)
		{
			wakingTimer++;
			yield return new SkipThisFrame();
		}
		int direction = 1;
		if (Main.player[NPC.target].Center.X < NPC.Center.X)
		{
			direction = -1;
		}
		_coroutineManager.StartCoroutine(new Coroutine(Rush(direction)));
	}

	/// <summary>
	/// 冲撞
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="acceleration"></param>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> Rush(int direction, float acceleration = 0.2f)
	{
		while (!NPC.collideX || NPC.velocity.Y != 0)
		{
			if (Math.Abs(NPC.velocity.X) < 20)
			{
				if (Collision.SolidCollision(NPC.Center + new Vector2(50, 80), 0, 0))
				{
					NPC.velocity.X += direction * acceleration / 3f;
				}
				if (Collision.SolidCollision(NPC.Center + new Vector2(0, 80), 0, 0))
				{
					NPC.velocity.X += direction * acceleration / 3f;
				}
				if (Collision.SolidCollision(NPC.Center + new Vector2(-50, 80), 0, 0))
				{
					NPC.velocity.X += direction * acceleration / 3f;
				}
				NPC.velocity.X += direction * acceleration / 2f;
			}
			if (NPC.collideX)
			{
				NPC.velocity.Y -= 16;
			}
			if (!NPC.collideY)
			{
				NPC.velocity.Y += 0.6f;
			}
			NPC.spriteDirection = direction;
			if (Math.Abs(Main.player[NPC.target].Center.X - NPC.Center.X) > 1000)
			{
				direction *= -1;
				NPC.velocity *= 0;
				NPC.position.X += direction * 20;
				_coroutineManager.StartCoroutine(new Coroutine(NextAttack(NPC.direction)));
				yield break;
			}
			if (Math.Abs(Main.player[NPC.target].Center.X - NPC.Center.X) > 800)
			{
				NPC.velocity *= 0.95f;
			}
			if (NPC.velocity.Y < -20)
			{
				NPC.velocity.Y = -20;
			}
			NPC.rotation = (float)GetVector2Rot(GetRotationVec());
			yield return new SkipThisFrame();
		}

		// 这里需要一个震屏
		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact.WithVolume(0.6f), NPC.Center);
		yield return new WaitForFrames(30);
		int newDirection = 1;
		if (Main.player[NPC.target].Center.X < NPC.Center.X)
		{
			newDirection = -1;
		}
		if (newDirection == direction)
		{
			NPC.velocity.Y -= 16f;
			NPC.position.Y -= 40f;
		}
		NPC.position.X += newDirection * 20;
		_coroutineManager.StartCoroutine(new Coroutine(NextAttack(newDirection)));
	}

	/// <summary>
	/// 坐地飞石雨
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> ShootSmallRocks()
	{
		Player player = Main.player[NPC.target];
		Vector2 vel = new Vector2((player.Center.X - NPC.Center.X) / 70f, -25);
		for (int k = 0; k < 9; k++)
		{
			NPC.position += new Vector2(0, -MathF.Sqrt(Main.rand.NextFloat(1f)) * 4).RotatedByRandom(6.283);
			Vector2 vel2 = vel.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f));
			Vector2 pos = NPC.Center + new Vector2(0, -MathF.Sqrt(Main.rand.NextFloat(1f)) * 40).RotatedBy(Main.rand.NextFloat(-1.5f, 1.5f));
			Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, vel2, ModContent.ProjectileType<SquamousRockProj_small>(), 20, 3, NPC.whoAmI);
			yield return new SkipThisFrame();
		}
		_coroutineManager.StartCoroutine(new Coroutine(NextAttack(NPC.direction)));
	}

	/// <summary>
	/// 飞石雨2
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> ShootSmallRocks2()
	{
		Player player = Main.player[NPC.target];
		Vector2 vel = new Vector2((player.Center.X - NPC.Center.X) / 70f, -25);
		for (int k = 0; k < 3; k++)
		{
			NPC.position += new Vector2(0, -MathF.Sqrt(Main.rand.NextFloat(1f)) * 4).RotatedByRandom(6.283);
			Vector2 vel2 = vel.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f));
			Vector2 pos = NPC.Center + new Vector2(0, -MathF.Sqrt(Main.rand.NextFloat(1f)) * 40).RotatedBy(Main.rand.NextFloat(-1.5f, 1.5f));
			Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, vel2, ModContent.ProjectileType<SquamousRockProj>(), 40, 3, 0);
			yield return new WaitForFrames(5);
		}
		_coroutineManager.StartCoroutine(new Coroutine(NextAttack(NPC.direction)));
	}

	/// <summary>
	/// 滚石
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> RollingARock(int direction)
	{
		yield return new WaitForFrames(5);
		int waitCount = 0;
		int count = 0;
		while (Collision.SolidCollision(NPC.BottomLeft, NPC.width, 1))
		{
			count++;
			NPC.position.Y -= 2f;
			if (count > 100)
			{
				break;
			}
		}
		while (NPC.velocity != NPC.oldVelocity)
		{
			waitCount++;
			if (waitCount > 600)
			{
				break;
			}
			NPC.velocity *= 0.95f;
			NPC.velocity.Y += 0.6f;
			yield return new SkipThisFrame();
		}
		NPC.velocity *= 0;
		Projectile rock = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, new Vector2(direction * 4, 0), ModContent.ProjectileType<Projectiles.SquamousRollingStone>(), 60, 6, 0, direction);
		rock.scale = 0;
		NPC.spriteDirection = direction;
		NPC.rotation = 0;
		if (NPC.spriteDirection == -1)
		{
			NPC.rotation = MathF.PI;
		}
		for (int x = 0; x < 20; x++)
		{
			NPC.rotation -= 0.04f;
			rock.scale += 0.05f;
			rock.Center = NPC.Center + new Vector2(135 * direction, 20);
			NPC.velocity *= 0.95f;
			if (!NPC.collideY)
			{
				NPC.velocity.Y += 0.6f;
			}
			yield return new SkipThisFrame();
		}
		for (int x = 0; x < 5; x++)
		{
			NPC.rotation += 0.2f;
			NPC.velocity *= 0.95f;
			if (!NPC.collideY)
			{
				NPC.velocity.Y += 0.6f;
			}
			yield return new SkipThisFrame();
		}
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(100 * direction, 70), Vector2.Zero, ModContent.ProjectileType<Squamous_HitTile>(), 0, 0, 0, 6, Main.rand.NextFloat(6.283f));
		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact.WithVolume(0.4f), NPC.Center);
		for (int x = 0; x < 20; x++)
		{
			NPC.rotation -= 0.01f;
			NPC.velocity *= 0.95f;
			if (!NPC.collideY)
			{
				NPC.velocity.Y += 0.6f;
			}
			yield return new SkipThisFrame();
		}
		_coroutineManager.StartCoroutine(new Coroutine(NextAttack(NPC.direction)));
	}

	/// <summary>
	/// 撼地跳
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> JumpAndShake(int direction)
	{
		NPC.spriteDirection = direction;
		NPC.rotation = 0;
		if (NPC.spriteDirection == -1)
		{
			NPC.rotation = MathF.PI;
		}
		NPC.velocity = new Vector2(direction * 4, -7);
		Player player = Main.player[NPC.target];
		for (int x = 0; x < 300; x++)
		{
			if ((player.Center - NPC.Center).X * direction < 0)
			{
				NPC.velocity.Y += 0.4f;
				if (x > 30)
				{
					break;
				}
			}
			if (NPC.Center.Y - player.Center.Y < -300)
			{
				if ((player.Center - NPC.Center).X * direction > 0)
				{
					if (Math.Abs(NPC.velocity.X) < 16)
					{
						NPC.velocity.X += direction * 0.2f;
					}
				}
				else
				{
					NPC.velocity.X *= 0;
					break;
				}
				if (!Flying)
				{
					Flying = true;
					NPC.frame = new Rectangle(0, 0, 244, 130);
				}
				if (NPC.velocity.Y < (player.Center.Y - NPC.Center.Y) / 60f)
				{
					NPC.velocity.Y += 1f;
				}
			}
			else
			{
				NPC.velocity.Y -= 0.4f;
			}
			yield return new SkipThisFrame();
		}

		// 短暂悬停
		Flying = false;
		NPC.velocity *= 0;
		NPC.velocity.Y = -2;
		PhantomValue = 1;
		for (int x = 0; x < 16; x++)
		{
			PhantomValue = 1 - x / 16f;
			yield return new SkipThisFrame();
		}
		PhantomValue = 0;
		for (int x = 0; x < 100; x++)
		{
			NPC.velocity.X = 0;
			NPC.velocity.Y = 40;
			if (NPC.collideY)
			{
				int count = 0;
				while (Collision.SolidCollision(NPC.BottomLeft, NPC.width, 1))
				{
					count++;
					NPC.position.Y -= 2f;
					if (count > 100)
					{
						break;
					}
				}
				SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, NPC.Center);
				GenerateSmogAtBottom(120);
				break;
			}
			yield return new SkipThisFrame();
		}
		yield return new WaitForFrames(120);
		_coroutineManager.StartCoroutine(new Coroutine(NextAttack(NPC.direction)));
	}

	/// <summary>
	/// 飞天月刃
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> FlyingMoonBlade(int direction)
	{
		NPC.spriteDirection = direction;
		NPC.rotation = 0;
		if (NPC.spriteDirection == -1)
		{
			NPC.rotation = MathF.PI;
		}
		NPC.velocity = new Vector2(direction * 4, -7);
		Player player = Main.player[NPC.target];
		Flying = true;
		for (int x = 0; x < 100; x++)
		{
			Vector2 aimPos = player.Center + new Vector2(-400 * direction, -300);
			Vector2 toTarget = aimPos - NPC.Center - NPC.velocity;
			NPC.velocity = NPC.velocity * 0.8f + toTarget * 0.2f * 0.1f;
			if ((NPC.Center - aimPos).Length() < 50)
			{
				break;
			}
			yield return new SkipThisFrame();
		}

		// 减速
		for (int x = 0; x < 12; x++)
		{
			NPC.velocity *= 0.8f;
			yield return new SkipThisFrame();
		}

		// 出刀
		Vector2 playerPos = player.Center;
		for (int x = 0; x < 140; x++)
		{
			if (x % 20 == 0)
			{
				playerPos = player.Center;
			}
			if (playerPos.X < NPC.Center.X)
			{
				direction = -1;
			}
			else
			{
				direction = 1;
			}
			if (NPC.spriteDirection > direction)
			{
				NPC.rotation += MathF.PI;
				NPC.spriteDirection = direction;
			}
			if (NPC.spriteDirection < direction)
			{
				NPC.rotation -= MathF.PI;
				NPC.spriteDirection = direction;
			}
			Vector2 aimPos = playerPos + new Vector2(-400 * direction, -300);
			Vector2 toTarget = aimPos - NPC.Center - NPC.velocity;
			NPC.velocity = NPC.velocity * 0.995f + toTarget * 0.005f * 0.1f;
			NPC.rotation -= 0.03f;
			if (x % 60 == 16)
			{
				NPC.rotation = 1;
				if (direction == -1)
				{
					NPC.rotation = MathF.PI + 1;
				}
				Vector2 toPlayer = playerPos - NPC.Center;
				int projDir = 1;
				if (Main.rand.NextBool(2))
				{
					projDir = -1;
				}
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Normalize(toPlayer) * 4f, ModContent.ProjectileType<YggdrasilMoonBlade>(), 40, 2, 0, 10, projDir);
			}
			yield return new SkipThisFrame();
		}

		// 落地
		NPC.rotation = 0;
		if (direction == -1)
		{
			NPC.rotation = MathF.PI;
		}
		Flying = false;
		for (int x = 0; x < 300; x++)
		{
			if (!NPC.collideY)
			{
				NPC.velocity.Y += 1f;
			}
			else
			{
				break;
			}
			yield return new SkipThisFrame();
		}
		_coroutineManager.StartCoroutine(new Coroutine(NextAttack(NPC.direction)));
	}

	/// <summary>
	/// 飞天风球
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> FlyingAirProjectiles(int direction)
	{
		NPC.spriteDirection = direction;
		NPC.rotation = 0;
		if (NPC.spriteDirection == -1)
		{
			NPC.rotation = MathF.PI;
		}
		NPC.velocity = new Vector2(direction * 4, -7);
		Player player = Main.player[NPC.target];
		Flying = true;
		for (int x = 0; x < 100; x++)
		{
			Vector2 aimPos = player.Center + new Vector2(-300 * direction, -180);
			Vector2 toTarget = aimPos - NPC.Center - NPC.velocity;
			NPC.velocity = NPC.velocity * 0.8f + toTarget * 0.2f * 0.1f;
			if ((NPC.Center - aimPos).Length() < 50)
			{
				break;
			}
			yield return new SkipThisFrame();
		}

		// 减速
		for (int x = 0; x < 12; x++)
		{
			NPC.velocity *= 0.8f;
			yield return new SkipThisFrame();
		}

		// 放球
		Vector2 playerPos = player.Center;
		for (int x = 0; x < 30; x++)
		{
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -15 * Main.rand.NextFloat(0.75f, 1.25f)).RotatedBy(Main.rand.NextFloat(-1.5f, 1.5f)), ModContent.ProjectileType<SquamousAirProj>(), 16, 0, 0, player.whoAmI);
			if (x % 20 == 0)
			{
				playerPos = player.Center;
			}
			Vector2 aimPos = playerPos + new Vector2((float)Utils.Lerp(-300 * direction, 300 * direction, x / 30f), -180);
			Vector2 toTarget = aimPos - NPC.Center - NPC.velocity;
			NPC.velocity = NPC.velocity * 0.9f + toTarget * 0.1f;
			NPC.position += new Vector2(0, -MathF.Sqrt(Main.rand.NextFloat(1f)) * 4).RotatedByRandom(6.283);
			yield return new SkipThisFrame();
		}

		// 落地
		NPC.rotation = 0;
		if (direction == -1)
		{
			NPC.rotation = MathF.PI;
		}
		Flying = false;
		for (int x = 0; x < 300; x++)
		{
			if (!NPC.collideY)
			{
				NPC.velocity.Y += 1f;
			}
			else
			{
				break;
			}
			yield return new SkipThisFrame();
		}
		_coroutineManager.StartCoroutine(new Coroutine(NextAttack(NPC.direction)));
	}

	/// <summary>
	/// 飞天岩牙
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> FlyingRockTusk(int direction)
	{
		NPC.spriteDirection = direction;
		NPC.rotation = 0;
		if (NPC.spriteDirection == -1)
		{
			NPC.rotation = MathF.PI;
		}
		NPC.velocity = new Vector2(direction * 4, -7);
		Player player = Main.player[NPC.target];
		Flying = true;
		for (int x = 0; x < 100; x++)
		{
			Vector2 aimPos = player.Center + new Vector2(-400 * direction, -300);
			Vector2 toTarget = aimPos - NPC.Center - NPC.velocity;
			NPC.velocity = NPC.velocity * 0.8f + toTarget * 0.2f * 0.1f;
			if ((NPC.Center - aimPos).Length() < 50)
			{
				break;
			}
			yield return new SkipThisFrame();
		}

		// 减速
		for (int x = 0; x < 12; x++)
		{
			NPC.velocity *= 0.8f;
			yield return new SkipThisFrame();
		}

		// 放锥体
		Vector2 playerPos = player.Center;
		for (int x = 0; x < 140; x++)
		{
			playerPos = player.Center;
			Vector2 aimPos = playerPos + new Vector2((float)Utils.Lerp(-400 * direction, 400 * direction, x / 140f), -300);
			Vector2 toTarget = aimPos - NPC.Center - NPC.velocity;
			NPC.velocity = NPC.velocity * 0.9f + toTarget * 0.1f * 0.1f;
			if (x % 20 == 0)
			{
				Vector2 toPlayer = playerPos - NPC.Center;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -50).RotatedBy(x / 7f * Math.PI), Vector2.Normalize(toPlayer), ModContent.ProjectileType<SquamousRockSpike>(), 40, 2, 0, player.whoAmI);
			}
			yield return new SkipThisFrame();
		}

		// 落地
		NPC.rotation = 0;
		if (direction == -1)
		{
			NPC.rotation = MathF.PI;
		}
		Flying = false;
		for (int x = 0; x < 300; x++)
		{
			if (!NPC.collideY)
			{
				NPC.velocity.Y += 1f;
			}
			else
			{
				break;
			}
			yield return new SkipThisFrame();
		}
		_coroutineManager.StartCoroutine(new Coroutine(NextAttack(NPC.direction)));
	}

	/// <summary>
	/// 下一个技能
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> NextAttack(int direction)
	{
		int newDirection = 1;
		if (Main.player[NPC.target].Center.X < NPC.Center.X)
		{
			newDirection = -1;
		}
		if (newDirection == direction)
		{
			NPC.position.Y -= 40f;
		}
		NPC.position.X += newDirection * 20;
		float randAttack = Main.rand.NextFloat(0, 18f);
		if (randAttack <= 1)
		{
			NPC.velocity.Y -= 16f;
			_coroutineManager.StartCoroutine(new Coroutine(Rush(newDirection)));
		}
		if (randAttack > 1 && randAttack <= 6)
		{
			_coroutineManager.StartCoroutine(new Coroutine(RollingARock(newDirection)));
		}
		if (randAttack > 6 && randAttack <= 8)
		{
			_coroutineManager.StartCoroutine(new Coroutine(JumpAndShake(newDirection)));
		}
		if (randAttack > 8 && randAttack <= 10)
		{
			_coroutineManager.StartCoroutine(new Coroutine(FlyingMoonBlade(newDirection)));
		}
		if (randAttack > 10 && randAttack <= 12)
		{
			_coroutineManager.StartCoroutine(new Coroutine(ShootSmallRocks()));
		}
		if (randAttack > 12 && randAttack <= 14)
		{
			_coroutineManager.StartCoroutine(new Coroutine(ShootSmallRocks2()));
		}
		if (randAttack > 14 && randAttack <= 16)
		{
			_coroutineManager.StartCoroutine(new Coroutine(FlyingAirProjectiles(newDirection)));
		}
		if (randAttack > 16 && randAttack <= 18)
		{
			_coroutineManager.StartCoroutine(new Coroutine(FlyingRockTusk(newDirection)));
		}
		yield break;
	}

	private IEnumerator<ICoroutineInstruction> FlyingFrame()
	{
		while (NPC.active)
		{
			if (Flying)
			{
				NPC.frameCounter++;
				if (NPC.frameCounter > 6)
				{
					NPC.frameCounter = 0;
					if (NPC.frame.Y == 130)
					{
						NPC.frame.Y = 0;
					}
					else
					{
						NPC.frame.Y += 130;
					}
				}
				yield return new SkipThisFrame();
			}
			else
			{
				yield return new SkipThisFrame();
			}
		}
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		// Texture2D mainTex = ModAsset.SquamousShell.Value;
		// SpriteEffects spe = SpriteEffects.None;
		// float drawRot = NPC.rotation * NPC.spriteDirection;
		// if (NPC.spriteDirection == -1)
		// {
		// spe = SpriteEffects.FlipHorizontally;
		// drawRot += MathF.PI;
		// }
		// if(Flying)
		// {
		// mainTex = ModAsset.SquamousShell_fly.Value;
		// spriteBatch.Draw(mainTex, NPC.Center - Main.screenPosition, NPC.frame, drawColor, drawRot, NPC.frame.Size() * 0.5f, NPC.scale, spe, 0);
		// }
		// else
		// {
		// spriteBatch.Draw(mainTex, NPC.Center - Main.screenPosition, null, drawColor, drawRot, mainTex.Size() * 0.5f, NPC.scale, spe, 0);
		// if (PhantomValue > 0)
		// {
		// spriteBatch.Draw(mainTex, NPC.Center - Main.screenPosition, null, drawColor * (1 - PhantomValue), drawRot, mainTex.Size() * 0.5f, NPC.scale + PhantomValue, spe, 0);
		// }
		// }

		// if (WakingTimer < 180)
		// {
		// Texture2D deadShell = ModAsset.DeadSquamousShell.Value;
		// float fade = 1 - WakingTimer / 180f;
		// spriteBatch.Draw(deadShell, NPC.Center - Main.screenPosition, null, drawColor * fade, drawRot, deadShell.Size() * 0.5f, NPC.scale, spe, 0);
		// }
		if (SquamousShellSkeleton == null)
		{
			var json = Mod.GetFileBytes("Yggdrasil/YggdrasilTown/NPCs/SquamousShell/Skeletons/monsterj.json");
			var altas = Mod.GetFileBytes("Yggdrasil/YggdrasilTown/NPCs/SquamousShell/Skeletons/monstera.atlas");
			SquamousShellSkeleton = Skeleton2DReader.ReadSkeleton(altas, json, ModAsset.monster.Value);
			SquamousShellSkeleton.AnimationState.SetAnimation(0, "walk", true);
		}

		SquamousShellSkeleton.Position = NPC.Bottom;
		SquamousShellSkeleton.Rotation = NPC.rotation;
		skeletonDebugRenderer.DisableAll();
		skeletonDebugRenderer.DrawBones = true;
		var ik = SquamousShellSkeleton.Skeleton.FindIkConstraint("Front3IK");
		float x, y;
		SquamousShellSkeleton.Skeleton.RootBone.WorldToLocal(Main.MouseWorld.X, Main.MouseWorld.Y, out x, out y);
		ik.Target.X = x;
		ik.Target.Y = y;

		SquamousShellSkeleton.Skeleton.UpdateWorldTransform();

		skeletonRenderer.UseEnvironmentLight = true;
		skeletonRenderer.DrawOffset = -Main.screenPosition;
		skeletonRenderer.DrawOrigin = new Vector2(0, 100);
		skeletonRenderer.DrawRotation = (float)Math.Sin(Main.time * 0.1f);
		//// skeleton2D.InverseKinematics(Main.MouseWorld);
		// float framesOfAnimation = 35;
		// SquamousShellSkeleton.PlayAnimation(0, "walk", ((float)Main.timeForVisualEffects % framesOfAnimation / framesOfAnimation) * framesOfAnimation / 60f);
		//// SquamousShellSkeleton.DrawDebugView(spriteBatch);

		var cmdList = skeletonRenderer.Draw(SquamousShellSkeleton);
		cmdList.AddRange(skeletonDebugRenderer.Draw(SquamousShellSkeleton));
		NaiveExecuter executer = new NaiveExecuter();
		executer.Execute(cmdList, Main.graphics.graphicsDevice);
		return false;
	}

	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
	}

	//--------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------
	public void GenerateSmogAtBottom(int Frequency)
	{
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(0f, 1f)) * 12).RotatedByRandom(MathHelper.TwoPi);
			newVelocity.Y *= 0.05f;
			newVelocity.Y -= 2f;
			var somg = new RockSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = NPC.Bottom + new Vector2(Main.rand.NextFloat(-6f, 6f), -20).RotatedByRandom(6.283) + new Vector2(newVelocity.X * 10, 0),
				maxTime = Main.rand.Next(47, 145),
				scale = Main.rand.NextFloat(20f, 75f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
	}

	public double GetVector2Rot(Vector2 value)
	{
		if (value == Vector2.zeroVector)
		{
			double value1 = 0;
			if (NPC.spriteDirection == -1)
			{
				value1 = Math.PI;
			}
			return value1;
		}
		return Math.Atan2(value.Y, value.X) * NPC.spriteDirection + Math.PI / 2d;
	}

	public double CheckRotDir(double OldRot)
	{
		return -OldRot;
	}

	public void Exchange(ref float value1, ref float value2)
	{
		(value1, value2) = (value2, value1);
	}

	private Vector2 GetRotationVec()
	{
		Vector2 outValue = Vector2.Zero;
		for (int rad = 16; rad < 256; rad += 32)
		{
			for (int rot = 0; rot < rad / 4; rot++)
			{
				float value = rot / (float)rad * 4f;
				if (!Collision.SolidCollision(NPC.Center + new Vector2(0, rad).RotatedBy(value * MathHelper.TwoPi), 0, 0))
				{
					outValue += Vector2.Normalize(new Vector2(0, rad).RotatedBy(value * MathHelper.TwoPi)) / rad;
				}
				else
				{
					outValue -= Vector2.Normalize(new Vector2(0, rad).RotatedBy(value * MathHelper.TwoPi)) / rad;
				}
			}
		}
		if (outValue.Length() < 0.005)
		{
			outValue = Vector2.Zero;
		}
		return Utils.SafeNormalize(outValue, Vector2.Zero);
	}

	private void CheckSpriteDir()
	{
		if (NPC.velocity.X > 0)
		{
			NPC.spriteDirection = -1;
		}

		if (NPC.velocity.X < 0)
		{
			NPC.spriteDirection = 1;
		}
	}

	private void MoveTo(Vector2 aimPosition, float Speed = 1)
	{
		Vector2 v0 = Utils.SafeNormalize(aimPosition - NPC.Center, Vector2.Zero);
		NPC.velocity.X = v0.X * Speed;
	}

	private void DashTo(Vector2 aim, float Speed)
	{
		NPC.noTileCollide = true;
		Vector2 v0 = Utils.SafeNormalize(aim - NPC.Center, Vector2.Zero);
		NPC.velocity = v0 * Speed;
		NPC.noTileCollide = false;
	}

	private CoroutineManager _coroutineManager = new CoroutineManager();
	private int wakingTimer = 0;
}