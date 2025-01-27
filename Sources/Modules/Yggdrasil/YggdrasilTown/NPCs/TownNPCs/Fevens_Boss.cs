using Everglow.Commons.Coroutines;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles.FevensAttack;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;

[AutoloadHead]
public class Fevens_Boss : ModNPC
{
	private bool canDespawn = false;

	/// <summary>
	/// 0 default; 1 hold magic array; 2 attack1; 3 attack2; 4 stand; 5 attack3
	/// </summary>
	public int State;

	public int Phase;

	public override string Texture => ModAsset.Fevens_Boss_Mod;

	public override string HeadTexture => ModAsset.Fevens_Head_Boss_Mod;

	public override void SetDefaults()
	{
		NPC.width = 26;
		NPC.height = 36;
		NPC.aiStyle = 7;
		NPC.damage = 80;
		NPC.defense = 100;
		NPC.lifeMax = 250000;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath6;
		NPC.knockBackResist = 0.5f;
		NPC.boss = true;
		NPC.knockBackResist = 0;
		NPC.noTileCollide = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
		Phase = 0;
		NPC.friendly = false;
		NPC.aiStyle = -1;

		NPC.lifeMax = 25000;
		NPC.life = 25000;
		if (Main.expertMode)
		{
			NPC.lifeMax = 37500;
			NPC.life = 37500;
		}
		if (Main.masterMode)
		{
			NPC.lifeMax = 45000;
			NPC.life = 45000;
		}
		NPC.boss = true;
		NPC.localAI[0] = 0;
		NPC.aiStyle = -1;
		NPC.width = 40;
		NPC.height = 56;
		StartToBeABoss();
	}

	public override bool CheckActive()
	{
		return canDespawn;
	}

	public override void AI()
	{
		Player player = Main.player[NPC.target];
		_fevensCoroutine.Update();
		if (!player.active || player.dead)
		{
			NPC.active = false;
		}
		NPC.damage = 40;
	}

	private CoroutineManager _fevensCoroutine = new CoroutineManager();

	public void StartToBeABoss()
	{
		NPC.TargetClosest(true);
		_fevensCoroutine.StartCoroutine(new Coroutine(StartFighting()));

		// Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<fevensSword_following>(), 0, 0f, -1, NPC.whoAmI);
	}

	private IEnumerator<ICoroutineInstruction> StartFighting()
	{
		Player player = Main.player[NPC.target];
		int dirChoose = 1;
		if (Main.rand.NextBool())
		{
			dirChoose = -1;
		}
		NPC.direction = dirChoose;
		NPC.spriteDirection = NPC.direction;
		NPC.noGravity = true;
		State = 1;
		Phase = 1;
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), player.Center, Vector2.zeroVector, ModContent.ProjectileType<Fevens_MagicArray>(), 0, 0f, -1, NPC.whoAmI);
		for (int t = 0; t < 60; t++)
		{
			Vector2 targetPos = player.Center + new Vector2(200 * NPC.direction, -200);
			NPC.position = targetPos * 0.05f + NPC.position * 0.95f;
			yield return new SkipThisFrame();
		}
		for (int i = 0; i < 4; i++)
		{
			Vector2 vel = new Vector2(0, 4).RotatedByRandom(MathHelper.TwoPi);
			Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<FevensChasingProj>(), 0, 1f, -1, 60);
			FevensChasingProj fCP = proj.ModProjectile as FevensChasingProj;
			if (fCP != null)
			{
				fCP.TargetPosition = player.Center;
			}
		}
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), player.Center, Vector2.zeroVector, ModContent.ProjectileType<Fevens_AttackTarget>(), 0, 0f, -1, NPC.whoAmI);
		_fevensCoroutine.StartCoroutine(new Coroutine(MagicAttack0()));
	}

	private IEnumerator<ICoroutineInstruction> MagicAttack0()
	{
		Player player = Main.player[NPC.target];
		GoToMagicAttackPosition();
		for (int t = 0; t < 60; t++)
		{
			var oldPos = NPC.Center;
			Vector2 targetPos = player.Center + new Vector2(600 * NPC.direction, -300);
			NPC.position = targetPos * 0.05f + NPC.position * 0.95f;
			GenerateVFXBatShortTime((int)(oldPos - NPC.Center).Length() / 4);
			yield return new SkipThisFrame();
		}

		for (int j = 0; j < 6; j++)
		{
			for (int i = 0; i < 8; i++)
			{
				Vector2 vel = new Vector2(0, 9).RotatedByRandom(MathHelper.TwoPi);
				Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<FevensChasingProj>(), (int)(NPC.damage * 1.1f), 1f, -1, 60);
				FevensChasingProj fCP = proj.ModProjectile as FevensChasingProj;
				if (fCP != null)
				{
					fCP.TargetPosition = player.Center;
				}
			}
			Projectile targetP = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), player.Center, Vector2.zeroVector, ModContent.ProjectileType<Fevens_AttackTarget>(), 0, 0f, -1, NPC.whoAmI);
			targetP.spriteDirection = player.direction * -1;
			State = 2;
			yield return new WaitForFrames(12);
			State = 1;
			yield return new WaitForFrames(8);
		}
		State = 3;
		yield return new WaitForFrames(20);
		for (int i = 0; i < 12; i++)
		{
			Vector2 vel = new Vector2(0, 9).RotatedBy(MathHelper.TwoPi / 12f * i);
			Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<Fevens_AttackProj0>(), (int)(NPC.damage * 1.1f), 1f, -1, 60);
		}
		State = 1;
		_fevensCoroutine.StartCoroutine(new Coroutine(ChooseAttack()));
	}

	private IEnumerator<ICoroutineInstruction> MagicAttack1()
	{
		Player player = Main.player[NPC.target];
		for (int i = 0; i < 3; i++)
		{
			NPC.velocity *= 0;
			NPC.Center = player.Center + new Vector2(0, Main.rand.Next(-220, -90)).RotatedBy(Main.rand.NextFloat(-1.1f, 1.1f));
			if (NPC.Center.X > player.Center.X)
			{
				NPC.direction = 1;
			}
			else
			{
				NPC.direction = -1;
			}
			NPC.spriteDirection = NPC.direction;

			for (int v = 0; v < 25; v++)
			{
				Vector2 startPos = NPC.position + new Vector2(Main.rand.NextFloat(NPC.width), Main.rand.NextFloat(NPC.height));
				var dustVFX = new Fevens_Bat_Small
				{
					velocity = new Vector2(0, 3).RotatedByRandom(Math.PI * 2),
					Active = true,
					Visible = true,
					position = startPos,
					maxTime = Main.rand.Next(70, 120),
					scale = Main.rand.Next(20, 40),
					ai = new float[] { Main.rand.NextFloat(1f, 8f), NPC.Center.X, NPC.Center.Y },
				};
				Ins.VFXManager.Add(dustVFX);
			}
			int lightingCount = 8;
			float offsetX = Main.rand.NextFloat(-100, 100);
			for (int j = 0; j < lightingCount; j++)
			{
				Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), player.Center + new Vector2((j - lightingCount / 2 + 0.5f) * 360 + offsetX + Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-30, 30) + 200), Vector2.zeroVector, ModContent.ProjectileType<Fevens_ThunderMark>(), (int)(NPC.damage * 1.1f), 1f, -1, 0);
			}
			yield return new WaitForFrames(60);
		}
		_fevensCoroutine.StartCoroutine(new Coroutine(ChooseAttack()));
		yield return new SkipThisFrame();
	}

	private IEnumerator<ICoroutineInstruction> MagicAttack2()
	{
		Player player = Main.player[NPC.target];
		GoToMagicAttackPosition();
		for (int t = 0; t < 60; t++)
		{
			var oldPos = NPC.Center;
			Vector2 targetPos = player.Center + new Vector2(600 * NPC.direction, -300);
			NPC.position = targetPos * 0.05f + NPC.position * 0.95f;
			GenerateVFXBatShortTime((int)(oldPos - NPC.Center).Length() / 4);
			yield return new SkipThisFrame();
		}

		for (int j = 0; j < 3; j++)
		{
			Vector2 vel = new Vector2(0, 9).RotatedByRandom(MathHelper.TwoPi);
			for (int i = 0; i < 5; i++)
			{
				for (int k = 0; k < 3; k++)
				{
					Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel.RotatedBy(k * 0.15f + MathHelper.TwoPi * i / 5f), ModContent.ProjectileType<FevensChasingProj1>(), (int)(NPC.damage * 1.1f), 1f, -1, 60);
					FevensChasingProj1 fCP = proj.ModProjectile as FevensChasingProj1;
					if (fCP != null)
					{
						fCP.TargetPosition = player.Center;
					}
				}
			}
			Projectile targetP = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), player.Center, Vector2.zeroVector, ModContent.ProjectileType<Fevens_AttackTarget>(), 0, 0f, -1, NPC.whoAmI);
			targetP.spriteDirection = player.direction * -1;
			targetP.timeLeft += 120;
			State = 2;
			yield return new WaitForFrames(12);
			State = 1;
			yield return new WaitForFrames(8);
		}
		State = 3;
		yield return new WaitForFrames(20);
		for (int i = 0; i < 12; i++)
		{
			Vector2 vel = new Vector2(0, 9).RotatedBy(MathHelper.TwoPi / 12f * i);
			Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<Fevens_AttackProj0>(), (int)(NPC.damage * 1.1f), 1f, -1, 60);
		}
		State = 1;
		_fevensCoroutine.StartCoroutine(new Coroutine(ChooseAttack()));
	}

	private IEnumerator<ICoroutineInstruction> MagicAttack3()
	{
		Player player = Main.player[NPC.target];
		GoToMagicAttackPosition();
		for (int t = 0; t < 60; t++)
		{
			var oldPos = NPC.Center;
			Vector2 targetPos = player.Center + new Vector2(600 * NPC.direction, -300);
			NPC.position = targetPos * 0.05f + NPC.position * 0.95f;
			GenerateVFXBatShortTime((int)(oldPos - NPC.Center).Length() / 4);
			yield return new SkipThisFrame();
		}

		Projectile targetP = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), player.Center, Vector2.zeroVector, ModContent.ProjectileType<Fevens_AttackTarget>(), 0, 0f, -1, NPC.whoAmI);
		targetP.spriteDirection = player.direction * -1;
		targetP.timeLeft += 120;
		var posTarget = targetP.Center;
		Vector2 vel = Vector2.Normalize(targetP.Center - NPC.Center) * 8;
		for (int j = 0; j < 60; j++)
		{
			for (int i = 0; i < 2; i++)
			{
				Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel.RotatedBy(j * Math.Sign(i - 0.5f) * 0.1f), ModContent.ProjectileType<FevensChasingProj1>(), (int)(NPC.damage * 1.1f), 1f, -1, 36);
				FevensChasingProj1 fCP = proj.ModProjectile as FevensChasingProj1;
				if (fCP != null)
				{
					fCP.TargetPosition = posTarget;
				}
			}
			yield return new WaitForFrames(1);
		}
		State = 3;
		yield return new WaitForFrames(20);
		for (int i = 0; i < 12; i++)
		{
			Vector2 vel2 = new Vector2(0, 9).RotatedBy(MathHelper.TwoPi / 12f * i);
			Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel2, ModContent.ProjectileType<Fevens_AttackProj0>(), (int)(NPC.damage * 1.1f), 1f, -1, 60);
		}
		State = 1;
		_fevensCoroutine.StartCoroutine(new Coroutine(ChooseAttack()));
	}

	private IEnumerator<ICoroutineInstruction> MagicAttack4()
	{
		Player player = Main.player[NPC.target];
		Vector2 startPlayerPos = player.Center - new Vector2(0, 380);
		Vector2 startPos = new Vector2(0, 180).RotatedByRandom(MathHelper.TwoPi);
		for (int i = 0; i < 6; i++)
		{
			NPC.velocity *= 0;
			Vector2 addPos = startPos.RotatedBy(i * MathHelper.TwoPi / 6f);
			NPC.Center = startPlayerPos + addPos;
			if (NPC.Center.X > player.Center.X)
			{
				NPC.direction = 1;
			}
			else
			{
				NPC.direction = -1;
			}
			NPC.spriteDirection = NPC.direction;

			GenerateVFXBat(25);
			int projCount = 10;
			Vector2 vel = Vector2.Normalize(addPos);
			for (int j = 0; j < projCount; j++)
			{
				Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel.RotatedBy((j - 5f) * 0.1f) * 6, ModContent.ProjectileType<FevensChasingProj1>(), (int)(NPC.damage * 1.1f), 1f, -1, 120);
				FevensChasingProj1 fCP = proj.ModProjectile as FevensChasingProj1;
				if (fCP != null)
				{
					fCP.TargetPosition = player.Center;
				}

				Projectile targetP = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), player.Center, Vector2.zeroVector, ModContent.ProjectileType<Fevens_AttackTarget>(), 0, 0f, -1, NPC.whoAmI);
				targetP.spriteDirection = player.direction * -1;
				targetP.timeLeft += 120;
			}
			yield return new WaitForFrames(30);
		}
		_fevensCoroutine.StartCoroutine(new Coroutine(ChooseAttack()));
		yield return new SkipThisFrame();
	}

	private IEnumerator<ICoroutineInstruction> SwitchToPhase2()
	{
		NPC.velocity *= 0;
		NPC.velocity.Y = 20;
		for (int i = 0; i < 1000; i++)
		{
			if (NPC.velocity.Y < 30)
			{
				NPC.velocity.Y += 1f;
			}
			if (TileCollisionUtils.PlatformCollision(NPC.Bottom))
			{
				NPC.velocity *= 0;
				break;
			}
			yield return new SkipThisFrame();
		}
		Phase = 2;
		State = 4;
		NPC.localAI[0] = 60;
		for (int t = 0; t < 60; t++)
		{
			NPC.localAI[0]--;
			yield return new SkipThisFrame();
		}
		NPC.localAI[0] = 0;
		_fevensCoroutine.StartCoroutine(new Coroutine(ChooseAttack()));
		yield return new SkipThisFrame();
	}

	private IEnumerator<ICoroutineInstruction> SlashAttack0()
	{
		Player player = Main.player[NPC.target];
		NPC.velocity *= 0;
		Vector2 attackPos = player.Center;
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), attackPos, Vector2.zeroVector, ModContent.ProjectileType<Fevens_Slash_Target>(), 0, 2, default, 2);
		yield return new WaitForFrames(20);
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<Fevens_Slash_Shine>(), 0, 2, default, 2);
		yield return new WaitForFrames(10);
		State = 5;
		for (int t = 0; t < 20; t++)
		{
			Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), attackPos, Vector2.zeroVector, ModContent.ProjectileType<Fevens_Slash>(), 30, 2, default, 2);
			NPC.position = attackPos + new Vector2(Main.rand.NextFloat(-270, 270), Main.rand.NextFloat(-90, 90));
			NPC.spriteDirection *= -1;
			NPC.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			if (t % 2 == 0)
			{
				Vector2 startPos = new Vector2(0, Main.rand.NextFloat(45, 200)).RotatedByRandom(MathHelper.TwoPi);
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), attackPos + startPos * 3, -startPos.RotatedByRandom(0.4f), ModContent.ProjectileType<Fevens_Wing_Slash>(), 30, 2, default, 2);
			}

			yield return new SkipThisFrame();
		}
		NPC.alpha = 255;
		Vector2 nextPos = player.Center + new Vector2(0, -320).RotatedByRandom(0.5f);
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), nextPos, Vector2.zeroVector, ModContent.ProjectileType<Fevens_WingTarget>(), 0, 2, default, 2);
		yield return new WaitForFrames(20);
		for (int i = 0; i < 8; i++)
		{
			Vector2 startPos = new Vector2(0, -120).RotatedBy(Main.time + i / 8d * MathHelper.TwoPi);
			Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), nextPos + startPos, -startPos.RotatedBy(MathHelper.PiOver4 / 2) * 0.4f, ModContent.ProjectileType<Fevens_Wing_Slash>(), 30, 2, default, 2);
		}
		yield return new WaitForFrames(4);
		for (int i = 0; i < 4; i++)
		{
			Vector2 startPos = new Vector2(-720, -675).RotatedBy(-i * 0.5f);
			Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), nextPos + startPos, -startPos * 0.2f, ModContent.ProjectileType<Fevens_Wing_Slash>(), 30, 2, default, 2);

			startPos = new Vector2(720, -675).RotatedBy(i * 0.5f);
			Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), nextPos + startPos, -startPos * 0.2f, ModContent.ProjectileType<Fevens_Wing_Slash>(), 30, 2, default, 2);
			yield return new WaitForFrames(2);
		}
		NPC.Center = nextPos;
		NPC.rotation = 0;
		NPC.alpha = 0;
		if(Main.rand.NextBool(2))
		{
			NPC.spriteDirection *= -1;
		}
		State = 4;

		yield return new WaitForFrames(30);
		_fevensCoroutine.StartCoroutine(new Coroutine(ChooseAttack()));

		yield return new SkipThisFrame();
	}

	private IEnumerator<ICoroutineInstruction> FallingSwordAttack0()
	{
		Player player = Main.player[NPC.target];
		NPC.velocity *= 0;
		
		_fevensCoroutine.StartCoroutine(new Coroutine(ChooseAttack()));

		yield return new SkipThisFrame();
	}

	public void GenerateVFXBat(int count)
	{
		for (int v = 0; v < count; v++)
		{
			Vector2 startVfxPos = NPC.position + new Vector2(Main.rand.NextFloat(NPC.width), Main.rand.NextFloat(NPC.height));
			var dustVFX = new Fevens_Bat_Small
			{
				velocity = new Vector2(0, 3).RotatedByRandom(Math.PI * 2),
				Active = true,
				Visible = true,
				position = startVfxPos,
				maxTime = Main.rand.Next(70, 120),
				scale = Main.rand.Next(20, 40),
				ai = new float[] { Main.rand.NextFloat(1f, 8f), NPC.Center.X, NPC.Center.Y },
			};
			Ins.VFXManager.Add(dustVFX);
		}
	}

	public void GenerateVFXBatShortTime(int count)
	{
		for (int v = 0; v < count; v++)
		{
			Vector2 startVfxPos = NPC.position + new Vector2(Main.rand.NextFloat(NPC.width), Main.rand.NextFloat(NPC.height));
			var dustVFX = new Fevens_Bat_Small
			{
				velocity = new Vector2(0, 2).RotatedByRandom(Math.PI * 2),
				Active = true,
				Visible = true,
				position = startVfxPos,
				maxTime = Main.rand.Next(40, 50),
				scale = Main.rand.Next(20, 40),
				ai = new float[] { Main.rand.NextFloat(1f, 8f), NPC.Center.X, NPC.Center.Y },
			};
			Ins.VFXManager.Add(dustVFX);
		}
	}

	public void GoToMagicAttackPosition()
	{
		Player player = Main.player[NPC.target];
		int dirChoose = 1;
		if (Main.rand.NextBool())
		{
			dirChoose = -1;
		}
		NPC.direction = dirChoose;
		NPC.spriteDirection = NPC.direction;
		NPC.noGravity = true;
		State = 1;
		bool lackArray = true;
		foreach (var proj in Main.projectile)
		{
			if (proj.active && proj.type == ModContent.ProjectileType<Fevens_MagicArray>())
			{
				if ((proj.Center - NPC.Center).Length() < 200)
				{
					lackArray = false;
					break;
				}
			}
		}
		if (lackArray)
		{
			Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), player.Center, Vector2.zeroVector, ModContent.ProjectileType<Fevens_MagicArray>(), 0, 0f, -1, NPC.whoAmI);
		}
	}

	private IEnumerator<ICoroutineInstruction> ChooseAttack()
	{
		if (Phase == 1)
		{
			if (NPC.life > NPC.lifeMax * 0.5f)
			{
				switch (Main.rand.Next(5))
				{
					case 0:
						_fevensCoroutine.StartCoroutine(new Coroutine(MagicAttack0()));
						break;
					case 1:
						_fevensCoroutine.StartCoroutine(new Coroutine(MagicAttack1()));
						break;
					case 2:
						_fevensCoroutine.StartCoroutine(new Coroutine(MagicAttack2()));
						break;
					case 3:
						_fevensCoroutine.StartCoroutine(new Coroutine(MagicAttack3()));
						break;
					case 4:
						_fevensCoroutine.StartCoroutine(new Coroutine(MagicAttack4()));
						break;
					default:
						_fevensCoroutine.StartCoroutine(new Coroutine(MagicAttack1()));
						break;
				}
			}
			else
			{
				_fevensCoroutine.StartCoroutine(new Coroutine(SwitchToPhase2()));
			}
		}
		if (Phase == 2)
		{
			switch (Main.rand.Next(3))
			{
				case 0:
					_fevensCoroutine.StartCoroutine(new Coroutine(SlashAttack0()));
					break;
				case 1:
					_fevensCoroutine.StartCoroutine(new Coroutine(SlashAttack0()));
					break;
				case 2:
					_fevensCoroutine.StartCoroutine(new Coroutine(SlashAttack0()));
					break;
				default:
					_fevensCoroutine.StartCoroutine(new Coroutine(MagicAttack1()));
					break;
			}
		}
		yield return new SkipThisFrame();
	}

	public override void FindFrame(int frameHeight)
	{
		if (State == 1)
		{
			if (NPC.frame.Width != 132)
			{
				NPC.frame.Y = 0;
				NPC.frame.X = 0;
				NPC.frame.Width = 132;
				NPC.frame.Height = 120;
			}
			NPC.frameCounter++;
			if (NPC.frameCounter >= 5)
			{
				NPC.frame.Y += 120;
				NPC.frame.X = 0;
				NPC.frame.Width = 132;
				NPC.frame.Height = 120;
				if (NPC.frame.Y > 480)
				{
					NPC.frame.Y = 0;
				}
				NPC.frameCounter = 0;
			}
		}

		if (State == 2)
		{
			if (NPC.frame.Width != 38)
			{
				NPC.frame.Y = 0;
				NPC.frame.X = 0;
				NPC.frame.Width = 38;
				NPC.frame.Height = 58;
			}
			NPC.frameCounter++;
			if (NPC.frameCounter >= 3)
			{
				NPC.frame.Y += 58;
				NPC.frame.X = 0;
				NPC.frame.Width = 38;
				NPC.frame.Height = 58;
				if (NPC.frame.Y > 174)
				{
					NPC.frame.Y = 0;
				}
				NPC.frameCounter = 0;
			}
		}

		if (State == 3)
		{
			if (NPC.frame.Width != 38)
			{
				NPC.frame.Y = 0;
				NPC.frame.X = 0;
				NPC.frame.Width = 38;
				NPC.frame.Height = 58;
			}
			NPC.frameCounter++;
			if (NPC.frameCounter >= 2)
			{
				NPC.frame.Y += 58;
				NPC.frame.X = 0;
				NPC.frame.Width = 38;
				NPC.frame.Height = 58;
				if (NPC.frame.Y > 522)
				{
					NPC.frame.Y = 0;
				}
				NPC.frameCounter = 0;
			}
		}
		base.FindFrame(frameHeight);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		drawColor *= (255 - NPC.alpha) / 255f;
		if (State == 0)
		{
			return true;
		}
		if (Phase == 2)
		{
			if (State == 4)
			{
				Texture2D wing = ModAsset.Fevens_Wing.Value;
				Texture2D wingFlame = ModAsset.Fevens_Wing_Flame.Value;
				float standardRot = NPC.rotation - MathHelper.PiOver4 * 3;
				for (int i = 0; i < 4; i++)
				{
					Vector2 offset = new Vector2(0, 0);
					float duration = MathF.Pow(1 - NPC.localAI[0] / 60f, 0.3f);
					float rotationWing = standardRot + duration * (1 - i * 0.24f) * 2.2f;
					float wingSize = 0.8f;
					if (NPC.spriteDirection == -1)
					{
						offset = new Vector2(-10, 0).RotatedBy(NPC.rotation);
						wingSize = 1f;
					}
					int frameIndex = (int)(Main.time * 0.17 + i * 5) % 7;
					Rectangle frameWing = new Rectangle(0, frameIndex * 40, 40, 40);

					spriteBatch.Draw(wing, NPC.Center - Main.screenPosition + offset, frameWing, drawColor, rotationWing, new Vector2(40), NPC.scale * (1 - i * 0.1f) * wingSize, SpriteEffects.None, 0);
					spriteBatch.Draw(wingFlame, NPC.Center - Main.screenPosition + offset, frameWing, new Color(1f, 1f, 1f, 0), rotationWing, new Vector2(40), NPC.scale * (1 - i * 0.1f) * wingSize, SpriteEffects.None, 0);
				}

				standardRot = NPC.rotation - MathHelper.PiOver4;
				for (int i = 0; i < 4; i++)
				{
					Vector2 offset = new Vector2(10, 0).RotatedBy(NPC.rotation);
					float duration = MathF.Pow(1 - NPC.localAI[0] / 60f, 0.3f);
					float rotationWing = standardRot - duration * (1 - i * 0.24f) * 2f - 0.2f;
					float wingSize = 1f;
					if (NPC.spriteDirection == -1)
					{
						offset = new Vector2(0);
						wingSize = 0.8f;
					}
					int frameIndex = (int)(Main.time * 0.17 + i * 5 + 3) % 7;
					Rectangle frameWing = new Rectangle(0, frameIndex * 40, 40, 40);
					spriteBatch.Draw(wing, NPC.Center - Main.screenPosition + offset, frameWing, drawColor, rotationWing, new Vector2(40, 0), NPC.scale * (1 - i * 0.1f) * wingSize, SpriteEffects.FlipVertically, 0);
					spriteBatch.Draw(wingFlame, NPC.Center - Main.screenPosition + offset, frameWing, new Color(1f, 1f, 1f, 0), rotationWing, new Vector2(40, 0), NPC.scale * (1 - i * 0.1f) * wingSize, SpriteEffects.FlipVertically, 0);
				}
			}
		}
		if (State == 1)
		{
			Rectangle frame = NPC.frame;
			Texture2D sorcererFevens = ModAsset.Fevens_Boss_MagicArray.Value;
			spriteBatch.Draw(sorcererFevens, NPC.Center - Main.screenPosition + new Vector2(0, -30), frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		}

		if (State == 2 || State == 3)
		{
			Rectangle frame = NPC.frame;
			Texture2D sorcererFevens = ModAsset.Fevens_Attack0.Value;
			if (State == 3)
			{
				sorcererFevens = ModAsset.Fevens_Attack1.Value;
			}
			spriteBatch.Draw(sorcererFevens, NPC.Center - Main.screenPosition + new Vector2(0, 0), frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		}

		if (State == 4)
		{
			Texture2D standFevens = ModAsset.Fevens_Boss.Value;
			Rectangle frame = new Rectangle(0, 0, standFevens.Width, standFevens.Height);
			spriteBatch.Draw(standFevens, NPC.Center - Main.screenPosition + new Vector2(0, 0), frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		}

		if (State == 5)
		{
			Texture2D knifeFevens = ModAsset.Fevens_Attack2.Value;
			Rectangle frame = new Rectangle(0, 0, knifeFevens.Width, knifeFevens.Height);
			spriteBatch.Draw(knifeFevens, NPC.Center - Main.screenPosition + new Vector2(0, 0), frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		}
		return false;
	}
}