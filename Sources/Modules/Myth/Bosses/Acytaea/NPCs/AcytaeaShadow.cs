using Terraria.Localization;

namespace Everglow.Myth.Bosses.Acytaea.NPCs
{
	[AutoloadBossHead]
	public class AcytaeaShadow : ModNPC
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
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "雅斯塔亚");
		}

		private int Dam = 12;

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
			if (Main.expertMode)
				Dam = 10;
			if (Main.masterMode)
				Dam = 8;
			if (NPC.CountNPCS(ModContent.NPCType<Acytaea>()) <= 0 || Acytaea.BossIndex == 0)
			{
				if (NPC.active)
				{
					//int g = Projectile.NewProjectile(null, NPC.Center, vz, ModContent.ProjectileType<BloodBlade2>(), NPC.damage, 3, player.whoAmI, 1);
					//Main.projectile[g].rotation = (float)Math.Atan2(vz.Y, vz.X);
					for (int j = 0; j < 6; j++)
					{
						Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(6.28);
						Projectile.NewProjectile(null, NPC.Center + v * 2f, v, ModContent.ProjectileType<Projectiles.BrokenAcytaea2>(), 0, 1, Main.myPlayer);
					}
					NPC.active = false;
				}
			}
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if (!player.active || player.dead)
			{
				if ((player.Center - NPC.Center).Length() > 6000)
					NPC.active = false;
			}
			else
			{
				if ((player.Center - NPC.Center).Length() > 15000)
					NPC.active = false;
			}
			NPC.localAI[0] += 1;
			MinorDir = NPC.spriteDirection * -1;
			RightWingPos = new Vector2(-18, 0) * NPC.spriteDirection;
			LeftWingPos = new Vector2(-18, 0) * NPC.spriteDirection;
			RightArmPos = new Vector2(-10, 0) * NPC.spriteDirection;
			if (NPC.localAI[0] is > 0 and <= 400)
			{
				AIMpos = new Vector2(500, 0).RotatedBy(NPC.ai[0] * Math.PI / 7d * 2);
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
					v1 = (v0 - NPC.Center + v1 * 60f) / 480f;
					NPC.noGravity = true;
					NPC.velocity += v1;
					NPC.velocity *= 1 - NPC.localAI[0] / 1000f;
				}
				CanUseWing = (AIMpos + player.Center - NPC.Center).Length() > 1 && (AIMpos + player.Center - NPC.Center).Y < 0;
			}
			if (NPC.localAI[0] is > 400 and <= 500)
			{
				NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
				NPC.velocity *= 0.9f;
				CanUseWing = false;
			}
			if (NPC.localAI[0] is > 500 and <= 530)
			{
				MinorDir = NPC.spriteDirection * -1;
				NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
				NPC.velocity *= 0.96f;
				CanUseWing = false;
				RightArmRot += (float)(Math.PI * 1.1 / 30f) * MinorDir;
			}
			if (NPC.localAI[0] is > 530 and <= 550)
			{
				MinorDir = NPC.spriteDirection * -1;
				HasBlade = true;
				NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;
				BladeRot = (float)(RightArmRot - Math.PI * 1.1 - (MinorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
				NPC.velocity *= 0.96f;
				float a0 = (NPC.localAI[0] - 530) / 20f;
				BladePro = a0 * a0;
				CanUseWing = false;
			}
			if (NPC.localAI[0] is > 650 and <= 680)
			{
				float CosA0 = (float)(Math.Cos((NPC.localAI[0] - 650) / 30d * Math.PI) + 1) / 2f;//构造辅助函数
				RightArmRot = CosA0 * (float)(Math.PI * 1.1) * MinorDir;//旋转角度撕裂感
				OldBladeRot = BladeRot;//保证旋转方向正确记录
				BladeRot = (float)(RightArmRot - Math.PI * 1.1 - (MinorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
				NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
				NPC.velocity *= 0.96f;//考虑阻力
				BladePro = 1;//进程打满
				CanUseWing = false;//不准飞
				if (NPC.localAI[0] == 675)
				{
					for (int k = -3; k < 5; k += 2)
					{
						Vector2 v0a = Vector2.Normalize(player.Center - NPC.Center).RotatedBy(k / 3d);
						Projectile.NewProjectile(null, NPC.Center - v0a * 60, v0a * 2, ModContent.ProjectileType<Projectiles.BloodBladeShadow>(), Dam, 3, player.whoAmI);
					}
				}
			}
			if (NPC.localAI[0] is > 690 and <= 720)
			{
				//NPC.active = false;
				if (NPC.localAI[0] < 692)
					AimBladeSquz = Main.rand.NextFloat(0.65f, 1f);
				float CosA0 = (float)(Math.Cos((NPC.localAI[0] - 690) / 30d * Math.PI) + 1) / 2f;//构造辅助函数
				RightArmRot = (1 - CosA0) * (float)(Math.PI * 1.1) * MinorDir;//旋转角度撕裂感
				OldBladeRot = BladeRot;//保证旋转方向正确记录
				BladeRot = (float)(RightArmRot - Math.PI * 1.1 - (MinorDir - 1) * -0.3854);//刀的角度等于手臂角度-1.1Pi,此项由贴图决定
				NPC.rotation = Math.Clamp(NPC.velocity.X / 10f * (NPC.velocity.X / 10f), 0, 0.8f) * NPC.spriteDirection;//随速度前倾
				NPC.velocity *= 0.96f;//考虑阻力
				BladePro = 1;//进程打满
				CanUseWing = false;//不准飞
			}

			if (Fly)
			{
				if (DrawAI % 6 == 0)
				{
					if (wingFrame > 0)
					{
						if (wingFrame < 3)
							wingFrame += 1;
						else
						{
							wingFrame = 0;
						}
					}
					else
					{
						if (CanUseWing)
						{
							if (wingFrame < 3)
								wingFrame += 1;
							else
							{
								wingFrame = 0;
							}
						}
					}
				}
			}
			else
			{
				wingFrame = 0;
			}
			if (NPC.collideX)
				NPC.velocity.X *= (float)(2 - Math.Pow(1.01, NPC.velocity.Length()));//空气阻力于速度指数相关
			if (NPC.collideY)
				NPC.velocity.Y *= (float)(2 - Math.Pow(1.01, NPC.velocity.Length()));//空气阻力于速度指数相关
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return NPC.AnyNPCs(NPC.type) ? 0f : 0f;
		}

		private Vector2 RightArmPos;
		private Vector2 LeftWingPos;
		private Vector2 RightWingPos;
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

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
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
					Texture2D tx = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaShadow").Value;
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
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaRightWing").Value, NPC.Center + RightWingPos - Main.screenPosition, new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaLeftWing").Value, NPC.Center + LeftWingPos - Main.screenPosition, new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
			}
			else
			{
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaRightWing").Value, NPC.Center + RightWingPos - Main.screenPosition + new Vector2(10, 0), new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaLeftWing").Value, NPC.Center + LeftWingPos - Main.screenPosition + new Vector2(10, 0), new Rectangle(0, wingFrame * 56, 86, 56), color, NPC.rotation, new Vector2(43, 28), 1f, effects, 0f);
			}
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaLeftArm").Value, NPC.Center - Main.screenPosition, null, color, NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaBody").Value, NPC.Center - Main.screenPosition, null, color, NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaLeg").Value, NPC.Center - Main.screenPosition, null, color, NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
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
						Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/GoldenBloodScaleShader").Value;
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

				Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/GoldenBloodScaleMirror").Value;
				if (MinorDir == -1)
					t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/GoldenBloodScale").Value;
				Main.graphics.GraphicsDevice.Textures[0] = t;//GoldenBloodScaleMirror
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
			}
			if (MinorDir == 1)
			{
				if (RightArmRot <= Math.PI / 2d)
					Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaRightArm").Value, NPC.Center + RightArmPos - Main.screenPosition, null, color, NPC.rotation + RightArmRot, new Vector2(33, 23), 1f, effects, 0f);
			}
			else
			{
				if (RightArmRot >= -Math.PI / 2d)
					Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaRightArm").Value, NPC.Center + RightArmPos - Main.screenPosition + new Vector2(10, 0), null, color, NPC.rotation + RightArmRot, new Vector2(17, 23), 1f, effects, 0f);
			}
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaHead").Value, NPC.Center + Vector2.Zero - Main.screenPosition, new Rectangle(0, headFrame * 56, 50, 56), color, NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaEye").Value, NPC.Center + Vector2.Zero - Main.screenPosition, new Rectangle(0, headFrame * 56, 50, 56), new Color(255, 255, 255, 0), NPC.rotation, NPC.Size / 2f, 1f, effects, 0f);
			if (MinorDir == 1)
			{
				if (RightArmRot > Math.PI / 2d)
					Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaRightArm").Value, NPC.Center + RightArmPos - Main.screenPosition, null, color, NPC.rotation + RightArmRot, new Vector2(33, 23), 1f, effects, 0f);
			}
			else
			{
				if (RightArmRot < -Math.PI / 2d)
					Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/NPCs/AcytaeaRightArm").Value, NPC.Center + RightArmPos - Main.screenPosition + new Vector2(10, 0), null, color, NPC.rotation + RightArmRot, new Vector2(17, 23), 1f, effects, 0f);
			}
			return false;
		}
	}
}