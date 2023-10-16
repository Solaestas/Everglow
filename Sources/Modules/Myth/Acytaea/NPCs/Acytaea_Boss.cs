using Everglow.Commons.Coroutines;
using Everglow.Commons.CustomTiles;
using Everglow.Myth.Acytaea.Projectiles;
using Everglow.Myth.Acytaea.VFXs;
using Terraria.DataStructures;

namespace Everglow.Myth.Acytaea.NPCs;

[AutoloadHead]
public class Acytaea_Boss : ModNPC
{
	private bool canDespawn = false;
	public override string Texture => "Everglow/Myth/Acytaea/NPCs/Acytaea";
	public override string HeadTexture => "Everglow/Myth/Acytaea/NPCs/Acytaea_Head_Boss";

	public override void SetDefaults()
	{
		NPC.width = 34;
		NPC.height = 48;
		NPC.aiStyle = 7;
		NPC.damage = 100;
		NPC.defense = 100;
		NPC.lifeMax = 250000;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath6;
		NPC.knockBackResist = 0.5f;
		NPC.boss = true;
		NPC.knockBackResist = 0;
		NPC.noTileCollide = true;
		Music = Common.MythContent.QuickMusic("AcytaeaFighting");
	}
	public override void OnSpawn(IEntitySource source)
	{
		NPC.friendly = false;
		NPC.aiStyle = -1;

		NPC.lifeMax = 165000;
		NPC.life = 165000;
		if (Main.expertMode)
		{
			NPC.lifeMax = 275000;
			NPC.life = 275000;
		}
		if (Main.masterMode)
		{
			NPC.lifeMax = 385000;
			NPC.life = 385000;
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
	public override void OnKill()
	{
		NPC.SetEventFlagCleared(ref DownedBossSystem.downedAcytaea, -1);
		if (Main.netMode == NetmodeID.Server)
			NetMessage.SendData(MessageID.WorldData);
	}
	public override void AI()
	{
		Player player = Main.player[NPC.target];
		UpdateWings();
		_acytaeaCoroutine.Update();
		if (!player.active || player.dead)
		{
			NPC.active = false;
			NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Acytaea>());
		}
	}
	#region Boss
	private int wingFrame = 0;
	private int wingFrameCounter = 0;
	private CoroutineManager _acytaeaCoroutine = new CoroutineManager();
	public void StartToBeABoss()
	{
		NPC.TargetClosest(true);
		_acytaeaCoroutine.StartCoroutine(new Coroutine(StartFighting()));
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<AcytaeaSword_following>(), 0, 0f, -1, NPC.whoAmI);
	}
	public void UpdateWings()
	{
		wingFrameCounter++;
		if (wingFrameCounter > 6)
		{
			wingFrameCounter = 0;
			wingFrame++;
			if (wingFrame > 3)
			{
				wingFrame = 0;
			}
		}
	}
	private IEnumerator<ICoroutineInstruction> StartFighting()
	{
		Player player = Main.player[NPC.target];
		NPC.direction = 1;
		NPC.noGravity = true;
		if (player.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		for (int t = 0; t < 60; t++)
		{
			Vector2 targetPos = player.Center + new Vector2(-200 * NPC.direction, 0);
			NPC.position = targetPos * 0.05f + NPC.position * 0.95f;
			yield return new SkipThisFrame();
		}
		_acytaeaCoroutine.StartCoroutine(new Coroutine(SlashPlayer()));
	}
	//砍一刀
	private IEnumerator<ICoroutineInstruction> SlashPlayer()
	{
		Player player = Main.player[NPC.target];
		NPC.rotation = 0;
		NPC.direction = 1;
		if (Main.rand.NextBool(2))
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		for (int t = 0; t < 20; t++)
		{
			float value = t / 20f;
			Vector2 targetPos = player.Center + new Vector2(-130 * NPC.direction, -60);
			NPC.position = targetPos * (value) + NPC.position * (1 - value);
			yield return new SkipThisFrame();
		}
		ClearFollowingSword();
		Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -5), Vector2.zeroVector, ModContent.ProjectileType<AcytaeaSword_projectile_Boss>(), 60, 4f, player.whoAmI, NPC.whoAmI);
		for (int t = 0; t < 10; t++)
		{
			float value = 0.05f;
			Vector2 targetPos = player.Center + new Vector2(-130 * NPC.direction, -60);
			NPC.position = targetPos * (value) + NPC.position * (1 - value);
			yield return new SkipThisFrame();
		}
		for (int t = 0; t < 90; t++)
		{
			NPC.velocity *= 0.6f;
			yield return new SkipThisFrame();
		}
		_acytaeaCoroutine.StartCoroutine(new Coroutine(FaceToPlayer()));
		yield return new SkipThisFrame();
	}
	//龙爪划过
	private IEnumerator<ICoroutineInstruction> ScratchPlayer()
	{
		Player player = Main.player[NPC.target];
		NPC.rotation = 0;
		NPC.direction = 1;
		if (player.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		Vector2 aimPos = player.Center;
		Projectile.NewProjectile(NPC.GetSource_FromAI(), aimPos, Vector2.zeroVector, ModContent.ProjectileType<Acytaea_Lock>(), 0, 0f, player.whoAmI, NPC.whoAmI);

		for (int t = 0; t < 20; t++)
		{
			float value = t / 20f;
			NPC.position = (aimPos + new Vector2(-640 * NPC.direction, -240)) * (value) + NPC.position * (1 - value);
			yield return new SkipThisFrame();
		}
		Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<Acytaea_ShineStar>(), 0, 0f, player.whoAmI, NPC.whoAmI);
		Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<AcytaeaScratch>(), 60, 4f, player.whoAmI, NPC.whoAmI);
		NPC.velocity *= 0;
		for (int t = 0; t < 20; t++)
		{
			float value = t / 20f;
			NPC.velocity += value * Vector2.Normalize(aimPos - NPC.Center);
			NPC.rotation = NPC.velocity.X * 0.14f;
			if (Math.Abs(NPC.velocity.X) > 1.5)
			{
				NPC.rotation = 1.5f * Math.Sign(NPC.velocity.X);
			}
			yield return new SkipThisFrame();
		}
		for (int t = 0; t < 4; t++)
		{
			NPC.velocity *= 1.7f;
			NPC.rotation = NPC.velocity.X * 0.14f;
			if (Math.Abs(NPC.velocity.X) > 1.5)
			{
				NPC.rotation = 1.5f * Math.Sign(NPC.velocity.X);
			}
			yield return new SkipThisFrame();
		}
		yield return new WaitForFrames(6);
		for (int t = 0; t < 12; t++)
		{
			NPC.rotation = NPC.velocity.X * 0.14f;
			if (Math.Abs(NPC.velocity.X) > 1.5)
			{
				NPC.rotation = 1.5f * Math.Sign(NPC.velocity.X);
			}
			NPC.velocity *= 0.8f;
			NPC.velocity.Y -= 2.4f;
			yield return new SkipThisFrame();
		}
		_acytaeaCoroutine.StartCoroutine(new Coroutine(FaceToPlayer()));
		yield return new SkipThisFrame();
	}
	//3x龙爪划过
	private IEnumerator<ICoroutineInstruction> Scratch3Player()
	{
		Player player = Main.player[NPC.target];
		NPC.rotation = 0;
		NPC.direction = 1;
		if (player.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;



		for (int x = 0; x < 3; x++)
		{
			Vector2 aimPos = player.Center;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), aimPos, Vector2.zeroVector, ModContent.ProjectileType<Acytaea_Lock>(), 0, 0f, player.whoAmI, NPC.whoAmI);
			Vector2 aimPos2 = aimPos + new Vector2(-440 * NPC.direction, -40).RotatedBy(x * NPC.spriteDirection);
			for (int t = 0; t < 10; t++)
			{
				float value = t / 5f;
				NPC.position = aimPos2 * (value) + NPC.position * (1 - value);
				yield return new SkipThisFrame();
			}
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<Acytaea_ShineStar>(), 0, 0f, player.whoAmI, NPC.whoAmI);
			Projectile scratch = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<AcytaeaScratch>(), 60, 4f, player.whoAmI, NPC.whoAmI);
			scratch.timeLeft = 36;
			NPC.velocity *= 0;
			for (int t = 0; t < 15; t++)
			{
				float value = t / 15f;
				NPC.velocity += value * Vector2.Normalize(aimPos - NPC.Center) * 3.5f;
				NPC.rotation = NPC.velocity.X * 0.14f;
				if (Math.Abs(NPC.velocity.X) > 1.5)
				{
					NPC.rotation = 0.4f * Math.Sign(NPC.velocity.X);
				}
				yield return new SkipThisFrame();
			}
			for (int t = 0; t < 4; t++)
			{
				NPC.velocity *= 1.7f;
				NPC.rotation = NPC.velocity.X * 0.14f;
				if (Math.Abs(NPC.velocity.X) > 1.5)
				{
					NPC.rotation = 1.5f * Math.Sign(NPC.velocity.X);
				}
				yield return new SkipThisFrame();
			}
		}
		for (int t = 0; t < 12; t++)
		{
			NPC.rotation = NPC.velocity.X * 0.14f;
			if (Math.Abs(NPC.velocity.X) > 1.5)
			{
				NPC.rotation = 1.5f * Math.Sign(NPC.velocity.X);
			}
			NPC.velocity *= 0.8f;
			NPC.velocity.Y -= 2.4f;
			yield return new SkipThisFrame();
		}
		_acytaeaCoroutine.StartCoroutine(new Coroutine(FaceToPlayer()));
		yield return new SkipThisFrame();
	}
	//连续龙爪划过
	private IEnumerator<ICoroutineInstruction> ContinueScratchPlayer(int times = 5)
	{
		Player player = Main.player[NPC.target];
		NPC.rotation = 0;
		NPC.direction = 1;
		if (player.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		Vector2 aimPos = player.Center;

		Vector2 aimPos2 = aimPos + new Vector2(-440 * NPC.direction, -40);
		for (int t = 0; t < 10; t++)
		{
			float value = t / 5f;
			NPC.position = aimPos2 * (value) + NPC.position * (1 - value);
			yield return new SkipThisFrame();
		}


		for (int x = 0; x < times; x++)
		{
			aimPos = player.Center;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), aimPos, Vector2.zeroVector, ModContent.ProjectileType<Acytaea_Lock>(), 0, 0f, player.whoAmI, NPC.whoAmI);
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<Acytaea_ShineStar>(), 0, 0f, player.whoAmI, NPC.whoAmI);
			Projectile scratch = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<AcytaeaScratch>(), 60, 4f, player.whoAmI, NPC.whoAmI);
			scratch.timeLeft = 36;
			NPC.velocity *= 0;
			for (int t = 0; t < 15; t++)
			{
				float value = t / 15f;
				NPC.velocity += value * Vector2.Normalize(aimPos - NPC.Center) * 4.5f;
				NPC.rotation = NPC.velocity.X * 0.14f;
				if (Math.Abs(NPC.velocity.X) > 1.5)
				{
					NPC.rotation = 0.4f * Math.Sign(NPC.velocity.X);
				}
				yield return new SkipThisFrame();
			}
			for (int t = 0; t < 4; t++)
			{
				NPC.velocity *= 1.7f;
				NPC.rotation = NPC.velocity.X * 0.14f;
				if (Math.Abs(NPC.velocity.X) > 1.5)
				{
					NPC.rotation = 1.5f * Math.Sign(NPC.velocity.X);
				}
				yield return new SkipThisFrame();
			}
		}
		for (int t = 0; t < 12; t++)
		{
			NPC.rotation = NPC.velocity.X * 0.14f;
			if (Math.Abs(NPC.velocity.X) > 1.5)
			{
				NPC.rotation = 1.5f * Math.Sign(NPC.velocity.X);
			}
			NPC.velocity *= 0.8f;
			NPC.velocity.Y -= 2.4f;
			yield return new SkipThisFrame();
		}
		_acytaeaCoroutine.StartCoroutine(new Coroutine(FaceToPlayer()));
		yield return new SkipThisFrame();
	}
	//剑阵
	private IEnumerator<ICoroutineInstruction> SwordArray()
	{
		NPC.TargetClosest();
		Player player = Main.player[NPC.target];
		NPC.rotation = 0;
		NPC.direction = 1;
		if (player.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		for (int t = 0; t < 20; t++)
		{
			NPC.velocity *= 0.1f;
			float value = t / 20f;
			Vector2 targetPos = player.Center + new Vector2(0 * NPC.direction, -300);
			NPC.position = targetPos * (value) + NPC.position * (1 - value);
			yield return new SkipThisFrame();
		}
		for (int x = -7; x <= 7; x++)
		{
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -5), Vector2.zeroVector, ModContent.ProjectileType<AcytaeaSwordArray_0>(), 60, 4f, player.whoAmI, NPC.whoAmI, x / 3f, 120);
		}
		for (int t = 0; t < 40; t++)
		{
			NPC.velocity *= 0.1f;
			yield return new SkipThisFrame();
		}
		for (int x = -15; x <= 15; x++)
		{
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -5), Vector2.zeroVector, ModContent.ProjectileType<AcytaeaSwordArray_0>(), 60, 4f, player.whoAmI, NPC.whoAmI, x / 6f, -200);
		}
		var acytaeaLaserSwordHDREffect = new AcytaeaLaserSwordHDREffect
		{
			Active = true,
			Visible = true,
			maxTime = 300,
			timer = 0,
			owner = NPC
		};
		Ins.VFXManager.Add(acytaeaLaserSwordHDREffect);
		for (int t = 0; t < 30; t++)
		{
			NPC.velocity *= 0.1f;
			yield return new SkipThisFrame();
		}
		ClearFollowingSword();
		Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -5), Vector2.zeroVector, ModContent.ProjectileType<AcytaeaLaserSword>(), 60, 4f, player.whoAmI, NPC.whoAmI);
		p0.rotation = NPC.spriteDirection + MathHelper.PiOver2;
		for (int t = 0; t < 240; t++)
		{
			NPC.velocity *= 0.1f;
			NPC.direction = 1;
			if (player.Center.X < NPC.Center.X)
			{
				NPC.direction = -1;
			}
			NPC.spriteDirection = NPC.direction;
			Vector2 targetPos = player.Center + new Vector2(0 * NPC.direction, -300);
			NPC.position = targetPos * 0.01f + NPC.position * 0.99f;
			float aimRot = NPC.spriteDirection + MathHelper.PiOver2 - NPC.spriteDirection * 2 * t / 240f;
			p0.rotation = aimRot * 0.05f + p0.rotation * 0.95f;
			AcytaeaLaserSword newP0 = p0.ModProjectile as AcytaeaLaserSword;
			foreach (Projectile p in Main.projectile)
			{
				if (p != null && p.active)
				{
					if (p.type == ModContent.ProjectileType<AcytaeaFlySword>())
					{
						AcytaeaFlySword acySword = p.ModProjectile as AcytaeaFlySword;
						if (newP0 != null)
						{
							acySword.Aim = newP0.EndPos;
						}
						else
						{
							acySword.Aim = player.Center;
						}
					}
				}
			}
			yield return new SkipThisFrame();
		}
		yield return new WaitForFrames(60);
		_acytaeaCoroutine.StartCoroutine(new Coroutine(FaceToPlayer(20)));
	}
	//轮状剑阵
	private IEnumerator<ICoroutineInstruction> SwordArray_Round()
	{
		NPC.TargetClosest();
		Player player = Main.player[NPC.target];
		NPC.rotation = 0;
		NPC.direction = 1;
		if (player.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		for (int t = 0; t < 20; t++)
		{
			NPC.velocity *= 0.1f;
			float value = t / 20f;
			Vector2 targetPos = player.Center + new Vector2(-450 * NPC.direction, 240);
			while (Collision.SolidCollision(targetPos, NPC.width, NPC.height))
			{
				targetPos.Y -= 10;
				if (targetPos.Y < player.Center.Y - 200)
				{
					break;
				}
			}
			NPC.position = targetPos * (value) + NPC.position * (1 - value);
			yield return new SkipThisFrame();
		}
		ClearFollowingSword();	
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<AcytaeaMagicArraySword>(), 0, 4f, player.whoAmI, NPC.whoAmI);
		yield return new WaitForFrames(1);
		Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -455), Vector2.zeroVector, ModContent.ProjectileType<AcytaeaSwordArray_1>(), 60, 4f, player.whoAmI, NPC.whoAmI, 8, 1.6f);
		p0.spriteDirection = -1;
		yield return new WaitForFrames(15);
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -455), Vector2.zeroVector, ModContent.ProjectileType<AcytaeaSwordArray_1>(), 60, 4f, player.whoAmI, NPC.whoAmI, 12, 2.7f);
		yield return new WaitForFrames(15);
		Projectile p2 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -455), Vector2.zeroVector, ModContent.ProjectileType<AcytaeaSwordArray_1>(), 60, 4f, player.whoAmI, NPC.whoAmI, 20, 4.5f);
		p2.spriteDirection = -1;
		for (int t = 0; t < 40; t++)
		{
			NPC.velocity *= 0.1f;
			float value = 0.15f;
			Vector2 targetPos = player.Center + new Vector2(-450 * NPC.direction, 240);
			while (Collision.SolidCollision(targetPos, NPC.width, NPC.height))
			{
				targetPos.Y -= 10;
				if (targetPos.Y < player.Center.Y - 200)
				{
					break;
				}
			}
			NPC.position = targetPos * (value) + NPC.position * (1 - value);
			yield return new SkipThisFrame();
		}
		for (int t = 0; t < 220; t++)
		{
			NPC.velocity *= 0.8f;
			yield return new SkipThisFrame();
		}
		_acytaeaCoroutine.StartCoroutine(new Coroutine(FaceToPlayer(20)));
	}
	//龙卷
	private IEnumerator<ICoroutineInstruction> SwordArray_Tornado()
	{
		Player player = Main.player[NPC.target];
		NPC.rotation = 0;
		NPC.direction = 1;
		if (Main.rand.NextBool(2))
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		for (int t = 0; t < 20; t++)
		{
			float value = t / 20f;
			Vector2 targetPos = player.Center + new Vector2(-130 * NPC.direction, -60);
			NPC.position = targetPos * (value) + NPC.position * (1 - value);
			yield return new SkipThisFrame();
		}
		ClearFollowingSword();
		Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -5), Vector2.zeroVector, ModContent.ProjectileType<AcytaeaSword_projectile_Tornado>(), 60, 4f, player.whoAmI, NPC.whoAmI);
		for (int t = 0; t < 330; t++)
		{
			NPC.velocity.Y *= 0.5f;
			float value = 0;
			foreach(var proj in Main.projectile)
			{
				if(proj != null && proj.active)
				{
					if(proj.type == ModContent.ProjectileType<AcytaeaSword_projectile_Tornado>())
					{
						AcytaeaSword_projectile_Tornado aSPT = proj.ModProjectile as AcytaeaSword_projectile_Tornado;
						if(aSPT.mainVec.X > 0)
						{
							NPC.spriteDirection = 1;
						}
						else
						{
							NPC.spriteDirection = -1;
						}
						value = aSPT.Omega;
						break;
					}
				}
			}
			if(t == 319)
			{
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(Math.Sign((player.Center - NPC.Center).X) * 240, 0), new Vector2(Math.Sign((player.Center - NPC.Center).X) * 60, 0), ModContent.ProjectileType<AcytaeaTornado>(), 60, 4f, player.whoAmI, NPC.whoAmI);
			}
			Vector2 targetPos = player.Center;
			Vector2 toTarget = targetPos - NPC.Center - NPC.velocity;
			if(toTarget.Length() > 200)
			{
				toTarget = Utils.SafeNormalize(toTarget, Vector2.zeroVector) * 200;
			}
			toTarget *= 0.4f * value;
			NPC.velocity = toTarget * 0.05f + NPC.velocity * 0.95f;
			yield return new SkipThisFrame();
		}
		_acytaeaCoroutine.StartCoroutine(new Coroutine(FaceToPlayer()));
	}
	private IEnumerator<ICoroutineInstruction> FaceToPlayer(int time = 60)
	{
		Player player = Main.player[NPC.target];
		NPC.direction = 1;
		NPC.noGravity = true;
		if (player.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		for (int t = 0; t < time; t++)
		{
			NPC.rotation *= 0.95f;
			Vector2 targetPos = player.Center + new Vector2(-200 * NPC.direction, 0);
			NPC.position = targetPos * 0.05f + NPC.position * 0.95f;
			yield return new SkipThisFrame();
		}


		if (NPC.life > NPC.lifeMax * 0.5f)
		{
			switch (Main.rand.Next(3))
			{
				case 0:
					_acytaeaCoroutine.StartCoroutine(new Coroutine(ScratchPlayer()));
					break;
				case 1:
					_acytaeaCoroutine.StartCoroutine(new Coroutine(SlashPlayer()));
					break;
				case 2:
					_acytaeaCoroutine.StartCoroutine(new Coroutine(Scratch3Player()));
					break;
			}
		}
		else
		{
			switch (Main.rand.Next(5))
			{
				case 0:
					_acytaeaCoroutine.StartCoroutine(new Coroutine(SwordArray_Round()));
					break;
				case 1:
					_acytaeaCoroutine.StartCoroutine(new Coroutine(ContinueScratchPlayer(Main.rand.Next(4, 13))));
					break;
				case 2:
					_acytaeaCoroutine.StartCoroutine(new Coroutine(SwordArray_Tornado()));
					break;
				case 3:
					_acytaeaCoroutine.StartCoroutine(new Coroutine(SlashPlayer()));
					break;
				case 4:
					_acytaeaCoroutine.StartCoroutine(new Coroutine(SwordArray()));
					break;
			}
		}
	}
	private void ClearFollowingSword()
	{
		foreach(var proj in Main.projectile)
		{
			if(proj != null && proj.active)
			{
				if(proj.type == ModContent.ProjectileType<AcytaeaSword_following>())
				{
					proj.Kill();
				}
			}
		}
	}
	public void DrawSelfBoss(SpriteBatch spriteBatch, Color drawColor)
	{
		Vector2 drawPos = NPC.Center - Main.screenPosition;
		Vector2 commonOrigin = NPC.Hitbox.Size() / 2f;
		if (NPC.spriteDirection == -1)
		{
			drawPos += new Vector2(-10, 0);
		}
		SpriteEffects drawEffect = SpriteEffects.None;
		Vector2 wingorigin = commonOrigin + new Vector2(10, 0);
		if (NPC.spriteDirection == 1)
		{
			drawEffect = SpriteEffects.FlipHorizontally;
			wingorigin = commonOrigin + new Vector2(26, 0);
		}
		Texture2D backWing = ModAsset.AcytaeaBackWing.Value;
		Texture2D frontWing = ModAsset.AcytaeaFrontWing.Value;
		Texture2D backArm = ModAsset.AcytaeaBackArm.Value;
		Texture2D body = ModAsset.AcytaeaBody.Value;
		Texture2D legs = ModAsset.AcytaeaLeg.Value;
		Texture2D frontArm = ModAsset.AcytaeaFrontArm.Value;
		Texture2D head = ModAsset.AcytaeaHead.Value;
		Texture2D[] backWingFrames = new Texture2D[] { ModAsset.AcytaeaBackWing_0.Value, ModAsset.AcytaeaBackWing_1.Value, ModAsset.AcytaeaBackWing_2.Value, ModAsset.AcytaeaBackWing_3.Value };
		Texture2D[] frontWingFrames = new Texture2D[] { ModAsset.AcytaeaFrontWing_0.Value, ModAsset.AcytaeaFrontWing_1.Value, ModAsset.AcytaeaFrontWing_2.Value, ModAsset.AcytaeaFrontWing_3.Value };
		spriteBatch.Draw(backWing, drawPos, new Rectangle(0, 56 * wingFrame, 86, 56), drawColor, NPC.rotation, wingorigin, 1f, drawEffect, 0);
		spriteBatch.Draw(frontWing, drawPos, new Rectangle(0, 56 * wingFrame, 86, 56), drawColor, NPC.rotation, wingorigin, 1f, drawEffect, 0);
		spriteBatch.Draw(backArm, drawPos, null, drawColor, NPC.rotation, commonOrigin, 1f, drawEffect, 0);
		spriteBatch.Draw(body, drawPos, null, drawColor, NPC.rotation, commonOrigin, 1f, drawEffect, 0);
		spriteBatch.Draw(legs, drawPos, null, drawColor, NPC.rotation, commonOrigin, 1f, drawEffect, 0);
		spriteBatch.Draw(frontArm, drawPos, null, drawColor, NPC.rotation, commonOrigin, 1f, drawEffect, 0);
		spriteBatch.Draw(head, drawPos, null, drawColor, NPC.rotation, commonOrigin, 1f, drawEffect, 0);
	}
	#endregion
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		DrawSelfBoss(spriteBatch, drawColor);
		return false;
	}
}