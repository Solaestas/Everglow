using Everglow.Sources.Modules.MythModule.MiscBuffs;
using Terraria.Audio;
namespace Everglow.Sources.Modules.MythModule.Common
{
	public class MythContentGlobalNPC : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
		{
			return true;
		}
		//public static int[] MothStack = new int[200];
		public override void AI(NPC npc)
		{
			//Make the guide giant and green.
			/*npc.scale = 1.5f;
			npc.color = Color.ForestGreen;*/
		}
		public static int[] LaserMark = new int[200];
		public static int[] LaserMark2 = new int[200];
		public static bool Des0 = false;
		/*int[] Ty1 = { ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinDamage1>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinCrit1>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinDefense1>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinSpeed1>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinMelee1>() };
        int[] Ty2 = { ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinDamage2>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinCrit2>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinDefense2>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinSpeed2>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinMelee2>() };
        int[] Ty3 = { ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinDamage3>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinCrit3>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinDefense3>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinSpeed3>(), ModContent.ItemType<Items.Weapons.Activity.Always.FixCoinMelee3>() };
        int HasL = 0;*/
		public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			Player player = Main.LocalPlayer;

			if (crit)
			{
				damage *= (MythContentPlayer.CritDamage + 1) / 2f;
			}
			if (player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.SilveralGun>() || player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.SilveralRifle>())
			{
				MythContentPlayer.SilverBuff = 300;
			}
			if (MythContentPlayer.GoldLiquidPupil > 0)
			{
				damage += npc.defense * 0.35;
			}
			//if (npc.HasBuff(ModContent.BuffType<OnMoth>()))
			//{
			//	if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.Legendary.DarknessFan>())
			//	{
			//		damage *= 1.0f + (MothStack[npc.whoAmI]) / 10f;
			//	}
			//}
			if (LaserMark2[npc.whoAmI] > 0)
			{
				double OldDam = damage;
				damage *= 1.5;
				if (crit)
				{
					damage *= 1.25;
				}
				//player.addDPS((int)(damage - OldDam));
			}
			/*for (int f = 0; f < player.armor.Length; f++)
            {
                if (player.armor[f].type == ModContent.ItemType<Items.Accessories.Odd8Ring>())
                {
                    damage += 8;
                }
                if (player.armor[f].type == ModContent.ItemType<Items.Accessories.WalnutClip>())
                {
                    if (player.statLife < player.statLifeMax2 / 2f)
                    {
                        damage += (player.statLifeMax2 / 2f - player.statLife) * 2 * (player.GetDamage(DamageClass.Generic).Additive + 0);
                    }
                }
            }*/
			if (MythContentPlayer.BlueTorchFlower > 0 && MythContentPlayer.BlueTorchFlowerTime == 0)
			{
				if ((npc.Center - player.Center).Length() < 1800)
				{
					MythContentPlayer.BlueTorchFlowerTime = 12;
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 30, player.width, player.height), Color.Blue, "2");
					player.statMana += 2;
				}
			}
			if (MythContentPlayer.ThreeColorCrown > 0 && MythContentPlayer.ThreeColorCrownCool == 0)
			{
				if ((npc.Center - player.Center).Length() < 1800)
				{
					int X = Main.rand.Next(3);
					if (X == 0)
					{
						MythContentPlayer.ThreeColorCrownBuff1 = 420;
					}
					if (X == 1)
					{
						MythContentPlayer.ThreeColorCrownBuff2 = 420;
					}
					if (X == 2)
					{
						MythContentPlayer.ThreeColorCrownBuff3 = 420;
					}
					MythContentPlayer.ThreeColorCrownCool = 1800;
				}
			}
			if (MythContentPlayer.PurpleBallFlower > 0 && MythContentPlayer.PurpleBallFlowerCool == 0)
			{
				if ((npc.Center - player.Center).Length() < 100)
				{
					int d = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Typeless.PurpleBallEffect>(), (int)damage, 0f, player.whoAmI, Main.rand.NextFloat(0, 6.283f), (float)damage);
					Main.projectile[d].timeLeft = 240;
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MiscItems.Weapons.Slingshots.Projectiles.KSSlingshotHitPink>(), (int)(damage / 4f), 0f, player.whoAmI, 1f, 1f);
					for (int t = 0; t < 8; t++)
					{
						Vector2 v2 = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(Math.PI * 2d);
						int y = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + v2 * 5, v2, ModContent.ProjectileType<MiscProjectiles.Typeless.FlowerPetalPurple>(), (int)damage, 0.5f, player.whoAmI);
						Main.projectile[y].scale = Main.rand.NextFloat(0.9f, 1.1f);
						Main.projectile[y].damage = (int)(damage * Main.projectile[y].scale);
						Main.projectile[y].frame = Main.rand.Next(0, 8);
					}
					MythContentPlayer.PurpleBallFlowerCool = 30;
				}
			}
			return true;
		}
		public override void HitEffect(NPC npc, int hitDirection, double damage)
		{
			if (npc.life < 0)
			{
				LaserMark[npc.whoAmI] = 0;
			}
			if (LaserMark[npc.whoAmI] != 0 && LaserMark[npc.whoAmI] < 175)
			{
				SoundEngine.PlaySound(SoundID.Item33, npc.Center);
				Vector2 v = new Vector2(0, 50).RotatedBy(Math.PI * 2);
				int num2 = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, v, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.LaserWorm>(), (int)damage, 0.2f, Main.LocalPlayer.whoAmI, Main.LocalPlayer.GetCritChance(DamageClass.Summon), (float)damage);
				Main.projectile[num2].timeLeft = 90;
				LaserMark[npc.whoAmI] = 0;
				for (int z = 0; z < 40; z++)
				{
					Vector2 v4 = new Vector2(0, Main.rand.NextFloat(0.15f, 5.05f)).RotatedByRandom(MathHelper.TwoPi);
					int h = Dust.NewDust(npc.Center, 0, 0, 182, v4.X, v4.Y, 0, default(Color), Main.rand.NextFloat(1.5f, 3f));
					Main.dust[h].noGravity = true;
				}
			}
			/*if (!(npc.type == 134 || npc.type == 135 || npc.type == 136) && Des0)
            {
                Des0 = false;
            }*/
		}
		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{//TODO: port freeze buff
			if (npc.HasBuff(ModContent.BuffType<Freeze2>()) && !npc.HasBuff(ModContent.BuffType<Freeze>()))
			{
				if (4f * npc.width * npc.height / 10300f * npc.scale > 1.5f)
				{
					for (int i = 0; i < 20; i++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, 33, 0, 0, 0, default(Color), 3f);
					}
				}
				else
				{
					for (int i = 0; i < 20; i++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, 33, 0, 0, 0, default(Color), 2f);
					}
				}
			}
		}
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			//Mod mod = ModLoader.GetMod("Everglow");
			//if (npc.HasBuff(ModContent.BuffType<Freeze>()))
			//{
			//	npc.color = new Color(50, 50, 50, 0);
			//	npc.position = npc.oldPosition;
			//	//spriteBatch.Draw(base.mod.GetTexture("UIImages/冰封"), npc.Center, null, new Color(1,1,1,1), 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0f);
			//	if (4f * npc.width * npc.height / 10300f * npc.scale > 1.5f)
			//	{
			//		spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/UIImages/冰封").Value, npc.Center - Main.screenPosition, null, new Color(150, 150, 150, 50), 0, new Vector2(103, 100), 1.5f, SpriteEffects.None, 0f);
			//	}
			//	else
			//	{
			//		spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/UIImages/冰封").Value, npc.Center - Main.screenPosition, null, new Color(150, 150, 150, 50), 0, new Vector2(103, 100), 4f * npc.width * npc.height / 10300f * npc.scale, SpriteEffects.None, 0f);
			//	}
			//	if (!npc.noGravity)
			//	{
			//		npc.velocity.Y += 7.5f;
			//	}
			//	float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100f);
			//}
			//if (npc.HasBuff(ModContent.BuffType<TheFirefly.Buffs.OnMoth>()))
			//{
			//	float Stre = 0;
			//	float Stre2 = 0;
			//	for (int t = 0; t < 5; t++)
			//	{
			//		if (npc.buffType[t] == ModContent.BuffType<TheFirefly.Buffs.OnMoth>())
			//		{
			//			Stre = Math.Clamp((npc.buffTime[t] - 280) / 20f, 0, 1);
			//			Stre2 = Math.Clamp((npc.buffTime[t]) / 120f, 0, 0.3f);
			//			break;
			//		}
			//	}
			//	int Index = MothStack[npc.whoAmI];
			//	if (4f * npc.width * npc.height / 10300f * npc.scale > 1.5f)
			//	{
			//		Texture2D Number = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/GlowFanTex/" + Index.ToString()).Value;
			//		spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/GlowFanTex/BlueFly").Value, npc.Center - Main.screenPosition, null, new Color(Stre, Stre, Stre, 0), 0, new Vector2(33, 33), 3f, SpriteEffects.None, 0f);
			//		spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/GlowFanTex/BlueFlyD").Value, npc.Center - Main.screenPosition, null, new Color(Stre2 * 0.5f, Stre2 * 0.5f, Stre2 * 0.5f, 0), 0, new Vector2(33, 33), 3f, SpriteEffects.None, 0f);
			//		spriteBatch.Draw(Number, npc.Center - Main.screenPosition, null, new Color(Stre2 * 2, Stre2 * 2, Stre2 * 2, 0), 0, new Vector2(Number.Width / 2f, Number.Height / 2f), 3f, SpriteEffects.None, 0f);

			//	}
			//	else
			//	{
			//		Texture2D Number = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/GlowFanTex/" + Index.ToString()).Value;
			//		spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/GlowFanTex/").Value, npc.Center - Main.screenPosition, null, new Color(Stre, Stre, Stre, 0), 0, new Vector2(33, 33), 4f * npc.width * npc.height / 10300f * npc.scale * 2, SpriteEffects.None, 0f);
			//		spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/GlowFanTex/").Value, npc.Center - Main.screenPosition, null, new Color(Stre2 * 0.5f, Stre2 * 0.5f, Stre2 * 0.5f, 0), 0, new Vector2(33, 33), 4f * npc.width * npc.height / 10300f * npc.scale * 2, SpriteEffects.None, 0f);
			//		spriteBatch.Draw(Number, npc.Center - Main.screenPosition, null, new Color(Stre2 * 2, Stre2 * 2, Stre2 * 2, 0), 0, new Vector2(Number.Width / 2f, Number.Height / 2f), 4f * npc.width * npc.height / 10300f * npc.scale * 2, SpriteEffects.None, 0f);
			//	}
			//}
			if (LaserMark[npc.whoAmI] > 0)
			{
				float Stre = Math.Clamp(LaserMark[npc.whoAmI] / 30f, 0, 1);
				float Stre2 = Math.Clamp(LaserMark[npc.whoAmI] / 30f, 0, 1);

				if (4f * npc.width * npc.height / 10300f * npc.scale > 1.5f)
				{
					spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/LaserMark").Value, npc.Center - Main.screenPosition, null, new Color(Stre, Stre, Stre, 0), 0, new Vector2(60, 60), 2.4f, SpriteEffects.None, 0f);
					spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/GeerRed").Value, npc.Center - Main.screenPosition, null, new Color(Stre2 * 0.5f, Stre2 * 0.5f, Stre2 * 0.5f, 0), (float)(Main.time / 100d), new Vector2(40, 40), 3f, SpriteEffects.None, 0f);

				}
				else
				{
					spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/LaserMark").Value, npc.Center - Main.screenPosition, null, new Color(Stre, Stre, Stre, 0), 0, new Vector2(60, 60), 4f * npc.width * npc.height / 10300f * npc.scale * 1.6f, SpriteEffects.None, 0f);
					spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/GeerRed").Value, npc.Center - Main.screenPosition, null, new Color(Stre2 * 0.5f, Stre2 * 0.5f, Stre2 * 0.5f, 0), (float)(Main.time / 100d), new Vector2(40, 40), 4f * npc.width * npc.height / 10300f * npc.scale * 2, SpriteEffects.None, 0f);
				}
			}
		}
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			Player player = Main.LocalPlayer;
			bool HasRedDustWeapon = false;
			for (int t = 0; t < 58; t++)
			{
				if (player.inventory[t].type == ModContent.ItemType<MiscItems.Weapons.Legendary.BloodGoldBlade>() && player.inventory[t].stack > 0)
				{
					HasRedDustWeapon = true;
					break;
				}
			}
			//if (type == 227 && HasRedDustWeapon)
			//{
			//	shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Drawings.RedDust>(), false);
			//	shop.item[nextSlot].shopCustomPrice = new int?(Item.buyPrice(5, 0, 0, 0));
			//	nextSlot++;
			//}
			//if (type == 20)
			//{
			//	shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Flowers.FlowerBrochure>(), false);
			//	shop.item[nextSlot].shopCustomPrice = new int?(Item.buyPrice(0, 3, 0, 0));
			//	nextSlot++;
			//}
		}
		public override bool PreAI(NPC npc)
		{
			if (LaserMark[npc.whoAmI] > 0)
			{
				LaserMark[npc.whoAmI]--;
			}
			else
			{
				LaserMark[npc.whoAmI] = 0;
			}
			if (LaserMark2[npc.whoAmI] > 0)
			{
				LaserMark2[npc.whoAmI]--;
			}
			else
			{
				LaserMark2[npc.whoAmI] = 0;
			}
			if (npc.HasBuff(ModContent.BuffType<Freeze>()))
			{
				return false;
			}
			return true;
		}
	}
}
