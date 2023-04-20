using Everglow.Myth.Bosses.Acytaea.Projectiles;
using Terraria.Localization;

namespace Everglow.Myth.Bosses.Acytaea.NPCs;

[AutoloadBossHead]
public class AcytaeaShadow2 : ModNPC
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Acytaea");
		/*Main.npcFrameCount[NPC.type] = 50;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;*/
		NPCID.Sets.DangerDetectRange[NPC.type] = 400;
		NPCID.Sets.AttackType[NPC.type] = 0;
		NPCID.Sets.AttackTime[NPC.type] = 60;
		NPCID.Sets.AttackAverageChance[NPC.type] = 15;
			}

	private bool canDespawn = false;

	public override bool CheckActive()
	{
		return canDespawn;
	}

	public override void SetDefaults()
	{
		NPC.friendly = true;
		NPC.width = 40;
		NPC.height = 56;
		NPC.aiStyle = -1;
		NPC.damage = 100;
		NPC.defense = 100;
		NPC.lifeMax = 500;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath6;
		NPC.knockBackResist = 0.5f;
		NPC.friendly = false;
		NPC.dontTakeDamage = true;
		NPC.noTileCollide = true;
		// NPC.aiStyle = -1;
		//NPC.lifeMax = 50000;
		//NPC.life = 50000;
		//NPC.boss = true;
		// NPC.localAI[0] = 0;
		NPCID.Sets.TrailingMode[NPC.type] = 0;
		NPCID.Sets.TrailCacheLength[NPC.type] = 8;
	}

	public bool Fly = false;
	public bool Battle = false;
	public bool CanUseWing = false;
	private Vector2 AIMpos = new Vector2(200, 0);
	private int MinorDir = -1;

	public override void AI()
	{
		if (NPC.CountNPCS(ModContent.NPCType<Acytaea>()) <= 0)
		{
			if (NPC.active)
			{
				//int g = Projectile.NewProjectile(null, NPC.Center, vz, ModContent.ProjectileType<BloodBlade2>(), NPC.damage, 3, player.whoAmI, 1);
				//Main.projectile[g].rotation = (float)Math.Atan2(vz.Y, vz.X);
				for (int j = 0; j < 6; j++)
				{
					Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(6.28);
					Projectile.NewProjectile(null, NPC.Center + v * 2f, v, ModContent.ProjectileType<BrokenAcytaea2>(), 0, 1, Main.myPlayer);
				}
				NPC.active = false;
			}
		}
		if (Main.expertMode)
			Dam = 10;
		if (Main.masterMode)
			Dam = 8;
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
		if (!player.active || player.dead)
		{
			if ((player.Center - NPC.Center).Length() > 6000)
				NPC.active = false;
			canDespawn = true;
		}
		else
		{
			if ((player.Center - NPC.Center).Length() > 15000)
			{
				canDespawn = true;
				NPC.active = false;
			}
			canDespawn = false;
		}
		NPC.localAI[0] += 1;
		MinorDir = NPC.spriteDirection * -1;
		RightWingPos = new Vector2(-18, 0) * NPC.spriteDirection;
		LeftWingPos = new Vector2(-18, 0) * NPC.spriteDirection;
		RightArmPos = new Vector2(-10, 0) * NPC.spriteDirection;
		if (NPC.localAI[0] is > 0 and <= 40)
		{
			if (NPC.localAI[0] == 30)
			{
				if (NPC.ai[2] == 1)
				{
					if (Math.Abs(NPC.ai[1] - 300) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 150, 100, 2, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
					else if (Math.Abs(NPC.ai[1] + 300) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 150, -100, 2, 1);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
					else if (Math.Abs(NPC.ai[1] + 100) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 150, 0, 2, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
						g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 150, -100, 2, 0);
						vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
					else
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 150, 0, 2, 1);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
						g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 150, 100, 2, 1);
						vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
				}
				if (NPC.ai[2] == 2 && NPC.ai[3] == 0)
				{
					if (Math.Abs(NPC.ai[1] - 100) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 270, 190, 3, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
						g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 360, 120, 3, 0);
						vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
					else if (Math.Abs(NPC.ai[1] + 100) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 270, -190, 3, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
						g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 360, -120, 3, 0);
						vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
					else
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, -250, -100, 3, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
						g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, -250, 100, 3, 0);
						vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
				}
				if (NPC.ai[2] == 3 && NPC.ai[3] == 0)
				{
					if (Math.Abs(NPC.ai[1] + 190) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, -150, -150, 4, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
					else if (Math.Abs(NPC.ai[1] - 190) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 150, 150, 4, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
					else if (Math.Abs(NPC.ai[1] - 120) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 150, -150, 4, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
					else if (Math.Abs(NPC.ai[1] + 120) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, -150, 150, 4, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
					else if (Math.Abs(NPC.ai[1] - 100) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 0, 150, 4, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
						g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, -90, 200, 4, 0);
						vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
						g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 90, 200, 4, 0);
						vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
					else if (Math.Abs(NPC.ai[1] + 100) < 10f)
					{
						int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 0, -150, 4, 0);
						Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
						g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, -90, -200, 4, 0);
						vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
						g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 90, -200, 4, 0);
						vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
						Main.npc[g].velocity = vc;
					}
				}
				if (NPC.ai[2] == 4 && NPC.ai[3] == 0)
				{
					int g = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AcytaeaShadow2>(), 0, 175, 0, 5, 1);
					Vector2 vc = (new Vector2(Main.npc[g].ai[0], Main.npc[g].ai[1]) - AIMpos) / 30f;
					Main.npc[g].velocity = vc;
				}
			}
			AIMpos = new Vector2(NPC.ai[0], NPC.ai[1]) * 2;
			if ((AIMpos + player.Center - NPC.Center).Length() < 240)
			{
				NPC.spriteDirection = NPC.Center.X > player.Center.X ? -1 : 1;
				NPC.rotation *= 0.99f;
			}
			else
			{
				NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
				NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
			}
			Fly = true;
			if (Fly)
			{
				Vector2 v0 = AIMpos + player.Center;
				var v1 = Vector2.Normalize(v0 - NPC.Center);
				v1 = (v0 - NPC.Center + v1 * 60f) / 48f;
				NPC.noGravity = true;
				NPC.velocity += v1;
				NPC.velocity *= 1 - NPC.localAI[0] / 40f;
			}
			CanUseWing = (AIMpos + player.Center - NPC.Center).Length() > 1 && (AIMpos + player.Center - NPC.Center).Y < 0;
		}
		if (NPC.localAI[0] is > 40 and <= 500)
		{
			if (NPC.active)
			{
				if (NPC.ai[3] == 0)
				{
					Vector2 vz = new Vector2(0, 0.0007f).RotatedByRandom(6.28);
					int g = Projectile.NewProjectile(null, NPC.Center, vz, ModContent.ProjectileType<BloodBlade2>(), Dam, 3, player.whoAmI, 1);
					Main.projectile[g].rotation = (float)Math.Atan2(vz.Y, vz.X);
					for (int j = 0; j < 6; j++)
					{
						Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(6.28);
						Projectile.NewProjectile(null, NPC.Center + v * 2f, v, ModContent.ProjectileType<BrokenAcytaea2>(), 0, 1, Main.myPlayer);
					}
				}
				NPC.active = false;
			}
		}
		if (NPC.CountNPCS(ModContent.NPCType<Acytaea>()) <= 0 || Acytaea.BossIndex == 0)
		{
			if (NPC.active)
			{
				//int g = Projectile.NewProjectile(null, NPC.Center, vz, ModContent.ProjectileType<BloodBlade2>(), NPC.damage, 3, player.whoAmI, 1);
				//Main.projectile[g].rotation = (float)Math.Atan2(vz.Y, vz.X);
				for (int j = 0; j < 6; j++)
				{
					Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(6.28);
					Projectile.NewProjectile(null, NPC.Center + v * 2f, v, ModContent.ProjectileType<BrokenAcytaea2>(), 0, 1, Main.myPlayer);
				}
				NPC.active = false;
			}
		}
	}

	private int Dam = 12;

	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return NPC.AnyNPCs(NPC.type) ? 0f : 0f;
	}

	/*String Ta = "挑战";*/

	private Vector2 RightArmPos;
	private Vector2 LeftWingPos;
	private Vector2 RightWingPos;
	private float LeftArmRot = 0;
	private float RightArmRot = 0;
	private float BladePro = 0;
	private float BladeRot = 0;
	private float OldBladeRot = 0;
	private float BladeSquz = 1;
	private float AimBladeSquz = 1;
	private int wingFrame = 0;
	private int headFrame = 0;
	private bool HasBlade = false;
	private int DrawAI = 0;
	private Vector2[] OldBladePos = new Vector2[70];

	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		if (!Main.gamePaused)
			DrawAI++;
		if (DrawAI % 480 == 440)
			headFrame = 1;
		if (DrawAI % 480 == 455)
			headFrame = 0;
		SpriteEffects effects = SpriteEffects.None;
		if (NPC.spriteDirection == 1)
			effects = SpriteEffects.FlipHorizontally;
		for (int h = 0; h < 8; h++)
		{
			if (h % 2 == 1)
			{
				Texture2D tx = ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaShadow").Value;
				if (NPC.oldPos[h] != Vector2.Zero)
				{
					float Col = Math.Clamp(NPC.velocity.Length() / 10f, 0, 1) * (8 - h) / 8f;
					var colorφ = new Color(Col, Col, Col, 0);
					spriteBatch.Draw(tx, NPC.oldPos[h] - Main.screenPosition + NPC.Size / 2f, null, colorφ, NPC.rotation, NPC.Size / 2f, NPC.scale, effects, 0f);
				}
			}
		}
		var color = new Color(255, 255, 255, 0);
		if (MinorDir == 1)
		{
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaRightWing").Value, NPC.Center + RightWingPos - Main.screenPosition, new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaLeftWing").Value, NPC.Center + LeftWingPos - Main.screenPosition, new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
		}
		else
		{
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaRightWing").Value, NPC.Center + RightWingPos - Main.screenPosition + new Vector2(10, 0), new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaLeftWing").Value, NPC.Center + LeftWingPos - Main.screenPosition + new Vector2(10, 0), new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
		}
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaLeftArm").Value, NPC.Center - Main.screenPosition, null, color, NPC.rotation + LeftArmRot, NPC.Size / 2f, 1f, effects, 0f);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaBody").Value, NPC.Center - Main.screenPosition, null, color, NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaLeg").Value, NPC.Center - Main.screenPosition, null, color, NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
		if (HasBlade)
		{
			if (NPC.localAI[0] <= 900)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
				if (BladePro == 1)
				{
					var Vx = new List<Vertex2D>();
					Vector2 vBla = new Vector2(88 - (MinorDir - 1) * 8, -158).RotatedBy(BladeRot);
					vBla.Y *= BladeSquz;
					Vector2 vc = NPC.Center + vBla;
					BladeSquz = BladeSquz * 0.75f + AimBladeSquz * 0.25f;

					if (!Main.gamePaused)
					{
						for (int h = 59; h > 0; h--)
						{
							OldBladePos[h] = OldBladePos[h - 1];
						}
						OldBladePos[0] = vc;
					}
					int MaxH = 0;
					for (int h = 0; h < 60; h++)
					{
						if (OldBladePos[h + 1] == Vector2.Zero)
							break;
						MaxH++;
					}
					Vector2 vf = NPC.Center + RightArmPos - Main.screenPosition + new Vector2(-7f, 3).RotatedBy(RightArmRot);

					for (int h = 0; h < 60; h++)
					{
						var color3 = new Color(255, 255, 255, 0)
						{
							A = 0
						};
						color3.R = (byte)(color3.R * (MaxH - h - 1) / (float)MaxH);
						color3.G = (byte)(color3.G * (MaxH - h - 1) / (float)MaxH);
						color3.B = (byte)(color3.B * (MaxH - h - 1) / (float)MaxH);
						if (OldBladePos[h + 1] == Vector2.Zero)
							break;
						if (BladeRot < OldBladeRot)
						{
							Vx.Add(new Vertex2D(OldBladePos[h] - Main.screenPosition, color3, new Vector3(h / 60f, 0, 0)));
							Vx.Add(new Vertex2D(OldBladePos[h + 1] - Main.screenPosition, color3, new Vector3((h + 1) / 60f, 0, 0)));
							Vx.Add(new Vertex2D(vf, color3, new Vector3(0, 1, 0)));
						}
						else
						{
							Vx.Add(new Vertex2D(OldBladePos[h + 1] - Main.screenPosition, color3, new Vector3((h + 1) / 60f, 0, 0)));
							Vx.Add(new Vertex2D(OldBladePos[h] - Main.screenPosition, color3, new Vector3(h / 60f, 0, 0)));
							Vx.Add(new Vertex2D(vf, color3, new Vector3(0, 1, 0)));
						}
					}
					Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/GoldenBloodScaleShader").Value;
					Main.graphics.GraphicsDevice.Textures[0] = t;//GoldenBloodScaleMirror
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
				}
			}
		}
		if (HasBlade)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			var Vx = new List<Vertex2D>();
			var color3 = new Color((int)(255 * BladePro), (int)(255 * BladePro), (int)(255 * BladePro), 0);
			Vector2 vBla = new Vector2(88 - (MinorDir - 1) * 8, -158).RotatedBy(BladeRot);
			vBla.Y *= BladeSquz;
			Vector2 vc = NPC.Center + vBla - Main.screenPosition;
			Vector2 vd = new Vector2(17.02f, 0).RotatedBy(0.4 + BladeRot) + vc;
			Vector2 ve = new Vector2(-17.02f, 0).RotatedBy(0.4 + BladeRot) + vc;
			Vector2 vf = NPC.Center + RightArmPos - Main.screenPosition + new Vector2(-7f, 3).RotatedBy(RightArmRot);
			Vector2 vg = new Vector2(17.02f, 0).RotatedBy(0.4 + BladeRot) + vf;
			Vector2 vh = new Vector2(-17.02f, 0).RotatedBy(0.4 + BladeRot) + vf;

			Vx.Add(new Vertex2D(vg, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
			Vx.Add(new Vertex2D(vh, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
			Vx.Add(new Vertex2D(vd, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.68f, 0, 1), Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).Y + 0.32f, 0, 1), 0)));

			Vx.Add(new Vertex2D(vh, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.03f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.97f, 0, 1), 0)));
			Vx.Add(new Vertex2D(ve, color3, new Vector3(Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).X + 0.68f, 0, 1), Math.Clamp(new Vector2(-11.84f / 122f, 0).RotatedBy(0.4).Y + 0.32f, 0, 1), 0)));
			Vx.Add(new Vertex2D(vd, color3, new Vector3(Math.Clamp(new Vector2(11.84f / 122f, 0).RotatedBy(0.4).X + 0.68f, 0, 1), Math.Clamp(new Vector2(11.85f / 122f, 0).RotatedBy(0.4).Y + 0.32f, 0, 1), 0)));

			Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/GoldenBloodScaleMirror").Value;
			if (MinorDir == -1)
				t = ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/GoldenBloodScale").Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;//GoldenBloodScaleMirror
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
		}
		if (MinorDir == 1)
		{
			if (RightArmRot <= Math.PI / 2d)
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaRightArm").Value, NPC.Center + RightArmPos - Main.screenPosition, null, color, NPC.rotation + RightArmRot, new Vector2(33, 23), 1f, effects, 0f);
		}
		else
		{
			if (RightArmRot >= -Math.PI / 2d)
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaRightArm").Value, NPC.Center + RightArmPos - Main.screenPosition + new Vector2(10, 0), null, color, NPC.rotation + RightArmRot, new Vector2(17, 23), 1f, effects, 0f);
		}
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaHead").Value, NPC.Center - Main.screenPosition, new Rectangle(0, headFrame * 56, 50, 56), color, NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaEye").Value, NPC.Center - Main.screenPosition, new Rectangle(0, headFrame * 56, 50, 56), new Color(255, 255, 255, 0), NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
		if (MinorDir == 1)
		{
			if (RightArmRot > Math.PI / 2d)
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaRightArm").Value, NPC.Center + RightArmPos - Main.screenPosition, null, color, NPC.rotation + RightArmRot, new Vector2(33, 23), 1f, effects, 0f);
		}
		else
		{
			if (RightArmRot < -Math.PI / 2d)
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/NPCs/AcytaeaRightArm").Value, NPC.Center + RightArmPos - Main.screenPosition + new Vector2(10, 0), null, color, NPC.rotation + RightArmRot, new Vector2(17, 23), 1f, effects, 0f);
		}
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		return false;
	}
}