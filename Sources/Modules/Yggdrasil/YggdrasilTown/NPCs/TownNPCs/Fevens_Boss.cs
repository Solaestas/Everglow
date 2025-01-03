using Everglow.Commons.Coroutines;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;

[AutoloadHead]
public class Fevens_Boss : ModNPC
{
	private bool canDespawn = false;

	/// <summary>
	/// 0 default; 1 hold magic array; 2 attack 1
	/// </summary>
	public int State;

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
		if (State == 0)
		{
			return true;
		}
		if (State == 1)
		{
			Rectangle frame = NPC.frame;
			Texture2D sorcererFevens = ModAsset.Fevens_Boss_MagicArray.Value;

			// Texture2D sorcererFevensFlame = ModAsset.Fevens_Boss_MagicArray_flame.Value;
			spriteBatch.Draw(sorcererFevens, NPC.Center - Main.screenPosition + new Vector2(0, -30), frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

			// spriteBatch.Draw(sorcererFevensFlame, NPC.Center - Main.screenPosition, frame, new Color(1f, 1f, 1f, 0), NPC.rotation, frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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
		return false;
	}
}