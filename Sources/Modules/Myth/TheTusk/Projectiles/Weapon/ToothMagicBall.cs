using Everglow.Myth.Common;
using Terraria.Localization;
namespace Everglow.Myth.TheTusk.Projectiles.Weapon
{
	public class ToothMagicBall : ModProjectile
	{
		//TODO
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tooth Magic Ball");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "血色法球");
		}
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 1080;
			Projectile.alpha = 0;
			Projectile.penetrate = -1;
			Projectile.scale = 1;
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.PI / 2d));
			if (Projectile.timeLeft >= 1000)
				Projectile.timeLeft = 10;
			if (Main.mouseLeft && player.statMana > player.HeldItem.mana)
				Projectile.timeLeft = 10;
			if (addi % player.HeldItem.useTime == 0)
			{
				Vector2 velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * player.HeldItem.shootSpeed;
				Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), player.Center + new Vector2(28 * player.direction, -5), velocity, ModContent.ProjectileType<ToothMagic>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
			}
			Projectile.Center = player.Center;
			player.heldProj = Projectile.whoAmI;
			addi++;
		}
		Vector2 VC = Vector2.Zero;
		Vector2[] VB = new Vector2[4];
		Vector2[] VT = new Vector2[10];
		Vector2[] VTMax = new Vector2[10];
		int addi = 0;
		int[] DustBlood = new int[30];
		public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D TC = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodCore");
			Texture2D TB0 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDrop0");
			Texture2D TB1 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDrop1");
			Texture2D TB2 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDrop2");
			Texture2D TB3 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDrop3");
			Texture2D TT0 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk0");
			Texture2D TT1 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk1");
			Texture2D TT2 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk2");
			Texture2D TT3 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk3");
			Texture2D TT4 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk4");
			Texture2D TT5 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk5");
			Texture2D TT6 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk6");
			Texture2D TT7 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk7");
			Texture2D TT8 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk8");
			Texture2D TT9 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk9");
			var drawOrigin = new Vector2(TC.Width * 0.5f, TC.Height * 0.5f);
			Color c0 = Lighting.GetColor((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f));
			SpriteEffects sp = SpriteEffects.None;
			if (DustBlood[0] == 0)
			{
				for (int f = 0; f < 30; f++)
				{
					DustBlood[f] = f + 5;
				}
			}
			if (!Main.gamePaused)
			{
				Vector2 Vcen = player.Center + VC + new Vector2(20 * player.direction, -7);
				for (int f = 0; f < 30; f++)
				{
					if (!Main.dust[DustBlood[f]].active || Main.dust[DustBlood[f]].type != ModContent.DustType<Dusts.BloodBall>())
					{
						if (Main.rand.NextBool(8))
						{
							DustBlood[f] = Dust.NewDust(Vcen - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(4f, 12f)).RotatedByRandom(6.283), 0, 0, ModContent.DustType<Dusts.BloodBall>(), 0, 0, 0, default, 0.1f);
							Main.dust[DustBlood[f]].velocity = Vector2.Zero;
							Main.dust[DustBlood[f]].noGravity = true;
						}
					}
					else
					{
						Vector2 vadd = Vcen - (Main.dust[DustBlood[f]].position + new Vector2(4));
						if (vadd.Length() < 1.2f)
							Main.dust[DustBlood[f]].active = false;
						else
						{
							if (vadd.Length() < 30f)
							{
								if (Main.mouseLeft)
								{
									Main.dust[DustBlood[f]].velocity += Vector2.Normalize(vadd) / 30f;
									if (Main.dust[DustBlood[f]].scale < 1)
										Main.dust[DustBlood[f]].scale += 0.01f;
								}
							}
							else
							{
								Main.dust[DustBlood[f]].scale -= 0.02f;
							}
						}
					}
				}
			}
			if (player.direction == -1)
				sp = SpriteEffects.FlipHorizontally;
			if (VTMax[0] == Vector2.Zero)
			{
				for (int s = 0; s < 10; s++)
				{
					VTMax[s] = new Vector2(0, Main.rand.NextFloat(2.5f, 4f)).RotatedBy(s / 7.5 * Math.PI);
				}
			}
			for (int s = 0; s < 10; s++)
			{
				VT[s] = VTMax[s] * (float)(Math.Sin(s + addi / 30f) + 0.4);
			}
			Main.spriteBatch.Draw(TC, player.Center + VC + new Vector2(20 * player.direction, -7) - Main.screenPosition, null, c0, 0, drawOrigin, addi % player.HeldItem.useTime / 26f, sp, 0);
			Main.spriteBatch.Draw(TB0, player.Center + VB[0] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TB1, player.Center + VB[1] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TB2, player.Center + VB[2] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TB3, player.Center + VB[3] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TT0, player.Center + VT[0] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TT1, player.Center + VT[1] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TT2, player.Center + VT[2] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TT3, player.Center + VT[3] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TT4, player.Center + VT[4] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TT5, player.Center + VT[5] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TT6, player.Center + VT[6] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TT7, player.Center + VT[7] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TT8, player.Center + VT[8] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
			Main.spriteBatch.Draw(TT9, player.Center + VT[9] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		}
	}
}