using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.CyanVine;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Everglow.Yggdrasil.YggdrasilTown.Walls;
using Terraria.Utilities;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;
namespace Everglow.Yggdrasil.WorldGeneration;
public class YggdrasilTownGeneration
{
	public static void BuildYggdrasilTown()
	{
		Initialize();
		Main.statusText = "Yggdrasil Town Bark Cliff...";
		PlaceRectangleAreaOfBlock(20, 10650, 155, 12000, ModContent.TileType<StoneScaleWood>());
		PlaceRectangleAreaOfBlock(1045, 10650, 1180, 12000, ModContent.TileType<StoneScaleWood>());
		PlaceRectangleAreaOfBlock(0, 11700, 1200, 12000, ModContent.TileType<StoneScaleWood>());

		PlaceRectangleAreaOfWall(20, 10650, 155, 12000, ModContent.WallType<StoneDragonScaleWoodWall>());
		PlaceRectangleAreaOfWall(1045, 10650, 1180, 12000, ModContent.WallType<StoneDragonScaleWoodWall>());
		PlaceRectangleAreaOfWall(0, 11700, 1200, 12000, ModContent.WallType<StoneDragonScaleWoodWall>());
		Main.statusText = "Filling Midnight Bayou With Mud...";
		BuildMidnightBayou();

		Main.statusText = "Carving The Heavenly Portal...";
		BuildHeavenlyPortal();
		Main.statusText = "Flooding The Azure Grotto...";
		BuildAzureGrotto();
		Main.statusText = "Digging The Tangled Submine...";
		BuildTangledSubmine();
		Main.statusText = "Another Side, The Fossilized Mine Road...";
		BuildFossilizedMineRoad();
		Main.statusText = "Constructing The Yggdrasil Town Below...";
		BuildTownBelow();
		Main.statusText = "Constructing The Yggdrasil Town Upper...";
		BuildTownUpper();
		Main.statusText = "The Barrier To 2rd Floor Of Yggdrasil...";
		BuildDuskfallBarrier();
		Main.statusText = "The Stone Cage Of Challenges...";
		BuildStoneCageOfChallenges();
	}
	public static int[,] PerlinPixelR = new int[512, 512];
	public static int[,] PerlinPixelG = new int[512, 512];
	public static int[,] PerlinPixelB = new int[512, 512];
	public static int[,] PerlinPixel2 = new int[512, 512];
	public static int[,] CellPixel = new int[512, 512];
	public static int AzureGrottoCenterX;
	public static UnifiedRandom GenRand = new UnifiedRandom();
	public static List<YggdrasilTownStreetElement> StreetConstructorsSheet;
	public static List<YggdrasilTownStreetElement> InDoorChineseStyleHangingSheet;
	/// <summary>
	/// 初始化
	/// </summary>
	public static void Initialize()
	{
		GenRand = WorldGen.genRand;
		AzureGrottoCenterX = GenRand.Next(-100, 100) + 600;
		FillPerlinPixel();
		StreetConstructorsSheet = new List<YggdrasilTownStreetElement>()
		{
			new Lamppost(),
			new Bench(),
			new Crate(),
			new ThreeCrate(),
			new FolkHouseofChineseStyle(),
			new FolkHouseofWoodStoneStruture(),
			new FolkHouseofWoodStruture(),
			new TwoStoriedFolkHouse(),
			new SmithyType()
		};
		InDoorChineseStyleHangingSheet = new List<YggdrasilTownStreetElement>()
		{
			new BambooChandelier(),
			new CrystalChandelier(),
			new CylinderChandelierGroup(),
			new DynasticChandelier(),
			new EvilChandelier(),
			new GoldenChandelier(),
			new GraniteChandelier(),
			new GreenDungeonChandelier(),
			new HexagonalCeilingChandelier(),
			new MetalChandelier(),
			new PalmChandelier(),
			new RichMahoganyChandelier(),

			new DiscoBall(),

			new FireflyBottle(),
			new LavaFlyBottle(),
			new LightningBugBottle(),
			new SoulBottle(),

			new BambooLantern(),
			new BowlLantern(),
			new ChineseLantern(),
			new DynasticLantern(),
			new FleshLantern(),
			new GlassLantern(),
			new LivingWoodLantern(),
			new MetalLantern(),
			new SpellLantern(),
			new BurningBowl(),
			new PlantBowl(),
			new EmptyAnchoredTop()
		};
	}
	/// <summary>
	/// 噪声信息获取
	/// </summary>
	public static void FillPerlinPixel()
	{
		var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Yggdrasil/WorldGeneration/Noise_rgb.bmp");
		Vector2 perlinCoordCenter = new Vector2(GenRand.NextFloat(0f, 1f), GenRand.NextFloat(0f, 1f));
		imageData.ProcessPixelRows(accessor =>
		{
			for (int y = 0; y < accessor.Height; y++)
			{
				int newY = (int)(accessor.Height * perlinCoordCenter.Y + y) % accessor.Height;
				var pixelRow = accessor.GetRowSpan(newY);
				for (int x = 0; x < pixelRow.Length; x++)
				{
					int newX = (int)(accessor.Width * perlinCoordCenter.X + x) % accessor.Width;
					ref var pixel = ref pixelRow[newX];
					PerlinPixelR[x, y] = pixel.R;
					PerlinPixelG[x, y] = pixel.G;
					PerlinPixelB[x, y] = pixel.B;
				}
			}
		});

		imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Yggdrasil/WorldGeneration/Noise_perlin.bmp");
		perlinCoordCenter = new Vector2(GenRand.NextFloat(0f, 1f), GenRand.NextFloat(0f, 1f));
		imageData.ProcessPixelRows(accessor =>
		{
			for (int y = 0; y < accessor.Height; y++)
			{
				int newY = (int)(accessor.Height * perlinCoordCenter.Y + y) % accessor.Height;
				var pixelRow = accessor.GetRowSpan(newY);
				for (int x = 0; x < pixelRow.Length; x++)
				{
					int newX = (int)(accessor.Width * perlinCoordCenter.X + x) % accessor.Width;
					ref var pixel = ref pixelRow[newX];
					PerlinPixel2[x, y] = pixel.R;
				}
			}
		});

		imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Yggdrasil/WorldGeneration/Noise_cell.bmp");
		perlinCoordCenter = new Vector2(GenRand.NextFloat(0f, 1f), GenRand.NextFloat(0f, 1f));
		imageData.ProcessPixelRows(accessor =>
		{
			for (int y = 0; y < accessor.Height; y++)
			{
				int newY = (int)(accessor.Height * perlinCoordCenter.Y + y) % accessor.Height;
				var pixelRow = accessor.GetRowSpan(newY);
				for (int x = 0; x < pixelRow.Length; x++)
				{
					int newX = (int)(accessor.Width * perlinCoordCenter.X + x) % accessor.Width;
					ref var pixel = ref pixelRow[newX];
					CellPixel[x, y] = pixel.R;
				}
			}
		});
	}
	/// <summary>
	/// 黑沉沼泽
	/// </summary>
	public static void BuildMidnightBayou()
	{
		Point center = new Point(600, 11640);
		int radious = 300;

		for (int x = (int)(center.X - radious * 1.5); x < (int)(center.X + radious * 1.5); x++)
		{
			for (int y = center.Y - 60; y < center.Y + radious; y++)
			{
				Tile tile = SafeGetTile(x, y);
				float color = PerlinPixel2[Math.Clamp((int)(x - center.X + radious * 1.5f) / 3, 0, 512), Math.Clamp((y - center.Y + radious) / 2, 0, 512)] / 255f;
				float distance = new Vector2((x - center.X) * 0.6667f, y - center.Y).Length() / 200f;
				if (color + distance > 1.5f)
				{
					tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
					tile.HasTile = true;
					if (color + distance > 1.6f)
					{
						tile.wall = (ushort)(ModContent.WallType<StoneDragonScaleWoodWall>());
					}
				}
				else
				{
					tile.ClearEverything();
				}
				if (y > center.Y + 30)
				{
					if (!tile.HasTile)
					{
						tile.TileType = (ushort)ModContent.TileType<DarkSludge>();
						tile.HasTile = true;
					}
				}
			}
		}
		int deltaX = 40;
		if (AzureGrottoCenterX > 600)
		{
			deltaX = -40;
		}
		int leftBound = GenRand.Next(380, 400) + deltaX;
		int rightBound = GenRand.Next(800, 820) + deltaX;
		int startY = 11632;
		KillRectangleAreaOfTile(leftBound, startY - 10, rightBound, startY);
		PlaceFrameImportantTiles(595, startY - 37, 8, 12, ModContent.TileType<OriginPylon>());
		for (int x = leftBound + 5; x < rightBound - 5; x++)
		{
			if(x % 20 == 0)
			{
				PlaceFrameImportantTiles(x, startY, 20, 1, ModContent.TileType<StoneBridgeTile>(), 0, 0);
				StoneBridge_fence sBF = new StoneBridge_fence { position = new Vector2(x, startY - 24) * 16, Active = true, Visible = true };
				Ins.VFXManager.Add(sBF);
			}
			//Tile tile = SafeGetTile(x, startY);
			//Tile tileLeft = SafeGetTile(x - 1, startY);
			//Tile tileRight = SafeGetTile(x + 1, startY);
			//tile.wall = WallID.IronFence;
			//if (x % 12 == 0)
			//{
			//	if (!tile.HasTile && !tileLeft.HasTile && !tileRight.HasTile)
			//	{
			//		for (int y = 1; y < 7; y++)
			//		{
			//			Tile tile2 = SafeGetTile(x, startY - y);
			//			tile2.wall = WallID.IronFence;
			//			if (y == 6)
			//			{
			//				Tile tile2Left = SafeGetTile(x + 1, startY - y);
			//				Tile tile2Right = SafeGetTile(x - 1, startY - y);
			//				tile2.TileType = TileID.Platforms;
			//				tile2.TileFrameY = 162;
			//				tile2.HasTile = true;
			//				tile2Left.TileType = TileID.Platforms;
			//				tile2Left.TileFrameY = 162;
			//				tile2Left.HasTile = true;
			//				tile2Right.TileType = TileID.Platforms;
			//				tile2Right.TileFrameY = 162;
			//				tile2Right.HasTile = true;

			//				/*PlaceFrameImportantTiles(x - 1, startY - y + 1, 1, 2, TileID.HangingLanterns, 0, 72);*/
			//				PlaceFrameImportantTiles(x + 1, startY - y + 1, 1, 2, TileID.HangingLanterns, 0, 72);
			//			}
			//		}
			//	}
			//}
			//if (x % 40 == 0 && x > leftBound + 20 && x < rightBound - 20)
			//{
			//	PlaceFrameImportantTiles(x - 6, 11636, 13, 8, ModContent.TileType<PierWithSlabsTop>());
			//	int y = 11644;
			//	while (!SafeGetTile(x, y).HasTile)
			//	{
			//		PlaceFrameImportantTiles(x - 1, y, 3, 3, ModContent.TileType<PierWithSlabs>());
			//		y += 3;
			//	}
			//}
		}
	}
	/// <summary>
	/// 登天石拱
	/// </summary>
	public static void BuildHeavenlyPortal()
	{
		int x0 = 155;
		int x1 = 1045;
		int y0 = 11200;
		for (int y = 11200; y < 11650; y++)
		{
			if (y > 11650)
			{
				break;
			}
			for (int x = 156; x <= 1044; x++)
			{
				Tile tile = SafeGetTile(x, y);
				if (tile.HasTile)
				{
					if (x > 155 && x < 400)
					{
						x0 = x;
					}
					if (x > 800)
					{
						x1 = x;
						y0 = y;
						y = 11651;
						break;
					}
				}
			}
		}
		int xLength0 = x0 - 155;
		int yLength = y0 - 11111;
		int y0CoordPerlin = GenRand.Next(512);
		int y1CoordPerlin = GenRand.Next(512);
		//左侧坡面
		for (int y = y0; y > 11111; y--)
		{
			for (int x = 155; x < x0; x++)
			{
				Tile tile = SafeGetTile(x, y);
				if (!tile.HasTile)
				{
					float colorPerlinValue = (x - 155f) * (x0 - x) / (x0 - 155 + 0.01f) / (x0 - 155 + 0.01f);
					float xValue = (x - 155) / (float)xLength0 + PerlinPixelB[x % 512, y0CoordPerlin] * colorPerlinValue / 255f * 0.3f * 300f / (x0 - 155 + 0.1f);
					if (xValue < (y - 11111) / (float)yLength)
					{
						tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
						tile.HasTile = true;
						if (xValue < (y - 11111) / (float)yLength + 4)
						{
							tile.wall = (ushort)(ModContent.WallType<StoneDragonScaleWoodWall>());
						}
					}
				}
			}
		}
		//右侧坡面
		int xLength1 = 1045 - x1;
		for (int y = y0; y > 11111; y--)
		{
			for (int x = x1; x < 1045; x++)
			{
				Tile tile = SafeGetTile(x, y);
				if (!tile.HasTile)
				{
					float colorPerlinValue = (x - x1) * (1045 - x) / (x - x1 + 0.01f) / (1045 - x + 0.01f);
					float xValue = (1045 - x) / (float)xLength1 + PerlinPixelB[x % 512, y1CoordPerlin] * colorPerlinValue / 255f * 0.3f * 300f / (1045 - x0 + 0.1f);
					if (xValue < (y - 11111) / (float)yLength)
					{
						tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
						tile.HasTile = true;
						if (xValue < (y - 11111) / (float)yLength + 4)
						{
							tile.wall = (ushort)(ModContent.WallType<StoneDragonScaleWoodWall>());
						}
					}
				}
			}
		}
		int coordRandomY = GenRand.Next(512);
		for (int x = 155; x < 1045; x++)
		{
			float y1 = y0 + (x - 600) * (x - 600) / 500f - 200;
			int y2 = PerlinPixelB[x % 512, coordRandomY] / 20;
			int y3 = PerlinPixelG[x % 512, coordRandomY] / 9;
			for (int y = (int)y1 - y2; y < y1 + 80 + y3; y++)
			{
				Tile tile = SafeGetTile(x, y);
				if (!tile.HasTile)
				{
					tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
					tile.HasTile = true;
				}
				if (y > (int)y1 - y2 + 4 && y < y1 + 76 + y3)
				{
					tile.wall = (ushort)(ModContent.WallType<StoneDragonScaleWoodWall>());
				}
			}
		}
	}
	/// <summary>
	/// 靛琉璃海
	/// </summary>
	public static void BuildAzureGrotto()
	{
		int startX = AzureGrottoCenterX;
		int bottomY = 11000;
		while (!SafeGetTile(startX, bottomY).HasTile)
		{
			bottomY++;
			if (bottomY > 11900)
			{
				break;
			}
		}
		int height = GenRand.Next(270, 321);
		int maxWidth = GenRand.Next(960, 981);
		int y0CoordPerlin = GenRand.Next(512);
		int y1CoordPerlin = GenRand.Next(512);

		for (int y = -30; y < height; y++)
		{
			float heightValue = y / (float)height;
			int width = (int)(Math.Pow(2, 8 * (heightValue - 0.9)) / 4d * maxWidth) + 25;
			for (int x = -width; x <= width; x++)
			{
				float thickValue = PerlinPixelG[(int)(x * 0.9f + maxWidth * 1f) % 512, y0CoordPerlin] * 0.2f;
				float thickValueUp = PerlinPixelG[(int)(x * 0.9f + maxWidth * 1f) % 512, y1CoordPerlin] * 0.08f;
				float mulThickValue = 1;
				if (maxWidth * 0.4377f - Math.Abs(x) < 30)
				{
					mulThickValue = (maxWidth * 0.4377f - Math.Abs(x)) / 60f;
					mulThickValue = MathF.Sin(mulThickValue * MathF.PI);
				}
				thickValue *= mulThickValue;
				thickValue = Math.Max(thickValue, 6);
				thickValueUp *= mulThickValue;
				thickValueUp = Math.Max(thickValueUp, 4);
				if (x <= -width + 8 || x >= width - 8)
				{
					if ((startX - 600) * x > 0)
					{
						int y1 = (int)-thickValueUp;
						while (true)
						{
							y1++;
							if (y1 > bottomY + 500)
							{
								break;
							}
							int finalX = x + startX;
							int finalY = bottomY - y + y1;
							Tile tile = SafeGetTile(finalX, finalY);
							if (!tile.HasTile)
							{
								tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
								tile.HasTile = true;
								if ((x <= -width + 12 && x >= -width + 3) || (x >= width - 12 && x <= width - 3))
								{
									tile.wall = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
								}
								if (y1 > 0)
								{
									tile.wall = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
								}
							}
							else if (SafeGetTile(finalX, finalY + 5).HasTile)
							{
								for (int y2 = 0; y2 < 6; y2++)
								{
									Tile tile2 = SafeGetTile(finalX, finalY + y2);
									tile2.wall = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
								}
								break;
							}
						}
					}
					else
					{
						for (int y1 = (int)-thickValueUp; y1 < thickValue; y1++)
						{
							int finalX = x + startX;
							int finalY = bottomY - y + y1;
							Tile tile = SafeGetTile(finalX, finalY);
							if (!tile.HasTile)
							{
								tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
								tile.HasTile = true;
								if ((x <= -width + 12 && x >= -width + 3) || (x >= width - 12 && x <= width - 3))
								{
									tile.wall = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
								}
								if (y1 > 0 && y1 < thickValue - 4)
								{
									tile.wall = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
								}
							}
						}
					}
				}
				else
				{
					int finalX = x + startX;
					int finalY = bottomY - y;
					Tile tile = SafeGetTile(finalX, finalY);
					tile.LiquidType = LiquidID.Water;
					tile.LiquidAmount = 255;
				}
			}
		}
	}
	/// <summary>
	/// 千回矿道
	/// </summary>
	public static void BuildTangledSubmine()
	{
		if (AzureGrottoCenterX > 600)
		{
			for (int x = 0; x < 110; x++)
			{
				WorldGen.digTunnel((GenRand.NextFloat(AzureGrottoCenterX + 90, 1140)), GenRand.NextFloat(11120, 11600), GenRand.NextFloat(-1, 1), GenRand.NextFloat(0, 1), GenRand.Next(27, 72), GenRand.Next(3, 7));
			}
			for (int x = 0; x < 30; x++)
			{
				WorldGen.digTunnel((GenRand.NextFloat(AzureGrottoCenterX + 200, 1140)), GenRand.NextFloat(11020, 11240), GenRand.NextFloat(-1, 1), GenRand.NextFloat(0, 1), GenRand.Next(27, 72), GenRand.Next(3, 7));
			}
			for (int x = 0; x < 30; x++)
			{
				WorldGen.digTunnel((GenRand.NextFloat(AzureGrottoCenterX + 160, 1140)), GenRand.NextFloat(11140, 11500), GenRand.NextFloat(-1, 1), GenRand.NextFloat(-1, 1), GenRand.Next(81, 144), GenRand.Next(8, 12));
			}
		}
		else
		{
			for (int x = 0; x < 110; x++)
			{
				WorldGen.digTunnel((GenRand.NextFloat(60, AzureGrottoCenterX - 90)), GenRand.NextFloat(11120, 11600), GenRand.NextFloat(-1, 1), GenRand.NextFloat(0, 1), GenRand.Next(27, 72), GenRand.Next(3, 7));
			}
			for (int x = 0; x < 30; x++)
			{
				WorldGen.digTunnel((GenRand.NextFloat(60, AzureGrottoCenterX - 200)), GenRand.NextFloat(11020, 11240), GenRand.NextFloat(-1, 1), GenRand.NextFloat(0, 1), GenRand.Next(27, 72), GenRand.Next(3, 7));
			}
			for (int x = 0; x < 30; x++)
			{
				WorldGen.digTunnel((GenRand.NextFloat(60, AzureGrottoCenterX - 160)), GenRand.NextFloat(11140, 11500), GenRand.NextFloat(-1, 1), GenRand.NextFloat(-1, 1), GenRand.Next(81, 144), GenRand.Next(8, 12));
			}
		}
		for (int x = 20; x < 1180; x++)
		{
			for (int y = 11000; y < 11800; y++)
			{
				Tile tile = SafeGetTile(x, y);
				Tile tileUp = SafeGetTile(x, y - 1);
				if (tile.LiquidAmount < tileUp.LiquidAmount)
				{
					for (int y1 = 0; y1 < 8; y1++)
					{
						Tile newTile = SafeGetTile(x, y + y1);
						newTile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
						newTile.HasTile = true;
					}
				}
			}
		}
		int mineLeft = AzureGrottoCenterX + 90;
		int mineRight = 1140;
		if (AzureGrottoCenterX < 600)
		{
			mineLeft = 60;
			mineRight = AzureGrottoCenterX - 90;
		}
		for (int x = mineLeft; x < mineRight; x++)
		{
			for (int y = 11000; y < 11680; y++)
			{
				Tile tile = SafeGetTile(x, y);
				Tile tileUp = SafeGetTile(x, y - 1);
				Tile tileUp1 = SafeGetTile(x - 1, y - 1);
				Tile tileUp2 = SafeGetTile(x - 2, y - 1);
				Tile tileUp3 = SafeGetTile(x - 3, y - 1);
				Tile tileUp4 = SafeGetTile(x - 4, y - 1);
				Tile tileLeft1 = SafeGetTile(x - 1, y);
				Tile tileLeft2 = SafeGetTile(x - 2, y);
				Tile tileLeft3 = SafeGetTile(x - 3, y);
				Tile tileLeft4 = SafeGetTile(x - 4, y);
				if (tile.LiquidAmount > 0 || tileUp.LiquidAmount > 0 || tileUp1.LiquidAmount > 0 || tileUp2.LiquidAmount > 0 || tileUp3.LiquidAmount > 0 || tileUp4.LiquidAmount > 0 || tileLeft1.LiquidAmount > 0 || tileLeft1.LiquidAmount > 0 || tileLeft2.LiquidAmount > 0 || tileLeft3.LiquidAmount > 0 || tileLeft4.LiquidAmount > 0)
				{
					continue;
				}
				if (tileLeft1.HasTile && tileLeft2.HasTile && tileLeft3.HasTile && tileLeft4.HasTile && tile.HasTile && !tileUp.HasTile && !tileUp4.HasTile)
				{
					if (tileLeft1.TileType == ModContent.TileType<StoneScaleWood>() && tileLeft2.TileType == ModContent.TileType<StoneScaleWood>() && tileLeft3.TileType == ModContent.TileType<StoneScaleWood>() && tileLeft4.TileType == ModContent.TileType<StoneScaleWood>() && tile.TileType == ModContent.TileType<StoneScaleWood>())
					{
						if (GenRand.NextBool(3))
						{
							PlaceLargeCyanVineOre(x - 4, y - 3);
						}
					}
				}
				if (tileLeft1.HasTile && tileLeft2.HasTile && tile.HasTile && !tileUp2.HasTile)
				{
					if (tileLeft1.TileType == ModContent.TileType<StoneScaleWood>() && tileLeft2.TileType == ModContent.TileType<StoneScaleWood>() && tile.TileType == ModContent.TileType<StoneScaleWood>())
					{
						if (GenRand.NextBool(12))
						{
							PlaceMiddleCyanVineOre(x - 2, y - 2);
						}
					}
				}
				if (tileLeft1.HasTile && tileLeft2.HasTile && tile.HasTile && !tileUp1.HasTile)
				{
					if (tileLeft1.TileType == ModContent.TileType<StoneScaleWood>() && tileLeft2.TileType == ModContent.TileType<StoneScaleWood>() && tile.TileType == ModContent.TileType<StoneScaleWood>())
					{
						if (GenRand.NextBool(12))
						{
							PlaceSmallCyanVineOre(x - 2, y - 2);
						}
					}
				}
				if (!tileLeft1.HasTile && tileUp.HasTile && tileUp1.HasTile && tileUp2.HasTile)
				{
					if (tileUp1.TileType == ModContent.TileType<StoneScaleWood>() && tileUp2.TileType == ModContent.TileType<StoneScaleWood>() && tileUp.TileType == ModContent.TileType<StoneScaleWood>())
					{
						if (GenRand.NextBool(6))
						{
							PlaceSmallUpCyanVineOre(x - 2, y);
						}
					}
				}
				if (!tile.HasTile && !tileLeft4.HasTile && tileUp.HasTile && tileUp1.HasTile && tileUp2.HasTile && tileUp3.HasTile && tileUp4.HasTile)
				{
					if (tileUp1.TileType == ModContent.TileType<StoneScaleWood>() && tileUp2.TileType == ModContent.TileType<StoneScaleWood>() && tileUp.TileType == ModContent.TileType<StoneScaleWood>())
					{
						if (GenRand.NextBool(3))
						{
							PlaceLargeUpCyanVineOre(x - 4, y);
						}
					}
				}
			}
		}

		for (int x = 60; x < 1140; x++)
		{
			for (int y = 11000; y < 12000; y++)
			{
				if (GenRand.NextBool(1500))
				{
					Tile tile = SafeGetTile(x, y);
					Tile tileUp = SafeGetTile(x, y - 1);
					Tile tileDown = SafeGetTile(x, y + 1);
					Tile tileLeft = SafeGetTile(x - 1, y);
					Tile tileRight = SafeGetTile(x + 1, y);
					if (tile.HasTile && tile.TileType == ModContent.TileType<StoneScaleWood>())
					{
						if (tileUp.LiquidAmount <= 0 && tileDown.LiquidAmount <= 0 && tileLeft.LiquidAmount <= 0 && tileRight.LiquidAmount <= 0)
						{
							WorldGen.TileRunner(x, y, GenRand.NextFloat(2f, 6f), GenRand.Next(4, 18), ModContent.TileType<CyanVineStone>());
						}
					}
				}
			}
		}
	}
	/// <summary>
	/// 石化古道
	/// </summary>
	public static void BuildFossilizedMineRoad()
	{
		int deltaX = 120;
		if (AzureGrottoCenterX > 600)
		{
			deltaX = -120;
		}
		int step = Math.Sign(deltaX);
		int startX = 600 + deltaX;
		int startY = 11632;
		while (SafeGetTile(startX, startY + 1).TileType == TileID.GrayBrick)
		{
			startX += step;
		}
		int lengthX = GenRand.Next(140, 152);
		for (int x0 = 0; x0 < lengthX; x0++)
		{
			KillRectangleAreaOfTile(x0 * step + startX, startY - 17, x0 * step + startX, startY);
			PlaceRectangleAreaOfBlock(x0 * step + startX, startY + 1, x0 * step + startX, startY + 3, TileID.GrayBrick, false);
		}
		int continueEmpty = 0;
		float radius = 5f;
		Vector2 velocity = new Vector2(step, 0);
		Vector2 position = new Vector2(startX + lengthX * step, startY - radius);
		int times = 0;
		int coordY = GenRand.Next(512);
		int rotatedTimes = 0;
		int noRotatedTimes = 0;
		while (continueEmpty < 15)
		{
			times++;
			velocity = Vector2.Normalize(velocity);
			position += velocity;
			int x = (int)position.X;
			int y = (int)position.Y;
			for (int x0 = -10; x0 <= 10; x0++)
			{
				for (int y0 = -10; y0 <= 10; y0++)
				{
					if (new Vector2(x0, y0).Length() < radius)
					{
						Tile tile = SafeGetTile(x0 + x, y0 + y);
						tile.HasTile = false;
					}
				}
			}
			if (rotatedTimes > 0)
			{
				rotatedTimes--;
				velocity = velocity.RotatedBy(step * Math.PI / 40d);
				noRotatedTimes = 0;
			}
			else
			{
				rotatedTimes = 0;
				Vector2 probePos = position + velocity * 50;
				if ((!SafeGetTile((int)probePos.X, (int)(probePos.Y)).HasTile && position.Y > 11451) || probePos.X > 1200 || probePos.X < 0)
				{
					rotatedTimes = 40;
					step *= -1;
					continue;
				}
				velocity = velocity.RotatedBy((PerlinPixelG[times % 512, coordY] - 127.5) * 0.0002);
				velocity.Y -= 0.015f;
				noRotatedTimes++;
				if (noRotatedTimes > Math.Max(80 + 11540 - position.Y, 80))
				{
					if (GenRand.NextBool(60))
					{
						rotatedTimes = 40;
						step *= -1;
					}
				}
				velocity.X *= 1.12f;
			}
			Vector2 probePosII = position + velocity * 5;
			if (SafeGetTile((int)probePosII.X, (int)(probePosII.Y)).TileType != ModContent.TileType<StoneScaleWood>() && y < 11451)
			{
				continueEmpty++;
			}
			else
			{
				continueEmpty = 0;
			}
			if (times > 8000)
			{
				break;
			}
		}

	}
	/// <summary>
	/// 下天穹镇
	/// </summary>
	public static void BuildTownBelow()
	{
		int randX = AzureGrottoCenterX + 50;
		if (AzureGrottoCenterX > 600)
		{
			randX = AzureGrottoCenterX - 50 - 434;
		}
		QuickBuild(randX, 11270, "YggdrasilTown/MapIOs/434x159YggdrasilTown.mapio");
		int randY = GenRand.Next(512);
		//平滑嵌入左上段
		for(int x = randX - 70;x < randX + 150;x++)
		{
			float yMin = (randX + 150 - x) / 220f;
			yMin *= yMin;
			yMin *= 45f;
			yMin = 11270 - yMin;
			yMin -= PerlinPixelB[x % 512, randY] * 0.02f;
			for (int y = 11270; y > yMin; y--)
			{
				Tile tile = SafeGetTile(x, y);
				if (y == 11270)
				{
					if (!tile.HasTile)
					{
						break;
					}
				}
				else
				{
					tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
					tile.HasTile = true;
				}
			}
		}
		//平滑嵌入右侧
		for (int x = randX + 390; x < randX + 600; x++)
		{
			float yMin = (randX + 600 - x) / 210f;
			yMin *= yMin;
			yMin *= 55f;
			yMin = 11200 + yMin;
			yMin -= PerlinPixelB[x % 512, randY] * 0.06f;
			for (int y = (int)yMin; y <  11400; y++)
			{
				Tile tile = SafeGetTile(x, y);
				if (!tile.HasTile && tile.LiquidAmount == 0)
				{ 
					tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
					tile.HasTile = true;
				}
			}
		}
		Point digStart = new Point(randX, 11415);
		for (int t = 0;t < 75;t++)
		{
			digStart += new Point(-1, t / 24);
			int range = (75 - t) / 5 + 5;
			for(int x0 = -range; x0 <= range; x0++)
			{
				for (int y0 = -range; y0 <= range; y0++)
				{
					Tile tile = Main.tile[x0 + digStart.X, y0 + digStart.Y];
					if (new Vector2(x0, y0).Length() < range)
					{
						tile.HasTile = false;
					}
				}
			}
		}
	}
	/// <summary>
	/// 上天穹镇
	/// </summary>
	public static void BuildTownUpper()
	{
		//for (int y = 10900; y < 11030; y++)
		//{
		//	int randX = GenRand.Next(150, 1050);
		//	int maxLeft = -120;
		//	int maxRight = 120;
		//	for (int x0 = 0; x0 < 90; x0++)
		//	{
		//		Tile tile = SafeGetTile(x0 + randX, y);
		//		if (tile.HasTile || tile.LiquidAmount > 0)
		//		{
		//			maxRight = x0;
		//			break;
		//		}
		//	}
		//	for (int x0 = 0; x0 > -90; x0--)
		//	{
		//		Tile tile = SafeGetTile(x0 + randX, y);
		//		if (tile.HasTile || tile.LiquidAmount > 0)
		//		{
		//			maxLeft = x0;
		//			break;
		//		}
		//	}
		//	if (maxLeft >= -5 || maxRight <= 5)
		//	{
		//		continue;
		//	}
		//	else
		//	{
		//		if (!GenRand.NextBool(6))
		//		{
		//			maxLeft = GenRand.Next(maxLeft, 0);
		//		}
		//		if (!GenRand.NextBool(6))
		//		{
		//			maxRight = GenRand.Next(0, maxRight);
		//		}
		//		for (int checkX = maxLeft; checkX < maxRight; checkX++)
		//		{
		//			for (int checkY = 8; checkY > -20; checkY--)
		//			{
		//				Tile tile = SafeGetTile(checkX + randX, checkY + y);
		//				if ((tile.HasTile && tile.TileType == TileID.GrayBrick) || (tile.LiquidAmount > 0 && Math.Abs(checkY) < 4))
		//				{
		//					if (checkX < 0)
		//					{
		//						maxLeft = checkX + 12;
		//						break;
		//					}
		//					if (checkX > 0)
		//					{
		//						maxRight = checkX - 12;
		//						break;
		//					}
		//				}
		//			}
		//		}
		//		if (maxRight - maxLeft < 10)
		//		{
		//			continue;
		//		}
		//		if (maxLeft >= -5 || maxRight <= 5)
		//		{
		//			continue;
		//		}
		//		CreateStreet(maxLeft + randX, y, maxRight + randX, y + 2);
		//		if (GenRand.NextBool(3))
		//		{
		//			y -= 2;
		//		}
		//		else
		//		{
		//			y += GenRand.Next(8, 11);
		//		}
		//	}
		//}
	}
	/// <summary>
	/// 隐天玄壁
	/// </summary>
	public static void BuildDuskfallBarrier()
	{
		int centerY = 10700;
		int coordX = GenRand.Next(512);
		int coordY = GenRand.Next(150, 612);
		for (int x = 0; x < 1200; x++)
		{
			for (int y = -150; y < 150; y++)
			{
				float thick = (x - 600) * (x - 600) / 2400f + 30f;
				thick += PerlinPixelR[(x + coordX) % 512, (y + coordY) % 512] / 25f;
				if (Math.Abs(y) < thick)
				{
					Tile tile = SafeGetTile(x, y + centerY);
					tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
					tile.HasTile = true;
				}
				if (Math.Abs(y) < thick - 4)
				{
					Tile tile = SafeGetTile(x, y + centerY);
					tile.wall = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
				}
			}
		}
	}
	/// <summary>
	/// 挑战者石牢
	/// </summary>
	public static void BuildStoneCageOfChallenges()
	{
		int startY = 10900;
		while (!SafeGetTile(AzureGrottoCenterX, startY).HasTile)
		{
			startY--;
			if(startY < 10500)
			{
				break;
			}
		}
		startY -= 30;
		int randX = GenRand.Next(512);
		int randY = GenRand.Next(512);
		int step = -1;
		if (AzureGrottoCenterX > 600)
		{
			step = 1;
		}
		int x = AzureGrottoCenterX - step * 240;
		while(x >= 50 && x <= 1150)
		{
			x += step;
			float noiseX = PerlinPixelG[(randX + x) % 512, randY] / 256f;
			float valueX = Math.Abs(x - (AzureGrottoCenterX - step * 170) + noiseX * 27f) / 550f;
			float valueY = 1 - MathF.Cos(valueX * MathF.PI);
			valueY *= 200;
			for (int y = startY; y <= startY + valueY; y++)
			{
				Tile target = SafeGetTile(x, y);
				target.HasTile = true;
				target.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
				if(y <= startY + valueY - 6)
				{
					target.wall = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
				}
			}
		}
		int tunnelLength = GenRand.Next(447, 528);
		x = AzureGrottoCenterX;
		if(step > 0)
		{
			while (x + tunnelLength > Main.maxTilesX - 50)
			{
				tunnelLength--;
			}
		}
		else
		{
			while (x - tunnelLength < 50)
			{
				tunnelLength--;
			}
		}
		int count = 0;
		int startY2 = startY + 170;
		int cageMiddleX = AzureGrottoCenterX + (tunnelLength - 100) * step;
		randX = GenRand.Next(512);
		randY = GenRand.Next(512);
		while (count <= tunnelLength)
		{
			count++;
			x += step;
			for(int y = startY2;y <= startY2 + 8;y++)
			{
				Tile target = SafeGetTile(x, y);
				
				if(y != startY2 + 8)
				{
					target.HasTile = false;
				}
				else
				{
					target.TileType = TileID.GrayBrick;
					target.HasTile = true;
				}
			}
			if(count > tunnelLength - 200)
			{
				float valueX = tunnelLength - count;
				valueX -= 100;
				float deltaY = 80 - valueX * valueX / 450f;
				deltaY += PerlinPixelB[(randX + count) % 512, randY] / 90f;
				for (int y = startY2 - (int)deltaY; y <= startY2 + 8; y++)
				{
					Tile target = SafeGetTile(x, y);
					target.HasTile = false;
					Vector2 centerPoint = new Vector2(cageMiddleX, startY2 - deltaY * 0.5f);
					float colorValue = 1 - CellPixel[(x * 4 + randX) % 512, (y * 4 + randY) % 512] / 255f;
					colorValue *= colorValue * 2;
					if (colorValue > (centerPoint - new Vector2(x, y)).Length() / 100f - 0.7f)
					{
						target.ClearEverything();
					}
				}
			}
		}
		int sealY = startY2 - 20;
		int sealCountY = 0;
		while(!SafeGetTile(cageMiddleX, sealY + sealCountY).HasTile)
		{
			sealCountY++;
			if(sealCountY > 100)
			{
				break;
			}
		}
		sealY = sealY + sealCountY - 10;
		PlaceFrameImportantTiles(cageMiddleX, sealY, 20, 10, ModContent.TileType<SquamousShellSeal>());
		Point centerOfCage = new Point(cageMiddleX, startY2 - 40);
		randX = GenRand.Next(512);
		randY = GenRand.Next(512);
		int startX = Math.Max(cageMiddleX - 300, 0);
		int endX = Math.Min(cageMiddleX + 300, Main.maxTilesX);
		for (int x0 = startX;x0 <= endX; x0++)
		{
			for (int y = centerOfCage.Y - 200; y <= centerOfCage.Y + 30; y++)
			{
				Tile target = SafeGetTile(x0, y);
				if(target.TileType == ModContent.TileType<StoneScaleWood>() && target.HasTile)
				{
					Vector2 dis = (new Vector2(x0, y) - new Vector2(centerOfCage.X, centerOfCage.Y + 40));
					dis.X *= 1f;
					float myLength = dis.Length();
					float cellC = 1 - CellPixel[(int)(x0 * 3.5f + randX) % 512, (int)(myLength * 3 + randY) % 512] / 255f;
					//float xValue = Math.Abs(x0 - cageMiddleX) / 300f;
					//xValue = 1 - xValue;
					//xValue *= 3;
					//xValue = Math.Min(xValue, 1);
					Vector2 angle = new Vector2(Math.Abs(x0 - cageMiddleX), y - startY2);
					float xValue = angle.X / angle.Length();
					float lengthValue = myLength / 300f;
					if (cellC > lengthValue + xValue)
					{
						target.ClearEverything();
						target.TileType = (ushort)(ModContent.TileType<YggdrasilAmber>());
						target.HasTile = true;
					}
				}
			}
		}
		if(SubworldLibrary.SubworldSystem.Current != null)
		{
			YggdrasilWorld yWorld = SubworldLibrary.SubworldSystem.Current as YggdrasilWorld;
			if (yWorld != null)
			{
				yWorld.StoneCageOfChallengesCenter = new Vector2(cageMiddleX, startY2 - 40) * 16;
			}
		}
	}
	/// <summary>
	/// 建造一个灯柱
	/// </summary>
	public static void PlaceChineseStyleLampPost(int x, int y, int style, int ai0 = 0, int ai1 = 0, int ai2 = 0, int ai3 = 0)
	{
		switch (style)
		{
			case 0:
				PlaceFrameImportantTiles(x, y - 5, 1, 5, ModContent.TileType<DoubleArmsChineseStreetLamp>());
				break;
			case 1:
				PlaceRectangleAreaOfWall(x, y - 5, x, y, WallID.BambooFence);
				Tile tile0 = SafeGetTile(x, y - 5);
				int tile1Dir = x + 1;
				if (ai0 % 2 == 0)
				{
					tile1Dir = x - 1;
				}
				Tile tile1 = SafeGetTile(tile1Dir, y - 5);
				tile0.TileType = TileID.Platforms;
				tile0.frameY = 786;
				tile0.HasTile = true;
				tile1.TileType = TileID.Platforms;
				tile1.frameY = 786;
				tile1.HasTile = true;
				int lanternType = 1620;
				if (ai1 % 4 == 0)
				{
					lanternType = 936;
				}
				PlaceFrameImportantTiles(tile1Dir, y - 4, 1, 2, TileID.HangingLanterns, 0, lanternType);
				break;
			case 2:
				int lampStyle = 918;
				int lampStyleX = 0;
				if (ai0 % 12 == 1)
				{
					lampStyle = 324;
				}
				if (ai0 % 12 == 2)
				{
					lampStyle = 432;
				}
				if (ai0 % 12 == 3)
				{
					lampStyle = 756;
				}
				if (ai0 % 12 == 4)
				{
					lampStyle = 0;
				}
				if (ai0 % 12 == 5)
				{
					lampStyle = 108;
					lampStyleX = 36;
				}
				PlaceFrameImportantTiles(x, y - 3, 1, 3, TileID.Lamps, lampStyleX, lampStyle);
				break;
		}
	}
	/// <summary>
	/// 建造一个中式的民居
	/// </summary>
	/// <param name="startX"></param>
	/// <param name="startY"></param>
	/// <param name="endX"></param>
	/// <param name="endY"></param>
	public static void CreateChineseFolkBox(int startX, int startY, int endX, int endY)
	{
		int width = Math.Abs(endX - startX);
		int height = Math.Abs(endY - startY);
		int squire = width * height;
		if (!GenRand.NextBool(4))
		{
			switch (WorldGen.genRand.Next(5))
			{
				case 0:
					if (width >= 26 && height > 10)
					{
						switch (WorldGen.genRand.Next(2))
						{
							case 0:
								QuickBuild(startX, endY - 11, "YggdrasilTown/MapIOs/1FolkHouseofChineseStyleTypeA28x11.mapio");
								break;
							case 1:
								QuickBuild(startX, endY - 11, "YggdrasilTown/MapIOs/1FolkHouseofChineseStyleTypeB28x11.mapio");
								break;
						}
						return;
					}
					else
						break;
				case 1:
					if (width >= 20 && height > 8)
					{
						switch (WorldGen.genRand.Next(2))
						{
							case 0:
								QuickBuild(startX, endY - 8, "YggdrasilTown/MapIOs/3SmithyTypeA22x8.mapio");
								break;
							case 1:
								QuickBuild(startX, endY - 8, "YggdrasilTown/MapIOs/3SmithyTypeB22x8.mapio");
								break;
						}
						return;
					}
					else
						break;
				case 2:
					if (width >= 26 && height > 10)
					{
						switch (WorldGen.genRand.Next(4))
						{
							case 0:
								QuickBuild(startX, endY - 11, "YggdrasilTown/MapIOs/2FolkHouseofWoodAndStoneStrutureTypeA28x11.mapio");
								break;
							case 1:
								QuickBuild(startX, endY - 11, "YggdrasilTown/MapIOs/2FolkHouseofWoodAndStoneStrutureTypeB28x11.mapio");
								break;
							case 2:
								QuickBuild(startX, endY - 11, "YggdrasilTown/MapIOs/2FolkHouseofWoodStoneStrutureTypeA28x11.mapio");
								break;
							case 3:
								QuickBuild(startX, endY - 11, "YggdrasilTown/MapIOs/2FolkHouseofWoodStoneStrutureTypeB28x11.mapio");
								break;
						}
						return;
					}
					else
						break;
				case 3:
					if (width >= 20 && height > 9)
					{
						switch (WorldGen.genRand.Next(4))
						{
							case 0:
								QuickBuild(startX, endY - 10, "YggdrasilTown/MapIOs/4FolkHouseofWoodStrutureTypeA22x10.mapio");
								break;
							case 1:
								QuickBuild(startX, endY - 10, "YggdrasilTown/MapIOs/4FolkHouseofWoodStrutureTypeB22x10.mapio");
								break;
							case 2:
								QuickBuild(startX, endY - 10, "YggdrasilTown/MapIOs/4FolkHouseofWoodStrutureTypeC22x10.mapio");
								break;
							case 3:
								QuickBuild(startX, endY - 10, "YggdrasilTown/MapIOs/4FolkHouseofWoodStrutureTypeD22x10.mapio");
								break;
						}
						return;
					}
					else
						break;
				case 4:
					if (width >= 21 && height > 12)
					{
						switch (WorldGen.genRand.Next(3))
						{
							case 0:
								QuickBuild(startX, endY - 13, "YggdrasilTown/MapIOs/5TwoStoriedFolkHouseTypeA23x13.mapio");
								break;
							case 1:
								QuickBuild(startX, endY - 13, "YggdrasilTown/MapIOs/5TwoStoriedFolkHouseTypeB23x13.mapio");
								break;
							case 2:
								QuickBuild(startX, endY - 13, "YggdrasilTown/MapIOs/5TwoStoriedFolkHouseTypeC23x13.mapio");
								break;
						}
						return;
					}
					else
						break;
			}
		}
		//本体
		CreateBoxRoom(startX, startY, endX, endY, TileID.DynastyWood, WallID.Ebonwood, true);
		//黄色的屋檐
		PlaceRectangleAreaOfBlock(startX - 1, startY, startX - 1, startY, ModContent.TileType<YellowDynastyShingles>());
		PlaceRectangleAreaOfBlock(endX + 1, startY, endX + 1, startY, ModContent.TileType<YellowDynastyShingles>());
		PlaceRectangleAreaOfBlock(startX - 2, startY + 1, startX - 1, startY + 1, ModContent.TileType<YellowDynastyShingles>());
		PlaceRectangleAreaOfBlock(endX + 1, startY + 1, endX + 2, startY + 1, ModContent.TileType<YellowDynastyShingles>());
		if (GenRand.NextBool(2))
		{
			PlaceRectangleAreaOfBlock(startX - 3, startY + 2, startX - 1, startY + 2, ModContent.TileType<YellowDynastyShingles>());
			PlaceRectangleAreaOfBlock(endX + 1, startY + 2, endX + 3, startY + 2, ModContent.TileType<YellowDynastyShingles>());
		}
		//50%的概率侧面有灯笼
		if (GenRand.NextBool(2))
		{
			int type = ModContent.TileType<SideHangingLantern>();
			if (GenRand.NextBool(3))
			{
				type = ModContent.TileType<SideHangingLantern_White>();
			}
			int placeY = startY;
			//左侧
			bool canPlaceLeft = true;
			while (SafeGetTile(startX - 1, placeY).HasTile)
			{
				placeY++;
				if (placeY - startY > 5)
				{
					canPlaceLeft = false;
					break;
				}
			}
			for (int x = startX - 2; x <= startX - 1; x++)
			{
				for (int y = placeY; y <= placeY + 2; y++)
				{
					Tile check = SafeGetTile(x, y);
					if (check.HasTile)
					{
						canPlaceLeft = false;
					}
				}
			}
			if (canPlaceLeft)
			{
				PlaceFrameImportantTiles(startX - 2, placeY, 2, 3, type);
			}
			//右侧
			bool canPlaceRight = true;
			placeY = startY;
			while (SafeGetTile(endX + 1, placeY).HasTile)
			{
				placeY++;
				if (placeY - startY > 5)
				{
					canPlaceRight = false;
					break;
				}
			}
			for (int x = endX + 1; x <= endX + 2; x++)
			{
				for (int y = placeY; y <= placeY + 2; y++)
				{
					Tile check = SafeGetTile(x, y);
					if (check.HasTile)
					{
						canPlaceRight = false;
					}
				}
			}
			if (canPlaceRight)
			{
				PlaceFrameImportantTiles(endX + 1, placeY, 2, 3, type, 36);
			}
		}
		//判定是否应该有门
		bool hasEndXDoor = true;
		for (int x = endX + 1; x < endX + 5; x++)
		{
			int y = endY;
			Tile tile = SafeGetTile(x, y);
			if (!tile.HasTile)
			{
				hasEndXDoor = false;
				break;
			}
		}
		if (hasEndXDoor)
		{
			PlaceFrameImportantTiles(endX, endY - 3, 1, 3, TileID.ClosedDoor);
		}
		bool hasStartXDoor = true;
		for (int x = startX - 1; x > startX - 5; x--)
		{
			int y = endY;
			Tile tile = SafeGetTile(x, y);
			if (!tile.HasTile)
			{
				hasStartXDoor = false;
				break;
			}
		}
		if (hasStartXDoor)
		{
			PlaceFrameImportantTiles(startX, endY - 3, 1, 3, TileID.ClosedDoor);
		}
		DistributeChineseStyleDecorations(startX, startY, endX, endY);
	}
	/// <summary>
	/// 装饰一个中式的房间
	/// </summary>
	/// <param name="startX"></param>
	/// <param name="startY"></param>
	/// <param name="endX"></param>
	/// <param name="endY"></param>
	public static void DistributeChineseStyleDecorations(int startX, int startY, int endX, int endY)
	{
		int width = Math.Abs(endX - startX);
		int height = Math.Abs(endY - startY);
		int squire = width * height;
		//面积大于100的情况拆分房间
		if (squire > 100)
		{
			if (height >= width * 0.84f)
			{
				int middleCutY = (int)(height / 2f + GenRand.NextFloat(-height * 0.02f, height * 0.02f)) + startY;
				PlaceRectangleAreaOfBlock(startX, middleCutY, endX, middleCutY, TileID.DynastyWood);
				DistributeChineseStyleDecorations(startX, startY, endX, middleCutY);
				DistributeChineseStyleDecorations(startX, middleCutY, endX, endY);
				return;
			}
			if (width >= 1.5f * height && height >= 3)
			{
				int middleCutX = (int)(width / 2f + GenRand.NextFloat(-width * 0.2f, width * 0.2f)) + startX;
				PlaceRectangleAreaOfBlock(middleCutX, startY, middleCutX, endY - 3, TileID.DynastyWood);
				//横向分割需要加门
				PlaceFrameImportantTiles(middleCutX, endY - 3, 1, 3, TileID.ClosedDoor);

				DistributeChineseStyleDecorations(startX, startY, middleCutX, endY);
				DistributeChineseStyleDecorations(middleCutX, startY, endX, endY);
				return;
			}
		}
		List<int> emptyBottomX = new List<int>();
		for (int x = startX; x < endX; x++)
		{
			int y = endY + 1;
			Tile tile = SafeGetTile(x, y);
			Tile tileUp = SafeGetTile(x, y - 2);
			if (tile.wall != 0 && !tile.HasTile && tileUp.wall != 0 && !tileUp.HasTile)
			{
				emptyBottomX.Add(x);
			}
		}
		//随机获取一个放置平台的点位
		List<int[]> continuePlatforms = new List<int[]>();
		List<int> continuePlatform = new List<int>();
		for (int times = 1; times < emptyBottomX.Count; times++)
		{
			if (emptyBottomX[times] - emptyBottomX[times - 1] == 1)
			{
				if (continuePlatform.Count == 0)
				{
					continuePlatform.Add(emptyBottomX[times - 1]);
				}
				continuePlatform.Add(emptyBottomX[times]);
			}
			else
			{
				if (continuePlatform.Count >= 3)
				{
					continuePlatforms.Add(continuePlatform.ToArray());
				}
				continuePlatform = new List<int>();
			}
			if (continuePlatform.Count >= 3)
			{
				if (GenRand.NextBool(3))
				{
					continuePlatforms.Add(continuePlatform.ToArray());
					continuePlatform = new List<int>();
				}
			}
		}
		if (continuePlatform.Count > 0)
		{
			continuePlatforms.Add(continuePlatform.ToArray());
		}
		int createPlatformIndex = GenRand.Next(continuePlatforms.Count);
		if (continuePlatforms.Count > 0)
		{
			foreach (int x in continuePlatforms[createPlatformIndex])
			{
				int y = endY;
				Tile tile = SafeGetTile(x, y);
				tile.TileType = TileID.Platforms;
				tile.TileFrameY = 342;
				tile.TileFrameX = 0;
				if (continuePlatforms[createPlatformIndex][0] == x)
				{
					tile.TileFrameX = 252;
				}
				if (continuePlatforms[createPlatformIndex][continuePlatforms[createPlatformIndex].Length - 1] == x)
				{
					tile.TileFrameX = 216;
				}


				tile.wall = SafeGetTile(x, y - 1).wall;
			}
		}
		//Hanging Tiles
		PlaceARowOfHangingItems(startX, endX, startY + 1);
	}
	/// <summary>
	/// 建造一个火柴盒
	/// </summary>
	/// <param name="startX"></param>
	/// <param name="startY"></param>
	/// <param name="endX"></param>
	/// <param name="endY"></param>
	/// <param name="boundTileType"></param>
	/// <param name="contentWallType"></param>
	/// <param name="Forced"></param>
	public static void CreateBoxRoom(int startX, int startY, int endX, int endY, int boundTileType, int contentWallType, bool Forced = false)
	{
		if (endX < startX)
		{
			(startX, endX) = (endX, startX);
		}
		if (endY < startY)
		{
			(startY, endY) = (endY, startY);
		}
		for (int x = startX; x <= endX; x++)
		{
			for (int y = startY; y <= endY; y++)
			{
				Tile tile = SafeGetTile(x, y);
				if (!Forced)
				{
					if (x == startX || x == endX || y == startY || y == endY)
					{
						if (!tile.HasTile)
						{
							tile.TileType = (ushort)(boundTileType);
							tile.HasTile = true;
						}
					}
					else
					{
						if (tile.wall == 0)
						{
							tile.wall = (ushort)(contentWallType);
						}
					}
				}
				else
				{
					if (x == startX || x == endX || y == startY || y == endY)
					{
						tile.TileType = (ushort)(boundTileType);
						tile.HasTile = true;
					}
					else
					{
						tile.HasTile = false;
						tile.wall = (ushort)(contentWallType);
					}
				}
			}
		}
	}
	/// <summary>
	/// 造一条斜索
	/// </summary>
	public static void CreateSlantCable(int x, int y, int direction, int type, int density = 12, int maxStep = 65535)
	{
		int count = 0;
		while (!SafeGetTile(x, y).HasTile || count <= 2)
		{
			count++;
			Tile tile1 = SafeGetTile(x, y);
			Tile tile2 = SafeGetTile(x + direction, y);
			tile1.HasTile = true;
			tile1.TileType = (ushort)(type);
			if (direction == -1)
			{
				tile1.slope((byte)SlopeType.SlopeUpLeft);
			}
			else
			{
				tile1.slope((byte)SlopeType.SlopeUpRight);
			}
			tile2.HasTile = true;
			tile2.TileType = (ushort)(type);
			if (direction == -1)
			{
				tile2.slope((byte)SlopeType.SlopeDownRight);
			}
			else
			{
				tile2.slope((byte)SlopeType.SlopeDownLeft);
			}
			if (x < 20 || y < 20 || x > Main.maxTilesX - 20 || y > Main.maxTilesY - 20)
			{
				break;
			}
			if (count > maxStep)
			{
				break;
			}
			if (count % density == density - 1 && maxStep == 65535)
			{
				for (int y0 = 0; y0 < count; y0++)
				{
					Tile tile3 = SafeGetTile(x + direction, y - y0);
					tile3.wall = WallID.Shadewood;
					if (y0 % (density * 2) == density - 1 && !(SafeGetTile(x + direction, y - y0).HasTile))
					{
						CreateSlantCable(x + direction, y - y0, direction, type, density, density - 1);
						CreateSlantCable(x + direction * 0, y - y0, -direction, type, density, density - 1);
					}
				}
			}
			x += direction;
			y += 1;
		}
	}
	/// <summary>
	/// 排布一行悬挂物
	/// </summary>
	/// <param name="startX"></param>
	/// <param name="endX"></param>
	/// <param name="y"></param>
	public static void PlaceARowOfHangingItems(int startX, int endX, int y)
	{
		int x = startX + 1;
		while (x + 1 < endX)
		{
			float totalRare = 0;
			foreach (YggdrasilTownStreetElement element in InDoorChineseStyleHangingSheet)
			{
				if (element.Width <= endX - (x + 1))
				{
					if (element.Cooling == 0)
					{
						totalRare += 1 / element.Rare;
					}
				}
			}
			if (totalRare == 0)
			{
				foreach (YggdrasilTownStreetElement element in InDoorChineseStyleHangingSheet)
				{
					if (element.Cooling > 0)
					{
						element.Update();
					}
				}
				x++;
				continue;
			}
			float buildIndex = GenRand.NextFloat(totalRare);
			float rareIndex = 0;
			int width = 0;
			foreach (YggdrasilTownStreetElement element in InDoorChineseStyleHangingSheet)
			{
				if (element.Width <= endX - (x + 1))
				{
					if (element.Cooling == 0)
					{
						rareIndex += 1 / element.Rare;
					}
					if (rareIndex - 1 / element.Rare < buildIndex && rareIndex >= buildIndex)
					{
						element.Build(ref x, y);
						width = element.Width;
						break;
					}
				}
			}
			foreach (YggdrasilTownStreetElement element in InDoorChineseStyleHangingSheet)
			{
				if (element.Cooling == 0)
				{
					element.Update(width);
				}
			}
		}
	}
	/// <summary>
	/// 造一条街
	/// </summary>
	/// <param name="startX"></param>
	/// <param name="startY"></param>
	/// <param name="direction"></param>
	/// <param name="step"></param>
	/// <param name="thick"></param>
	public static void CreateStreet(int startX, int startY, int endX, int endY)
	{
		PlaceRectangleAreaOfBlock(startX, startY, endX, endY, TileID.GrayBrick);
		int x = startX + 1;
		int y = startY;
		while (x + 1 < endX)
		{
			float totalRare = 0;
			foreach (YggdrasilTownStreetElement element in StreetConstructorsSheet)
			{
				if (element.Width <= endX - (x + 1))
				{
					if (element.Cooling == 0)
					{
						totalRare += 1 / element.Rare;
					}
				}
			}
			if (totalRare == 0)
			{
				foreach (YggdrasilTownStreetElement element in StreetConstructorsSheet)
				{
					if (element.Cooling > 0)
					{
						element.Update();
					}
				}
				x++;
				continue;
			}
			float buildIndex = GenRand.NextFloat(totalRare);
			float rareIndex = 0;
			int width = 0;
			foreach (YggdrasilTownStreetElement element in StreetConstructorsSheet)
			{
				if (element.Width <= endX - (x + 1))
				{
					if (element.Cooling == 0)
					{
						rareIndex += 1 / element.Rare;
					}
					if (rareIndex - 1 / element.Rare < buildIndex && rareIndex >= buildIndex)
					{
						element.Build(ref x, y);
						width = element.Width;
						break;
					}
				}
			}
			foreach (YggdrasilTownStreetElement element in StreetConstructorsSheet)
			{
				if (element.Cooling == 0)
				{
					element.Update(width);
				}
			}
		}
	}
	/// <summary>
	/// 以ij为左上点放置青缎矿5x3
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	public static void PlaceLargeCyanVineOre(int i, int j)
	{
		switch (GenRand.Next(2))
		{
			case 0:
				for (int x = 0; x < 5; x++)
				{
					for (int y = 0; y < 3; y++)
					{
						var tile = SafeGetTile(i + x, j + y);
						if (x == 0 && y == 0)
							continue;
						if (x == 4 && y == 0)
							continue;
						if (x == 4 && y == 1)
							continue;
						if (x == 0 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.IsHalfBlock = true;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 4 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownLeft;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreLarge>();
							tile.TileFrameX = 36;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
						if (x == 0 && y == 2)
						{
							var tile2 = SafeGetTile(i + x - 1, j + y + 1);
							tile2.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
							tile2.HasTile = true;
						}
					}
				}
				break;
			case 1:
				for (int x = 1; x < 4; x++)
				{
					for (int y = 1; y < 3; y++)
					{
						var tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);
						if (x == 4 && y == 1)
							continue;
						if (x == 2 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreLarge>();
							tile.TileFrameX = 144;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.IsHalfBlock = true;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
		}
	}
	/// <summary>
	/// 以ij为左上点放置青缎矿4x3
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	public static void PlaceMiddleCyanVineOre(int i, int j)
	{
		switch (GenRand.Next(4))
		{
			case 0:
				for (int x = 0; x < 3; x++)
				{
					for (int y = 0; y < 3; y++)
					{
						var tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);
						if (x == 0 && y == 0)
							continue;
						if (x == 1 && y == 0)
							continue;
						if (x == 0 && y == 1)
							continue;
						if (x == 2 && y == 0)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownRight;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreMiddle>();
							tile.TileFrameX = 18;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 1:
				for (int x = 0; x < 3; x++)
				{
					for (int y = 1; y < 3; y++)
					{
						var tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);

						if (x == 0 && y == 1)
							continue;
						if (x == 2 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownLeft;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreMiddle>();
							tile.TileFrameX = 90;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 2:
				for (int x = 1; x < 4; x++)
				{
					for (int y = 1; y < 3; y++)
					{
						var tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);

						if (x == 3 && y == 1)
							continue;
						if (x == 1 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreMiddle>();
							tile.TileFrameX = 162;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 3:
				for (int x = 0; x < 3; x++)
				{
					for (int y = 1; y < 3; y++)
					{
						var tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);

						if (x == 0 && y == 1)
							continue;
						if (x == 0 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownRight;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 2 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.IsHalfBlock = true;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreMiddle>();
							tile.TileFrameX = 234;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
		}
	}
	/// <summary>
	/// 以ij为左上点放置青缎矿3x2
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	public static void PlaceSmallCyanVineOre(int i, int j)
	{
		switch (GenRand.Next(3))
		{
			case 0:
				for (int x = 0; x < 2; x++)
				{
					for (int y = 0; y < 2; y++)
					{
						var tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);
						if (x == 0 && y == 0)
							continue;
						if (x == 0 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmall>();
							tile.TileFrameX = 0;
							tile.TileFrameY = 36;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 1:
				for (int x = 0; x < 2; x++)
				{
					for (int y = 0; y < 2; y++)
					{
						var tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);

						if (x == 0 && y == 0)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownRight;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 0)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownLeft;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 0 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmall>();
							tile.TileFrameX = 54;
							tile.TileFrameY = 36;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 2:
				for (int x = 0; x < 3; x++)
				{
					for (int y = 0; y < 2; y++)
					{
						var tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);

						if (x == 0 && y == 0)
							continue;
						if (x == 2 && y == 0)
							continue;
						if (x == 2 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownLeft;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 0 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmall>();
							tile.Slope = SlopeType.SlopeDownRight;
							tile.TileFrameX = 108;
							tile.TileFrameY = 36;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
		}
	}
	/// <summary>
	/// 以ij为左上点放置青缎矿3x2
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	public static void PlaceSmallUpCyanVineOre(int i, int j)
	{
		switch (GenRand.Next(4))
		{
			case 0:
				{
					var tile = YggdrasilWorldGeneration.SafeGetTile(i + 1, j);
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmallUp>();
					tile.TileFrameX = 18;
					tile.TileFrameY = 0;
					tile.HasTile = true;

					var tileII = YggdrasilWorldGeneration.SafeGetTile(i + 1, j + 1);
					tileII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileII.TileFrameX = 18;
					tileII.TileFrameY = 18;
					tileII.Slope = SlopeType.SlopeUpRight;
					tileII.HasTile = true;
				}

				break;
			case 1:
				{
					var tile = YggdrasilWorldGeneration.SafeGetTile(i + 1, j);
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmallUp>();
					tile.TileFrameX = 72;
					tile.TileFrameY = 0;
					tile.HasTile = true;

					var tileII = YggdrasilWorldGeneration.SafeGetTile(i + 1, j + 1);
					tileII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileII.TileFrameX = 18;
					tileII.TileFrameY = 18;
					tileII.HasTile = true;
				}
				break;
			case 2:
				{
					var tile = YggdrasilWorldGeneration.SafeGetTile(i + 1, j);
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmallUp>();
					tile.TileFrameX = 126;
					tile.TileFrameY = 0;
					tile.HasTile = true;

					var tileII = YggdrasilWorldGeneration.SafeGetTile(i + 1, j + 1);
					tileII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileII.TileFrameX = 18;
					tileII.TileFrameY = 18;
					tileII.HasTile = true;

					var tileIII = YggdrasilWorldGeneration.SafeGetTile(i, j);
					tileIII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileIII.TileFrameX = 0;
					tileIII.TileFrameY = 0;
					tileIII.Slope = SlopeType.SlopeUpRight;
					tileIII.HasTile = true;

					var tileIV = YggdrasilWorldGeneration.SafeGetTile(i + 2, j);
					tileIV.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileIV.TileFrameX = 36;
					tileIV.TileFrameY = 0;
					tileIV.Slope = SlopeType.SlopeUpLeft;
					tileIV.HasTile = true;
				}
				break;
			case 3:
				{
					var tile = YggdrasilWorldGeneration.SafeGetTile(i + 1, j);
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmallUp>();
					tile.TileFrameX = 180;
					tile.TileFrameY = 0;
					tile.HasTile = true;

					var tileII = YggdrasilWorldGeneration.SafeGetTile(i + 1, j + 1);
					tileII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileII.TileFrameX = 18;
					tileII.TileFrameY = 18;
					tileII.Slope = SlopeType.SlopeUpRight;
					tileII.HasTile = true;

					var tileIII = YggdrasilWorldGeneration.SafeGetTile(i, j);
					tileIII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileIII.TileFrameX = 0;
					tileIII.TileFrameY = 0;
					tileIII.HasTile = true;

					var tileIV = YggdrasilWorldGeneration.SafeGetTile(i + 2, j);
					tileIV.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileIV.TileFrameX = 36;
					tileIV.TileFrameY = 0;
					tileIV.Slope = SlopeType.SlopeUpLeft;
					tileIV.HasTile = true;
				}
				break;
		}
	}
	/// <summary>
	/// 以ij为左上点放置青缎矿5x3
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	public static void PlaceLargeUpCyanVineOre(int i, int j)
	{
		for (int x = 1; x < 5; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				var tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);
				if (x == 1 && y == 2)
					continue;
				if (x == 3 && y == 2)
					continue;
				if (x == 4 && y == 2)
					continue;
				if (x == 1 && y == 1)
				{
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tile.Slope = SlopeType.SlopeUpRight;
					tile.TileFrameX = (short)(x * 18);
					tile.TileFrameY = (short)(y * 18);
					tile.HasTile = true;
					continue;
				}
				if (x == 4 && y == 1)
				{
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tile.Slope = SlopeType.SlopeUpLeft;
					tile.TileFrameX = (short)(x * 18);
					tile.TileFrameY = (short)(y * 18);
					tile.HasTile = true;
					continue;
				}
				if (x == 2 && y == 0)
				{
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreLargeUp>();
					tile.TileFrameX = 36;
					tile.TileFrameY = 0;
					tile.HasTile = true;
					continue;
				}
				tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
				tile.TileFrameX = (short)(x * 18);
				tile.TileFrameY = (short)(y * 18);
				tile.HasTile = true;
			}
		}
	}
}

