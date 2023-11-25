using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using Everglow.Myth.TheFirefly.Items.BossDrop;
using Everglow.Myth.TheFirefly.Items.Weapons;
using Everglow.Myth.TheFirefly.Projectiles;
using Everglow.Myth.TheFirefly.VFXs;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Everglow.Myth.TheFirefly.NPCs.Bosses;

[AutoloadBossHead]
public class CorruptMoth : ModNPC
{
	public static NPC CorruptMothNPC;
	public override bool CloneNewInstances => true;

	private readonly Color IdentifierValue = new Color(58, 169, 255);

	private static List<ImageKeyPoint> BBowColors;
	private const int BBowColorsWidth = 60;
	private const int BBowColorsHeight = 60;

	private static List<ImageKeyPoint> BArrowColors;
	private const int BArrowColorsWidth = 60;
	private const int BArrowColorsHeight = 60;

	private static List<ImageKeyPoint> BSwordColors;
	private const int BSwordColorsWidth = 60;
	private const int BSwordColorsHeight = 60;

	private static List<ImageKeyPoint> BFistColors;
	private const int BFistColorsWidth = 60;
	private const int BFistColorsHeight = 60;
	private static bool startLoading = false;

	private bool canDespawn = true;
	private bool start = false;
	private float lightVisual = 0;

	//public static int secondStageHeadSlot = -1;
	//public static int StaTime = 0;
	private float PhamtomDis//特效幻影的距离和透明度
	{
		get => NPC.localAI[2];
		set => NPC.localAI[2] = value;
	}

	[CloneByReference]
	private readonly Vector3[] cubeVec = new Vector3[]
	{
		new Vector3(1,1,1),
		new Vector3(1,1,-1),
		new Vector3(1,-1,-1),
		new Vector3(1,-1,1),
		new Vector3(-1,1,1),
		new Vector3(-1,1,-1),
		new Vector3(-1,-1,-1),
		new Vector3(-1,-1,1)
	};

	private int timer
	{
		set => NPC.ai[1] = value;
		get => (int)NPC.ai[1];
	}

	private Player Player => Main.player[NPC.target];
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 11;
		var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
		{
			CustomTexturePath = "Everglow/Myth/TheFirefly/NPCs/Bosses/CorruptMothBoss",
			Position = new Vector2(20f, 24f),
			PortraitPositionXOverride = 0f,
			PortraitPositionYOverride = 12f
		};
		NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
		NPCID.Sets.TrailCacheLength[NPC.type] = 4;
	}
	public override void SetDefaults()
	{
		NPC.behindTiles = true;
		NPC.damage = 40;
		NPC.width = 120;
		NPC.height = 120;
		NPC.defense = 5;
		NPC.lifeMax = 9000;
		NPC.npcSlots = 80;
		NPC.scale = 0.8f;
		if (Main.getGoodWorld)
		{
			NPC.defense += 10;
			NPC.scale = 1.2f;
		}
		if (Main.expertMode)
		{
			NPC.lifeMax = 6000;
			NPC.damage = 42;
		}
		if (Main.masterMode)
		{
			NPC.lifeMax = 5000;
			NPC.damage = 44;
		}
		NPC.knockBackResist = 0f;
		NPC.value = Item.buyPrice(0, 5, 0, 0);
		NPC.color = new Color(0, 0, 0, 0);
		NPC.alpha = 0;
		NPC.aiStyle = -1;
		NPC.boss = true;
		NPC.lavaImmune = true;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.HitSound = SoundID.NPCHit23;
		NPC.DeathSound = SoundID.NPCDeath52;
		NPC.dontTakeDamage = false;
		NPC.dontTakeDamageFromHostiles = true;
		NPCID.Sets.TrailCacheLength[NPC.type] = 4;
		//NPCID.Sets.TrailingMode[NPC.type] = 0;
		if (!Main.dedServ)
		{
			Music = MythContent.QuickMusic("MothFighting");
		}
	}
	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
		{
			// Sets the spawning conditions of this NPC that is listed in the bestiary.
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,

			// Sets the description of this NPC that is listed in the bestiary.

			new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.Everglow.Bestiary.CorruptMoth.Flavor"))
		});
	}
	public override void OnKill()
	{
		//NPC.SetEventFlagCleared(ref DownedBossSystem.downedMoth, -1);
		if (Main.netMode == NetmodeID.Server)
			NetMessage.SendData(MessageID.WorldData);
	}
	public override bool CheckActive()
	{
		return canDespawn;
	}
	public override void BossHeadSlot(ref int index)
	{
	}
	/// <summary>
	/// 发射弹幕，调用前检测服务端
	/// </summary>
	private void Create4DCube()
	{
		int scale = 300;
		int counts = 5;
		for (int w = -scale; w <= scale; w += scale * 2)
		{
			for (int ii = 0; ii < 4; ii++)
			{
				for (int a = 0; a <= 4; a += 4)
				{
					Vector3 v1 = cubeVec[ii + a] * scale;
					Vector3 v2 = cubeVec[(ii + 1) % 4 + a] * scale;
					for (int i = 0; i < counts - 1; i++)
					{
						var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CorMoth4DProj>(), NPC.damage / 7, 0, Main.myPlayer, NPC.whoAmI);
						(proj.ModProjectile as CorMoth4DProj).targetPos = new Vector4(Vector3.Lerp(v1, v2, (float)i / (counts - 1)), w);
						proj.netUpdate2 = true;
					}
				}
				Vector3 v3 = cubeVec[ii] * scale;
				Vector3 v4 = cubeVec[ii + 4] * scale;
				for (int i = 1; i < counts - 1; i++)
				{
					var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CorMoth4DProj>(), NPC.damage / 7, 0, Main.myPlayer, NPC.whoAmI);
					(proj.ModProjectile as CorMoth4DProj).targetPos = new Vector4(Vector3.Lerp(v3, v4, (float)i / (counts - 1)), w);
					proj.netUpdate2 = true;
				}
			}
		}

		for (int a = 0; a < cubeVec.Length; a++)
		{
			Vector3 v = cubeVec[a];
			for (int i = 1; i < counts - 1; i++)
			{
				float c = (counts - 1) / 2;
				var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CorMoth4DProj>(), NPC.damage / 7, 0, Main.myPlayer, NPC.whoAmI);
				(proj.ModProjectile as CorMoth4DProj).targetPos = new Vector4(v * scale, (float)(i - c) * scale / c);
				proj.netUpdate2 = true;
			}
		}
	}
	public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
	{
		if (NPC.ai[0] == 6 && timer > 260)
		{
			modifiers.FinalDamage *= 0.0001f;
			SoundEngine.PlaySound(SoundID.NPCHit4, NPC.Center);
			if (lightVisual < 0.6f)
				lightVisual += 0.5f;

			if (Main.rand.NextBool() && Main.netMode != NetmodeID.MultiplayerClient)
				Projectile.NewProjectile(NPC.GetSource_OnHurt(projectile), projectile.Center, (projectile.Center - NPC.Center).SafeNormalize(Vector2.One) * 12, ModContent.ProjectileType<BlueMissil>(), NPC.damage / 6, 0, Main.myPlayer); // Higher number on NPC.damage / int = less damage
		}
	}

	public override void AI()
	{
		if (!startLoading)
		{
			startLoading = true;
			Task.Factory.StartNew(() =>
			{
				BBowColors = ImageReader.ReadImageKeyPoints("Everglow/Myth/TheFirefly/Projectiles/BBow", IdentifierValue);
				BArrowColors = ImageReader.ReadImageKeyPoints("Everglow/Myth/TheFirefly/Projectiles/BArrow", IdentifierValue);
				BSwordColors = ImageReader.ReadImageKeyPoints("Everglow/Myth/TheFirefly/Projectiles/BSword", IdentifierValue);
				BFistColors = ImageReader.ReadImageKeyPoints("Everglow/Myth/TheFirefly/Projectiles/BFist", IdentifierValue);
			});
		}
		bool phase2 = NPC.life < NPC.lifeMax * 0.6f;
		Lighting.AddLight(NPC.Center, 0f, 0f, 0.1f * (1 - NPC.alpha / 255f));
		NPC.friendly = NPC.dontTakeDamage;
		if (lightVisual > 0)//光效
			lightVisual -= 0.04f;
		else
		{
			lightVisual = 0;
		}
		if (timer % 15 == 0)
			NPC.netUpdate2 = true;
		//贴图旋转
		if (NPC.spriteDirection > 0)
			NPC.rotation = NPC.velocity.Y / 20;
		else
		{
			NPC.rotation = -NPC.velocity.Y / 20;
		}
		if (Math.Abs(NPC.rotation) > 1.2f)
			NPC.rotation = Math.Sign(NPC.rotation) * 1.2f;

		#region #前言

		if (!start)
		{
			if (CorruptMothNPC != null)
			{
				if (CorruptMothNPC.active && NPC != CorruptMothNPC)
				{
					NPC.active = false;
					return;
				}
			}
			CorruptMothNPC = NPC;
			NPC.ai[0] = 0;
			NPC.noTileCollide = true;
			start = true;
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int h = 0; h < 15; h++)
				{
					NPC.NewNPC(null, (int)NPC.Center.X + 25, (int)NPC.Center.Y + 150, ModContent.NPCType<MothSummonEffect>());
				}
			}
			NPC.localAI[0] = 0;
			return;
		}
		//StaTime++;
		Player player = Main.player[NPC.target];
		NPC.TargetClosest(false);
		if (!player.active || player.dead)
		{
			NPC.TargetClosest(false);
			player = Main.player[NPC.target];
			if (!player.active || player.dead)
			{
				NPC.velocity = new Vector2(0f, 10f);
				if (NPC.timeLeft > 150)
					NPC.timeLeft = 150;
				return;
			}
		}

		#endregion #前言

		if (NPC.ai[0] == 0)
		{
			NPC.dontTakeDamage = true;
			NPC.noTileCollide = false;
			NPC.noGravity = false;
			PhamtomDis = (200 - timer) * 120f / 200;
			if (timer > 50)
			{
				NPC.noGravity = true;
				NPC.velocity *= 0.9f;
			}
			if (++timer > 200)
			{
				NPC.dontTakeDamage = false;
				NPC.noTileCollide = true;
				NPC.noGravity = true;
				NPC.ai[0]++;
				timer = 0;
			}
		}//生成
		if (NPC.ai[0] == 1)
		{
			if (++timer < 200)
			{
				MoveTo(player.Center, 5, 40);
				GetDir_ByPlayer();
			}
			if (timer == 200)
				NPC.ai[2] = phase2 ? 1 : 0;
			if (timer > 200 && timer < 650 && NPC.ai[2] == 0)//冲刺
			{
				int tt = (timer - 200) % 150;
				var getVec = new Vector2(NPC.direction);
				if (timer > 500)
					getVec = new Vector2(NPC.direction, 0);

				if (tt < 50)
				{
					GetDir_ByPlayer();
					MoveTo(player.Center - getVec * 300, 15, 15);
				}
				if (tt is > 50 and < 70)//前摇
					NPC.velocity = Vector2.Lerp(NPC.velocity, -getVec * 10, 0.1f);
				if (tt is > 30 and < 70)
					PhamtomDis += (50 - tt) * 0.5f;
				if (tt == 70)
					lightVisual = 2;
				if (tt is > 70 and < 120)//冲刺
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						GenerateGrayVFX(1);
						if (timer > 500 && timer % 10 == 0)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer);
							if (Main.getGoodWorld)
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(-1.4f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(1.4f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
							}
						}
					}
					for (int d = 0; d < 2; d++)
					{
						float scale = Main.rand.NextFloat(0.8f, 2.7f);
						Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<BlueGlow>(), NPC.velocity.X, NPC.velocity.Y, 0, default, scale);
						dust.position += NPC.velocity.RotatedBy(-Math.PI * 0.75 * NPC.spriteDirection + Main.rand.NextFloat(-0.4f, 0.4f)) * MathF.Cos(NPC.frame.Y / 5016f * MathF.PI * 2) * 6;
						dust.velocity = NPC.velocity * scale * 0.3f;
					}
					if (timer > 500)
						NPC.velocity = Vector2.Lerp(NPC.velocity, getVec * 20, 0.15f);
					else if (Vector2.Distance(NPC.Center, player.Center) > 100)
					{
						NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 20, 0.1f);
					}
				}
				if (tt > 120)
				{
					if (Vector2.Distance(NPC.Center, player.Center) > 600)
						timer++;

					GetDir_ByPlayer();
					MoveTo(player.Center, 6, 20);
				}
			}
			if (timer > 200 && timer < 500 && NPC.ai[2] == 1)//二阶段冲刺
			{
				int tt = timer % 100;
				var getVec = new Vector2(NPC.direction);
				if (timer > 400)
					getVec = new Vector2(NPC.direction, 0);

				if (tt < 20)
				{
					GetDir_ByPlayer();
					MoveTo(player.Center + player.velocity * 10 - getVec * 300, 15, 15);
				}
				if (tt is > 20 and < 40)//前摇
					NPC.velocity = Vector2.Lerp(NPC.velocity, -getVec * 10, 0.1f);
				if (tt is > 0 and < 40)
					PhamtomDis += (20 - tt) * 0.5f;
				if (tt == 40)
					lightVisual = 2;
				if (tt is > 40 and < 80)//冲刺
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						GenerateGrayVFX(1);
						if (timer > 400 && timer % 6 == 0)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer);
							if (Main.getGoodWorld)
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(-1.4f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(1.4f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
							}
						}
					}
					for (int d = 0; d < 2; d++)
					{
						float scale = Main.rand.NextFloat(0.8f, 2.7f);
						Dust dust = Dust.NewDustDirect(-NPC.position, NPC.width, NPC.height, ModContent.DustType<BlueGlow>(), NPC.velocity.X, NPC.velocity.Y, 0, default, scale);
						dust.position += NPC.velocity.RotatedBy(Math.PI * 0.75 * NPC.spriteDirection + Main.rand.NextFloat(-0.4f, 0.4f)) * MathF.Cos(NPC.frame.Y / 5016f * MathF.PI * 2) * 6;
						dust.velocity = NPC.velocity * scale * 0.3f;
					}
					if (timer > 500)
						NPC.velocity = Vector2.Lerp(NPC.velocity, getVec * 20, 0.15f);
					else if (Vector2.Distance(NPC.Center, player.Center) > 100)
					{
						NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 21, 0.1f);
					}
				}
				if (tt > 80)
				{
					if (Vector2.Distance(NPC.Center, player.Center) > 600)
						timer++;

					GetDir_ByPlayer();
					MoveTo(player.Center, 6, 20);
				}
			}
			if (timer > (NPC.ai[2] == 1 ? 510 : 680))
			{
				PhamtomDis = 0;
				NPC.ai[2] = 0;
				NPC.ai[0]++;
				timer = 0;
				if (phase2)
					timer += 80;
			}
		}//冲刺1
		if (NPC.ai[0] == 2)
		{
			if (++timer < 100)
			{
				MoveTo(player.Center, 5, 40);
				GetDir_ByPlayer();
			}
			if (timer is > 100 and < 550)
			{
				int tt = (timer - 100) % 150;
				if (tt < 20)
					NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(0, -8), 0.1f);

				if (tt is > 20 and < 120)
				{
					int Freq = 27;
					if (Main.expertMode)
						Freq = 20;
					if (Main.masterMode)
						Freq = 16;
					if (Main.getGoodWorld)
						Freq -= 4;
					GetDir_ByVel();
					if (!phase2)
						MoveTo(player.Center + new Vector2(0, -230), timer > 400 ? 18 : 10, 30);
					else
					{
						MoveTo(player.Center + new Vector2(0, -200), timer > 400 ? 22 : 13, 30);
					}
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<MothBlue>(), NPC.velocity.X, NPC.velocity.Y, 0, default, Main.rand.NextFloat(0.8f, 1.7f));
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<MothBlue2>(), NPC.velocity.X, NPC.velocity.Y, 0, default, Main.rand.NextFloat(0.8f, 1.7f));
					if (timer % Freq == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, 1), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
						if (Main.expertMode && !Main.masterMode)
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
						if (Main.masterMode)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(-1.4f, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(1.4f, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
						}
						if (Main.getGoodWorld)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(-1.8f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1); //Originally: NPC.damage / 4
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(1.8f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
						}
					}
				}
				if (tt is > 120 and < 150)
					MoveTo(player.Center + NPC.DirectionFrom(player.Center) * 150, 10, 20);
			}
			if (timer > 550)
			{
				timer = 0;
				if (NPC.life < NPC.lifeMax * 0.9f)
					NPC.ai[0]++;
				else
				{
					NPC.ai[0] = 1;
				}
			}
		}//光球
		if (NPC.ai[0] == 3)
		{
			int endTime = 220;
			int counts = 2;
			if (phase2)
			{
				counts = 3;
				endTime = 150;
			}
			if (++timer < 30)
			{
				NPC.alpha += (int)(255 / 30f);
				PhamtomDis += 5;
				MoveTo(player.Center, 5, 40);
				NPC.dontTakeDamage = true;
			}
			if (timer == 40)
			{
				NPC.Center = player.Center + Main.rand.NextVector2Unit() * new Vector2(1.4f, 1f) * 300;
				PhamtomDis = 0;
				NPC.alpha = 255;
				NPC.netUpdate2 = true;
				if (phase2)
					timer += 20;
			}
			if (timer is > 60 and < 90)
			{
				if (timer == 70)
					NPC.dontTakeDamage = false;

				NPC.alpha -= (int)(255 / 30f);
				PhamtomDis += 75 - timer;
				NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionFrom(player.Center) * 10, 0.08f);
			}
			if (timer == 90)
				lightVisual = 2;

			if (timer is > 90 and < 130)//冲刺中
			{
				GenerateGrayVFX(1);
				GetDir_ByVel();
				for (int d = 0; d < 2; d++)
				{
					float scale = Main.rand.NextFloat(0.8f, 2.7f);
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<BlueGlow>(), NPC.velocity.X, NPC.velocity.Y, 0, default, scale);
					dust.position += NPC.velocity.RotatedBy(-Math.PI * 0.75 * NPC.spriteDirection + Main.rand.NextFloat(-0.4f, 0.4f)) * MathF.Cos(NPC.frame.Y / 5016f * MathF.PI * 2) * 6;
					dust.velocity = NPC.velocity * scale * 0.3f;
				}
				if (timer % 8 == 0 && (NPC.ai[2] == 2 || phase2) && Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer);
					if (Main.getGoodWorld)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(-1.4f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(1.4f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
					}
				}

				if (Vector2.Distance(NPC.Center, player.Center) > 100)
					NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 20, 0.1f);
			}
			if (timer > 130 && timer < endTime)
			{
				MoveTo(player.Center, 5, 40);
				GetDir_ByPlayer();
			}
			if (timer > endTime)
			{
				timer = 0;
				if (++NPC.ai[2] >= counts)
				{
					NPC.ai[0]++;
					NPC.ai[2] = 0;
				}
			}
		}//瞬移冲刺
		if (NPC.ai[0] == 4)
		{
			if (PhamtomDis > 0)
				PhamtomDis -= 1;

			if (++timer < 60)
			{
				MoveTo(player.Center + new Vector2(0, -200), 10, 20);
				GetDir_ByPlayer();
			}
			else if (timer == 60)
			{
				NPC.ai[2] = phase2 ? 1 : 0;
			}
			if (timer >= 60)
			{   //90,240,390
				//140,240,340
				if ((timer + 60) % (NPC.ai[2] == 1 ? 100 : 150) == 0)
				{
					//NPC.Center += NPC.DirectionTo(player.Center)*Main.rand.Next(100,150);
					for (int y = 0; y < 60; y++)
					{
						int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
						Main.dust[index].noGravity = true;
						Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)//发射弹幕
					{
						lightVisual = 1.5f;
						PhamtomDis = 80;
						int style = Main.rand.Next(3);
						float r = Main.rand.NextFloat() * 10;
						if (style == 0)
						{
							int c = phase2 ? 6 : 5;
							for (int i = 0; i < c; i++)
							{
								for (int j = -3; j <= 3; j++)
								{
									Vector2 v = new Vector2(0.1f + j * 0.11f, 0).RotatedBy(j * 0.15f + i * MathHelper.TwoPi / c + r);
									Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<BlackCorruptRain3>(), NPC.damage / 6, 0f, Main.myPlayer, Main.rand.NextFloat(MathF.PI * 2)); //Originally: NPC.damage / 5
								}
							}
						}
						if (style == 1)
						{
							if (phase2)
							{
								for (int i = 0; i < 40; i++)
								{
									Vector2 v = new Vector2(0.1f + i % 5 / 16f, 0).RotatedBy(i * MathHelper.TwoPi / 40 + r);
									Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<BlackCorruptRain3>(), NPC.damage / 6, 0f, Main.myPlayer, Main.rand.NextFloat(MathF.PI * 2));
								}
							}
							else
							{
								for (int i = 0; i < 30; i++)
								{
									Vector2 v = new Vector2(0.1f + i % 5 / 16f, 0).RotatedBy(i * MathHelper.TwoPi / 30 + r);
									Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<BlackCorruptRain3>(), NPC.damage / 6, 0f, Main.myPlayer, Main.rand.NextFloat(MathF.PI * 2));
								}
							}
						}
						if (style == 2)
						{
							int c = phase2 ? 60 : 50;
							for (int i = 0; i < c; i++)
							{
								Vector2 v = new Vector2(0.18f + (float)Math.Sin(i * MathHelper.TwoPi / 10) * 0.17f, 0).RotatedBy(i * MathHelper.TwoPi / c + r);
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<BlackCorruptRain3>(), NPC.damage / 6, 0f, Main.myPlayer, Main.rand.NextFloat(MathF.PI * 2));
							}
						}
					}
				}
				MoveTo(player.Center + new Vector2(0, -300), 8, 40);
				GetDir_ByPlayer();
			}
			if (timer >= 400)
			{
				PhamtomDis = 0;
				NPC.ai[2] = 0;
				timer = 0;
				if (phase2)
					NPC.ai[0]++;
				else
				{
					NPC.ai[0] = 1;
				}
			}
		}//弹幕
		if (NPC.ai[0] is 5 or 7)
		{
			if (++timer < 50)
			{
				MoveTo(player.Center, 8, 20);
				GetDir_ByPlayer();
				if (timer > 20)
					PhamtomDis = MathHelper.Lerp(PhamtomDis, 120, 0.1f);
			}

			if (timer is > 50 and < 80)
			{
				StraightMoveTo(player.Center + NPC.DirectionFrom(player.Center) * 200, 0.1f);
				GetDir_ByPlayer();
			}
			if (timer is > 70 and < 90)
				PhamtomDis = MathHelper.Lerp(PhamtomDis, 0, 0.1f);
			if (timer is > 80 and < 90)
				NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center).RotatedBy(NPC.spriteDirection * 1.57f) * 20f, 0.1f);
			if (timer is > 90 and < 140)
			{
				lightVisual = 1;
				SpinAI(NPC, player.Center, NPC.spriteDirection * MathHelper.TwoPi / 30, true);
				int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
				if (timer % 2 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.05f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
					if (Main.getGoodWorld)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.15f + new Vector2(-1.4f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.15f + new Vector2(1.4f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
					}
				}
				//GetDir_ByVel();
			}
			if (timer > 140)
			{
				NPC.velocity *= 0.5f;
				NPC.ai[0]++;

				timer = 0;
			}
		}//绕玩家转圈发弹幕
		if (NPC.ai[0] == 6)
		{
			if (++timer < 60)
			{
				MoveTo(player.Center + new Vector2(0, -230), 15, 20);
				GetDir_ByPlayer();
			}
			if (timer == 60)
			{
				lightVisual += 1;
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Create4DCube();
			}
			if (timer >= 60 && timer <= 210 && (timer - 10) % 50 == 0)
			{
				int counts = timer / 10;
				for (int y = 0; y < counts; y++)
				{
					int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
					Main.dust[index].noGravity = true;
					Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
				}
			}
			if (timer == 60 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < 30; i++)
				{
					Vector2 vel = (i * MathHelper.TwoPi / 30).ToRotationVector2();
					var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<ButterflyDream>(), NPC.damage / 10, 0, Main.myPlayer, NPC.whoAmI, 1); //This circles around the 4D Cube
					proj.timeLeft = 800;
					proj.netUpdate2 = true;
				}
			}
			if (timer is > 60 and < 260)
			{
				PhamtomDis = MathHelper.Lerp(PhamtomDis, 120, 0.02f);
				NPC.velocity *= 0.95f;
				GetDir_ByPlayer();
			}
			if (timer is > 260 and < 960)
			{
				PhamtomDis = MathHelper.Lerp(PhamtomDis, 0, 0.02f);
				MoveTo(player.Center, 3, 20);
				GetDir_ByPlayer();
			}
			if (timer > 960)
			{
				timer = 0;
				if (NPC.life < NPC.lifeMax * 0.5f)
					NPC.ai[0]++;
				else
				{
					NPC.ai[0] = 1;
					timer += 150;
				}
			}
		}//超立方体
		if (NPC.ai[0] == 8)
		{
			if (++timer < 60)
				MoveTo(player.Center - new Vector2(0, 500), 10, 20);
			if (timer is > 60 and < 120)
			{
				GetDir_ByPlayer();
				MoveTo(player.Center - new Vector2(+player.velocity.X * 10, 500), 40, 20);
			}
			if (timer == 120)
			{
				NPC.velocity = new Vector2(0, 25);
				NPC.ai[2] = 6;//ai2->发射数量
			}
			if (timer is >= 120 and <= 160)
			{
				if (timer % 8 == 0)
				{
					if (timer < 140)
						NPC.ai[2] += 2;
					else
					{
						NPC.ai[2] -= 2;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						float r = Main.rand.NextFloat(0, 1f);
						for (int i = 0; i < NPC.ai[2]; i++)
						{
							Vector2 vel = (r + i * MathHelper.TwoPi / NPC.ai[2]).ToRotationVector2();
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel * NPC.ai[2] / 2, ModContent.ProjectileType<ButterflyDream>(), NPC.damage / 10, 0, Main.myPlayer, -vel.Y);

							for (int z = 0; z < 3; z++)
							{
								r = Main.rand.NextFloat(0, 1f);
								vel = (r + i * MathHelper.TwoPi / NPC.ai[2]).ToRotationVector2();
								vel.Y *= 0.5f;
								int ra = Dust.NewDust(NPC.Center, 0, 0, ModContent.DustType<PureBlue>(), 0, 0, 0, default, 4f * Main.rand.NextFloat(0.7f, 2.9f));
								Main.dust[ra].noGravity = true;
								Main.dust[ra].velocity = vel * NPC.ai[2] * 8;

								int index = Dust.NewDust(NPC.Center, 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.3f, 2.2f));
								Main.dust[index].noGravity = true;
								Main.dust[index].velocity = vel * NPC.ai[2] * 1;

								index = Dust.NewDust(NPC.Center, 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.3f, 2.2f));
								Main.dust[index].noGravity = true;
								Main.dust[index].velocity = vel * NPC.ai[2] * 1;

								index = Dust.NewDust(NPC.Center, 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.3f, 2.2f));
								Main.dust[index].noGravity = true;
								Main.dust[index].velocity = vel * NPC.ai[2] * 1;
							}
						}
					}
				}
			}
			if (timer is > 160 and < 240)
			{
				GetDir_ByPlayer();
				MoveTo(player.Center, 5, 20);
			}
			if (timer >= 240)
			{
				NPC.ai[0]++;
				timer = 0;
			}
		}//下冲+蝶弹
		if (NPC.ai[0] == 9)
		{
			if (++timer < 60)
			{
				MoveTo(player.Center + new Vector2(0, -200), 10, 20);
				GetDir_ByPlayer();
			}
			if (timer == 60 && Main.netMode != NetmodeID.MultiplayerClient)
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MothBall>(), NPC.damage / 4, 0, Main.myPlayer);
			if (timer is > 60 and < 700)
			{
				MoveTo(player.Center, 6, 20);
				GetDir_ByPlayer();
			}
			if (timer > 700)
			{
				NPC.ai[0]++;
				timer = 0;
			}
		}//飞蛾球
		if (NPC.ai[0] == 10)
		{
			if (timer == 0)
			{
				Stack<NPC> butterfies = new();
				foreach (NPC NPC in Main.npc)//记录所有蝴蝶
				{
					if (NPC.active && NPC.type == ModContent.NPCType<Butterfly>())
						butterfies.Push(NPC);
				}

				int scale = 15;
				float rot = 3.14f;//把贴图旋转为向右边

				//TODO 只需要一个通道就可以了，有时间可以来稍微优化下

				//切换为弓AI
				//清空计时器
				FormatButterfliesFlocks(BBowColors, butterfies, scale, rot, 1);

				//切换为箭AI
				//清空计时器
				FormatButterfliesFlocks(BArrowColors, butterfies, scale, rot, 2);

				//foreach (var keypoint in BBowColors)
				//{
				//    if (butterfies.Count == 0)
				//    {
				//        break;
				//    }
				//    NPC butterfly = butterfies.Pop();
				//    butterfly.ai[0] = 1;
				//    butterfly.ai[1] = 0;
				//    butterfly.ai[3] = NPC.whoAmI;
				//    butterfly.velocity = Vector2.Zero;
				//    (butterfly.ModNPC as Butterfly).targetPos = new Vector2((keypoint.Column - BBowColorsWidth / 2f) * scale, (keypoint.Row - BBowColorsHeight / 2f) * scale).RotatedBy(rot);//指定其目标
				//}

				//for (int x = 0; x < BBowColorsWidth; x++)
				//{
				//    for (int y = 0; y < BBowColorsHeight; y++)
				//    {
				//        if (BBowColors[x, y] == new Rgba32(58, 169, 255) && butterfies.Count > 0)
				//        {
				//            NPC butterfly = butterfies.Pop();
				//            butterfly.ai[0] = 1;//切换为弓AI
				//            butterfly.ai[1] = 0;//清空计时器
				//            butterfly.ai[3] = NPC.whoAmI;
				//            butterfly.velocity = Vector2.Zero;
				//            (butterfly.ModNPC as Butterfly).targetPos = new Vector2((x - BBowColorsWidth / 2f) * scale, (y - BBowColorsHeight / 2f) * scale).RotatedBy(rot);//指定其目标
				//        }
				//    }
				//}

				//for (int x = 0; x < BArrowColorsWidth; x++)
				//{
				//    for (int y = 0; y < BArrowColorsHeight; y++)
				//    {
				//        if (BArrowColors[x, y] == new Color(58, 169, 255) && butterfies.Count > 0)
				//        {
				//            NPC butterfly = butterfies.Pop();
				//            butterfly.ai[0] = 2;//切换为箭AI
				//            butterfly.ai[1] = 0;//清空计时器
				//            butterfly.ai[3] = NPC.whoAmI;
				//            butterfly.velocity = Vector2.Zero;
				//            (butterfly.ModNPC as Butterfly).targetPos = new Vector2((x - BArrowColorsWidth / 2f) * scale, (y - BArrowColorsHeight / 2f) * scale).RotatedBy(rot);//指定其目标
				//        }
				//    }
				//}
				foreach (NPC butterfly in butterfies)//对于剩余蝴蝶
				{
					butterfly.ai[0] = -1;//切换为游荡AI
					butterfly.ai[1] = 0;//清空计时器
					butterfly.ai[3] = NPC.whoAmI;
				}
			}
			timer++;
			if (timer < 120)
			{
				MoveTo(player.Center + new Vector2(0, -300), 8, 20);
				GetDir_ByPlayer();
			}
			if (timer > 120)
			{
				MoveTo(player.Center + new Vector2(0, -300), 5, 20);
				GetDir_ByPlayer();
			}
			if (timer is > 120 and < 180)
			{
				if (timer < 150)
					PhamtomDis += 4;
				else
				{
					PhamtomDis -= 4;
				}
			}
			if (timer is > 340 and < 400)
			{
				if (timer < 370)
					PhamtomDis += 4;
				else
				{
					PhamtomDis -= 4;
				}
			}
			if (timer == 400)//第二次射箭
			{
			}
			if (timer > 480)
			{
				NPC.ai[0]++;
				timer = 0;
			}
		}//弓
		if (NPC.ai[0] == 11)
		{
			if (timer < 180)
			{
				MoveTo(player.Center + new Vector2(0, -200), 8, 20);
				if (timer is > 140 and < 160)
					PhamtomDis += 5;
				else
				{
					PhamtomDis -= 5;
				}
			}
			if (timer == 180)//开始挥剑
			{
				lightVisual += 2;
				NPC.velocity = NPC.DirectionTo(player.Center) * 20;
			}
			if (timer > 220)
				MoveTo(player.Center, 5, 20);
			GetDir_ByPlayer();
			if (timer == 0)
			{
				Stack<NPC> butterfies = new();
				foreach (NPC NPC in Main.npc)//记录所有蝴蝶
				{
					if (NPC.active && NPC.type == ModContent.NPCType<Butterfly>())
						butterfies.Push(NPC);
				}

				float rot = 0.785f;
				int scale = 15;

				foreach (var keypoint in BSwordColors)
				{
					if (butterfies.Count == 0)
						break;
					NPC butterfly = butterfies.Pop();
					butterfly.ai[0] = 3;            //切换为剑AI
					butterfly.ai[1] = 0;            //清空计时器
					butterfly.ai[3] = NPC.whoAmI;
					butterfly.velocity = Vector2.Zero;
					(butterfly.ModNPC as Butterfly).targetPos = new Vector2(keypoint.Column * scale, (keypoint.Row - BSwordColorsHeight) * scale).RotatedBy(rot);//指定其目标
				}

				//for (int x = 0; x < BSwordColorsWidth; x++)
				//{
				//    for (int y = 0; y < BSwordColorsHeight; y++)
				//    {
				//        if (BSwordColors[x, y] == new Color(58, 169, 255) && butterfies.Count > 0)
				//        {
				//            NPC butterfly = butterfies.Pop();
				//            butterfly.ai[0] = 3;//切换为剑AI
				//            butterfly.ai[1] = 0;//清空计时器
				//            butterfly.ai[3] = NPC.whoAmI;
				//            butterfly.velocity = Vector2.Zero;
				//            (butterfly.ModNPC as Butterfly).targetPos = new Vector2(x * scale, (y - BSwordColorsHeight) * scale).RotatedBy(rot);//指定其目标
				//        }
				//    }
				//}
				foreach (NPC butterfly in butterfies)//对于剩余蝴蝶
				{
					butterfly.ai[0] = -1;//切换为游荡AI
					butterfly.ai[1] = 0;//清空计时器
					butterfly.ai[3] = NPC.whoAmI;
				}
			}
			if (timer == 150 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int i = -10; i < 10; i++)
				{
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), i * 10), ModContent.ProjectileType<ButterflyDream>(), NPC.damage / 10, 0, Main.myPlayer, -i * 0.1f);
				}
			}
			timer++;
			if (timer > 240)
			{
				timer = 0;
				NPC.ai[0]++;
			}
		}//剑
		if (NPC.ai[0] == 12)
		{
			if (timer is > 120 and < 160)
			{
				MoveTo(player.Center + NPC.DirectionFrom(player.Center) * 600, 8, 20);
				if (timer < 140)
					PhamtomDis += 5;
				else
				{
					PhamtomDis -= 5;
				}
			}
			else if (timer < 300)
			{
				MoveTo(player.Center + new Vector2(0, -200), 8, 20);
			}

			GetDir_ByPlayer();
			if (timer == 0)
			{
				Stack<NPC> butterfies = new();
				foreach (NPC NPC in Main.npc)//记录所有蝴蝶
				{
					if (NPC.active && NPC.type == ModContent.NPCType<Butterfly>())
						butterfies.Push(NPC);
				}

				float rot = 3.14f;
				int scale = 10;

				foreach (var keypoint in BFistColors)
				{
					if (butterfies.Count == 0)
						break;
					NPC butterfly = butterfies.Pop();
					butterfly.ai[0] = 4;            //切换为拳AI
					butterfly.ai[1] = 0;            //清空计时器
					butterfly.ai[3] = NPC.whoAmI;
					butterfly.velocity = Vector2.Zero;
					(butterfly.ModNPC as Butterfly).targetPos = new Vector2((keypoint.Column - BFistColorsWidth / 2) * scale, (keypoint.Row - BFistColorsHeight / 2) * scale).RotatedBy(rot);//指定其目标
				}
				//for (int y = 0; y < BFistColorsHeight; y++)
				//{
				//    for (int x = 0; x < BFistColorsWidth; x++)
				//    {
				//        if (BFistColors[x, y] == new Color(58, 169, 255) && butterfies.Count > 0)
				//        {
				//            NPC butterfly = butterfies.Pop();
				//            butterfly.ai[0] = 4;//切换为拳AI
				//            butterfly.ai[1] = 0;//清空计时器
				//            butterfly.ai[3] = NPC.whoAmI;
				//            butterfly.velocity = Vector2.Zero;
				//            (butterfly.ModNPC as Butterfly).targetPos = new Vector2((x - BFistColorsWidth / 2) * scale, (y - BFistColorsHeight / 2) * scale).RotatedBy(rot);//指定其目标
				//        }
				//    }
				//}
				foreach (NPC butterfly in butterfies)//对于剩余蝴蝶
				{
					butterfly.ai[0] = -1;//切换为游荡AI
					butterfly.ai[1] = 0;//清空计时器
					butterfly.ai[3] = NPC.whoAmI;
				}
			}
			timer++;
			if (timer > 300)
			{
				NPC.ai[0]++;
				timer = 0;
			}
		}//拳
		if (NPC.ai[0] == 13)//
		{
			int counts = 0;
			foreach (NPC NPC in Main.npc)
			{
				if (NPC.type == ModContent.NPCType<Butterfly>() && NPC.active)
					counts++;
			}
			if (timer == 0)
			{
				NPC.dontTakeDamage = true;
				foreach (NPC NPC in Main.npc)
				{
					if (NPC.type == ModContent.NPCType<Butterfly>() && NPC.active && NPC.ai[0] == -1)
					{
						NPC.ai[0]--;
						NPC.ai[2] = Main.rand.NextFloat() * 6.28f;
					}
				}
			}
			if (timer > 300 && timer % 60 == 0)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, 1), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
					if (Main.expertMode && !Main.masterMode)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(0, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
					if (Main.masterMode)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(-1.4f, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(1.4f, -2), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
					}
					if (Main.getGoodWorld)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(-1.8f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + new Vector2(1.8f, -3), ModContent.ProjectileType<BlackCorruptRain>(), NPC.damage / 8, 0f, Main.myPlayer, 1);
					}
				}
			}
			timer++;
			PhamtomDis = MathHelper.Lerp(PhamtomDis, MathHelper.Clamp(counts, 0, 40) * 3, 0.1f);
			if (counts > 0)
			{
				MoveTo(player.Center, 5, 20);
				GetDir_ByPlayer();
			}
			else
			{
				NPC.ai[0] = 1;
				timer = 150;
				NPC.dontTakeDamage = false;
			}
		}
		for (int i = NPC.oldPos.Length - 1; i > 0; i--)
		{
			NPC.oldPos[i] = NPC.oldPos[i - 1];
		}

		NPC.oldPos[0] = NPC.Center;
	}

	private void MoveTo(Vector2 targetPos, float Speed, float n)
	{
		Vector2 targetVec = (targetPos - NPC.Center).SafeNormalize(Vector2.Zero) * Speed;
		NPC.velocity = (NPC.velocity * n + targetVec) / (n + 1);
	}

	private void StraightMoveTo(Vector2 targetPos, float n)
	{
		NPC.Center = Vector2.Lerp(NPC.Center, targetPos, n);
	}

	/// <summary>
	/// 将蝴蝶按照keypoints重新编队，并且赋予新的AI
	/// </summary>
	private void FormatButterfliesFlocks(List<ImageKeyPoint> keypoints, Stack<NPC> butterflies,
		float scale, float rot, float AISwitch)
	{
		foreach (var keypoint in keypoints)
		{
			if (butterflies.Count == 0)
				break;
			NPC butterfly = butterflies.Pop();
			butterfly.ai[0] = AISwitch;
			butterfly.ai[1] = 0;            //清空计时器
			butterfly.ai[3] = NPC.whoAmI;
			butterfly.velocity = Vector2.Zero;
			(butterfly.ModNPC as Butterfly).targetPos = new Vector2((keypoint.Column - BBowColorsWidth / 2f) * scale, (keypoint.Row - BBowColorsHeight / 2f) * scale).RotatedBy(rot);//指定其目标
		}
	}

	private static void SpinAI(Entity entity, Vector2 center, float v, bool changeVelocity = true)
	{
		Vector2 oldPos = entity.Center;
		entity.Center = center + (oldPos - center).RotatedBy(v);
		if (changeVelocity)
		{
			entity.velocity = entity.Center - oldPos;
			entity.position -= entity.velocity;
		}
	}

	/// <summary>
	/// 发射弹幕，在调用时判断是否不为客户端
	/// </summary>
	private void GenerateGrayVFX(int Frequency)
	{
		if (NPC.velocity.Length() > 10 && Main.netMode != NetmodeID.MultiplayerClient)
		{
			float mulVelocity = 2f;
			for (int g = 0; g < Frequency; g++)
			{
				float rotatedVelocity = Main.rand.NextFloat(-0.2f, 0.2f);
				var gFL = new GrayFlowLine
				{
					velocity = NPC.velocity.RotatedBy(rotatedVelocity) * Main.rand.NextFloat(0.85f, 1.15f) * mulVelocity + NPC.velocity.SafeNormalize(new Vector2(0, -1)),
					Active = true,
					Visible = true,
					position = NPC.Center + new Vector2(Main.rand.NextFloat(-50f, 50f), 0).RotatedByRandom(6.283) - NPC.velocity * 12,
					maxTime = Main.rand.Next(25, 40),
					ai = new float[] { Main.rand.NextFloat(-1f, 1f), -rotatedVelocity * 0.2f * Main.rand.NextFloat(0.5f, 1.5f), Main.rand.NextFloat(1.8f, 2f) }
				};
				Ins.VFXManager.Add(gFL);
			}
		}
	}

	private void GetDir_ByPlayer()
	{
		NPC.direction = NPC.spriteDirection = NPC.Center.X - Player.Center.X > 0 ? -1 : 1;
	}

	private void GetDir_ByVel()
	{
		NPC.direction = NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
		if (lightVisual < 1)
			lightVisual += 0.3f;
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}

	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CorruptMothTreasureBag>()));
		npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<MothRelic>()));
		npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<DarknessFan>(), 100, 1, 1, 1)); //Classic Darkness Fan
		if (Main.expertMode)
			npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<DarknessFan>(), 25, 1, 1, 1)); //Expert Darkness Fan
		else if (Main.masterMode)
			npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<DarknessFan>(), 10, 1, 1, 1)); //Master Darkness Fan
		npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Weapons.GlowBeadGun>(), 80, 1, 1, 1)); //Classic Bead Gun
		if (Main.expertMode)
			npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<Items.Weapons.GlowBeadGun>(), 20, 1, 1, 1)); //Expert Bead Gun
		else if (Main.masterMode)
			npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<Items.Weapons.GlowBeadGun>(), 8, 1, 1, 1)); //Master Bead Gun

		var rule = new LeadingConditionRule(new Conditions.NotExpert());
		rule.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Weapons.ShadowWingBow>(), ModContent.ItemType<ScaleWingBlade>(), ModContent.ItemType<Items.Weapons.PhosphorescenceGun>(), ModContent.ItemType<Items.Weapons.EvilChrysalis>(), ModContent.ItemType<DustOfCorrupt>()));

		npcLoot.Add(rule);

		npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Accessories.MothEye>(), 3, 1, 1, 1)); //Classic Moth Eye Accessory

		//npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<Items.Weapons.Legendary.ToothSpear>(), 40/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		/*
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<Items.Weapons.Legendary.DarknessFan>(), 8, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomM(), ModContent.ItemType<Items.BossDrop.MothRelic>(), 1, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomM(), ModContent.ItemType<Items.Bosses.CorruptMothTreasureBag>(), 1, 1, 1, 1));

            //npcLoot.Add(ItemDropRule.ByCondition(new OnlyExper(), ModContent.ItemType<Items.Weapons.Legendary.ToothSpear>(), 125, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new OnlyExper(), ModContent.ItemType<Items.Weapons.Legendary.DarknessFan>(), 25, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new OnlyExper(), ModContent.ItemType<Items.Bosses.CorruptMothTreasureBag>(), 1, 1, 1, 1));

            //npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Weapons.Legendary.ToothSpear>(), 500, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Weapons.Legendary.DarknessFan>(), 100, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Accessories.MothEye>(), 6, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.Moth.ShadowWingBow>(), 6, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.Moth.ScaleWingBlade>(), 6, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.Moth.PhosphorescenceGun>(), 6, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.Moth.EvilChrysalis>(), 6, 1, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.Moth.DustOfCorrupt>(), 6, 1, 1, 1));
            */
	}

	public override void FindFrame(int frameHeight)
	{
		NPC.frameCounter += NPC.velocity.Length() / 50f + 0.1f;
		int num = (int)NPC.frameCounter % Main.npcFrameCount[NPC.type];
		NPC.frame.Y = num * frameHeight;
	}

	public void DrawCube()
	{
		int[][] array = new int[][]
		{
			 new int[] { 1,2,4,3},
			 new int[] { 5,6,8,7},
			 new int[] {1,4,5,8},
			 new int[] { 2,3,6,7},
			 new int[] { 4,3,8,7},
			 new int[] { 1,2,5,6}
		};
		List<Vertex2D> vertices = new();
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Main.graphics.GraphicsDevice.Textures[0] = Terraria.GameContent.TextureAssets.MagicPixel.Value;
		Color color = new Color(0f, 0.3f, 1f) * lightVisual * 0.8f;
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Vector3 pos = cubeVec[array[i][j] - 1] * 80;
				pos.X += NPC.Center.X - Main.screenPosition.X;
				pos.Y += NPC.Center.Y - Main.screenPosition.Y;
				Vector2 v2Pos = Projection2(pos, new Vector2(Main.screenWidth, Main.screenHeight) / 2, out float scale, 1000);
				float alpha = 1;
				if (i is 2 or 3)
					alpha *= 0.5f;

				vertices.Add(new(v2Pos, color * alpha, Vector3.Zero));
				;
			}
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, 2);
			vertices.Clear();
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}

	private static Vector2 Projection2(Vector3 v3, Vector2 center, out float scale, float viewZ)
	{
		float k2 = -viewZ / (v3.Z - viewZ);
		scale = k2;
		var v = new Vector2(v3.X, v3.Y);
		return v + (k2 - 1) * (v - center);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		SpriteEffects effects = SpriteEffects.None;
		if (NPC.spriteDirection == 1)
			effects = SpriteEffects.FlipHorizontally;
		if (NPC.ai[0] == 6 && timer > 260)
			DrawCube();

		Texture2D tx = ModContent.Request<Texture2D>(Texture).Value;
		Texture2D GlowTexture = ModAsset.CorruptMoth_Glow.Value;
		Texture2D WingDustTexture = ModAsset.CorruptMoth_WingEffect.Value;
		Vector2 origin = new Vector2(tx.Width, tx.Height / 11) / 2;
		Color origColor = NPC.GetAlpha(drawColor);
		for (int k = 0; k < NPC.oldPos.Length; k++)
		{
			float fadeValue = ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
			fadeValue *= fadeValue;
			if (NPC.velocity.Length() < 10)
			{
				fadeValue *= (NPC.velocity.Length() - 5) / 5f;
			}
			if (NPC.velocity.Length() < 5)
			{
				break;
			}
			Color color = origColor * fadeValue;
			spriteBatch.Draw(tx, NPC.oldPos[k] - Main.screenPosition, NPC.frame, color, NPC.rotation, origin, NPC.scale, effects, 0f);
		}
		spriteBatch.Draw(tx, NPC.Center - Main.screenPosition, NPC.frame, origColor, NPC.rotation, origin, NPC.scale, effects, 0f);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Main.spriteBatch.Draw(GlowTexture, NPC.Center - Main.screenPosition, new Rectangle?(NPC.frame), Color.White * lightVisual, NPC.rotation, origin, NPC.scale, effects, 0f);
		Main.spriteBatch.Draw(WingDustTexture, NPC.Center - Main.screenPosition, new Rectangle?(NPC.frame), Color.White * (MathF.Pow(NPC.velocity.Length() / 20f, 1.0f)), NPC.rotation, origin, NPC.scale, effects, 0f);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


		//删掉注释显示碰撞箱
		//Texture2D tBox = TextureAssets.MagicPixel.Value;
		//Rectangle rt = NPC.Hitbox;
		//rt.X -= (int)Main.screenPosition.X;
		//rt.Y -= (int)Main.screenPosition.Y;
		//Main.spriteBatch.Draw(tBox, rt, new Color(55, 0, 0, 0));
		return false;
	}
}

// "For The Worthy" Seed notes: Corrupted Moth's defense is increased by +10. Corrupted Moth's rain attacks fire two extra projectiles. The moth minions (butterflies) have 3 HP instead on 1 and has 9999 defense, meaning that you have to hit them 3 times instead of once. Corrupted Moth's corrupt rain attacks go through tiles (also applies on Master Mode).