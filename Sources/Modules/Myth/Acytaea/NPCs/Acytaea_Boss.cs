using Everglow.Commons.Coroutines;
using Everglow.Myth.Acytaea.Projectiles;
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
		Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -5), Vector2.zeroVector, ModContent.ProjectileType<AcytaeaSword_projectile_Boss>(), 60, 4f, player.whoAmI, NPC.whoAmI);
		for (int t = 0; t < 60; t++)
		{
			float value = 0.05f;
			Vector2 targetPos = player.Center + new Vector2(-130 * NPC.direction, -60);
			NPC.position = targetPos * (value) + NPC.position * (1 - value);
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
			if(Math.Abs(NPC.velocity.X) > 1.5)
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
		for(int x = -7;x <= 7;x++)
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
		for (int t = 0; t < 30; t++)
		{
			NPC.velocity *= 0.1f;
			yield return new SkipThisFrame();
		}
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
			foreach(Projectile p in Main.projectile)
			{
				if(p!=null && p.active)
				{
					if(p.type == ModContent.ProjectileType<AcytaeaFlySword>())
					{
						AcytaeaFlySword acySword = p.ModProjectile as AcytaeaFlySword;
						if(newP0 != null)
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
		yield return new SkipThisFrame();
		_acytaeaCoroutine.StartCoroutine(new Coroutine(FaceToPlayer(20)));
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
		switch (Main.rand.Next(3))
		{
			case 0:
				_acytaeaCoroutine.StartCoroutine(new Coroutine(SwordArray()));
				break;
			case 1:
				_acytaeaCoroutine.StartCoroutine(new Coroutine(ScratchPlayer()));
				break;
			case 2:
				_acytaeaCoroutine.StartCoroutine(new Coroutine(SlashPlayer()));
				break;
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