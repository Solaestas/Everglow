using Everglow.Commons.DataStructures;
using Everglow.Myth.Common;
using Everglow.Myth.LanternMoon.Gores;
using Everglow.Myth.LanternMoon.Projectiles.LanternKing;
using Everglow.Myth.LanternMoon.Projectiles.LanternKing.VFXs;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Map;

namespace Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;

[AutoloadBossHead]
public class LanternGhostKing : ModNPC
{
	internal bool NearDie = false;
	internal Vector2 RingCenterTrend;
	internal Vector2 RingCenter;
	internal Vector2 Lantern3RingCenter;
	internal float RingRadius = 0;
	internal float RingRadiusTrend = 1800;

	public float LeftRotation = 0;
	public float RightRotation = 0;
	private static Texture2D[] breads;

	private static Texture2D back;
	private static Texture2D front;
	private static Texture2D front_face;
	private static Texture2D left;
	private static Texture2D right;
	private static Texture2D left_glow;
	private static Texture2D right_glow;
	public float ShakeStrength = 2;
	public int Phase = 1;
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 3;
		back = ModAsset.LanternGhostKing_back.Value;
		front = ModAsset.LanternGhostKing_bone_front.Value;
		front_face = ModAsset.LanternGhostKing_face_front.Value;
		left = ModAsset.LanternGhostKing_side_left.Value;
		right = ModAsset.LanternGhostKing_side_right.Value;
		left_glow = ModAsset.LanternGhostKing_side_left_glow.Value;
		right_glow = ModAsset.LanternGhostKing_side_right_glow.Value;
		breads = new Texture2D[10];
		for (int i = 0; i < 10; i++)
		{
			breads[i] = ModContent.Request<Texture2D>("Everglow/Myth/LanternMoon/NPCs/LanternGhostKing/LanternGhostKing_bread_" + i.ToString()).Value;
		}
	}
	public override void SetDefaults()
	{
		NPC.damage = 100;
		if (Main.expertMode)
			NPC.lifeMax = 20000;
		else
		{
			NPC.lifeMax = 30000;
		}
		NPC.width = 250;
		NPC.height = 150;
		NPC.defense = 50;
		NPC.value = 20000;
		NPC.aiStyle = -1;
		NPC.boss = true;
		NPC.knockBackResist = 0f;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.HitSound = SoundID.NPCHit3;
		Music = MythContent.QuickMusic("DashCore");
	}
	public override bool CheckActive()
	{
		return Main.dayTime;
	}
	public override void OnSpawn(IEntitySource source)
	{
		for (int i = 0; i < 10; i++)
		{
			breads[i] = ModContent.Request<Texture2D>("Everglow/Myth/LanternMoon/NPCs/LanternGhostKing/LanternGhostKing_bread_" + i.ToString()).Value;
		}
		NPC.localAI[0] = 0;
		Phase = 1;
		var spark = new LanternFlameRingDust
		{
			OwnerLanternKing = NPC,
			Active = true,
			Visible = true,
			maxTime = 240
		};
		Ins.VFXManager.Add(spark);
		RingCenter = NPC.Center;
	}
	public void UpdateDrawParameter()
	{
		LeftRotation = (float)Utils.Lerp(LeftRotation, NPC.rotation, 0.02f);
		RightRotation = (float)Utils.Lerp(RightRotation, NPC.rotation, 0.02f);
		ShakeStrength = (float)Utils.Lerp(ShakeStrength, 2, 0.02f);
	}
	public void CheckPlayerTouchRing()
	{
		if (RingRadius < 1000)
		{
			return;
		}
		foreach (var player in Main.player)
		{
			if (player != null && player.active && !player.dead)
			{
				float distance = (player.Center - RingCenter).Length();
				int dir = 1;
				if ((player.Center - RingCenter).X > 0)
				{
					dir = -1;
				}
				if (Math.Abs(distance - RingRadius - 50) < 50)
				{
					player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), 400, dir, false, true, false, player.immuneTime);
					player.AddBuff(BuffID.OnFire3, 240);
				}
			}
		}
	}
	public override void AI()
	{
		CheckPlayerTouchRing();
		NPC.localAI[0] += 1;
		UpdateDrawParameter();
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
		if (NPC.localAI[0] > 14400)
			NPC.ai[0] = (14680 - NPC.localAI[0]) / 240f;
		else
		{
			NPC.ai[0] = 1;
		}

		Lighting.AddLight(NPC.Center, new Vector3(0.75f, 0.2f, 0) * ((255 - NPC.alpha) / 255f));
		if (!NearDie)
			NPC.dontTakeDamage = false;
		if (player.dead)
		{
			if (NPC.life < NPC.lifeMax)
				NPC.life += 10;
			else
			{
				NPC.life = NPC.lifeMax;
			}
		}

		if (NPC.localAI[0] <= 0)
		{
			NPC.rotation = NPC.velocity.X / 120f;
			Vector2 v = player.Center + new Vector2((float)Math.Sin(NPC.localAI[0] / 40f) * 500f, (float)Math.Sin((NPC.localAI[0] + 200) / 40f) * 50f - 350) - NPC.Center;
			if (NPC.velocity.Length() < 9f)
				NPC.velocity += v / v.Length() * 0.35f;
			NPC.velocity *= 0.96f;
			RingCenterTrend = NPC.Center;
			RingRadiusTrend = 1800;
		}
		if (Phase == 1)
		{
			//闪烁金线 ai1借用为晃动力
			if (NPC.localAI[0] < 700 && NPC.localAI[0] > 0)
			{
				float duration = NPC.localAI[0] % 100;
				if (duration < 50)
				{
					NPC.rotation = NPC.velocity.X / 120f;
					Vector2 v = player.Center + new Vector2((float)Math.Sin(Main.time * 0.045f) * 500f, (float)Math.Sin(Main.time * 0.075f + 20) * 50f - 350) - NPC.Center;
					NPC.velocity += v / v.Length() * 1f;
					NPC.velocity *= 0.96f;
				}
				else if (duration >= 50 && duration < 60)
				{
					NPC.rotation = NPC.velocity.X / 120f;
					NPC.velocity *= 0.8f;
				}
				else
				{
					if (duration == 60)
					{
						NPC.rotation = 0.6f;
						NPC.ai[1] = -0.2f;
						ShakeStrength = 25;

					}
					if (duration % 2 == 1)
					{
						Vector2 v0 = new Vector2(0, 24 * MathF.Abs(NPC.ai[1]) + 12).RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f));
						Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 20) + v0 * 6, v0, ModContent.ProjectileType<GoldLanternLine>(), 40, 0f, player.whoAmI, 0, 0);
						float myDamage = 40;
						if(Main.expertMode)
						{
							myDamage = 55;
						}
						if (Main.masterMode)
						{
							myDamage = 70;
						}
						p0.damage = (int)myDamage / 2;
					}
					NPC.rotation += NPC.ai[1];
					NPC.ai[1] -= NPC.rotation * 0.07f;
					NPC.rotation *= 0.9f;
				}

				RingCenterTrend = NPC.Center;
				RingRadiusTrend = 1200;
			}
			//灯火炸环
			if (NPC.localAI[0] >= 700 && NPC.localAI[0] < 1500)
			{
				if (NPC.localAI[0] % 250 == 0)
					Lantern3RingCenter = new Vector2(0, -300).RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f) * Math.PI);
				if (NPC.localAI[0] % 250 < 20)
				{
					NPC.velocity *= 0.95f;
					NPC.rotation *= 0.95f;
				}
				if (NPC.localAI[0] % 250 < 30 && NPC.localAI[0] % 250 < 20)
				{
					NPC.velocity *= 0;
					NPC.rotation *= 0.95f;
				}
				if (NPC.localAI[0] % 250 == 30)
				{
					int count = 6;
					if (Main.expertMode)
					{
						count = 12;
					}
					for (int i = 0; i < count; i++)
					{
						Vector2 v1 = new Vector2(0, 100).RotatedBy(i / (double)count * 2 * Math.PI);
						Vector2 v2 = v1 + NPC.Center;
						var p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), v2, Vector2.zeroVector, ModContent.ProjectileType<DarkLantern>(), 50, 0f, player.whoAmI, 120 - NPC.localAI[0] % 250, (float)(i / (double)count * 2 * Math.PI));
						float myDamage = 50;
						if (Main.expertMode)
						{
							myDamage = 75;
						}
						if (Main.masterMode)
						{
							myDamage = 100;
						}
						p0.damage = (int)myDamage / 2;
					}
				}
				if (NPC.localAI[0] % 250 == 60)
				{
					int count = 9;
					if (Main.expertMode)
					{
						count = 24;
					}
					for (int i = 0; i < count; i++)
					{
						Vector2 v1 = new Vector2(0, 150).RotatedBy(i / (double)count * 2 * Math.PI);
						Vector2 v2 = v1 + NPC.Center;
						var p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), v2, Vector2.zeroVector, ModContent.ProjectileType<DarkLantern>(), 50, 0f, player.whoAmI, 120 - NPC.localAI[0] % 250, (float)(i / (double)count * 2 * Math.PI));
						float myDamage = 50;
						if (Main.expertMode)
						{
							myDamage = 75;
						}
						if (Main.masterMode)
						{
							myDamage = 100;
						}
						p0.damage = (int)myDamage / 2;
					}
				}
				if (NPC.localAI[0] % 250 == 90)
				{
					int count = 15;
					if (Main.expertMode)
					{
						count = 36;
					}
					for (int i = 0; i < count; i++)
					{
						Vector2 v1 = new Vector2(0, 200).RotatedBy(i / (double)count * 2 * Math.PI);
						Vector2 v2 = v1 + NPC.Center;
						var p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), v2, Vector2.zeroVector, ModContent.ProjectileType<DarkLantern>(), 50, 0f, player.whoAmI, 120 - NPC.localAI[0] % 250, (float)(i / (double)count * 2 * Math.PI));
						float myDamage = 50;
						if (Main.expertMode)
						{
							myDamage = 75;
						}
						if (Main.masterMode)
						{
							myDamage = 100;
						}
						p0.damage = (int)myDamage / 2;
					}
				}
				if (NPC.localAI[0] % 250 > 120)
				{
					Vector2 v = player.Center + Lantern3RingCenter + player.velocity * 30f;
					NPC.velocity += (v - NPC.Center) / (v - NPC.Center).Length() * 0.25f;
					if (NPC.velocity.Length() > 20f)
						NPC.velocity *= 0.96f;
				}
				if(NPC.localAI[0] >= 999)
				{
					NPC.localAI[0] = 1500;
				}
				RingCenterTrend = NPC.Center;
				RingRadiusTrend = 1200;
			}
			//灯火炸弹阵法
			if (NPC.localAI[0] >= 1500 && NPC.localAI[0] < 1700)
			{
				NPC.rotation *= 0.95f;
				NPC.velocity *= 0.95f;
				if (NPC.localAI[0] == 1600)
				{
					int count = 80;
					if (Main.expertMode)
					{
						count = 120;
					}
					if (Main.masterMode)
					{
						count = 180;
					}
					for (int j = 0; j < count; j++)
					{
						Vector2 v2 = new Vector2(0, Main.rand.Next(Main.rand.Next(0, 1200), 1200)).RotatedByRandom(Math.PI * 2);
						var p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + v2, Vector2.zeroVector, ModContent.ProjectileType<DarkLanternBomb2>(), 50, 0f, player.whoAmI, 0, 0);
						float myDamage = 50;
						if (Main.expertMode)
						{
							myDamage = 75;
						}
						if (Main.masterMode)
						{
							myDamage = 100;
						}
						p0.damage = (int)myDamage / 2;
					}
				}
			}
			//游离灯火
			if (NPC.localAI[0] >= 1700 && NPC.localAI[0] < 2300)
			{
				NPC.rotation *= 0.95f;
				NPC.velocity *= 0.95f;

				float frequency = 3;
				if (Main.expertMode)
				{
					frequency = 4;
				}
				if (Main.masterMode)
				{
					frequency = 6;
				}
				if (NPC.localAI[0] % 9 == 0 && NPC.localAI[0] > 2000)
				{
					float dx = (NPC.localAI[0] - 2000) / 300f;
					for (int j = 0; j < frequency; j++)
					{
						Vector2 v2 = new Vector2(0, 1 + dx).RotatedBy(Math.PI * j / frequency * 2 + dx * dx * 4);
						var p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + v2, v2, ModContent.ProjectileType<FloatLantern_style2>(), 50, 0f, player.whoAmI, 0, 0);
						float myDamage = 50;
						if (Main.expertMode)
						{
							myDamage = 75;
						}
						if (Main.masterMode)
						{
							myDamage = 100;
						}
						p0.damage = (int)myDamage / 2;
					}
					if (Main.masterMode)
					{
						for (int j = 0; j < 3; j++)
						{
							Vector2 v2 = new Vector2(0, 1 + dx).RotatedBy(Math.PI * j / 1.5 + dx * dx * 4);
							var p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + v2, v2, ModContent.ProjectileType<FloatLantern_style3>(), 50, 0f, player.whoAmI, 0, 0);
							float myDamage = 50;
							if (Main.expertMode)
							{
								myDamage = 75;
							}
							if (Main.masterMode)
							{
								myDamage = 100;
							}
							p0.damage = (int)myDamage / 2;
						}
					}
					if (Main.expertMode)
					{
						for (int j = 0; j < 3; j++)
						{
							Vector2 v2 = new Vector2(0, 1 + dx).RotatedBy(Math.PI * (j + 1.5) / 1.5 + dx * dx * 4);
							var p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + v2, v2, ModContent.ProjectileType<FloatLantern_style4>(), 50, 0f, player.whoAmI, 0, 0);
							float myDamage = 50;
							if (Main.expertMode)
							{
								myDamage = 75;
							}
							if (Main.masterMode)
							{
								myDamage = 100;
							}
							p0.damage = (int)myDamage / 2;
						}
					}
				}
				if (NPC.localAI[0] >= 2298)
				{
					NPC.localAI[0] = 0;
				}
			}
		}
		if (Phase == 2)
		{
			//召唤小弟
			if (NPC.localAI[0] < 400)
			{
				NPC.defense = 1000;
				int maxCount = 20;
				if (Main.expertMode)
				{
					maxCount = 30;
				}
				if (Main.masterMode)
				{
					maxCount = 45;
				}
				if (NPC.CountNPCS(ModContent.NPCType<FloatLantern>()) < maxCount)
				{
					if (NPC.localAI[0] < 388)
					{
						if (NPC.localAI[0] % 7 == 0)
							NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y + 100, ModContent.NPCType<FloatLantern>(), 0, 0, 0, 0, 0, 255);
					}
				}

				NPC.rotation *= 0.95f;
				Vector2 v = player.Center + new Vector2(0, -350) - NPC.Center;
				if (NPC.velocity.Length() < 9f)
				{
					NPC.velocity += v / v.Length() * 0.35f;
					NPC.velocity.X += player.velocity.X * 0.07f;
				}
				NPC.velocity *= 0.96f;
				RingCenterTrend = NPC.Center;
				RingRadiusTrend = 1200;
				if (NPC.localAI[0] >= 395)
				{
					if (NPC.CountNPCS(ModContent.NPCType<FloatLantern>()) >= 1)
					{
						NPC.localAI[0] = 395;
					}
					else
					{
						NPC.dontTakeDamage = false;
						NPC.localAI[0] = 405;
						NPC.defense = 30;
					}
				}
			}
			//流灯河 ai1借用为位置
			if (NPC.localAI[0] > 400 && NPC.localAI[0] < 1200)
			{
				if (NPC.localAI[0] == 406)
				{
					NPC.ai[1] = 1;
					if (Main.rand.NextBool(2))
					{
						NPC.ai[1] = -1;
					}
				}
				if (NPC.localAI[0] < 430)
				{
					NPC.velocity *= 0;
					NPC.alpha += 20;
				}
				if (NPC.localAI[0] == 430)
				{
					Vector2 testPos = player.Center + new Vector2(600 * NPC.ai[1], 0);
					int count = 0;
					while (Collision.SolidCollision(testPos - new Vector2(50), 100, 100))
					{
						testPos.Y -= 100;
						count++;
						if (count > 10 || testPos.Y < 1000)
						{
							break;
						}
					}
					NPC.position = testPos;
				}
				if (NPC.localAI[0] > 430 && NPC.localAI[0] < 450)
				{
					NPC.velocity *= 0;
					NPC.alpha -= 20;
					if (NPC.alpha < 0)
					{
						NPC.alpha = 0;
					}
				}
				if (NPC.localAI[0] == 450)
				{
					NPC.velocity *= 0;
					NPC.alpha = 0;
					RingCenterTrend = NPC.Center;
				}
				if (NPC.localAI[0] == 455)
				{
					if (Main.masterMode)
					{
						float addValue = Main.rand.NextFloat(6.283f);
						for (int x = 0; x < 5; x++)
						{
							Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(2000, 0).RotatedBy(x / 5f * MathHelper.TwoPi + addValue), new Vector2(-11, 0).RotatedBy(x / 5f * MathHelper.TwoPi + addValue), ModContent.ProjectileType<LanternFlow>(), 85, 0f, player.whoAmI, 0.02f, 0);
							LanternFlow l0 = p0.ModProjectile as LanternFlow;
							l0.OwnerNPC = NPC;
						}
					}
					else if (Main.expertMode)
					{
						float addValue = Main.rand.NextFloat(6.283f);
						for (int x = 0; x < 3; x++)
						{
							Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(2000, 0).RotatedBy(x / 3f * MathHelper.TwoPi + addValue), new Vector2(-11, 0).RotatedBy(x / 3f * MathHelper.TwoPi + addValue), ModContent.ProjectileType<LanternFlow>(), 67, 0f, player.whoAmI, 0.02f, 0);
							LanternFlow l0 = p0.ModProjectile as LanternFlow;
							l0.OwnerNPC = NPC;
						}
					}
					else
					{
						Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(2000, 0), new Vector2(-11, 0), ModContent.ProjectileType<LanternFlow>(), 45, 0f, player.whoAmI, 0.02f, 0);
						LanternFlow l0 = p0.ModProjectile as LanternFlow;
						l0.OwnerNPC = NPC;
						Projectile p1 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-2000, 0), new Vector2(11, 0), ModContent.ProjectileType<LanternFlow>(), 45, 0f, player.whoAmI, 0.02f, 0);
						LanternFlow l1 = p1.ModProjectile as LanternFlow;
						l1.OwnerNPC = NPC;
					}
				}
			}
			//千灯夜雨
			if (NPC.localAI[0] >= 1200 && NPC.localAI[0] < 1600)
			{
				NPC.rotation *= 0.96f;
				if (NPC.localAI[0] == 1206)
				{
					NPC.ai[1] = 1;
					if (Main.rand.NextBool(2))
					{
						NPC.ai[1] = -1;
					}
				}
				if (NPC.localAI[0] < 1230)
				{
					NPC.velocity *= 0;
					NPC.alpha += 20;
				}
				if (NPC.localAI[0] == 1230)
				{
					Vector2 testPos = player.Center + new Vector2(200 * NPC.ai[1], 0);
					int count = 0;
					while (Collision.SolidCollision(testPos - new Vector2(50), 100, 100))
					{
						testPos.Y -= 100;
						count++;
						if (count > 10 || testPos.Y < 1000)
						{
							break;
						}
					}
					NPC.position = testPos;
				}
				if (NPC.localAI[0] > 1230 && NPC.localAI[0] < 1250)
				{
					NPC.velocity *= 0;
					NPC.alpha -= 20;
					if (NPC.alpha < 0)
					{
						NPC.alpha = 0;
					}
				}
				if (NPC.localAI[0] == 1250)
				{
					NPC.velocity *= 0;
					NPC.alpha = 0;
					RingCenterTrend = NPC.Center;
				}
				//释放一波灯雨
				if (NPC.localAI[0] >= 1250 && NPC.localAI[0] % 67 == 0)
				{
					for (int x = -10; x < 11; x++)
					{
						float deltaY = MathF.Sin(x + (float)Main.time * 0.2f) * 60;
						float deltaY2 = MathF.Cos(x + (float)Main.time * 0.2f) * 60;
						NPC npc0 = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 1000, ModContent.NPCType<ExplosiveLantern_growing>(), 0, 0f, 0, 0, 0);

						npc0.Center = NPC.Center + new Vector2(x * 200 + NPC.localAI[0] % 135 - 70, -700 + deltaY);
						npc0.velocity = new Vector2(0, -deltaY2 * 0.005f);
						npc0.damage = NPC.damage / 2;
					}
				}
				if (NPC.localAI[0] == 1599)
				{
					SwitchAttackTypeInPhase2();
				}
			}
			//甩出金丝 ai3借用存方向  临时向量0借为旋转中心,临时向量1借为转动半径
			if (NPC.localAI[0] >= 1600 && NPC.localAI[0] < 2000)
			{
				if (NPC.localAI[0] == 1601)
				{
					NPC.ai[3] = 1;
					if (Main.rand.NextBool(2))
					{
						NPC.ai[3] = -1;
					}
				}
				if (NPC.localAI[0] < 1630)
				{
					NPC.velocity *= 0;
					NPC.alpha += 20;
				}
				if (NPC.localAI[0] == 1630)
				{
					Vector2 testPos = player.Center + new Vector2(600 * NPC.ai[3], 200);
					int count = 0;
					while (Collision.SolidCollision(testPos - new Vector2(50), 100, 100))
					{
						testPos.Y -= 100;
						count++;
						if (count > 10 || testPos.Y < 1000)
						{
							break;
						}
					}
					NPC.Center = testPos;
					RingCenterTrend = NPC.Center;
				}
				if (NPC.localAI[0] > 1630 && NPC.localAI[0] < 1650)
				{
					NPC.velocity *= 0;
					NPC.alpha -= 20;
					if (NPC.alpha < 0)
					{
						NPC.alpha = 0;
					}
				}
				if (NPC.localAI[0] == 1650)
				{
					NPC.velocity *= 0;
					NPC.alpha = 0;
					innerVector0 = NPC.Center + new Vector2(-NPC.ai[3] * 600, -200);
					innerVector1 = new Vector2(NPC.ai[3] * 600, 200);
				}
				if(NPC.localAI[0] > 1680 && NPC.localAI[0] <= 1710)
				{
					float rotValue = (1710 - NPC.localAI[0]) / 30f;
					rotValue = MathF.Pow(rotValue, 3);
					rotValue = 1 - rotValue;
					rotValue *= 3.0f;
					Vector2 addValue = innerVector1.RotatedBy(rotValue * NPC.ai[3]);
					NPC.Center = innerVector0 + addValue;
					NPC.rotation = MathF.Atan2(addValue.Y, addValue.X) - MathHelper.PiOver2;
					if (NPC.localAI[0] % 2 == 0)
					{
						Vector2 v0 = -Vector2.Normalize(addValue) * 10f;
						Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + v0 * 6, v0, ModContent.ProjectileType<GoldLanternLine3>(), 40, 0f, player.whoAmI, 0, 0);
						float myDamage = 40;
						if (Main.expertMode)
						{
							myDamage = 55;
						}
						if (Main.masterMode)
						{
							myDamage = 70;
						}
						p0.damage = (int)myDamage / 2;
					}
				}
				if (NPC.localAI[0] > 1710)
				{
					LeftRotation *= 0.9f;
					RightRotation *= 0.9f;
					NPC.rotation *= 0.7f;
				}
				if (NPC.localAI[0] == 1739)
				{
					SwitchAttackTypeInPhase2();
				}
			}
			//闪烁金线2 ai1借用为晃动力
			if (NPC.localAI[0] >= 2000 && NPC.localAI[0] < 2300)
			{
				float duration = NPC.localAI[0] % 60;
				if (duration < 30)
				{
					NPC.rotation = NPC.velocity.X / 120f;
					Vector2 v = player.Center + new Vector2((float)Math.Sin(Main.time * 0.045f) * 500f, (float)Math.Sin(Main.time * 0.075f + 20) * 50f - 350) - NPC.Center;
					NPC.velocity += Vector2.Normalize(v) * 1.3f;
					NPC.velocity *= 0.96f;
				}
				else if (duration >= 30 && duration < 35)
				{
					NPC.rotation = NPC.velocity.X / 120f;
					NPC.velocity *= 0.8f;
				}
				else
				{
					if (duration == 40)
					{
						NPC.ai[2] = 1;
						if (Main.rand.NextBool(2))
						{
							NPC.ai[2] = -1;
						}
						NPC.rotation = 0.9f * NPC.ai[2];
						NPC.ai[1] = -0.3f;
						ShakeStrength = 25;
					}
					Vector2 v0 = new Vector2(0, 24 * MathF.Abs(NPC.ai[1]) + 12).RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f));
					Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 20) + v0 * 6, v0, ModContent.ProjectileType<GoldLanternLine>(), 40, 0f, player.whoAmI, 0, 0);
					float myDamage = 40;
					if (Main.expertMode)
					{
						myDamage = 55;
					}
					if (Main.masterMode)
					{
						myDamage = 70;
					}
					p0.damage = (int)myDamage / 2;
					NPC.rotation += NPC.ai[1];
					NPC.ai[1] -= NPC.rotation * 0.07f;
					NPC.rotation *= 0.9f;
				}

				RingCenterTrend = NPC.Center;
				RingRadiusTrend = 1200;
				if (NPC.localAI[0] == 2299)
				{
					SwitchAttackTypeInPhase2();
				}
			}
			//灯火炸环2
			if (NPC.localAI[0] >= 2400 && NPC.localAI[0] < 3100)
			{
				if(NPC.localAI[0] < 3000)
				{
					if (NPC.localAI[0] % 250 == 0)
						Lantern3RingCenter = new Vector2(0, -300).RotatedBy(Main.rand.NextFloat(-1.3f, 1.3f));
					if (NPC.localAI[0] % 250 < 20)
					{
						NPC.velocity *= 0.95f;
						NPC.rotation *= 0.85f;
					}
					if (NPC.localAI[0] % 250 < 30 && NPC.localAI[0] % 250 < 20)
					{
						NPC.velocity *= 0;
						NPC.rotation *= 0.95f;
					}
					if (NPC.localAI[0] % 250 == 30)
					{
						int count = 6;
						if (Main.expertMode)
						{
							count = 12;
						}
						for (int i = 0; i < count; i++)
						{
							Vector2 v1 = new Vector2(0, 100).RotatedBy(i / (double)count * 2 * Math.PI);
							Vector2 v2 = v1 + NPC.Center;
							var p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), v2, Vector2.zeroVector, ModContent.ProjectileType<DarkLantern>(), 50, 0f, player.whoAmI, 120 - NPC.localAI[0] % 250, (float)(i / (double)count * 2 * Math.PI));
							float myDamage = 50;
							if (Main.expertMode)
							{
								myDamage = 75;
							}
							if (Main.masterMode)
							{
								myDamage = 100;
							}
							p0.damage = (int)myDamage / 2;
						}
					}
					if (NPC.localAI[0] % 250 == 60)
					{
						int count = 9;
						if (Main.expertMode)
						{
							count = 24;
						}
						for (int i = 0; i < count; i++)
						{
							Vector2 v1 = new Vector2(0, 150).RotatedBy(i / (double)count * 2 * Math.PI);
							Vector2 v2 = v1 + NPC.Center;
							var p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), v2, Vector2.zeroVector, ModContent.ProjectileType<DarkLantern>(), 50, 0f, player.whoAmI, 120 - NPC.localAI[0] % 250, (float)(i / (double)count * 2 * Math.PI));
							float myDamage = 50;
							if (Main.expertMode)
							{
								myDamage = 75;
							}
							if (Main.masterMode)
							{
								myDamage = 100;
							}
							p0.damage = (int)myDamage / 2;
						}
					}
					if (NPC.localAI[0] % 250 == 90)
					{
						int count = 15;
						if (Main.expertMode)
						{
							count = 36;
						}
						for (int i = 0; i < count; i++)
						{
							Vector2 v1 = new Vector2(0, 200).RotatedBy(i / (double)count * 2 * Math.PI);
							Vector2 v2 = v1 + NPC.Center;
							var p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), v2, Vector2.zeroVector, ModContent.ProjectileType<DarkLantern>(), 50, 0f, player.whoAmI, 120 - NPC.localAI[0] % 250, (float)(i / (double)count * 2 * Math.PI));
							float myDamage = 50;
							if (Main.expertMode)
							{
								myDamage = 75;
							}
							if (Main.masterMode)
							{
								myDamage = 100;
							}
							p0.damage = (int)myDamage / 2;
						}
					}
					if (NPC.localAI[0] % 250 > 120)
					{
						Vector2 v = player.Center + Lantern3RingCenter;
						NPC.velocity += (v - NPC.Center - NPC.velocity) * 0.015f;
						NPC.velocity *= 0.96f;
						NPC.rotation = NPC.velocity.X * 0.02f;
					}
					RingCenterTrend = NPC.Center;
					RingRadiusTrend = 1200;
				}
				else
				{
					NPC.velocity *= 0.95f;
					NPC.rotation *= 0.85f;
					if (NPC.localAI[0] >= 3029)
					{
						SwitchAttackTypeInPhase2();
					}
				}
			}
			//落灯柱,临时向量0借为位移目标
			if (NPC.localAI[0] >= 3100 && NPC.localAI[0] < 3600)
			{
				if(NPC.localAI[0] == 3103)
				{
					for(int i = 0;i < 8;i++)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.NextFloat(-1500, 1500), -600), Vector2.Zero, ModContent.ProjectileType<LanternFlowLine>(), 40, 0f, player.whoAmI, 0, 0);
					}
				}
				NPC.rotation *= 0.9f;
				NPC.velocity *= 0.9f;

				if (NPC.localAI[0] % 100 < 30)
				{
					NPC.velocity *= 0;
					NPC.alpha += 20;
				}
				if (NPC.localAI[0] % 100 == 30)
				{
					Vector2 testPos = player.Center + new Vector2(0, -700).RotatedBy(Main.rand.NextFloat(1.4f, 2.1f) * (Main.rand.NextBool(2) ? 1 : -1));
					int count = 0;
					while (Collision.SolidCollision(testPos - new Vector2(50), 100, 100))
					{
						testPos.Y -= 100;
						count++;
						if (count > 10 || testPos.Y < 1000)
						{
							break;
						}
					}
					NPC.Center = testPos;
					RingCenterTrend = NPC.Center;
				}
				if (NPC.localAI[0] % 100 > 30 && NPC.localAI[0] % 100 < 50)
				{
					NPC.velocity *= 0;
					NPC.alpha -= 20;
					if (NPC.alpha < 0)
					{
						NPC.alpha = 0;
					}
				}
				if (NPC.localAI[0] % 100 == 50)
				{
					NPC.velocity *= 0;
					NPC.alpha = 0;
					int count = 9;
					if(Main.expertMode)
					{
						count = 15;
					}
					for(int i = 0;i < count; i++)
					{
						Vector2 v0 = new Vector2(0, count).RotatedBy(i / (float)count * MathHelper.TwoPi);
						Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 20) + v0, v0, ModContent.ProjectileType<GoldLanternLine>(), 40, 0f, player.whoAmI, 0, 0);
						float myDamage = 40;
						if (Main.expertMode)
						{
							myDamage = 55;
						}
						if (Main.masterMode)
						{
							myDamage = 70;
						}
						p0.damage = (int)myDamage / 2;
					}
					if(Main.masterMode)
					{
						for (int i = 0; i < 15; i++)
						{
							Vector2 v0 = new Vector2(0, 8).RotatedBy((i + 0.5f) / 15f * MathHelper.TwoPi);
							Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 20) + v0, v0, ModContent.ProjectileType<GoldLanternLine>(), 40, 0f, player.whoAmI, 0, 0);
							float myDamage = 40;
							if (Main.expertMode)
							{
								myDamage = 55;
							}
							if (Main.masterMode)
							{
								myDamage = 70;
							}
							p0.damage = (int)myDamage / 2;
						}
					}
				}
				if (NPC.localAI[0] >= 3599)
				{
					SwitchAttackTypeInPhase2();
				}
			}
			if (NPC.localAI[0] >= 3600)
			{
				SwitchAttackTypeInPhase2();
			}
		}
		if (Main.dayTime)
		{
			NPC.velocity.Y += 1;	
			if((NPC.Center - player.Center).Length() > 2500)
			{
				NPC.active = false;
			}
		}

		RingRadius = RingRadius * 0.99f + RingRadiusTrend * 0.01f;
		RingCenter = RingCenter * 0.99f + RingCenterTrend * 0.01f;
	}
	/// <summary>
	/// 内部临时向量，用来存各种技能位置
	/// </summary>
	private Vector2 innerVector0 = Vector2.zeroVector;
	private Vector2 innerVector1 = Vector2.zeroVector;
	private Vector2 innerVector2 = Vector2.zeroVector;
	public override void HitEffect(NPC.HitInfo hit)
	{
		if (NPC.life <= 0)
		{
			for (int f = 0; f < 13; f++)
			{
				var gore2 = new FloatLanternGore3
				{
					Active = true,
					Visible = true,
					velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
					noGravity = false,
					position = NPC.Center
				};
				Ins.VFXManager.Add(gore2);
				var gore3 = new FloatLanternGore4
				{
					Active = true,
					Visible = true,
					velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
					noGravity = false,
					position = NPC.Center
				};
				Ins.VFXManager.Add(gore3);
				var gore4 = new FloatLanternGore5
				{
					Active = true,
					Visible = true,
					velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
					noGravity = false,
					position = NPC.Center
				};
				Ins.VFXManager.Add(gore4);
				var gore5 = new FloatLanternGore6
				{
					Active = true,
					Visible = true,
					velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
					noGravity = false,
					position = NPC.Center
				};
				Ins.VFXManager.Add(gore5);
			}
			var gore0Large = new LanternGhostKingGore0
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
				noGravity = false,
				position = NPC.Center
			};
			Ins.VFXManager.Add(gore0Large);

			var gore7Large = new LanternGhostKingGore7
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
				noGravity = false,
				position = NPC.Center
			};
			Ins.VFXManager.Add(gore7Large);

			var gore8Large = new LanternGhostKingGore8
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
				noGravity = false,
				position = NPC.Center
			};
			Ins.VFXManager.Add(gore8Large);

			var gore9Large = new LanternGhostKingGore9
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
				noGravity = false,
				position = NPC.Center
			};
			Ins.VFXManager.Add(gore9Large);

			Vector2 goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
			int gr1 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore1").Type, 1f);
			Main.gore[gr1].timeLeft = 900;

			goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
			int gr2 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore2").Type, 1f);
			Main.gore[gr2].timeLeft = 900;

			goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
			int gr3 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore3").Type, 1f);
			Main.gore[gr3].timeLeft = 900;

			goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
			int gr4 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore4").Type, 1f);
			Main.gore[gr4].timeLeft = 900;

			goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
			int gr16 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore10").Type, 1f);
			Main.gore[gr16].timeLeft = 900;
			for (int i = 0; i < 8; i++)
			{

				goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
				int gr17 = Gore.NewGore(null, NPC.position + new Vector2(Main.rand.NextFloat(-60, 60f), 40), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore11").Type, 1f);
				Main.gore[gr17].timeLeft = 900;
			}
			for (int i = 0; i < 8; i++)
			{

				goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
				int gr18 = Gore.NewGore(null, NPC.position + new Vector2(Main.rand.NextFloat(-60, 60f), 40), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore12").Type, 1f);
				Main.gore[gr18].timeLeft = 900;
			}
			for (int i = 0; i < 8; i++)
			{

				goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
				int gr19 = Gore.NewGore(null, NPC.position + new Vector2(Main.rand.NextFloat(-60, 60f), 40), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore13").Type, 1f);
				Main.gore[gr19].timeLeft = 900;
			}

			var lanternExplosion = new LanternExplosion
			{
				velocity = Vector2.Zero,
				Active = true,
				Visible = true,
				position = NPC.Center,
				maxTime = Main.rand.Next(126, 136),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(2.45f, 2.8f) * 3, Main.rand.NextFloat(8f, 12f) },
				FireBallVelocity = new Vector2[]
	{
				RandomVector2(2f, 0.01f),
				RandomVector2(1f, 0.01f)
	}
			};
			Ins.VFXManager.Add(lanternExplosion);

			for (int x = 0; x < 37; x++)
			{
				var flameDust = new FlameDust
				{
					velocity = RandomVector2(45f, 10f),
					Active = true,
					Visible = true,
					position = NPC.Center,
					maxTime = Main.rand.Next(26, 96),
					ai = new float[] { Main.rand.NextFloat(0.1f, 1f), 0, Main.rand.NextFloat(1f, 3.4f) }
				};
				Ins.VFXManager.Add(flameDust);
			}
			//Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<DarkLanternBombExplosion_II>(), 10000, 10);
			SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact.WithPitchOffset(-1f), NPC.Center);
		}
		if (NPC.life < NPC.lifeMax / 2)
		{
			if (Phase == 1)
			{
				Phase = 2;
				NPC.localAI[0] = 0;
			}
		}
	}
	public void SwitchAttackTypeInPhase2()
	{
		switch (Main.rand.Next(5))
		{
			case 0:
				NPC.localAI[0] = 1200;
				break;
			case 1:
				NPC.localAI[0] = 2000;
				break;
			case 2:
				NPC.localAI[0] = 1600;
				break;
			case 3:
				NPC.localAI[0] = 2400;
				break;
			case 4:
				NPC.localAI[0] = 3100;
				break;
		}
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		return false;
	}
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Vector2 originOfTexture = front.Size() * 0.5f;
		spriteBatch.Draw(back, NPC.Center - Main.screenPosition, null, GetColorRed(drawColor), NPC.rotation, originOfTexture, NPC.scale, SpriteEffects.None, 0f);

		DrawFlame();
		DrawVertexTexture(ModAsset.LanternGhostKing_side.Value, new Vector2(-160, -20), LeftRotation);
		DrawVertexTexture(ModAsset.LanternGhostKing_side.Value, new Vector2(160, -20), RightRotation, true);

		//第一阶段全屏炸雷时发光
		if (Phase == 1)
		{
			if (NPC.localAI[0] >= 1600 && NPC.localAI[0] < 1700)
			{
				float value = 1 - (NPC.localAI[0] - 1600) / 100f;
				value *= value;
				Color glowColor = new Color(value, value * value, value * value * value, 0);
				spriteBatch.Draw(left_glow, NPC.Center - Main.screenPosition, null, GetColorRed(glowColor), LeftRotation, originOfTexture, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(right_glow, NPC.Center - Main.screenPosition, null, GetColorRed(glowColor), RightRotation, originOfTexture, 1f, SpriteEffects.None, 0f);

				Lighting.AddLight(NPC.Center + new Vector2(64, 0), new Vector3(value * 0.5f, 0, 0));
				Lighting.AddLight(NPC.Center - new Vector2(64, 0), new Vector3(value * 0.5f, 0, 0));
			}
		}


		for (int i = 9; i >= 0; i--)
		{
			Color breadColor = Lighting.GetColor((NPC.Center / 16f).ToPoint() + new Point(5 - i, 8));
			breadColor *= 0.7f;
			breadColor.A = 255;
			spriteBatch.Draw(breads[i], NPC.Center - Main.screenPosition + new Vector2(MathF.Sin(i + (float)Main.time * 0.4f) * ShakeStrength, 0).RotatedBy(i), null, GetColorRed(breadColor), NPC.rotation + MathF.Sin(i + (float)Main.time * 0.6f) * 0.01f, originOfTexture, 1f, SpriteEffects.None, 0f);
		}
		Color shellColor = drawColor * 0.6f;
		shellColor.A = 255;
		//第二阶段变透明
		if (Phase == 2)
		{
			if (NPC.localAI[0] >= 700)
			{
				float value = (NPC.localAI[0] - 700) / 100f;
				value = Math.Clamp(value, 0, 1);
				value *= value;
				shellColor = drawColor * (float)Utils.Lerp(0.6f, 1.2f, value);
				shellColor.R = (byte)(MathF.Pow(shellColor.R / 255f, (float)Utils.Lerp(1f, 0.2f, value)) * 255);
				shellColor.A = (byte)Utils.Lerp(255, 105, value);
			}
		}
		spriteBatch.Draw(front_face, NPC.Center - Main.screenPosition, null, GetColorRed(shellColor), NPC.rotation, originOfTexture, 1f, SpriteEffects.None, 0f);
		shellColor = drawColor * 0.6f;
		shellColor.A = 255;
		spriteBatch.Draw(front, NPC.Center - Main.screenPosition, null, GetColorRed(shellColor), NPC.rotation, originOfTexture, 1f, SpriteEffects.None, 0f);
		//第二阶段十字光辉
		if (Phase == 2)
		{
			if (NPC.localAI[0] >= 700)
			{
				float value = (NPC.localAI[0] - 700) / 100f;
				value = Math.Clamp(value, 0, 1);
				value *= value;
				Texture2D star = Commons.ModAsset.StarSlash.Value;
				Vector2 orig = star.Size() / 2f;
				Vector2 offset = new Vector2(0, 50);
				float value2 = (255 - NPC.alpha) / 255f;
				float mulSize = MathF.Sin((float)Main.timeForVisualEffects * 0.4f) * 0.05f + 1;
				spriteBatch.Draw(star, NPC.Center - Main.screenPosition + offset, null, new Color(1f, 0.7f, 0.5f, 0), 0, orig, new Vector2(value, 0.5f * mulSize) * mulSize * value2, SpriteEffects.None, 0f);
				spriteBatch.Draw(star, NPC.Center - Main.screenPosition + offset, null, new Color(1f, 0.7f, 0.5f, 0), MathHelper.PiOver2, orig, new Vector2(value, 2.5f * mulSize) * mulSize * value2, SpriteEffects.None, 0f);
			}
		}

	}
	public void DrawVertexTexture(Texture2D tex, Vector2 offset, float rotation, bool flipH = false)
	{
		float width = tex.Width;
		float height = tex.Height;
		Vector2 position = NPC.Center + offset.RotatedBy(rotation);
		Vector2 v0 = position + new Vector2(-width, -height).RotatedBy(rotation) * 0.5f * NPC.scale;
		Vector2 v1 = position + new Vector2(width, -height).RotatedBy(rotation) * 0.5f * NPC.scale;
		Vector2 v2 = position + new Vector2(-width, height).RotatedBy(rotation) * 0.5f * NPC.scale;
		Vector2 v3 = position + new Vector2(width, height).RotatedBy(rotation) * 0.5f * NPC.scale;

		float alpha = (255 - NPC.alpha) / 255f;

		Color c0 = Lighting.GetColor((v0 / 16f).ToPoint()) * alpha;
		Color c1 = Lighting.GetColor((v1 / 16f).ToPoint()) * alpha;
		Color c2 = Lighting.GetColor((v2 / 16f).ToPoint()) * alpha;
		Color c3 = Lighting.GetColor((v3 / 16f).ToPoint()) * alpha;

		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(v0 - Main.screenPosition, c0, new Vector3(0, 0, 0)),
			new Vertex2D(v1 - Main.screenPosition, c1, new Vector3(1, 0, 0)),

			new Vertex2D(v2 - Main.screenPosition, c2, new Vector3(0, 1, 0)),
			new Vertex2D(v3 - Main.screenPosition, c3, new Vector3(1, 1, 0))
		};
		if(flipH)
		{
			bars = new List<Vertex2D>()
		{
			new Vertex2D(v0 - Main.screenPosition, c0, new Vector3(1, 0, 0)),
			new Vertex2D(v1 - Main.screenPosition, c1, new Vector3(0, 0, 0)),

			new Vertex2D(v2 - Main.screenPosition, c2, new Vector3(1, 1, 0)),
			new Vertex2D(v3 - Main.screenPosition, c3, new Vector3(0, 1, 0))
		};
		}
		Main.graphics.GraphicsDevice.Textures[0] = tex;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}
	public void DrawFlame()
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var effect = ModAsset.LanternFlame_King.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_rgb.Value);
		Vector2 pos = NPC.Center + new Vector2(0, 80);
		float timeValue = (float)(-Main.timeForVisualEffects * 0.001f);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.graphicsDevice.Textures[0] = ModAsset.FlameLightMap.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicClamp;
		Main.graphics.GraphicsDevice.Textures[1] = ModAsset.HeatMap_flameRing_lantern.Value;
		Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicClamp;
		Main.graphics.GraphicsDevice.Textures[2] = Commons.ModAsset.Noise_cell.Value;
		Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.AnisotropicClamp;
		float flameWidth = 260f;
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 0; i <= 10; i++)
		{
			Color color = GetColorRed(new Color(1f, 1f, 1f, 0));
			if (i > 7)
			{
				color *= (10 - i) / 4f;
			}
			bars.Add(pos + new Vector2(flameWidth, -i * flameWidth / 17f), color, new Vector3(0, 1 - i / 10f, i / 100f + timeValue));
			bars.Add(pos + new Vector2(0, -i * flameWidth / 17f), color, new Vector3(0.5f, 1 - i / 10f, i / 100f + timeValue));
		}

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 10; i++)
		{
			Color color = GetColorRed(new Color(1f, 1f, 1f, 0));
			if (i > 7)
			{
				color *= (10 - i) / 4f;
			}
			bars.Add(pos + new Vector2(0, -i * flameWidth / 17f), color, new Vector3(0.5f, 1 - i / 10f, i / 100f + timeValue));
			bars.Add(pos + new Vector2(-flameWidth, -i * flameWidth / 17f), color, new Vector3(1, 1 - i / 10f, i / 100f + timeValue));
		}

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
	public Color GetColorRed(Color c0)
	{
		Vector4 v0 = c0.ToVector4();
		float value = (255 - NPC.alpha) / 255f;
		value = Math.Clamp(value, 0, 1);
		v0.X *= MathF.Sqrt(value);
		v0.Y *= MathF.Pow(value, 2);
		v0.Z *= MathF.Pow(value, 3.3f);
		return new Color(v0.X, v0.Y, v0.Z, v0.W);
	}
	public Vector2 RandomVector2(float maxLength, float minLength = 0)
	{
		if (maxLength <= minLength)
		{
			maxLength = minLength + 0.001f;
		}
		return new Vector2(Main.rand.NextFloat(minLength, maxLength), 0).RotatedByRandom(6.283);
	}
}

