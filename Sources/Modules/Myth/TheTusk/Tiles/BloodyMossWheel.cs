using Everglow.Myth.MiscItems.FixCoins;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Myth.TheTusk.Tiles
{
	public class BloodyMossWheel : ModTile
	{
		public override void PostSetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.addTile(Type);
			DustType = 4;
			ModTranslation modTranslation = base.CreateMapEntryName(null);
			modTranslation.SetDefault("Dusty RockyBar");
			modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "破旧的石盘");
			base.AddMapEntry(new Color(96, 93, 91), modTranslation);
		}
		public override bool CanExplode(int i, int j)
		{
			return CanK;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return CanK;
		}

		private float[] Rot = new float[24];
		private float[] AimRot = new float[24];
		private float Rot2 = 0;
		private int[] Cooling = new int[24];
		private int[] Dir = new int[24];
		public static int Symbol = 1;
		private int Step = 0;
		private int StepChange = 0;
		public static int Killing = 0;
		public static float TileI = 0;
		public static float TileJ = 0;
		public static bool CanK = false;
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;

			Texture2D BaseCo1 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Tusk/ForgeWave").Value;
			Texture2D Sp1a = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/TileEffects/StonePanBottomLine").Value;
			Texture2D Sp1b = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/TileEffects/StonePanBottomRedLight").Value;
			Texture2D Sp1c = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/TileEffects/StonePanBottomFace").Value;
			Texture2D Sp2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/TileEffects/StonePanPin").Value;
			Texture2D SpL1 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/TileEffects/StonePanL1").Value;
			Texture2D SpL2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/TileEffects/StonePanL2").Value;
			Texture2D SpL3 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/TileEffects/StonePanL3").Value;
			Texture2D SpL4 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/TileEffects/StonePanL4").Value;
			Texture2D SpIC = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/TileEffects/StonePanStrick").Value;
			var origin = new Vector2(56);

			Color color = Lighting.GetColor(i, j);
			Vector2 v = new Vector2(0, 100).RotatedBy(Main.time / 60f);
			Main.spriteBatch.Draw(BaseCo1, new Vector2(i * 16 - 16 + 6, j * 16 - 68 - 16) + origin - Main.screenPosition + zero, new Rectangle(256 + (int)v.X, 256 + (int)v.Y, 32, 32), new Color(255, 0, 0, 255), 0f, origin, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(BaseCo1, new Vector2(i * 16 - 16 + 6, j * 16 - 68 - 16) + origin - Main.screenPosition + zero, new Rectangle(256 - (int)v.X, 256 - (int)v.Y, 32, 32), new Color(255, 125, 0, 0), 0f, origin, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Sp1a, new Vector2(i * 16 + 6, j * 16 - 68) - Main.screenPosition + zero, null, color, 0f, origin, 1f, SpriteEffects.None, 0f);
			if (StepChange > 0)
			{
				if (StepChange > 10)
					Main.spriteBatch.Draw(Sp1b, new Vector2(i * 16 + 6, j * 16 - 68) - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), 0f, origin, (30 - StepChange) / 20f, SpriteEffects.None, 0f);
				else
				{
					Main.spriteBatch.Draw(Sp1b, new Vector2(i * 16 + 6, j * 16 - 68) - Main.screenPosition + zero, null, new Color(StepChange * 25, StepChange * 25, StepChange * 25, 0), 0f, origin, 0.95f, SpriteEffects.None, 0f);
				}
				StepChange--;
			}
			else
			{
				StepChange = 0;
			}
			if (Step == 0)
				Rot2 = 0;
			if (Step == 1)
			{
				if (StepChange > 0)
					Rot2 = (float)(Math.PI / 2d) * (30 - StepChange) / 30f;
				else
				{
					Rot2 = (float)(Math.PI / 2d);
				}
			}
			if (Step == 2)
			{
				if (StepChange > 0)
					Rot2 = (float)(Math.PI / 2d) * (30 - StepChange) / 30f + (float)(Math.PI / 2d);
				else
				{
					Rot2 = (float)Math.PI;
				}
			}
			Main.spriteBatch.Draw(Sp1c, new Vector2(i * 16 + 6, j * 16 - 68) - Main.screenPosition + zero, null, color, 0f, origin, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Sp2, new Vector2(i * 16 + 6, j * 16 - 68) - Main.screenPosition + zero, null, color, Rot2, origin, 1f, SpriteEffects.None, 0f);
			Symbol = Step + 1;
			//0↑
			//1→
			//2↓
			//3←
			if (Dir[0] == 6 && Dir[1] == 1 && Dir[2] == 2 && Dir[3] == 0)
			{
				if (Step == 0)
				{
					Step += 1;
					StepChange = 30;
				}
			}
			if (Dir[0] == 7 && Dir[1] == 0 && Dir[2] == 4 && Dir[3] == 0)
			{
				if (Step == 1)
				{
					Step += 1;
					StepChange = 30;
				}
			}
			if (Dir[0] == 3 && Dir[1] == 2 && Dir[2] == 3 && Dir[3] == 5)
			{
				if (Step == 2)
				{
					SoundEngine.PlaySound(SoundID.Chat, new Vector2(i * 16 + 6, j * 16 - 64));
					Step += 1;
					CanK = true;
					Killing = 60;
				}
			}
			Vector2 vZoom = Main.GameViewMatrix.Zoom;

			Vector2 ScreenCenter = Main.screenTarget.Size() / 2f + Main.screenPosition;//世界位置屏幕中心
			Vector2 CorrectedMouseScreenCenter = (Main.MouseScreen - Main.screenTarget.Size() / 2f) / vZoom.X;//鼠标的中心指向位
			Vector2 CorrectedMouseWorld = CorrectedMouseScreenCenter + ScreenCenter;//鼠标世界坐标校正

			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/TileEffects/Symbol" + Symbol.ToString()).Value, new Vector2(i * 16 + 6, j * 16 - 68) + zero - Main.screenPosition, null, new Color(150 + (int)v.X, 150 + (int)v.X, 150 + (int)v.X, 0), 0f, origin, 1f, SpriteEffects.None, 0f);
			if ((CorrectedMouseWorld - (new Vector2(i * 16 + 6, j * 16 - 64) + new Vector2(0, -50))).Length() < 12 * vZoom.X)
			{
				Main.spriteBatch.Draw(SpL1, new Vector2(i * 16 + 6, j * 16 - 68) - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), 0, origin, 1f, SpriteEffects.None, 0f);
				if (Main.mouseRight && !Main.gamePaused)
				{
					if (Cooling[0] <= 0)
					{
						Dir[0]++;
						if (Dir[0] >= 8)
							Dir[0] = 0;
						AimRot[0] = (float)Math.PI / 4f * Dir[0];
						Cooling[0] = 4;
					}
				}
			}
			if ((CorrectedMouseWorld - (new Vector2(i * 16 + 6, j * 16 - 68) + new Vector2(50, 0))).Length() < 12 * vZoom.X)
			{
				Main.spriteBatch.Draw(SpL2, new Vector2(i * 16 + 6, j * 16 - 68) - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), 0, origin, 1f, SpriteEffects.None, 0f);
				if (Main.mouseRight && !Main.gamePaused)
				{
					if (Cooling[1] <= 0)
					{
						Dir[1]++;
						if (Dir[1] >= 8)
							Dir[1] = 0;
						AimRot[1] = (float)Math.PI / 4f * Dir[1];
						Cooling[1] = 4;
					}
				}
			}
			if ((CorrectedMouseWorld - (new Vector2(i * 16 + 6, j * 16 - 68) + new Vector2(0, 50))).Length() < 12 * vZoom.X)
			{
				Main.spriteBatch.Draw(SpL3, new Vector2(i * 16 + 6, j * 16 - 68) - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), 0, origin, 1f, SpriteEffects.None, 0f);
				if (Main.mouseRight && !Main.gamePaused)
				{
					if (Cooling[2] <= 0)
					{
						Dir[2]++;
						if (Dir[2] >= 8)
							Dir[2] = 0;
						AimRot[2] = (float)Math.PI / 4f * Dir[2];
						Cooling[2] = 4;
					}
				}
			}
			if ((CorrectedMouseWorld - (new Vector2(i * 16 + 6, j * 16 - 68) + new Vector2(-50, 0))).Length() < 12 * vZoom.X)
			{
				Main.spriteBatch.Draw(SpL4, new Vector2(i * 16 + 6, j * 16 - 68) - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), 0, origin, 1f, SpriteEffects.None, 0f);
				if (Main.mouseRight && !Main.gamePaused)
				{
					if (Cooling[3] <= 0)
					{
						Dir[3]++;
						if (Dir[3] >= 8)
							Dir[3] = 0;
						AimRot[3] = (float)Math.PI / 4f * Dir[3];
						Cooling[3] = 4;
					}
				}
			}
			Main.spriteBatch.Draw(SpIC, new Vector2(i * 16 + 6, j * 16 - 68) + zero + new Vector2(0, -46) - Main.screenPosition, null, color, Rot[0], new Vector2(12), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(SpIC, new Vector2(i * 16 + 6, j * 16 - 68) + zero + new Vector2(46, 0) - Main.screenPosition, null, color, Rot[1], new Vector2(12), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(SpIC, new Vector2(i * 16 + 6, j * 16 - 68 - 4) + zero + new Vector2(0, 50) - Main.screenPosition, null, color, Rot[2], new Vector2(12), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(SpIC, new Vector2(i * 16 + 6, j * 16 - 68) + zero + new Vector2(-46, 0) - Main.screenPosition, null, color, Rot[3], new Vector2(12), 1f, SpriteEffects.None, 0f);
			for (int l = 0; l < 4; l++)
			{
				if (Cooling[l] > 0)
				{
					Rot[l] += (float)Math.PI / 20f;
					Cooling[l]--;
				}
				else
				{
					Rot[l] = AimRot[l];
				}
			}

			if (Killing < -5)
			{
				CanK = false;
				TuskModPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<TuskModPlayer>();
				mplayer.Shake = 6;
				SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, new Vector2(i * 16 + 0, j * 16 - 72));
				Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
				Vector2 vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
				Gore.NewGore(null, new Vector2(i * 16 + 0, j * 16 - 72) + vF2, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak1").Type, 1f);
				vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
				vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
				Gore.NewGore(null, new Vector2(i * 16 + 0, j * 16 - 72) + vF2, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak1").Type, 1f);
				vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
				vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
				Gore.NewGore(null, new Vector2(i * 16 + 0, j * 16 - 72) + vF2, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak2").Type, 1f);
				vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
				vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
				Gore.NewGore(null, new Vector2(i * 16 + 0, j * 16 - 72) + vF2, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak3").Type, 1f);
				vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
				vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
				Gore.NewGore(null, new Vector2(i * 16 + 0, j * 16 - 72) + vF2, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak4").Type, 1f);
				vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
				vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
				Gore.NewGore(null, new Vector2(i * 16 + 0, j * 16 - 72) + vF2, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak5").Type, 1f);
				vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
				vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
				Gore.NewGore(null, new Vector2(i * 16 + 0, j * 16 - 72) + vF2, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak6").Type, 1f);
				for (int h = 0; h < 7; h++)
				{
					vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
					vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
					Gore.NewGore(null, new Vector2(i * 16 + 0, j * 16 - 72) + vF2, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak7").Type, 1f);
				}
				for (int h = 0; h < 10; h++)
				{
					vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
					vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
					Gore.NewGore(null, new Vector2(i * 16 + 0, j * 16 - 72) + vF2, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak8").Type, 1f);
				}
				for (int h = 0; h < 10; h++)
				{
					vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
					vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
					Gore.NewGore(null, new Vector2(i * 16 + 0, j * 16 - 72) + vF2, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak9").Type, 1f);
				}
				for (int z = 0; z < 120; z++)
				{
					vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
					vF2 = new Vector2(0, Main.rand.NextFloat(0, 45f)).RotatedByRandom(6.28);
					Dust.NewDust(new Vector2(i * 16 + 0, j * 16 - 72) + vF2, 0, 0, DustID.TintableDust, vF.X, vF.Y, 0, default, Main.rand.NextFloat(0.8f, 2.1f));
				}
				int[] Ty = { ModContent.ItemType<FixCoinDamage3>(), ModContent.ItemType<FixCoinCrit3>(), ModContent.ItemType<FixCoinDefense3>(), ModContent.ItemType<FixCoinSpeed3>(), ModContent.ItemType<FixCoinMelee3>() };
				Item.NewItem(null, new Vector2(i * 16 + 0, j * 16 - 72), Ty[Main.rand.Next(Ty.Length)]);
				Item.NewItem(null, new Vector2(i * 16 + 0, j * 16 - 72), ItemID.GoldCoin, 5);
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<BloodyMossWheelFinished>();
				((Tile)Main.tile[i, j]).HasTile = true;
			}
			TileI = i;
			TileJ = j;
		}
		public static void DrawAll(SpriteBatch sb)
		{
			var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;

			Texture2D Tdoor = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Tusk/CosmicFlame").Value;
			Texture2D Tdoor2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Tusk/CosmicVort").Value;
			Texture2D Tdoor3 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Tusk/CosmicPerlin").Value;
			if (CanK)
			{
				Killing--;
				var Correction = new Vector2(-186f, -260f);
				Main.spriteBatch.Draw(Tdoor, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), (float)Main.time / 30f, new Vector2(56), (60 - Killing) / 45f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(Tdoor, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(100, 100, 100, 0), -(float)Main.time / 20f, new Vector2(56), (60 - Killing) / 45f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(Tdoor, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), (float)Main.time / 15f, new Vector2(56), (60 - Killing) / 50f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(Tdoor2, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), (float)Main.time / 30f, new Vector2(56), (60 - Killing) / 45f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(Tdoor3, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), -(float)Main.time / 20f, new Vector2(56), (60 - Killing) / 45f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(Tdoor3, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), (float)Main.time / 15f, new Vector2(56), (60 - Killing) / 45f, SpriteEffects.None, 0f);
			}
		}
	}
}
