using Everglow.Myth.Common;
using Everglow.Myth.TheMarbleRemains.Tiles;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Everglow.Myth.TheMarbleRemains.WorldGeneration
{
	public class MarbleTempleGen : ModSystem
	{
		public static void QuickBuild(int x, int y, string Path)
		{
			var mapIO = new Commons.TileHelper.MapIO(x, y);

			mapIO.Read(ModIns.Mod.GetFileStream("Myth/" + Path));

			var it = mapIO.GetEnumerator();
			while (it.MoveNext())
			{
				WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
				WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
			}
		}
		internal class WorldMarbleTempleGenPass : GenPass
		{
			public WorldMarbleTempleGenPass() : base("MarbleTempleLand", 500)//TODO:给大地安装血肉之颌
			{
			}

			public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everglow.Common.WorldSystem.BuildWorldMarbleTempleTable");
				BuildTempleLand();
			}
		}
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			tasks.Add(new WorldMarbleTempleGenPass());
		}
		public static void ShapeTile(string Shapepath, int a, int b, int type)
		{
			var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Myth/TheMarbleRemains/WorldGeneration/" + Shapepath);
			imageData.ProcessPixelRows(accessor =>
			{
				for (int y = 0; y < accessor.Height; y++)
				{
					var pixelRow = accessor.GetRowSpan(y);
					if (y + b < 20)
					{
						continue;
					}

					if (y + b > Main.maxTilesY - 20)
					{
						break;
					}

					for (int x = 0; x < pixelRow.Length; x++)
					{
						if (x + a < 20)
						{
							continue;
						}

						if (x + a > Main.maxTilesX - 20)
						{
							break;
						}

						ref var pixel = ref pixelRow[x];
						Tile tile = Main.tile[x + a, y + b];
						switch (type)//21是箱子
						{
							case 0: //Kill
								if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)
								{
									if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
									{
										tile.ClearEverything();
									}
								}
								break;

							case 1: //Tiles
								var Plc = new Vector2[30];
								var Plc2 = new Vector2[60];
								if (pixel.R == 168 && pixel.G == 178 && pixel.B == 204)
								{
									if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
									{
										tile.TileType = 357;
										tile.HasTile = true;
									}
								}
								if (pixel.R == 63 && pixel.G == 89 && pixel.B == 255)
								{
									if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
									{
										tile.TileType = (ushort)ModContent.TileType<GiantMarbalClock>();
										tile.HasTile = true;
									}
								}
								if (pixel.R == 226 && pixel.G == 109 && pixel.B == 140)
								{
									if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
									{
										tile.TileType = 19;
										tile.HasTile = true;
									}
								}
								break;

							case 2: //Walls
								if (pixel.R == 111 && pixel.G == 117 && pixel.B == 135)
								{
									if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
									{
										tile.WallType = 179;
										tile.HasTile = false;
									}
								}
								break;
							case 3: //Liquid
								if (pixel.R == 0 && pixel.G == 0 && pixel.B == 255)
								{
									if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
									{
										tile.LiquidType = LiquidID.Water;
										tile.LiquidAmount = 50;
										tile.HasTile = false;
										//WorldGen.PlaceLiquid(x, y, byte.MaxValue, 255);
									}
								}
								break;
						}
					}
				}
			});
		}
		public int templeCenterX = 400;
		public int templeCenterY = 300;
		public static void BuildTempleLand()
		{
			Point16 abPos = GetTempleLandPosition();
			int a = abPos.X;
			int b = abPos.Y;
			Main.statusText = "TempleStart";
			ShapeTile("MarbleTempleTile.bmp", a, b, 1);
			Main.statusText = "TempleWaterStart";
			ShapeTile("MarbleTempleLiquid.bmp", a, b, 3);
			Main.statusText = "TempleWallStart";
			ShapeTile("MarbleTempleWall.bmp", a, b, 2);
			SmoothTempleTile(a, b, 160, 80);
			//MarbleTempleGen marbleTempleGen = ModContent.GetInstance<MarbleTempleGen>();
			//marbleTempleGen.templeCenterX = a + 80;
			//marbleTempleGen.templeCenterY = b + 10;
			//BuildTempleArray(a, b);
		}
		//public static void BuildTempleArray(int x, int y)
		//{
		//	x += 80;
		//	y += 120;
		//	for (int Dy = 0; Dy < 300; Dy++)
		//	{
		//		if (Main.tile[x, y + Dy].HasTile)
		//		{
		//			y += Dy + 12;
		//			break;
		//		}
		//	}
		//	for (int i = -30; i < 31; i++)
		//	{
		//		for (int j = -30; j < 31; j++)
		//		{
		//			float Length = new Vector2(i, j).Length();
		//			var tile = Main.tile[x + i, y + j];
		//			if (Length is < 30f and > 18f)
		//			{
		//				if (tile.HasTile)
		//				{
		//					if (Main.tileSolid[tile.TileType])
		//					{
		//						tile.TileType = 357;
		//					}
		//					else
		//					{
		//						tile.ClearEverything();
		//					}
		//				}
		//				if (Length is < 24f and > 20f && j > -7)
		//				{
		//					tile.TileType = 357;
		//					tile.HasTile = true;
		//				}
		//			}
		//			if (Length < 19f)
		//			{
		//				if (tile.HasTile)
		//				{
		//					tile.HasTile = false;
		//				}
		//			}
		//			if (Length < 28f)
		//			{
		//				if (tile.WallType != 0 || (Length < 22f && j > -5))
		//				{
		//					tile.WallType = 179;
		//				}
		//			}
		//		}
		//	}

		//	//var tileWheel = Main.tile[x, y + 14];
		//	//tileWheel.TileType = (ushort)ModContent.TileType<BloodyMossWheel>();
		//	//tileWheel.HasTile = true;
		//}
		public static Point16 GetTempleLandPosition()
		{
			int a = (int)(Main.maxTilesX * 0.3);
			int b = (int)(Main.maxTilesY * 0.3);
			while (!CanPlaceTemple(new Point(a, b)))
			{
				a = (int)(Main.maxTilesX * Main.rand.NextFloat(0.1f, 0.88f));
				while (Math.Abs(a - Main.maxTilesX * 0.5f) < Main.maxTilesX * 0.1f)
				{
					a = (int)(Main.maxTilesX * Main.rand.NextFloat(0.1f, 0.88f));
				}

				b = (int)(Main.maxTilesY * Main.rand.NextFloat(0.11f, 0.31f));
			}
			return new Point16(a, b);
		}
		public static bool CanPlaceTemple(Point position)
		{
			if (position.X < 20 || position.Y - 60 < 20)
			{
				return false;
			}

			if (position.X + 160 > Main.maxTilesX - 20 || position.Y + 80 > Main.maxTilesY - 20)
			{
				return false;
			}

			for (int x = 0; x < 161; x++)
			{
				for (int y = -60; y < 81; y++)
				{
					if (Main.tile[x + position.X, y + position.Y].HasTile)
					{
						return false;
					}
				}
			}
			return true;
		}
		private static void SmoothTempleTile(int a, int b, int width = 256, int height = 512)
		{
			for (int y = 0; y < width; y += 1)
			{
				for (int x = 0; x < height; x += 1)
				{
					if (Main.tile[x + a, y + b].TileType == 357)
					{
						Tile.SmoothSlope(x + a, y + b, false);
						WorldGen.TileFrame(x + a, y + b, true, false);
					}
					else
					{
						WorldGen.TileFrame(x + a, y + b, true, false);
					}
					WorldGen.SquareWallFrame(x + a, y + b, true);
				}
			}
		}
	}
}