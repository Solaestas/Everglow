using Everglow.Sources.Modules.MythModule.MiscItems.Buffs;
using Terraria.Audio;
namespace Everglow.Sources.Modules.MythModule.Common
{
	public class MythContentGlobalNPC : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
		{
			return true;
		}

		public static int[] LaserMark = new int[200];
		public static int[] LaserMark2 = new int[200];
		public static bool Des0 = false;

		public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			Player player = Main.LocalPlayer;
			if(Main.netMode == NetmodeID.Server)
			{
				return true;
			}
            MythContentPlayer myplayer = player.GetModPlayer<MythContentPlayer>();
			if (crit)
			{
				damage *= (myplayer.CritDamage + 1) / 2f;
			}
			if (player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.SilveralGun>() || player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.SilveralRifle>())
			{
				myplayer.SilverBuff = 300;
			}
			if (myplayer.GoldLiquidPupil > 0)
			{
				damage += npc.defense * 0.35;
			}

			if (LaserMark2[npc.whoAmI] > 0)
			{
				double OldDam = damage;
				damage *= 1.5;
				if (crit)
				{
					damage *= 1.25;
				}
			}

			if (myplayer.BlueTorchFlower > 0 && myplayer.BlueTorchFlowerTime == 0)
			{
				if ((npc.Center - player.Center).Length() < 1800)
				{
					myplayer.BlueTorchFlowerTime = 12;
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 30, player.width, player.height), Color.Blue, "2");
					player.statMana += 2;
				}
			}
			if (myplayer.ThreeColorCrown > 0 && myplayer.ThreeColorCrownCool == 0)
			{
				if ((npc.Center - player.Center).Length() < 1800)
				{
					int X = Main.rand.Next(3);
					if (X == 0)
					{
						myplayer.ThreeColorCrownBuff1 = 420;
					}
					if (X == 1)
					{
						myplayer.ThreeColorCrownBuff2 = 420;
					}
					if (X == 2)
					{
						myplayer.ThreeColorCrownBuff3 = 420;
					}
					myplayer.ThreeColorCrownCool = 1800;
				}
			}
			if (myplayer.PurpleBallFlower > 0 && myplayer.PurpleBallFlowerCool == 0)
			{
				if ((npc.Center - player.Center).Length() < 100)
				{
					int d = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MiscItems.Projectiles.Typeless.PurpleBallEffect>(), (int)damage, 0f, player.whoAmI, Main.rand.NextFloat(0, 6.283f), (float)damage);
					Main.projectile[d].timeLeft = 240;
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MiscItems.Weapons.Slingshots.Projectiles.KSSlingshotHitPink>(), (int)(damage / 4f), 0f, player.whoAmI, 1f, 1f);
					for (int t = 0; t < 8; t++)
					{
						Vector2 v2 = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(Math.PI * 2d);
						int y = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + v2 * 5, v2, ModContent.ProjectileType<MiscItems.Projectiles.Typeless.FlowerPetalPurple>(), (int)damage, 0.5f, player.whoAmI);
						Main.projectile[y].scale = Main.rand.NextFloat(0.9f, 1.1f);
						Main.projectile[y].damage = (int)(damage * Main.projectile[y].scale);
						Main.projectile[y].frame = Main.rand.Next(0, 8);
					}
					myplayer.PurpleBallFlowerCool = 30;
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
				LaserMark[npc.whoAmI] = 0;
				for (int z = 0; z < 40; z++)
				{
					Vector2 v4 = new Vector2(0, Main.rand.NextFloat(0.15f, 5.05f)).RotatedByRandom(MathHelper.TwoPi);
					int h = Dust.NewDust(npc.Center, 0, 0, 182, v4.X, v4.Y, 0, default(Color), Main.rand.NextFloat(1.5f, 3f));
					Main.dust[h].noGravity = true;
				}
			}
		}
		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
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
			if (LaserMark[npc.whoAmI] > 0)
			{
				float Stre = Math.Clamp(LaserMark[npc.whoAmI] / 30f, 0, 1);
				float Stre2 = Math.Clamp(LaserMark[npc.whoAmI] / 30f, 0, 1);

				if (4f * npc.width * npc.height / 10300f * npc.scale > 1.5f)
				{
					spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/LaserMark").Value, npc.Center - Main.screenPosition, null, new Color(Stre, Stre, Stre, 0), 0, new Vector2(60, 60), 2.4f, SpriteEffects.None, 0f);
					spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/GearRed").Value, npc.Center - Main.screenPosition, null, new Color(Stre2 * 0.5f, Stre2 * 0.5f, Stre2 * 0.5f, 0), (float)(Main.time / 100d), new Vector2(40, 40), 3f, SpriteEffects.None, 0f);

				}
				else
				{
					spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/LaserMark").Value, npc.Center - Main.screenPosition, null, new Color(Stre, Stre, Stre, 0), 0, new Vector2(60, 60), 4f * npc.width * npc.height / 10300f * npc.scale * 1.6f, SpriteEffects.None, 0f);
					spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/GearRed").Value, npc.Center - Main.screenPosition, null, new Color(Stre2 * 0.5f, Stre2 * 0.5f, Stre2 * 0.5f, 0), (float)(Main.time / 100d), new Vector2(40, 40), 4f * npc.width * npc.height / 10300f * npc.scale * 2, SpriteEffects.None, 0f);
				}
			}
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
