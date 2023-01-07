//using MythMod.Buffs;
using Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Fragrans;
using Terraria.DataStructures;
//using System.Drawing;
using Terraria.ModLoader.IO;

namespace Everglow.Sources.Modules.MythModule.Common
{
	public class MythContentPlayer : ModPlayer
	{
		public static int GoldLiquidPupil = 0;
		public static int WhitePedal = 0;
		public static int ThreeColorCrown = 0;
		public static int ThreeColorCrownCool = 0;
		public static int ThreeColorCrownBuff1 = 0;
		public static int ThreeColorCrownBuff2 = 0;
		public static int ThreeColorCrownBuff3 = 0;
		public static int OrangeStickCool = 0;
		public static int OrangeStick = 0;
		public static int PurpleBallFlower = 0;
		public static int PurpleBallFlowerCool = 0;
		public static int CyanPedal = 0;
		public static int CyanBranch = 0;
		public static int CyanBranchCool = 0;
		public static int BlueTorchFlower = 0;
		public static int BlueTorchFlowerTime = 0;
		public static bool create1sand = false;
		public static bool Turn = true;
		public static int ImmuCurseF = 0;
		public static int Dashcool = 0;
		public static int RainCritAdd = 0;
		public static float RainDamageAdd = 0;
		public static float StackDamageAdd = 0;
		public static int RainLifeAdd = 0;
		public static float RainManaAdd = 0;
		public static float RainManaAdd2 = 0;
		public static float RainSpeedAdd = 0;
		public static int RainSpeedAddTime = 0;
		public static int RainDefenseAdd = 0;
		public static float RainMissAdd = 0;
		public static float Miss = 0;
		public static int damage40 = 0;
		public static float CritDamage = 1f;
		public static int SilverBuff = 0;
		public static float AddCritDamage = 0;
		public static bool DefMoth;
		public static bool DefTusk;
		public static float AddDamagePoint = 0;
		Vector2[] PlayerOldPos = new Vector2[15];
		Vector2 ABTuskPosi = Vector2.Zero;
		Vector2 ABMothPosi = Vector2.Zero;
		bool HasABTusk = false;
		bool HasABMoth = false;
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (Player.name == "澹水飘烟")
			{
				Player.active = true;
				Player.dead = false;
				Player.statLife = Player.statLifeMax2;
				Player.respawnTimer = 0;
				return false;
			}
			return true;
		}
		public override void PreUpdate()
		{

			//TextureAssets. = ModContent.Request<Texture2D>("MythMod/UIImages/冰封");
			//Buff秩序之锁,施加之后禁止传送//PurpleBallFlower

			/*if (ZoneTusk > 0)
            {
                ZoneTusk--;
            }
            else
            {
                ZoneTusk = 0;
            }*/
			if (GoldLiquidPupil > 0)
			{
				GoldLiquidPupil--;
			}
			else
			{
				GoldLiquidPupil = 0;
			}
			if (WhitePedal > 0)
			{
				WhitePedal--;
			}
			else
			{
				WhitePedal = 0;
			}
			if (ThreeColorCrownBuff3 > 0)
			{
				ThreeColorCrownBuff3--;
			}
			else
			{
				ThreeColorCrownBuff3 = 0;
			}
			if (ThreeColorCrownBuff2 > 0)
			{
				ThreeColorCrownBuff2--;
			}
			else
			{
				ThreeColorCrownBuff2 = 0;
			}
			if (ThreeColorCrownBuff1 > 0)
			{
				ThreeColorCrownBuff1--;
			}
			else
			{
				ThreeColorCrownBuff1 = 0;
			}
			if (ThreeColorCrownCool > 0)
			{
				ThreeColorCrownCool--;
			}
			else
			{
				ThreeColorCrownCool = 0;
			}
			if (ThreeColorCrown > 0)
			{
				ThreeColorCrown--;
			}
			else
			{
				ThreeColorCrown = 0;
			}
			if (OrangeStickCool > 0)
			{
				OrangeStickCool--;
			}
			else
			{
				OrangeStickCool = 0;
			}
			if (OrangeStick > 0)
			{
				OrangeStick--;
			}
			else
			{
				OrangeStick = 0;
			}
			if (PurpleBallFlowerCool > 0)
			{
				PurpleBallFlowerCool--;
			}
			else
			{
				PurpleBallFlowerCool = 0;
			}
			if (PurpleBallFlower > 0)
			{
				PurpleBallFlower--;
			}
			else
			{
				PurpleBallFlower = 0;
			}
			if (CyanBranchCool > 0)
			{
				CyanBranchCool--;
			}
			else
			{
				CyanBranchCool = 0;
			}
			if (CyanBranch > 0)
			{
				CyanBranch--;
			}
			else
			{
				CyanBranch = 0;
			}
			if (CyanPedal > 0)
			{
				CyanPedal--;
			}
			else
			{
				CyanPedal = 0;
			}
			if (BlueTorchFlower > 0)
			{
				BlueTorchFlower--;
			}
			else
			{
				BlueTorchFlower = 0;
			}
			if (BlueTorchFlowerTime > 0)
			{
				BlueTorchFlowerTime--;
			}
			else
			{
				BlueTorchFlowerTime = 0;
			}



			//视频效果
			
			/*if(Player.HasBuff(ModContent.BuffType<Buffs.BanTransport>()) && !Player.name.Contains("万象"))
            {
                bool CheckHitLT = Collision.SolidCollision(Player.position, Player.width, Player.height);
                bool CheckCanHit = Collision.CanHit(Player.position, 1, 1, PlayerOldPos[1], 1, 1);
                bool LengMax = (Player.position - PlayerOldPos[1]).Length() > 30f;
                if (CheckHitLT && PlayerOldPos[1] != Vector2.Zero && LengMax)
                {
                    Player.position = PlayerOldPos[1];
                }
                if (!CheckCanHit && LengMax && PlayerOldPos[1] != Vector2.Zero)
                {
                    Player.position = PlayerOldPos[1];
                }
                PlayerOldPos[0] = Player.position;
                for (int f = PlayerOldPos.Length - 1; f > 0; f--)
                {
                    PlayerOldPos[f] = PlayerOldPos[f - 1];
                }
                for (int f = PlayerOldPos.Length - 1; f > 0; f--)
                {
                    if (PlayerOldPos[f] == Vector2.Zero && !Player.dead)
                    {
                        break;
                    }
                    if (Player.dead)
                    {
                        PlayerOldPos[f] = Vector2.Zero;
                    }
                    if ((PlayerOldPos[f] - PlayerOldPos[f - 1]).Length() > 100)
                    {
                        Player.position = PlayerOldPos[f];
                        PlayerOldPos[Math.Clamp(f - 1, 0, 14)] = PlayerOldPos[f];
                        PlayerOldPos[Math.Clamp(f, 0, 14)] = PlayerOldPos[f];
                        PlayerOldPos[0] = PlayerOldPos[f];
                        break;
                    }
                    bool CheckHitLT2 = Collision.SolidCollision(PlayerOldPos[f - 1] + new Vector2(6, 9), Player.width - 12, Player.height - 18);
                    if (CheckHitLT2 && PlayerOldPos[f] != Vector2.Zero)
                    {
                        Player.position = PlayerOldPos[f];
                        PlayerOldPos[Math.Clamp(f - 1, 0, 14)] = PlayerOldPos[f];
                        PlayerOldPos[Math.Clamp(f, 0, 14)] = PlayerOldPos[f];
                        PlayerOldPos[0] = PlayerOldPos[f];
                        break;
                    }
                }
            }*/
			//if (Main.ActiveWorldFileData.Path.Contains("OcEaNMyTh") && !Player.HasBuff(ModContent.BuffType<Buffs.BanTransport>()))
			//{
			//	Player.AddBuff(ModContent.BuffType<Buffs.BanTransport>(), 2000000000);
			//}


			AddCritDamage = 0;
			for (int f = 0; f < Player.armor.Length; f++)
			{
				//if (Player.armor[f].type == ModContent.ItemType<Items.Accessories.WalnutClip>())
				//{
				//	AddCritDamage += 0.16f;
				//}
				//if (Player.armor[f].type == ModContent.ItemType<Items.Accessories.SilverWing>())
				//{
				//	AddCritDamage += 0.08f;
				//}
				//if (Player.armor[f].type == ModContent.ItemType<Items.Accessories.RedGel>())
				//{
				//	AddCritDamage += 0.09f;
				//}
			}
			
			if (damage40 > 0)
			{
				damage40--;
			}
			else
			{
				damage40 = 0;
			}
			if (SilverBuff > 0)
			{
				SilverBuff--;
			}
			else
			{
				SilverBuff = 0;
			}
			if (ImmuCurseF > 0)
			{
				ImmuCurseF--;
			}
			else
			{
				ImmuCurseF = 0;
			}

			if (Dashcool > 0)
			{
				Dashcool--;
			}
			else
			{
				Dashcool = 0;
			}
			if (IMMUNE > 0)
			{
				Player.immune = true;
				IMMUNE--;
			}
			else
			{
				IMMUNE = 0;
			}

			if (RainSpeedAddTime > 0)
			{
				RainSpeedAddTime--;
			}
			else
			{
				RainSpeedAddTime = 0;
			}
			base.PreUpdate();
		}
		public override void PostUpdateMiscEffects()
		{
			int MaxS = -1;
			for (int p = 0; p < 58; p++)
			{
				if (Player.inventory[p].type == Player.HeldItem.type)
				{
					MaxS += 1;
				}
			}
			if (MaxS > 5)
			{
				MaxS = 5;
			}
			StackDamageAdd = MaxS * 0.05f;
			Miss = 0;
			CritDamage = 1f;
			Player.GetCritChance(DamageClass.Generic) += RainCritAdd;
			Player.GetDamage(DamageClass.Generic) *= RainDamageAdd + 1;
			Player.GetCritChance(DamageClass.Generic) += Fragrans.FragCritAdd;
			Player.GetDamage(DamageClass.Generic) *= Fragrans.FragDamageAdd + 1;
			Player.GetDamage(DamageClass.Generic) *= StackDamageAdd + 1;
			Miss += RainMissAdd;
			if (CyanPedal > 0)
			{
				Miss += 4;
			}
			if (WhitePedal > 0)
			{
				Miss += 2;
			}
			if (CyanBranchCool > 0)
			{
				Miss += 8;
			}
			Player.lifeRegen += RainLifeAdd;
			CritDamage += AddCritDamage;
			RainManaAdd2 = RainManaAdd;
			if (RainManaAdd2 > 10)
			{
				Player.statMana += 1;
				RainManaAdd2 -= 10;
				if (RainManaAdd2 > 10)
				{
					Player.statMana += 1;
					RainManaAdd2 -= 10;
					if (RainManaAdd2 > 10)
					{
						Player.statMana += 1;
						RainManaAdd2 -= 10;
						if (RainManaAdd2 > 10)
						{
							Player.statMana += 1;
							RainManaAdd2 -= 10;
							if (RainManaAdd2 > 10)
							{
								Player.statMana += 1;
								RainManaAdd2 -= 10;
								if (RainManaAdd2 > 10)
								{
									Player.statMana += 1;
									RainManaAdd2 -= 10;
									if (RainManaAdd2 > 10)
									{
										Player.statMana += 1;
										RainManaAdd2 -= 10;
									}
								}
							}
						}
					}
				}
			}
			if (Main.rand.Next(10) < RainManaAdd2)
			{
				Player.statMana += 1;
			}
			Player.statDefense += RainDefenseAdd;
			if (RainSpeedAddTime > 0)
			{
				Player.maxRunSpeed += RainSpeedAdd / 6f;
				Player.maxFallSpeed += RainSpeedAdd;
				Player.wingAccRunSpeed += RainSpeedAdd;
			}
			if (SilverBuff > 0)
			{
				Player.maxRunSpeed += 0.05f;
				Player.maxFallSpeed += 1.5f;
				Player.wingAccRunSpeed += 0.05f;
				Player.GetDamage(DamageClass.Generic) *= 1.05f;
			}

			if (TheTusk.Items.Weapons.ToothSpear.coll > 0)
			{
				Player.maxFallSpeed += 1000f;
			}
			if (MiscItems.Weapons.Fragrans.FragransSpear.coll > 0)
			{
				Player.maxFallSpeed += 1000f;
			}
			if (MiscItems.Weapons.Hepuyuan.coll > 0)
			{
				Player.maxFallSpeed += 1800f;
			}
			//if (Player.ownedProjectileCounts[ModContent.ProjectileType<EternalResolveMod.Items.Weapons.Stabbings.StabbingCatchEnemiesProj>()] > 0)
			//{
			//	Player.maxFallSpeed += 1800f;
			//}
		}
		public static int IMMUNE = 0;
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
		{
			if (damage < 3)
			{
				return false;
			}
			if (OrangeStick > 0)
			{
				OrangeStickCool = 300;
			}
			if (CyanBranch > 0)
			{
				CyanBranchCool = 600;
			}
			float vl = Player.velocity.Length();
			if (vl > 30)
			{
				vl = 30;
			}
			if (vl < 5)
			{
				vl = 3 * vl - 10;
			}
			float Misp = (Miss + vl * Miss * 0.1f) / 100f;
			Misp = (float)(-1.1 / (Misp + 1) + 1.1);
			if (Main.rand.NextFloat(0, 1f) <= Misp && IMMUNE == 0)
			{
				CombatText.NewText(new Rectangle((int)Player.Center.X - 10, (int)Player.Center.Y - 10, 20, 20), Color.Cyan, "Miss");
				IMMUNE = 30;
				if (Player.longInvince)
				{
					IMMUNE = 45;
				}

				return false;
			}
			if (IMMUNE > 0)
			{
				return false;
			}

			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
		}
		//public override void ModifyScreenPosition() //TODO: Provide a better FlyCamPosition tracking animation for BloodTusk
		//{
		//	Main.screenPosition += FlyCamPosition;

		//	if (MoveSCoolingTUsk > 0)
		//	{
		//		if (TuskWhoAmI != -1)
		//		{
		//			MoveSCoolingTUsk--;
		//			TuskDelP = Main.npc[TuskWhoAmI].Center - Player.Center;
		//			if (MoveSCoolingTUsk < 30)
		//			{
		//				float k = MoveSCoolingTUsk / 30f;
		//				k = (float)(Math.Sin((k - 0.5) * Math.PI) + 1) / 2f;
		//				TuskDelP = (Main.npc[TuskWhoAmI].Center - Player.Center) * k;
		//			}
		//			if (MoveSCoolingTUsk > 270)
		//			{
		//				float k = (200 - MoveSCoolingTUsk) / 30f;
		//				k = (float)(Math.Sin((k - 0.5) * Math.PI) + 1) / 2f;
		//				TuskDelP = (Main.npc[TuskWhoAmI].Center - Player.Center) * k;
		//			}
		//			Main.screenPosition += TuskDelP;
		//		}
		//	}
		//	else
		//	{
		//		if (NPC.CountNPCS(ModContent.NPCType<NPCs.BloodTusk.BloodTusk>()) > 0)
		//		{
		//			for (int g = 0; g < 200; g++)
		//			{
		//				if (Main.npc[g].active && Main.npc[g].life <= Main.npc[g].lifeMax / 2)
		//				{
		//					if (Main.npc[g].ai[2] == 254)
		//					{
		//						TuskWhoAmI = g;
		//						MoveSCoolingTUsk = 300;
		//					}
		//				}
		//			}
		//		}
		//		else
		//		{
		//			TuskWhoAmI = -1;
		//		}
		//		TuskDelP = Vector2.Zero;
		//	}
		//	FlyCamPosition = Vector2.Zero;
		//}
		private Vector2 TuskDelP;
		int TuskWhoAmI = -1;
		int MoveSCoolingTUsk = 0;
		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			//if (ImmuCurseF > 0)
			//{
			//	for (int u = 0; u < 5; u++)
			//	{
			//		Vector2 v = new Vector2(0, Main.rand.NextFloat(8f, 14f)).RotatedByRandom(Math.PI * 2f);
			//		int t = Projectile.NewProjectile(null, Player.Center, v, 96, 100, 5, Player.whoAmI, 0f, 0f);
			//		Main.projectile[t].hostile = false;
			//		Main.projectile[t].friendly = true;
			//		Main.projectile[t].timeLeft = 15;
			//	}
			//	Projectile.NewProjectile(null, Player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Magic.EndlessCurseFlame>(), 100, 5, Player.whoAmI, 0f, 0f);
			//}
			//if (GoldLiquidPupil > 0)
			//{
			//	for (int g = 0; g < Main.projectile.Length; g++)
			//	{
			//		if (Main.projectile[g].type == ModContent.ProjectileType<Projectiles.Accessory.IchorRing>() && Main.projectile[g].timeLeft > 60 && Main.projectile[g].owner == Player.whoAmI)
			//		{
			//			Main.projectile[g].timeLeft = 60;
			//		}
			//	}
			//	Projectile.NewProjectile(null, Player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Accessory.IchorRing>(), 0, 5, Player.whoAmI, Player.GetCritChance(DamageClass.Generic), 0f);
			//}
		}
		public override void PostUpdate()
		{
			if (Player.HeldItem.type == ModContent.ItemType<TheTusk.Items.Weapons.ToothBow>())
			{
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<TheTusk.Projectiles.Weapon.TuskArrow>()] > 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<TheTusk.Projectiles.Weapon.TuskAim>()] < 1)
				{
					if (Main.mouseRight)
					{
						Projectile.NewProjectile(null, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType< TheTusk.Projectiles.Weapon.TuskAim>(), 0, 0, Player.whoAmI);
					}
				}
			}
		}
		public override void PreUpdateBuffs()
		{
			////Player.AddBuff(ModContent.BuffType<Freeze>(),10);
			//if (Player.HasBuff(ModContent.BuffType<Freeze>()))
			//{
			//	Player.position = Player.oldPosition;
			//	//Main.spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/UIImages/冰封").Value, Player.Center - Main.screenPosition, null, new Color(150, 150, 150, 50), 0, new Vector2(103, 100), 1.5f, SpriteEffects.None, 0f);
			//}
			//base.PreUpdateBuffs();
		}
		public static float Y0 = 1;
		public static bool HasY0 = false;
		public override void Load()
		{

		}
		int GenshinPlayer = 0;
		int GenshinFrame = 0;
		int GenshinCount = 0;
		int GenshinMaxF = 0;
		public static bool CanDrawUnderItemUI = true;
		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (Main.playerInventory)
			{
				/*if (Main.GameViewMatrix.Zoom.Y == 1 && CanDrawUnderItemUI)
                {
                    Main.spriteBatch.DrawTenPieces(TextureAssets.MagicPixel.Value, (int)((Main.GameViewMatrix.Zoom.X - 1) * 100), (int)((Main.GameViewMatrix.Zoom.Y - 1) * 100), 10, 10, 10);
                }*/
				if (Main.GameViewMatrix.Zoom.Y != Y0 && !HasY0)
				{
					CombatText.NewText(new Rectangle((int)Player.Center.X - 10, (int)Player.Center.Y - 10, 20, 20), Color.Wheat, "翻页背包功能在放大情况下自动禁用");
					HasY0 = true;
				}
				Y0 = Main.GameViewMatrix.Zoom.Y;
			}
			//Vector2 aa = Main.MouseWorld;

			//Main.spriteBatch.DrawEffects(TextureAssets.MagicPixel.Value, 10, 10, 10, 10, 10);
			//if (Player.HasBuff((ModContent.BuffType<Freeze>())))
			//{
			//	drawInfo.DrawDataCache.Add(new DrawData(ModContent.Request<Texture2D>("MythMod/UIImages/冰封").Value, Player.Center - Main.screenPosition, null, new Color(150, 150, 150, 50), 0f, new Vector2(103, 100), 1.5f, SpriteEffects.None, 0));
			//}
			//if (Main.maxTilesX == 4200)
			//{
			//	sand = ModContent.Request<Texture2D>("MythMod/Map/SmallOce001").Value;
			//	sandWall = ModContent.Request<Texture2D>("MythMod/Map/SmallOce002").Value;
			//}
			//if (Main.maxTilesX == 6400)
			//{
			//	sand = ModContent.Request<Texture2D>("MythMod/Map/MiddleOce001").Value;
			//	sandWall = ModContent.Request<Texture2D>("MythMod/Map/MiddleOce002").Value;
			//}

			if (GenshinPlayer > 0)
			{
				Color color = Lighting.GetColor((int)(Player.Center.X / 16d), (int)(Player.Center.Y / 16d));
				Rectangle DesR = new Rectangle((int)(Player.position.X - Main.screenPosition.X), (int)(Player.position.Y - Main.screenPosition.Y), Player.width, Player.height);
				Texture2D PlayerTex = ModContent.Request<Texture2D>("MythMod/GenshinPlayers/Traveller").Value;
				Rectangle SouR = new Rectangle(0, (int)(PlayerTex.Height / 6f * GenshinFrame), PlayerTex.Width, PlayerTex.Height / 6);
				Main.spriteBatch.Draw(PlayerTex, DesR, SouR, color);
				if (Collision.SolidCollision(Player.Bottom, 1, 8) && Player.velocity.X != 0)
				{
					GenshinCount++;
				}
				else
				{
					GenshinFrame = 0;
					GenshinCount = 0;
				}
				if (GenshinCount == 8)
				{
					GenshinCount = 0;
					if (GenshinFrame < GenshinMaxF - 1)
					{
						GenshinFrame++;
					}
					else
					{
						GenshinFrame = 0;
					}
				}
				r = 0;
				g = 0;
				b = 0;
				a = 0;
			}
			base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
		}
		bool HasTrivel = false;
		public override void SaveData(TagCompound tag)
		{
			/*for (int f = 0; f < 292; f++)
            {
                TextureAssets.Background[f] = MythMod.UB[f];
            }*/
			//TextureAssets.Fade = Common.GlobalTiles.MythModGlobalWall.BlackT;
		}

		public override void LoadData(TagCompound tag)
		{
		}
	}
}
