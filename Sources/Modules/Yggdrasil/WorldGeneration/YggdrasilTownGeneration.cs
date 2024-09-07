using Everglow.CagedDomain.Tiles;
using Everglow.Yggdrasil.Common.Blocks;
using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.CyanVine;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood.Furniture;
using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items;
using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Walls;
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

		PlaceRectangleAreaOfBlock(20, Main.maxTilesY - 350, 155, Main.maxTilesY, ModContent.TileType<StoneScaleWood>());
		PlaceRectangleAreaOfBlock(Main.maxTilesX - 155, Main.maxTilesY - 350, Main.maxTilesX - 20, Main.maxTilesY, ModContent.TileType<StoneScaleWood>());
		PlaceRectangleAreaOfBlock(0, Main.maxTilesY - 350, Main.maxTilesX, Main.maxTilesY, ModContent.TileType<StoneScaleWood>());

		PlaceRectangleAreaOfWall(20, Main.maxTilesY - 350, 155, Main.maxTilesY, ModContent.WallType<StoneDragonScaleWoodWall>());
		PlaceRectangleAreaOfWall(Main.maxTilesX - 155, Main.maxTilesY - 350, Main.maxTilesX - 20, Main.maxTilesY, ModContent.WallType<StoneDragonScaleWoodWall>());
		PlaceRectangleAreaOfWall(0, Main.maxTilesY - 300, Main.maxTilesX, Main.maxTilesY, ModContent.WallType<StoneDragonScaleWoodWall>());

		Main.statusText = "Filling Midnight Bayou With Mud...";
		BuildMidnightBayou();
		Main.statusText = "Giant Pillars...";
		BuildGiantYggdrasilPosts();

		Main.statusText = "Carving The Heavenly Portal...";

		// BuildHeavenlyPortal();
		BuildTwilightRelic();
		BuildLampWoodLand();
		BuildTwilightLand();

		Main.statusText = "LampWood Forest...";

		// Main.statusText = "Flooding The Azure Grotto...";
		// BuildAzureGrotto();
		// Main.statusText = "Digging The Tangled Submine...";
		// BuildTangledSubmine();
		// Main.statusText = "Another Side, The Fossilized Mine Road...";
		// BuildFossilizedMineRoad();
		BuildHeavenlyPortal();
		Main.statusText = "Constructing The Yggdrasil Town Below...";
		BuildTownBelow();
		BuildStoneCageOfChallenges();

		// Main.statusText = "Growing LampWoods...";
		// BuildLampWoodLand();
		// Main.statusText = "Constructing The Yggdrasil Town Upper...";
		// BuildTownUpper();
		// Main.statusText = "The Barrier To 2rd Floor Of Yggdrasil...";
		// BuildDuskfallBarrier();
		// Main.statusText = "The Stone Cage Of Challenges...";
		// BuildStoneCageOfChallenges();
		// Main.statusText = "Twilight Forest...";
		// BuildTwilightLand();
	}

	public static int[,] PerlinPixelR = new int[1024, 1024];
	public static int[,] PerlinPixelG = new int[1024, 1024];
	public static int[,] PerlinPixelB = new int[1024, 1024];
	public static int[,] PerlinPixel2 = new int[1024, 1024];
	public static int[,] CellPixel = new int[1024, 1024];
	public static int AzureGrottoCenterX;
	public static Vector2 TwilightRelicCenter = new Vector2(1050, 20000);
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
			new SmithyType(),
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
			new EmptyAnchoredTop(),
		};
	}

	/// <summary>
	/// 噪声信息获取
	/// </summary>
	public static void FillPerlinPixel()
	{
		var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Yggdrasil/WorldGeneration/Noise_II_rgb.bmp");
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
		Point center = new Point(1400, Main.maxTilesY - 360);
		int radious = 300;

		// 沼泽本体
		for (int x = (int)(center.X - radious * 1.5); x < (int)(center.X + radious * 1.5); x++)
		{
			for (int y = center.Y - 60; y < center.Y + radious; y++)
			{
				Tile tile = SafeGetTile(x, y);
				float color = PerlinPixel2[Math.Clamp((int)(x - center.X + radious * 1.5f) / 3, 0, 1024), Math.Clamp((y - center.Y + radious) / 2, 0, 1024)] / 255f;
				float distance = new Vector2((x - center.X) * 0.6667f, y - center.Y).Length() / 200f;
				if (color + distance > 1.5f)
				{
					tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
					tile.HasTile = true;
					if (color + distance > 1.6f)
					{
						tile.wall = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
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

		// 确定左右界限
		int deltaX = 40;
		if (AzureGrottoCenterX > 600)
		{
			deltaX = -40;
		}
		int leftBound = GenRand.Next(1080, 1100) + deltaX;
		int rightBound = GenRand.Next(1700, 1720) + deltaX;
		int startY = Main.maxTilesY - 368;
		KillRectangleAreaOfTile(leftBound, startY - 10, rightBound, startY);

		// 源晶塔
		PlaceFrameImportantTiles(1395, startY - 37, 8, 12, ModContent.TileType<OriginPylon>());
		for (int x = leftBound + 5; x < rightBound - 5; x++)
		{
			if (x % 20 == 0)
			{
				PlaceFrameImportantTiles(x, startY, 20, 1, ModContent.TileType<StoneBridgeTile>(), 0, 0);

				PlaceFrameImportantTiles(x, startY + 7, 20, 1, ModContent.TileType<StoneBridgeTile>(), 36, 36);
			}
		}
	}

	/// <summary>
	/// 巨型天穹石柱
	/// </summary>
	public static void BuildGiantYggdrasilPosts()
	{
		int x0CoordPerlin = GenRand.Next(1024);
		int y0CoordPerlin = GenRand.Next(1024);
		int upBound = Main.maxTilesY - 2080;

		// 7次循环
		for (int i = 2; i < 9; i++)
		{
			Vector2 point = new Vector2(GenRand.Next(-40, 40) + i * 200, Main.maxTilesY - 460);
			int radious = GenRand.Next(50, 60);
			if (i == 3 || i == 6)
			{
				radious = (int)(radious * 2.5);
			}
			Vector2 velocity = new Vector2(0, -6).RotatedBy(GenRand.NextFloat(-0.2f, 0.2f));

			// 魔法数字特殊处理
			if (i == 2)
			{
				velocity = new Vector2(0, -6).RotatedBy(-0.3);
			}
			if (i == 8)
			{
				velocity = new Vector2(0, -6).RotatedBy(0.3);
			}
			if (i == 4)
			{
				point.X += 100;
			}
			if (i == 5)
			{
				continue;
			}
			if (i == 6)
			{
				point.X += 100;
			}
			for (int step = 0; step < 400; step++)
			{
				point += velocity;
				if (SafeGetTile((int)(point + velocity * 30).X, (int)(point + velocity).Y).HasTile)
				{
					point -= velocity;
					velocity.X *= -1;
					point += velocity;
				}
				if (point.X < 50 || point.X > 1950)
				{
					point -= velocity;
					velocity.X *= -1;
					point += velocity;
				}
				if (point.Y < upBound)
				{
					break;
				}

				// 隧道制造洞穴
				if (step > 50)
				{
					Vector2 point2 = point - velocity * 24 + velocity.RotatedBy(GenRand.NextFloat(MathHelper.TwoPi)) * 16;
					WorldGen.digTunnel(point2.X, point2.Y, GenRand.NextFloat(-1, 1), GenRand.NextFloat(0, 1), GenRand.Next(27, (int)(radious * 1.1f)), GenRand.Next(3, (int)(radious * 0.14f)));
					if (radious < 60)
					{
						point2 = point - velocity * 24 + velocity.RotatedBy(GenRand.NextFloat(MathHelper.TwoPi)) * 6;
						WorldGen.digTunnel(point2.X, point2.Y, GenRand.NextFloat(-1, 1), GenRand.NextFloat(0, 1), GenRand.Next(27, (int)(radious * 2.4f)), GenRand.Next(3, (int)(radious * 0.14f)));
					}
				}

				// 通过圆面积累加堆砌出柱体
				for (int dx = -radious; dx <= radious; dx++)
				{
					for (int dy = -radious / 8; dy <= radious / 8; dy++)
					{
						Vector2 v0 = new Vector2(dx, dy);
						int x = (int)point.X + dx;
						int y = (int)point.Y + dy;

						float aValue = PerlinPixelB[(x + x0CoordPerlin) % 1024, (y + y0CoordPerlin) % 1024] / 8f;
						if (v0.Length() < radious - aValue)
						{
							Tile tile = SafeGetTile(x, y);
							tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
							if (v0.Length() < radious - aValue - 2)
							{
								tile.wall = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
							}
							tile.HasTile = true;
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// 登天石拱
	/// </summary>
	public static void BuildHeavenlyPortal()
	{
		Point center = new Point(1400, Main.maxTilesY - 360);
		int radious = 300;

		// 沼泽本体
		for (int x = (int)(center.X - radious * 1.5); x < (int)(center.X + radious * 1.5); x++)
		{
			for (int y = center.Y - radious; y < center.Y - 60; y++)
			{
				Tile tile = SafeGetTile(x, y);
				float color = PerlinPixel2[Math.Clamp((int)(x - center.X + radious * 1.5f) / 3, 0, 1024), Math.Clamp((y - center.Y + radious) / 2, 0, 1024)] / 255f;
				float distance = new Vector2((x - center.X) * 0.6667f, y - center.Y).Length() / 200f;
				if (color + distance is > 1.5f and < 2f)
				{
					tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
					tile.HasTile = true;
					if (color + distance > 1.6f)
					{
						tile.wall = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
					}
				}
				else if (color + distance < 1.5f)
				{
					tile.ClearEverything();
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
		int y0CoordPerlin = GenRand.Next(1024);
		int y1CoordPerlin = GenRand.Next(1024);

		for (int y = -30; y < height; y++)
		{
			float heightValue = y / (float)height;
			int width = (int)(Math.Pow(2, 8 * (heightValue - 0.9)) / 4d * maxWidth) + 25;
			for (int x = -width; x <= width; x++)
			{
				float thickValue = PerlinPixelG[(int)(x * 0.9f + maxWidth * 1f) % 1024, y0CoordPerlin] * 0.2f;
				float thickValueUp = PerlinPixelG[(int)(x * 0.9f + maxWidth * 1f) % 1024, y1CoordPerlin] * 0.08f;
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
				WorldGen.digTunnel(GenRand.NextFloat(AzureGrottoCenterX + 90, 1140), GenRand.NextFloat(11120, 11600), GenRand.NextFloat(-1, 1), GenRand.NextFloat(0, 1), GenRand.Next(27, 72), GenRand.Next(3, 7));
			}
			for (int x = 0; x < 30; x++)
			{
				WorldGen.digTunnel(GenRand.NextFloat(AzureGrottoCenterX + 200, 1140), GenRand.NextFloat(11020, 11240), GenRand.NextFloat(-1, 1), GenRand.NextFloat(0, 1), GenRand.Next(27, 72), GenRand.Next(3, 7));
			}
			for (int x = 0; x < 30; x++)
			{
				WorldGen.digTunnel(GenRand.NextFloat(AzureGrottoCenterX + 160, 1140), GenRand.NextFloat(11140, 11500), GenRand.NextFloat(-1, 1), GenRand.NextFloat(-1, 1), GenRand.Next(81, 144), GenRand.Next(8, 12));
			}
		}
		else
		{
			for (int x = 0; x < 110; x++)
			{
				WorldGen.digTunnel(GenRand.NextFloat(60, AzureGrottoCenterX - 90), GenRand.NextFloat(11120, 11600), GenRand.NextFloat(-1, 1), GenRand.NextFloat(0, 1), GenRand.Next(27, 72), GenRand.Next(3, 7));
			}
			for (int x = 0; x < 30; x++)
			{
				WorldGen.digTunnel(GenRand.NextFloat(60, AzureGrottoCenterX - 200), GenRand.NextFloat(11020, 11240), GenRand.NextFloat(-1, 1), GenRand.NextFloat(0, 1), GenRand.Next(27, 72), GenRand.Next(3, 7));
			}
			for (int x = 0; x < 30; x++)
			{
				WorldGen.digTunnel(GenRand.NextFloat(60, AzureGrottoCenterX - 160), GenRand.NextFloat(11140, 11500), GenRand.NextFloat(-1, 1), GenRand.NextFloat(-1, 1), GenRand.Next(81, 144), GenRand.Next(8, 12));
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
			for (int y = 11000; y < Main.maxTilesY; y++)
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
		int coordY = GenRand.Next(1024);
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
				if ((!SafeGetTile((int)probePos.X, (int)probePos.Y).HasTile && position.Y > 11451) || probePos.X > 1200 || probePos.X < 0)
				{
					rotatedTimes = 40;
					step *= -1;
					continue;
				}
				velocity = velocity.RotatedBy((PerlinPixelG[times % 1024, coordY] - 127.5) * 0.0002);
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
			if (SafeGetTile((int)probePosII.X, (int)probePosII.Y).TileType != ModContent.TileType<StoneScaleWood>() && y < 11451)
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
		QuickBuild(40, Main.maxTilesY - 400, "YggdrasilTown/MapIOs/YggdrasilTown_Town.mapio");
	}

	/// <summary>
	/// 灯木森林
	/// </summary>
	public static void BuildLampWoodLand()
	{
		int upBound = Main.maxTilesY - 1900;
		int bottomBound = Main.maxTilesY - 460;
		int countLamp = 0;
		List<Vector2> roomPositions = new List<Vector2>();
		for (int i = 0; i < 1000; i++)
		{
			int x0CoordPerlin = GenRand.Next(1024);
			int y0CoordPerlin = GenRand.Next(1024);

			// 随机取点
			int checkX = GenRand.Next(50, Main.maxTilesX - 49);
			int checkY = GenRand.Next(upBound, bottomBound);

			// 如果上下左右都有大于100的空间且不在中心遗迹区
			if (CheckSpaceWidth(checkX, checkY) > 100 && CheckSpaceDown(checkX, checkY) > 100 && CheckSpaceUp(checkX, checkY) > 100 && (new Vector2(checkX, checkY) - TwilightRelicCenter).Length() > 400)
			{
				// 计入一个森林平台数量
				countLamp++;
				int startX = Math.Max(0, checkX - CheckSpaceLeft(checkX, checkY) - 60);
				int endX = Math.Min(checkX + CheckSpaceRight(checkX, checkY) + 60, Main.maxTilesX);

				// 大致铺设一遍地形
				for (int x = startX; x <= endX; x++)
				{
					for (int y = checkY - 30; y <= checkY + 30; y++)
					{
						Tile tile = SafeGetTile(x, y);
						float addXValue = 0;
						float addYValue = PerlinPixelG[(x + x0CoordPerlin) % 1024, (y + y0CoordPerlin) % 1024] / 255f * 0.5f;
						if (tile.WallType == ModContent.WallType<StoneDragonScaleWoodWall>())
						{
							addXValue = EmbeddingDepthOfTileType(x, y, ModContent.TileType<StoneScaleWood>(), 10) / 20f;
						}
						float value = addYValue + Math.Abs(checkY - y) / 30f + addXValue;
						int type = ModContent.TileType<DarkForestSoil>();
						if (value > 0.9)
						{
							type = ModContent.TileType<DarkForestGrass>();
						}
						if (value > 1)
						{
							continue;
						}
						tile.TileType = (ushort)type;
						tile.HasTile = true;
						if (value < 0.94)
						{
							tile.wall = (ushort)ModContent.WallType<DarkForestSoilWall>();
						}
					}
				}

				// 精细铺设边角
				for (int x = startX - 10; x <= endX + 10; x++)
				{
					for (int y = checkY - 40; y <= checkY + 40; y++)
					{
						int depth = EmbeddingDepthOfTileType(x, y, ModContent.TileType<DarkForestGrass>());
						if (depth <= 4 && depth > 0)
						{
							if (TerrianSurfaceDiscontinuity(x, y, 4) > 0.30f)
							{
								for (int t = -7; t <= 7; t++)
								{
									Vector2 pos = new Vector2(x, y);
									Vector2 vel = TerrianSurfaceNormal(x, y, 4, ModContent.TileType<FemaleLampWood>()).RotatedBy(t / 10f);
									for (int u = 0; u < 5; u++)
									{
										pos += vel;

										Tile tile = SafeGetTile((int)pos.X, (int)pos.Y);
										tile.TileType = (ushort)ModContent.TileType<FemaleLampWood>();
										tile.HasTile = true;
									}
								}
							}
						}
					}
				}

				// 铺设物替换为草
				for (int x = startX - 20; x <= endX + 20; x++)
				{
					for (int y = checkY - 50; y <= checkY + 50; y++)
					{
						Tile tile = SafeGetTile(x, y);
						if (tile.TileType == (ushort)ModContent.TileType<FemaleLampWood>())
						{
							tile.TileType = (ushort)ModContent.TileType<DarkForestGrass>();
							tile.HasTile = true;
						}
					}
				}

				// 点缀岩石
				for (int x = startX - 10; x <= endX + 10; x++)
				{
					for (int y = checkY - 40; y <= checkY + 40; y++)
					{
						float aValue = PerlinPixelR[(x + x0CoordPerlin) % 1024, (y + y0CoordPerlin) % 1024] / 255f;
						float bValue = Math.Abs(checkY - y) / 120f - 0.1f + Math.Max(0, Math.Abs(checkX - x) / 120f - 0.6f);
						if (aValue + bValue < 0.2f)
						{
							Tile tile = SafeGetTile(x, y);
							tile.TileType = (ushort)ModContent.TileType<YggdrasilGrayRock>();
							tile.HasTile = true;
						}
					}
				}

				// 平坦化
				SmoothTile(startX - 10, checkY - 30, endX + 10, checkY - 25);
				SmoothTile(startX - 10, checkY + 25, endX + 10, checkY + 30);

				// 房子
				int countCell = 0;
				bool built = false;
				while (countCell < 100)
				{
					countCell++;
					int x = 10 + GenRand.Next(startX, endX);
					int halfWidth = GenRand.Next(10, 13);
					if (x - halfWidth < 50 || x + halfWidth > Main.maxTilesX - 50)
					{
						continue;
					}
					for (int y = checkY - 25; y > checkY - 30; y--)
					{
						int roomHeight = GenRand.Next(8, 10);
						int chestPosXI = GenRand.Next(-halfWidth + 2, halfWidth - 1);
						bool canBuild = true;
						int xj = 0;
						int yj = 0;

						for (int j = 0; j < 50; j++)
						{
							Tile topLeft = SafeGetTile(x + xj - halfWidth, y + yj - roomHeight);
							Tile topRight = SafeGetTile(x + xj + halfWidth, y + yj - roomHeight);
							Tile bottomLeft = SafeGetTile(x + xj - halfWidth, y + yj);
							Tile bottomRight = SafeGetTile(x + xj + halfWidth, y + yj);

							if (!topLeft.HasTile && !topRight.HasTile &&
								bottomLeft.HasTile && (bottomLeft.TileType == ModContent.TileType<DarkForestGrass>() || bottomLeft.TileType == ModContent.TileType<DarkForestSoil>()) &&
								bottomRight.HasTile && (bottomRight.TileType == ModContent.TileType<DarkForestGrass>() || bottomRight.TileType == ModContent.TileType<DarkForestSoil>()))
							{
								break;
							}
							else
							{
								yj += 1;
							}
							if (j == 49)
							{
								canBuild = false;
								break;
							}
						}
						foreach (Vector2 point in roomPositions)
						{
							if ((point - new Vector2(x, y + yj)).Length() < 100)
							{
								canBuild = false;
								break;
							}
						}
						if (canBuild)
						{
							built = true;
							roomPositions.Add(new Vector2(x, y + yj));
						}
						else
						{
							continue;
						}
						for (int xi = x - halfWidth; xi <= x + halfWidth; xi++)
						{
							for (int yi = y + yj - roomHeight; yi <= y + yj; yi++)
							{
								Tile tile = SafeGetTile(xi, yi);
								if (xi == x - halfWidth || xi == x + halfWidth || yi == y + yj || yi == y + yj - roomHeight)
								{
									tile.TileType = (ushort)ModContent.TileType<LampWood_Wood_Tile>();
									tile.HasTile = true;
								}
								else
								{
									tile.wall = (ushort)ModContent.WallType<LampWood_Wood_Wall>();
									tile.HasTile = false;
								}
							}
						}
						for (int xi = x - halfWidth; xi <= x + halfWidth; xi++)
						{
							for (int yi = y + yj - roomHeight; yi <= y + yj; yi++)
							{
								if (xi == x + chestPosXI && yi == y + yj - 1)
								{
									PlaceLampWoodBiomeChest(xi, yi);
								}
							}
						}
					}
					if (built)
					{
						break;
					}
				}

				// 罐子
				for (int x = startX; x <= endX; x++)
				{
					for (int y = checkY - 40; y <= checkY + 10; y++)
					{
						float valueG = PerlinPixelG[(int)(x * 2.24f) % 1024, (int)(y * 2.24) % 1024] / 255f;
						Tile tile0 = SafeGetTile(x, y);
						Tile tile1 = SafeGetTile(x + 1, y);
						Tile tile2 = SafeGetTile(x, y + 1);
						Tile tile3 = SafeGetTile(x + 1, y + 1);
						Tile tile4 = SafeGetTile(x, y - 1);
						Tile tile5 = SafeGetTile(x + 1, y - 1);

						// 罐子
						if (valueG > 0.4f)
						{
							if (!tile0.HasTile && !tile1.HasTile && !tile4.HasTile && !tile5.HasTile && tile2.HasTile && tile3.HasTile && tile2.Slope == SlopeType.Solid && (tile2.TileType == ModContent.TileType<DarkForestGrass>() || tile2.TileType == ModContent.TileType<DarkForestSoil>()) && tile3.Slope == SlopeType.Solid && (tile3.TileType == ModContent.TileType<DarkForestGrass>() || tile3.TileType == ModContent.TileType<DarkForestSoil>()))
							{
								if (Main.rand.NextBool(3))
								{
									if (Main.rand.NextBool(2))
									{
										PlaceFrameImportantTiles(x, y - 1, 2, 2, ModContent.TileType<LampWoodPot>(), Main.rand.Next(6) * 36);
									}
									else
									{
										PlaceFrameImportantTiles(x, y - 1, 2, 2, TileID.Pots, Main.rand.Next(3) * 36, 36);
									}
								}
							}
						}
					}
				}
				if (countLamp > 12)
				{
					break;
				}
			}
		}

		// int direction = 1;
		// int randX = AzureGrottoCenterX;
		// if (AzureGrottoCenterX > 600)
		// {
		// direction = -1;
		// randX = AzureGrottoCenterX;
		// }
		// int startY = 11400;
		// int countX = 0;
		// int coordX = GenRand.Next(100, 200) + 1024;
		// int coordY = GenRand.Next(100, 200) + 1024;
		////放置泥块,草块
		// while (countX < 600)
		// {
		// countX++;
		// int x = randX + countX * direction;
		// float yMin = 11040 - countX * countX / 2000f;
		// for(int y = startY + PerlinPixelG[(x + coordX) % 1024, 200] / 20; y > yMin;y--)
		// {
		// float valueR = PerlinPixelR[(int)(x + coordX) % 1024, (int)(y * 3.3 + coordY) % 1024] / 255f;
		// if(y < yMin + 100)
		// {
		// valueR -= (yMin - y) / 100f + 1;
		// }
		// Tile tile = SafeGetTile(x, y);
		// if(!tile.HasTile && tile.LiquidAmount<=0)
		// {
		// //边缘区域为草方块
		// if(valueR > 0.32f)
		// {
		// tile.TileType = (ushort)ModContent.TileType<DarkForestGrass>();
		// tile.HasTile = true;
		// }
		// //其他区域是泥
		// if (valueR > 0.45f)
		// {
		// tile.TileType = (ushort)ModContent.TileType<DarkForestSoil>();
		// tile.HasTile = true;
		// }
		// }
		// }
		// }
		// 放置含有宝箱的房间

		////放置罐子
		// countX = -100;
		// coordX = GenRand.Next(1024) + 1024;
		// coordY = GenRand.Next(1024) + 1024;
		// while (countX < 800)
		// {
		// countX++;
		// int x = randX + countX * direction;
		// float yMin = 11040 - countX * countX / 2000f;
		// for (int y = startY + 150; y > yMin; y--)
		// {
		// float valueG = PerlinPixelG[(int)(x * 0.24f + coordX) % 1024, (int)(y * 0.24 + coordY) % 1024] / 255f;
		// Tile tile0 = SafeGetTile(x, y);
		// Tile tile1 = SafeGetTile(x + 1, y);
		// Tile tile2 = SafeGetTile(x, y + 1);
		// Tile tile3 = SafeGetTile(x + 1, y + 1);
		// Tile tile4 = SafeGetTile(x, y - 1);
		// Tile tile5 = SafeGetTile(x + 1, y - 1);
		// //罐子
		// if(valueG > 0.4f)
		// {
		// if (!tile0.HasTile && !tile1.HasTile && !tile4.HasTile && !tile5.HasTile && tile2.HasTile && tile3.HasTile && tile2.Slope == SlopeType.Solid && (tile2.TileType == ModContent.TileType<DarkForestGrass>() || tile2.TileType == ModContent.TileType<DarkForestSoil>()) && tile3.Slope == SlopeType.Solid && (tile3.TileType == ModContent.TileType<DarkForestGrass>() || tile3.TileType == ModContent.TileType<DarkForestSoil>()))
		// {
		// if (Main.rand.NextBool(3))
		// {
		// if (Main.rand.NextBool(2))
		// {
		// PlaceFrameImportantTiles(x, y - 1, 2, 2, ModContent.TileType<LampWoodPot>(), Main.rand.Next(6) * 36);
		// }
		// else
		// {
		// PlaceFrameImportantTiles(x, y - 1, 2, 2, TileID.Pots, Main.rand.Next(3) * 36, 36);
		// }
		// }
		// }
		// }
		// }
		// }
		// int smoothX = randX;
		// if (AzureGrottoCenterX > 600)
		// {
		// smoothX = randX - 500;
		// }
		////平滑区间
		// SmoothTile(smoothX, 11000, smoothX + 500, 11250);
		// Point treeRotPos = new Point(200, 11000);
		// if(AzureGrottoCenterX < 600)
		// {
		// treeRotPos = new Point(1000, 11000);
		// }
		////大灯塔树母树
		// ShapeData shapeData = new ShapeData();
		// WorldUtils.Gen(treeRotPos, new Shapes.Circle(15, 15), new Actions.Blank().Output(shapeData));
		// Vector2 offset = new Vector2(0);
		// Vector2 offsetVel = new Vector2(0, -3);
		// Point treeHat = treeRotPos;
		// float width = 13f;
		// for (int i = 0;i < 75;i++)
		// {
		// offset += offsetVel;
		// offsetVel = offsetVel.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f));
		// offsetVel = offsetVel * 0.85f + new Vector2(i * i / 3000f * -direction, -3) * 0.15f;
		// width *= 0.98f;
		// WorldUtils.Gen(treeRotPos, new Shapes.Circle((int)width, (int)width), Actions.Chain(new GenAction[]{new Modifiers.Offset((int)offset.X, (int)offset.Y), new Actions.Blank().Output(shapeData)}));
		// if(i > 24)
		// {
		// if(Main.rand.NextBool(15))
		// {
		// Vector2 offset2 = offset;
		// float dir2 = (Main.rand.Next(2) - 0.5f) * 2;
		// Vector2 offsetVel2 = offsetVel.RotatedBy(Main.rand.NextFloat(-1.3f, -0.5f) * dir2);
		// Vector2 offsetVel2final = offsetVel.RotatedBy(Main.rand.NextFloat(-1.7f, -1.0f) * dir2);
		// float width2 =  width * 0.97f;
		// int length = 75 - i + Main.rand.Next(-11, 45);

		// for (int j = 0; j < length; j++)
		// {
		// offset2 += offsetVel2;
		// offsetVel2 = offsetVel2.RotatedBy(Main.rand.NextFloat(-0.04f, 0.04f));
		// offsetVel2 = offsetVel2 * 0.85f + offsetVel2final * 0.15f;
		// width2 *= 0.98f;
		// WorldUtils.Gen(treeRotPos, new Shapes.Circle((int)width2, (int)width2), Actions.Chain(new GenAction[] { new Modifiers.Offset((int)offset2.X, (int)offset2.Y), new Actions.Blank().Output(shapeData) }));
		// }
		// }
		// if(i == 53)
		// {
		// treeHat += new Point((int)offset.X, (int)offset.Y);
		// }
		// }
		// }

		// for(int i = -250;i < 251;i++)
		// {
		// for (int j = -100; j < 101; j++)
		// {
		// float length = new Vector2(i, j * 2.5f).Length() / 250f;
		// float valueG = PerlinPixelR[(int)(i * 0.24f + coordX + 140) % 1024, (int)(j * 1.3 + coordY + 140) % 1024] / 255f;
		// if(Main.rand.NextBool(15) && length + valueG < 1)
		// {
		// int x = treeHat.X + i;
		// int y = treeHat.Y + j;
		// Tile tile = SafeGetTile(x, y);
		// if(tile != null && !tile.HasTile)
		// {
		// tile.TileType = (ushort)ModContent.TileType<FemaleLampLeaves>();
		// tile.HasTile = true;
		// }
		// }
		// }
		// }
	}

	/// <summary>
	/// 上天穹镇
	/// </summary>
	public static void BuildTownUpper()
	{
		// for (int y = 10900; y < 11030; y++)
		// {
		// int randX = GenRand.Next(150, 1050);
		// int maxLeft = -120;
		// int maxRight = 120;
		// for (int x0 = 0; x0 < 90; x0++)
		// {
		// Tile tile = SafeGetTile(x0 + randX, y);
		// if (tile.HasTile || tile.LiquidAmount > 0)
		// {
		// maxRight = x0;
		// break;
		// }
		// }
		// for (int x0 = 0; x0 > -90; x0--)
		// {
		// Tile tile = SafeGetTile(x0 + randX, y);
		// if (tile.HasTile || tile.LiquidAmount > 0)
		// {
		// maxLeft = x0;
		// break;
		// }
		// }
		// if (maxLeft >= -5 || maxRight <= 5)
		// {
		// continue;
		// }
		// else
		// {
		// if (!GenRand.NextBool(6))
		// {
		// maxLeft = GenRand.Next(maxLeft, 0);
		// }
		// if (!GenRand.NextBool(6))
		// {
		// maxRight = GenRand.Next(0, maxRight);
		// }
		// for (int checkX = maxLeft; checkX < maxRight; checkX++)
		// {
		// for (int checkY = 8; checkY > -20; checkY--)
		// {
		// Tile tile = SafeGetTile(checkX + randX, checkY + y);
		// if ((tile.HasTile && tile.TileType == TileID.GrayBrick) || (tile.LiquidAmount > 0 && Math.Abs(checkY) < 4))
		// {
		// if (checkX < 0)
		// {
		// maxLeft = checkX + 12;
		// break;
		// }
		// if (checkX > 0)
		// {
		// maxRight = checkX - 12;
		// break;
		// }
		// }
		// }
		// }
		// if (maxRight - maxLeft < 10)
		// {
		// continue;
		// }
		// if (maxLeft >= -5 || maxRight <= 5)
		// {
		// continue;
		// }
		// CreateStreet(maxLeft + randX, y, maxRight + randX, y + 2);
		// if (GenRand.NextBool(3))
		// {
		// y -= 2;
		// }
		// else
		// {
		// y += GenRand.Next(8, 11);
		// }
		// }
		// }
	}

	/// <summary>
	/// 隐天玄壁
	/// </summary>
	public static void BuildDuskfallBarrier()
	{
		int centerY = 10700;
		int coordX = GenRand.Next(1024);
		int coordY = GenRand.Next(150, 612);
		for (int x = 0; x < 1200; x++)
		{
			for (int y = -150; y < 150; y++)
			{
				float thick = (x - 600) * (x - 600) / 2400f + 30f;
				thick += PerlinPixelR[(x + coordX) % 1024, (y + coordY) % 1024] / 25f;
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
		int buildX = Main.maxTilesX / 2 - 300;
		int buildY = Main.maxTilesY - 1960;
		int x0CoordPerlin = GenRand.Next(1024);
		int y0CoordPerlin = GenRand.Next(1024);

		// 清扫周围物块腾出空间
		for (int x = 0; x < 450; x += 30)
		{
			CircleTileWithRandomNoise(new Vector2(buildX + x, buildY), 90, -1, 20, true);
		}
		CircleTileWithRandomNoise(new Vector2(buildX + 480, buildY), 90, ModContent.TileType<StoneScaleWood>(), 30, true);

		// 第一层与第二层交界处的穹顶
		for (int x = 0; x <= Main.maxTilesX - 1; x += 1)
		{
			for (int y = Main.maxTilesY - 2400; y <= Main.maxTilesY - 1800; y += 1)
			{
				float aValue = PerlinPixelR[(int)Math.Abs((x * 4.3f + x0CoordPerlin) % 1024), (int)Math.Abs((y * 4.3f + y0CoordPerlin) % 1024)] / 255f;
				float bValue = MathF.Abs((y - Main.maxTilesY + 2100) / 100f);
				float cValue = (x - Main.maxTilesX / 2f) / (Main.maxTilesX / 2f);
				cValue *= cValue;
				if (bValue > cValue)
				{
					bValue = -1 + (bValue - cValue) * 4;
				}
				else
				{
					bValue = -1;
				}
				if (aValue + bValue < 0.8f)
				{
					Tile tile = SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
					tile.HasTile = true;
				}
			}
		}

		// 电梯间
		QuickBuild(buildX, buildY, "YggdrasilTown/MapIOs/LiftRoomOfChallengerHall40x22.mapio");
		int step2X = buildX + 40;
		int step2Y = buildY + 3;
		PlaceRectangleAreaOfBlock(step2X, step2Y, step2X + 480, step2Y + 9, ModContent.TileType<StoneScaleWood>());
		KillRectangleAreaOfTile(step2X, step2Y + 2, step2X + 480, step2Y + 7);

		// 上半部分石壁
		for (int x = step2X; x <= step2X + 480; x += 1)
		{
			for (int y = Main.maxTilesY - 2100; y <= step2Y; y += 1)
			{
				float aValue = PerlinPixelR[(int)Math.Abs((x * 4.3f + x0CoordPerlin) % 1024), (int)Math.Abs((y * 4.3f + y0CoordPerlin) % 1024)] / 255f;
				float bValue = MathF.Abs((y - step2Y) / 100f);
				float cValue = (x - step2X + 20) / 50f;
				cValue *= cValue;
				cValue = MathF.Pow(cValue, 0.9f);
				if (aValue + bValue < cValue)
				{
					Tile tile = SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
					tile.HasTile = true;
				}
			}
		}

		// 上半部分挖空
		for (int x = step2X; x <= step2X + 480; x += 1)
		{
			for (int y = Main.maxTilesY - 2200; y <= step2Y + 1; y += 1)
			{
				float aValue = PerlinPixelR[(int)Math.Abs((x * 4.3f + x0CoordPerlin) % 1024), (int)Math.Abs((y * 4.3f + y0CoordPerlin) % 1024)] / 255f;
				float bValue = MathF.Abs((y - step2Y) / 70f);
				float cValue = (x - step2X - 240) / 200f;
				cValue *= cValue;
				cValue = 1 - cValue;
				cValue = MathF.Pow(cValue, 0.9f);
				if (aValue * 0.3f + bValue < cValue)
				{
					Tile tile = SafeGetTile(x, y);
					tile.HasTile = false;
				}
			}
		}

		// 下半部分石壁
		for (int x = step2X; x <= step2X + 480; x += 1)
		{
			float aValue = PerlinPixelR[(int)Math.Abs((x * 4.3f + x0CoordPerlin) % 1024), y0CoordPerlin % 1024] / 255f;
			float aValueNext = PerlinPixelR[(int)Math.Abs(((x + 1) * 4.3f + x0CoordPerlin) % 1024), y0CoordPerlin % 1024] / 255f;
			float aValueBack = PerlinPixelR[(int)Math.Abs(((x - 1) * 4.3f + x0CoordPerlin) % 1024), y0CoordPerlin % 1024] / 255f;
			for (int y = step2Y + 7; y <= step2Y + 500; y += 1)
			{
				float bValue = PerlinPixelR[(int)Math.Abs((x * 4.3f + x0CoordPerlin) % 1024), (int)(y * 4.3f + y0CoordPerlin) % 1024] / 255f;
				int thick = y - step2Y - 7;
				float value = (x - step2X) / 480f;
				value *= value;
				value = MathF.Pow(value, 0.7f);
				if (thick > value * 300)
				{
					break;
				}
				if (thick > value * 240)
				{
					bValue += (thick - value * 240) / 60f;
				}
				Tile tile = SafeGetTile(x, y);
				if (tile.HasTile)
				{
					bValue += 1f;
				}
				if (tile.wall > 0)
				{
					bValue += 0.5f;
					if (x > step2X + 400)
					{
						bValue += (x - step2X - 400) / 80f;
					}
				}
				if (bValue < 1f)
				{
					tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
					tile.HasTile = true;
				}
			}
		}

		PlaceFrameImportantTiles(step2X + 236, step2Y - 3, 20, 10, ModContent.TileType<SquamousShellSeal>());
	}

	/// <summary>
	/// 暮光之地
	/// </summary>
	public static void BuildTwilightLand()
	{
		int upBound = Main.maxTilesY - 1700;
		int bottomBound = Main.maxTilesY - 660;
		int count = 0;
		List<Vector2> oldPoses = new List<Vector2>();

		// 匍匐暮光草甸
		for (int i = 0; i < 1000; i++)
		{
			int x = GenRand.Next(20, Main.maxTilesX - 19);
			int y = GenRand.Next(upBound, bottomBound);
			if ((new Vector2(x, y) - TwilightRelicCenter).Length() > 300)
			{
				CrawlCarpetOfTile(x, y, GenRand.Next(150, 450), 12, ModContent.TileType<TwilightGrassBlock>());
			}
		}

		// 随机取点300次，但是只生成俩地形
		for (int i = 0; i < 300; i++)
		{
			int x = GenRand.Next(20, Main.maxTilesX - 19);
			int y = GenRand.Next(upBound, bottomBound);
			if (EmbeddingWallDepth(x, y, 100) > 80)
			{
				Vector2 basePos = new Vector2(x, y);
				bool canBuild = true;
				foreach (Vector2 v in oldPoses)
				{
					// 空出中心并且与另外一个点保持距离
					if ((basePos - v).Length() < 500 || (basePos - TwilightRelicCenter).Length() < 700)
					{
						canBuild = false;
						break;
					}
				}
				if (!canBuild)
				{
					continue;
				}
				oldPoses.Add(basePos);
				count++;
				int range = GenRand.Next(180, 200);
				CircleTileWithRandomNoise(basePos, range, ModContent.TileType<StoneScaleWood>(), 30);
				CircleTileWithRandomNoise(basePos, range - 20, -1, 30, true);

				// 低于某个点位则填满泥土
				int x0CoordPerlin = GenRand.Next(1024);
				int y0CoordPerlin = GenRand.Next(1024);
				int radiusI = range - 15;
				for (int x0 = -radiusI; x0 <= radiusI; x0++)
				{
					for (int y0 = -radiusI; y0 <= radiusI; y0++)
					{
						Tile tile = SafeGetTile(basePos + new Vector2(x0, y0));
						float aValue = PerlinPixelR[Math.Abs((x0 + x0CoordPerlin) % 1024), Math.Abs((y0 + y0CoordPerlin) % 1024)] / 255f;
						if (new Vector2(x0, y0).Length() <= radiusI - aValue * 10)
						{
							if (y0 > radiusI * 0.5f + aValue * 5)
							{
								tile.TileType = (ushort)ModContent.TileType<TwilightGrassBlock>();
								tile.HasTile = true;
							}
							if (y0 > radiusI * 0.51f + aValue * 5)
							{
								tile.TileType = (ushort)ModContent.TileType<DarkForestSoil>();
								tile.HasTile = true;
								tile.wall = (ushort)ModContent.WallType<DarkForestSoilWall>();
							}
						}
					}
				}

				// 圆壳结构下面穿破
				for (int j = 0; j < 4; j++)
				{
					WorldGen.digTunnel(basePos.X, basePos.Y - range * 0.74f - j * 0.09f, GenRand.NextFloat(-0.2f, 0.2f), -1, GenRand.Next(127, 143), GenRand.Next(5, 8));
				}
				for (int j = 0; j < 6; j++)
				{
					WorldGen.digTunnel(basePos.X, basePos.Y + range * 0.44f + j * 0.09f, GenRand.NextFloat(-0.5f, 0.5f), 1, GenRand.Next(127, 143), GenRand.Next(5, 8));
				}
			}
			if (count > 1)
			{
				break;
			}
		}
	}

	/// <summary>
	/// 中心暮光之地附带遗迹
	/// </summary>
	public static void BuildTwilightRelic()
	{
		int range = GenRand.Next(330, 340);

		// 大轮廓
		CircleTileWithRandomNoise(TwilightRelicCenter, range, ModContent.TileType<StoneScaleWood>(), 30);
		CircleTileWithRandomNoise(TwilightRelicCenter, range - 40, -1, 30, true);

		// 低于某个点位则填满泥土
		int x0CoordPerlin = GenRand.Next(1024);
		int y0CoordPerlin = GenRand.Next(1024);
		int radiusI = range - 35;
		for (int x0 = -radiusI; x0 <= radiusI; x0++)
		{
			for (int y0 = -radiusI; y0 <= radiusI; y0++)
			{
				Tile tile = SafeGetTile(TwilightRelicCenter + new Vector2(x0, y0));
				float aValue = PerlinPixelR[Math.Abs((x0 + x0CoordPerlin) % 1024), Math.Abs((y0 + y0CoordPerlin) % 1024)] / 255f;
				if (new Vector2(x0, y0).Length() <= radiusI - aValue * 10)
				{
					if (y0 > radiusI * 0.5f + aValue * 5)
					{
						tile.TileType = (ushort)ModContent.TileType<TwilightGrassBlock>();
						tile.HasTile = true;
					}
					if (y0 > radiusI * 0.51f + aValue * 5)
					{
						tile.TileType = (ushort)ModContent.TileType<DarkForestSoil>();
						tile.HasTile = true;
						tile.wall = (ushort)ModContent.WallType<DarkForestSoilWall>();
					}
				}
			}
		}
		int centerX = (int)TwilightRelicCenter.X;
		int centerY = (int)TwilightRelicCenter.Y;

		// 中心建筑遗迹 150 * 300
		for (int x = centerX - 80; x <= centerX + 80; x += 1)
		{
			for (int y = centerY - 103; y <= centerY + 210; y += 1)
			{
				float aValue = PerlinPixelR[(int)Math.Abs((x * 4.3f + x0CoordPerlin) % 1024), (int)Math.Abs((y * 4.3f + y0CoordPerlin) % 1024)] / 255f;
				if (aValue < 0.4f)
				{
					Tile tile = SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<GreenRelicBrick>();
					tile.HasTile = true;
				}
			}
		}
		PlaceRectangleAreaOfBlock(centerX - 75, centerY - 100, centerX + 75, centerY + 200, ModContent.TileType<GreenRelicBrick>());
		PlaceRectangleAreaOfWall(centerX - 78, centerY - 98, centerX + 78, centerY + 204, ModContent.WallType<GreenRelicWall>());

		// 顶部方波
		for (int x = -75; x < 75; x += 20)
		{
			PlaceRectangleAreaOfBlock(centerX + x, centerY - 108, centerX + 10 + x, centerY - 100, ModContent.TileType<GreenRelicBrick>());
		}

		// 种树
		for (int x0 = -radiusI; x0 <= radiusI; x0++)
		{
			for (int y0 = -radiusI; y0 <= radiusI; y0++)
			{
				int height = Main.rand.Next(7, 60);
				Tile tile = SafeGetTile(TwilightRelicCenter + new Vector2(x0, y0));
				if (tile.TileType == ModContent.TileType<TwilightGrassBlock>())
				{
					if (Main.rand.NextBool(3) && TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(centerX + x0 - 3, centerY + y0, 8, height))
					{
						TreePlacer.BuildTwilightTree(centerX + x0, centerY + y0 - 1, height);
					}
				}
			}
		}

		// 房间矩阵？
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 12; y++)
			{
				int roomOriginX = centerX - 75 + x * 50 + 25;
				int roomOriginY = centerY - 100 + y * 25 + 12;
				KillRectangleAreaOfTile(roomOriginX - 18, roomOriginY - 9, roomOriginX + 18, roomOriginY + 9);
			}
		}
	}

	/// <summary>
	/// 建造一个灯柱
	/// </summary>
	public static void PlaceChineseStyleLampPost(int x, int y, int style, int ai0 = 0, int ai1 = 0)
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
					{
						break;
					}

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
					{
						break;
					}

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
					{
						break;
					}

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
					{
						break;
					}

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
					{
						break;
					}
			}
		}

		// 本体
		CreateBoxRoom(startX, startY, endX, endY, TileID.DynastyWood, WallID.Ebonwood, true);

		// 黄色的屋檐
		PlaceRectangleAreaOfBlock(startX - 1, startY, startX - 1, startY, ModContent.TileType<YellowDynastyShingles>());
		PlaceRectangleAreaOfBlock(endX + 1, startY, endX + 1, startY, ModContent.TileType<YellowDynastyShingles>());
		PlaceRectangleAreaOfBlock(startX - 2, startY + 1, startX - 1, startY + 1, ModContent.TileType<YellowDynastyShingles>());
		PlaceRectangleAreaOfBlock(endX + 1, startY + 1, endX + 2, startY + 1, ModContent.TileType<YellowDynastyShingles>());
		if (GenRand.NextBool(2))
		{
			PlaceRectangleAreaOfBlock(startX - 3, startY + 2, startX - 1, startY + 2, ModContent.TileType<YellowDynastyShingles>());
			PlaceRectangleAreaOfBlock(endX + 1, startY + 2, endX + 3, startY + 2, ModContent.TileType<YellowDynastyShingles>());
		}

		// 50%的概率侧面有灯笼
		if (GenRand.NextBool(2))
		{
			int type = ModContent.TileType<SideHangingLantern_Red>();
			if (GenRand.NextBool(3))
			{
				type = ModContent.TileType<SideHangingLantern_White>();
			}
			int placeY = startY;

			// 左侧
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

			// 右侧
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

		// 判定是否应该有门
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

		// 面积大于100的情况拆分房间
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

				// 横向分割需要加门
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

		// 随机获取一个放置平台的点位
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

		// Hanging Tiles
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
							tile.TileType = (ushort)boundTileType;
							tile.HasTile = true;
						}
					}
					else
					{
						if (tile.wall == 0)
						{
							tile.wall = (ushort)contentWallType;
						}
					}
				}
				else
				{
					if (x == startX || x == endX || y == startY || y == endY)
					{
						tile.TileType = (ushort)boundTileType;
						tile.HasTile = true;
					}
					else
					{
						tile.HasTile = false;
						tile.wall = (ushort)contentWallType;
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
			tile1.TileType = (ushort)type;
			if (direction == -1)
			{
				tile1.slope((byte)SlopeType.SlopeUpLeft);
			}
			else
			{
				tile1.slope((byte)SlopeType.SlopeUpRight);
			}
			tile2.HasTile = true;
			tile2.TileType = (ushort)type;
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
					if (y0 % (density * 2) == density - 1 && !SafeGetTile(x + direction, y - y0).HasTile)
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
						{
							continue;
						}

						if (x == 4 && y == 0)
						{
							continue;
						}

						if (x == 4 && y == 1)
						{
							continue;
						}

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
						{
							continue;
						}

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
						{
							continue;
						}

						if (x == 1 && y == 0)
						{
							continue;
						}

						if (x == 0 && y == 1)
						{
							continue;
						}

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
						{
							continue;
						}

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
						{
							continue;
						}

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
						{
							continue;
						}

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
						{
							continue;
						}

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
						{
							continue;
						}

						if (x == 2 && y == 0)
						{
							continue;
						}

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
				{
					continue;
				}

				if (x == 3 && y == 2)
				{
					continue;
				}

				if (x == 4 && y == 2)
				{
					continue;
				}

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

	/// <summary>
	/// 生成一个灯塔木森林环境专属的宝箱
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public static void PlaceLampWoodBiomeChest(int x, int y)
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				Tile tile = SafeGetTile(i + x, y - j);
				if (tile.HasTile)
				{
					tile.ClearEverything();
				}
			}
		}
		List<Item> chestContents = new List<Item>();
		int mainItem = WorldGen.genRand.Next(2);

		// 尽可能出现不同奖励
		switch (mainItem)
		{
			case 0:
				chestContents.Add(new Item(setDefaultsToType: ModContent.ItemType<AmberMagicOrb>(), 1));
				break;
			case 1:
				chestContents.Add(new Item(setDefaultsToType: ModContent.ItemType<YggdrasilAmberLaser>(), 1));
				break;
		}

		// 金币
		if (WorldGen.genRand.NextBool(5))
		{
			chestContents.Add(new Item(setDefaultsToType: ItemID.GoldCoin, WorldGen.genRand.Next(1, 3)));
		}

		// 绳子
		chestContents.Add(new Item(setDefaultsToType: ItemID.Rope, WorldGen.genRand.Next(70, 151)));

		// 药水
		int potionType = 1;
		switch (WorldGen.genRand.Next(5))
		{
			case 0:
				potionType = ItemID.WarmthPotion;
				break;
			case 1:
				potionType = ItemID.GillsPotion;
				break;
			case 2:
				potionType = ItemID.WaterWalkingPotion;
				break;
			case 3:
				potionType = ItemID.SpelunkerPotion;
				break;
			case 4:
				potionType = ItemID.MiningPotion;
				break;
		}
		chestContents.Add(new Item(setDefaultsToType: potionType, WorldGen.genRand.Next(1, 4)));

		// 冰雪火把
		// if (WorldGen.genRand.NextBool(2))
		// {
		// chestContents.Add(new Item(setDefaultsToType: ItemID.IceTorch, WorldGen.genRand.Next(40, 91)));
		// }
		// 荧光棒
		if (WorldGen.genRand.NextBool(2))
		{
			if (WorldGen.genRand.NextBool(5))
			{
				chestContents.Add(new Item(setDefaultsToType: ItemID.StickyGlowstick, WorldGen.genRand.Next(20, 61)));
			}
			else
			{
				chestContents.Add(new Item(setDefaultsToType: ItemID.Glowstick, WorldGen.genRand.Next(20, 61)));
			}
		}
		int type = ModContent.TileType<LampWood_Chest>();

		WorldGenMisc.PlaceChest(x, y, (ushort)type, chestContents);
	}
}