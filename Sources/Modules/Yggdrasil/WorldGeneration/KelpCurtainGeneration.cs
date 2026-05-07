using Everglow.Yggdrasil.Common.Tiles;
using Everglow.Yggdrasil.Common.Walls;
using Everglow.Yggdrasil.KelpCurtain;
using Everglow.Yggdrasil.KelpCurtain.Tiles;
using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;
using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.WaterDeliveryHoles;
using Everglow.Yggdrasil.KelpCurtain.Tiles.GeyserAirBuds;
using Everglow.Yggdrasil.KelpCurtain.Walls;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using Terraria.ObjectData;
using static Everglow.Commons.Utilities.TileUtils;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.WorldGeneration;

public class KelpCurtainGeneration
{
	/// <summary>
	/// Tile coord.
	/// </summary>
	public static int UnderWaterMazeTopY = -1;

	public static List<int> WaterDeliveryHoleTiles = new List<int>();

	public static void BuildKelpCurtain()
	{
		Initialize();
		Main.statusText = "Kelp Curtain Bark Cliff...";

		// UnforcablePlaceAreaOfTile(20, 9600, 155, 10650, ModContent.TileType<DragonScaleWood>());
		//

		// PlaceRectangleAreaOfWall(20, 9600, 155, 10650, ModContent.WallType<DragonScaleWoodWall>());
		UnforcablePlaceAreaOfTile(Main.maxTilesX - 125, (int)(Main.maxTilesY * 0.72f), Main.maxTilesX - 20, (int)(Main.maxTilesY * 0.9f), ModContent.TileType<DragonScaleWood>());
		PlaceRectangleAreaOfWall(Main.maxTilesX - 125, (int)(Main.maxTilesY * 0.72f), Main.maxTilesX - 20, (int)(Main.maxTilesY * 0.9f), ModContent.WallType<DragonScaleWoodWall>());
		BuildBoundOf23Stratum();
		BuildDeathJadeLake();
		BuildTunnelTo2ndStratum();
		BuildMossyCavesLow();
		BuildMossyCavesHigh();
		GreenTundra();
		ScarletGarden();
		MazeUnderLake();
		DragonPond();

		// BuildRainValley();
	}

	/// <summary>
	/// 初始化
	/// </summary>
	public static void Initialize()
	{
		WaterDeliveryHoleTiles = new List<int>();
		WaterDeliveryHoleTiles.Add(ModContent.TileType<WaterDeliveryHole>());
		WaterDeliveryHoleTiles.Add(ModContent.TileType<WaterDeliveryHole_V>());
		WaterDeliveryHoleTiles.Add(ModContent.TileType<WaterDeliveryHole_BottomLeft>());
		WaterDeliveryHoleTiles.Add(ModContent.TileType<WaterDeliveryHole_BottomRight>());
		WaterDeliveryHoleTiles.Add(ModContent.TileType<WaterDeliveryHole_TopLeft>());
		WaterDeliveryHoleTiles.Add(ModContent.TileType<WaterDeliveryHole_TopRight>());
	}

	/// <summary>
	/// 亡碧湖
	/// </summary>
	public static void BuildDeathJadeLake()
	{
		int startY = (int)(Main.maxTilesY * 0.85f);
		int startX = GenRand.Next(60, 70);
		startX += Main.maxTilesX / 2;
		while (startY < (int)(Main.maxTilesY * 0.9f))
		{
			startY++;
			Tile tile = TileUtils.SafeGetTile(startX, startY);
			if (tile.HasTile)
			{
				startY -= 20;
				break;
			}
		}
		int randY = GenRand.Next(512);
		int randX = GenRand.Next(512);
		int bankWidth = GenRand.Next(220, 240);
		int peakHeight = 0; // 记录一个连续的高度

		// Lakeshore
		for (int step = 0; step < bankWidth; step++)
		{
			int height = (int)(step * step / 270f + PerlinPixelB[(step + randX) % 512, randY] / 30f) - 24;
			for (int deltaY = 0; deltaY < step; deltaY++)
			{
				int x = startX + step;
				int y = startY - height;
				int count = 0;
				while (!TileUtils.SafeGetTile(x, y).HasTile)
				{
					Tile tile = TileUtils.SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<OldMoss>();
					tile.HasTile = true;
					count++;
					if (count > 300)
					{
						break;
					}
					y++;
				}
			}
			if (height > peakHeight)
			{
				peakHeight = height;
			}
		}
		int countX = 0;
		for (int step = startX + bankWidth; step < Main.maxTilesX - 20; step++)
		{
			countX++;
			int countY = -100;
			int curveY = (int)(20 + MathF.Pow((step - (startX + bankWidth)) / (float)Main.maxTilesX, 2.2f) * 2400);
			for (int y = startY - curveY - 100; y < Main.maxTilesY * 0.901f; y++)
			{
				countY++;
				int x = step;
				int type = ModContent.TileType<OldMoss>();
				int wallType = ModContent.WallType<OldMossWall>();
				float stoneValue = 0;
				if (countY <= 0)
				{
					stoneValue = countY / 12f;
				}
				if (countY is > 12 and < 48)
				{
					stoneValue = (countY - 12) / 12f;
				}
				if (countY >= 48)
				{
					stoneValue = 3f;
				}
				Vector2 transform = new Vector2(x, y).RotatedBy(0.55f);
				stoneValue += MeltingPixel[(int)MathF.Abs(transform.X) % 256, (int)MathF.Abs(transform.Y) % 256] / 175f;
				if (countY <= 0 && countX < 50)
				{
					stoneValue = -1;
				}

				if (stoneValue > 1)
				{
					type = ModContent.TileType<MossProneSandSoil>();
					wallType = ModContent.WallType<MossProneSandSoilWall>();
				}
				if (stoneValue > 2)
				{
					type = ModContent.TileType<YggdrasilGrayRock>();
					wallType = ModContent.WallType<MossProneSandSoilWall>();
				}
				if (!TileUtils.SafeGetTile(x, y).HasTile && stoneValue >= 0)
				{
					Tile tile = TileUtils.SafeGetTile(x, y);
					tile.TileType = (ushort)type;
					tile.HasTile = true;
					tile.WallType = (ushort)wallType;
				}
			}
		}

		// Ascending Road
		int lakePeakX = startX + bankWidth;
		randY = GenRand.Next(512);
		randX = GenRand.Next(512);
		for (int step = 0; step < 30; step++)
		{
			int thick = (int)((30 - step) * (30 - step) / 26d + PerlinPixelB[(step + randX) % 512, randY] / 30f);
			for (int deltaY = 0; deltaY < thick; deltaY++)
			{
				int x = lakePeakX + step;
				int y = startY - peakHeight + deltaY;
				Tile tile = TileUtils.SafeGetTile(x, y);
				tile.TileType = (ushort)ModContent.TileType<OldMoss>();
				tile.HasTile = true;
			}
		}

		// Lake water
		for (int x = 50; x <= startX + bankWidth; x++)
		{
			int y = startY - peakHeight + 7;
			int count = 0;
			while (!TileUtils.SafeGetTile(x, y).HasTile)
			{
				count++;
				if (count > 300)
				{
					break;
				}
				if (x > KelpCurtainBiome.FindClosestStratumBoundPointX(y))
				{
					Tile tile = TileUtils.SafeGetTile(x, y);
					tile.LiquidType = LiquidID.Water;
					tile.LiquidAmount = 255;
				}
				y++;
			}
		}
	}

	/// <summary>
	/// 1，2层分界
	/// </summary>
	public static void BuildTunnelTo2ndStratum()
	{
		var checkPos = (FindSquamousShellTopLeft() + new Point(250, 5)).ToVector2();
		var checkVel = new Vector2(6, 0);
		float radius = 7f;
		for (int t = 0; t < 20; t++)
		{
			CircleTile(checkPos, radius + GenRand.NextFloat(-1.5f, 1.5f), -1, true);
			checkPos += checkVel;
		}
		checkVel = new Vector2(0, -7);
		radius = 24f;
		for (int t = 0; t < 30; t++)
		{
			CircleTile(checkPos, radius + GenRand.NextFloat(-3.5f, 3.5f), -1, true);
			radius += 0.2f;
			checkPos += checkVel;
		}
		checkPos += new Vector2(-120, 210);
		checkVel = new Vector2(12, 0);
		for (int t = 0; t < 13; t++)
		{
			GenerateStalactite(checkPos + new Vector2(0, 10), 6, Main.rand.NextFloat(24, 30), ModContent.TileType<StoneScaleWood>());
			checkPos += checkVel;
		}
		checkPos.X -= 45;
		checkPos.Y -= 30;
		int deltaY = CheckSpaceDown((int)checkPos.X, (int)checkPos.Y);
		checkPos.Y += deltaY - 2;
		for (int x = (int)(checkPos.X - 4); x <= (int)(checkPos.X + 5); x++)
		{
			var tile = TileUtils.SafeGetTile(x, (int)(checkPos.Y + 2));
			tile.TileType = (ushort)ModContent.TileType<OldMoss>();
			tile.HasTile = true;
		}

		// Place Geyser Air Buds.
		TileUtils.PlaceFrameImportantTiles((int)checkPos.X, (int)checkPos.Y, 2, 2, ModContent.TileType<GeyserAirBudsPlatform>());
		for (int t = 1; t < 16; t++)
		{
			Vector2 addPos = new Vector2((t % 2 - 0.5f) * 30 + 10 + Main.rand.NextFloat(-3, 3), -t * 24 + 10);
			Vector2 topPos = checkPos + addPos;
			GenerateStalactite(topPos, 6, Main.rand.NextFloat(12, 16), ModContent.TileType<OldMoss>());
			topPos.Y -= 10;
			int deltaYTop = CheckSpaceDown((int)topPos.X, (int)topPos.Y);
			topPos.Y += deltaYTop - 2;
			for (int x = (int)(topPos.X - 1); x <= (int)(topPos.X + 2); x++)
			{
				var tile = TileUtils.SafeGetTile(x, (int)(topPos.Y + 2));
				tile.ClearEverything();
				tile.wall = (ushort)ModContent.WallType<OldMossWall>();
				tile.TileType = (ushort)ModContent.TileType<OldMoss>();
				tile.HasTile = true;
				tile.Slope = SlopeType.Solid;
			}
			TileUtils.PlaceFrameImportantTiles((int)topPos.X, (int)topPos.Y, 2, 2, ModContent.TileType<GeyserAirBudsPlatform>());
		}
	}

	public static Point FindSquamousShellTopLeft()
	{
		for (int x = 500; x <= Main.maxTilesX - 500; x++)
		{
			for (int y = (int)(Main.maxTilesY * 0.89f); y <= (int)(Main.maxTilesY * 0.96f); y++)
			{
				Tile tile = TileUtils.SafeGetTile(x, y);
				if (tile.TileType == ModContent.TileType<SquamousShellSeal>())
				{
					return new Point(x - tile.TileFrameX / 18, y - tile.TileFrameY / 18);
				}
			}
		}
		return new Point(0, 0);
	}

	/// <summary>
	/// 低海拔苔穴
	/// </summary>
	public static void BuildMossyCavesLow()
	{
		int times = 500;
		for (int t = 0; t < times; t++)
		{
			int x = GenRand.Next((int)(Main.maxTilesX * 0.76f), (int)(Main.maxTilesX * 0.93f));
			int y = GenRand.Next((int)(Main.maxTilesY * 0.892f), (int)(Main.maxTilesY * 0.899f));
			DigAMossyCaveLow(x, y, GenRand.NextFloat(7, 8), GenRand.NextFloat(16, 42));
		}
		for (int t = 0; t < times; t++)
		{
			int x = GenRand.Next((int)(Main.maxTilesX * 0.76f), (int)(Main.maxTilesX * 0.93f));
			int y = GenRand.Next((int)(Main.maxTilesY * 0.887f), (int)(Main.maxTilesY * 0.8984f));
			Point dir = new Point(GenRand.Next(-2, 12), GenRand.Next(14, 45));
			Point p0 = new Point(x, y);
			Point p1 = new Point(x, y) + dir;
			if (To100NearestBlockDistance(p0.X, p0.Y) > 3 && To100NearestBlockDistance(p1.X, p1.Y) > 3)
			{
				ConnectMossyTunnel(p0, p1, GenRand.NextFloat(8, 10));
			}
		}
	}

	/// <summary>
	/// 高海拔苔穴
	/// </summary>
	public static void BuildMossyCavesHigh()
	{
		for (int x = (int)(Main.maxTilesX * 0.75); x < Main.maxTilesX - 20; x++)
		{
			for (int y = (int)(Main.maxTilesY * 0.877); y < (int)(Main.maxTilesY * 0.9); y++)
			{
				Vector2 origVec = new Vector2(x, y);
				Vector2 decayCenter = new Vector2(Main.maxTilesX * 0.78f, Main.maxTilesY * 0.89f);
				float distanceToDecayCenter = (decayCenter - origVec).Length();
				Vector2 transform = origVec.RotatedBy(MathHelper.PiOver4);
				float decayValue = GetMeltingPixel((int)transform.X, (int)transform.Y);
				if (distanceToDecayCenter < 250)
				{
					decayValue += (250 - distanceToDecayCenter) / 120f;
				}
				if (y < Main.maxTilesY * 0.88)
				{
					float lerpValue = (float)(Main.maxTilesY * 0.88 - y) / 63f;
					decayValue = (float)Utils.Lerp(decayValue, 0.6f, lerpValue);
				}
				var tile = TileUtils.SafeGetTile(x, y);
				if (!tile.HasTile && tile.wall == 0)
				{
					if (decayValue is > 0.2f and < 1f)
					{
						tile.HasTile = false;
						tile.WallType = (ushort)ModContent.WallType<OldMossWall>();
					}
					if (decayValue is > 0.5f and < 0.7f)
					{
						tile.HasTile = true;
						tile.TileType = (ushort)ModContent.TileType<OldMoss>();
						tile.WallType = (ushort)ModContent.WallType<OldMossWall>();
					}
				}
			}
		}
		for (int x = (int)(Main.maxTilesX * 0.6); x <= (int)(Main.maxTilesX * 0.75); x++)
		{
			for (int y = (int)(Main.maxTilesY * 0.877); y < (int)(Main.maxTilesY * 0.884); y++)
			{
				Vector2 origVec = new Vector2(x, y);
				Vector2 transform = origVec.RotatedBy(MathHelper.PiOver4);
				float decayValue = GetMeltingPixel((int)transform.X, (int)transform.Y);
				Vector2 decayCenter = new Vector2(Main.maxTilesX * 0.78f, Main.maxTilesY * 0.89f);
				float distanceToDecayCenter = (decayCenter - origVec).Length();
				if (distanceToDecayCenter < 250)
				{
					decayValue += (250 - distanceToDecayCenter) / 120f;
				}
				if (x <= (int)(Main.maxTilesX * 0.7))
				{
					decayValue += (float)(Main.maxTilesX * 0.7f - x) / 200f;
				}
				if (y < Main.maxTilesY * 0.88)
				{
					float lerpValue = (float)(Main.maxTilesY * 0.88 - y) / 63f;
					decayValue = (float)Utils.Lerp(decayValue, 0.6f, lerpValue);
				}
				else
				{
					decayValue += (y - Main.maxTilesY * 0.88f) / 80f;
				}
				var tile = TileUtils.SafeGetTile(x, y);
				if (!tile.HasTile && tile.wall == 0)
				{
					if (decayValue is > 0.2f and < 1f)
					{
						tile.HasTile = false;
						tile.WallType = (ushort)ModContent.WallType<OldMossWall>();
					}
					if (decayValue is > 0.5f and < 0.7f)
					{
						tile.HasTile = true;
						tile.TileType = (ushort)ModContent.TileType<OldMoss>();
						tile.WallType = (ushort)ModContent.WallType<OldMossWall>();
					}
				}
			}
		}
	}

	/// <summary>
	/// 碧绿苔原
	/// </summary>
	public static void GreenTundra()
	{
		Vector2 checkPos = new Vector2(Main.maxTilesX * 0.25f, Main.maxTilesY * 0.88f);
		Vector2 vel = new Vector2(3, -1.5f);
		int maxStep = 500;
		float radius = 4f;
		for (int t = 0; t < maxStep; t++)
		{
			CircleTile(checkPos, radius, ModContent.TileType<OldMoss>());
			CircleWall(checkPos, radius - 1, ModContent.WallType<OldMossWall>());
			vel = vel * 0.98f + new Vector2(4, 0) * 0.02f;
			checkPos += vel;
			radius = radius * 0.99f + 34 * 0.01f;
			if (checkPos.X > Main.maxTilesX * 0.96f)
			{
				break;
			}
		}

		// maxStep = 60;
		// for (int t = 0; t < maxStep; t++)
		// {
		// Vector2 check = new Vector2(GenRand.NextFloat(Main.maxTilesX * 0.75f, Main.maxTilesX * 0.94f), GenRand.NextFloat(Main.maxTilesY * 0.874f, Main.maxTilesY * 0.878f));
		// DigTunnel(check.X, check.Y, GenRand.NextFloat(-1, 1), 1, GenRand.Next(12, 65), GenRand.Next(4, 8));
		// }
		for (int x = (int)(Main.maxTilesX * 0.75f); x <= (int)(Main.maxTilesX * 0.96f); x++)
		{
			int y = (int)(Main.maxTilesY * 0.8795f);
			if (To100NearestBlockDistance(x, y) >= 3)
			{
				int middleX = x - CheckSpaceLeft(x, y) + (CheckSpaceRight(x, y) + CheckSpaceLeft(x, y)) / 2;
				x = middleX;
				float width = (CheckSpaceRight(x, y) + CheckSpaceLeft(x, y)) / 2;
				width = Math.Min((CheckSpaceRight(x, y) + CheckSpaceLeft(x, y)) / 2, 7);
				DigGreenTundraTunnel(x, y, width, new Vector2(-1, -1).RotatedByRandom(0.3), 0);
				x += CheckSpaceLeft(x, y) + 3;
			}
		}
	}

	/// <summary>
	/// 碧绿苔原专用的挖隧道
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="width"></param>
	/// <param name="velocity"></param>
	/// <param name="depth"></param>
	public static void DigGreenTundraTunnel(int x, int y, float width, Vector2 velocity, int depth)
	{
		if (depth >= 3)
		{
			return;
		}
		Vector2 checkPos = new Vector2(x, y);
		Vector2 vel = velocity.NormalizeSafe() * 2.6f;
		int maxStep = 400;
		for (int t = 0; t < maxStep; t++)
		{
			CircleTile(checkPos, width, -1, true);
			checkPos += vel;
			vel = vel.RotatedBy((GetPerlinPixelR(checkPos.X, checkPos.Y) - 60f / 255f) * 0.06f);
			if (!TileUtils.SafeGetTile(checkPos + vel.NormalizeSafe() * (width + 2)).HasTile && t < maxStep - 5 && t > 20)
			{
				maxStep = t + 4;
			}
			if (checkPos.Y < (int)(Main.maxTilesY * 0.875f) || checkPos.Y > (int)(Main.maxTilesY * 0.88f))
			{
				return;
			}
		}
	}

	/// <summary>
	/// 森雨幽谷
	/// </summary>
	public static void BuildRainValley()
	{
		int startY = (int)(Main.maxTilesY * 0.85f);
		int randY = GenRand.Next(512);
		int randX = GenRand.Next(512);
		while (startY < (int)(Main.maxTilesY * 0.89f))
		{
			startY++;
			Tile tile = TileUtils.SafeGetTile(Main.maxTilesX / 2, startY);
			if (tile.HasTile)
			{
				break;
			}
		}
		startY -= 200;
		for (int y = startY; y > (int)(Main.maxTilesY * 0.80f); y--)
		{
			for (int x = Main.maxTilesX / 2; x <= Main.maxTilesX - 20; x++)
			{
				int dense = PerlinPixelB[(x / 4 + randX) % 512, (y + randY) % 512];
				if (dense > 160)
				{
					Tile tile = TileUtils.SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<OldMoss>();
					tile.HasTile = true;
				}
			}
		}
	}

	/// <summary>
	/// 绯红花园
	/// </summary>
	public static void ScarletGarden()
	{
		int lakeSurfaceY = (int)(Main.maxTilesY * 0.88);
		int lakeCenterX = (int)(Main.maxTilesX * 0.4);
		lakeSurfaceY += CheckWaterSurfaceDown(lakeCenterX, lakeSurfaceY);
		int xBoundLeft = (int)(Main.maxTilesX * 0.22f);
		int xBoundRight = (int)(Main.maxTilesX * 0.4f);
		for (int x = xBoundLeft; x < xBoundRight; x++)
		{
			int lakeBottomY = (int)(Main.maxTilesY * 0.88);
			lakeBottomY += CheckSpaceDown(lakeCenterX, lakeBottomY);
			float xLength = xBoundRight - xBoundLeft;
			float height = (MathF.Sin((x - xBoundLeft) / xLength * MathHelper.TwoPi - MathHelper.PiOver2) + 1) * 465f;
			float heightMax = lakeBottomY - lakeSurfaceY + GetPerlinPixelG(x, 15) * 16;
			height = MathF.Min(heightMax, height);
			for (int y = 0; y < height; y++)
			{
				var tile = TileUtils.SafeGetTile(x, lakeBottomY - y);
				tile.TileType = (ushort)ModContent.TileType<DarkLakeBottomMud>();
				tile.HasTile = true;
			}
		}
	}

	/// <summary>
	/// 水下迷宫
	/// </summary>
	public static void MazeUnderLake()
	{
		int lakeSurfaceY = (int)(Main.maxTilesY * 0.88);
		int lakeCenterX = (int)(Main.maxTilesX * 0.4);
		lakeSurfaceY += CheckWaterSurfaceDown(lakeCenterX, lakeSurfaceY);
		int xBoundLeft = (int)(Main.maxTilesX * 0.34f);
		int xBoundRight = (int)(Main.maxTilesX * 0.64f);
		lakeCenterX = (int)(Main.maxTilesX * 0.5);
		int lakeBottomYHalfX = (int)(Main.maxTilesY * 0.88);
		lakeBottomYHalfX += CheckSpaceDown(lakeCenterX, lakeBottomYHalfX);
		int yBoundTop = lakeSurfaceY + 45;
		UnderWaterMazeTopY = yBoundTop;
		int yBoundBottom = lakeBottomYHalfX + 15;

		// We need a bound of Yggdrasil Black Rock.
		List<Vector2> MazeBoundPolygon = new List<Vector2>();
		int distance_Center_Left = CheckSpaceLeft(lakeCenterX, yBoundTop);
		int distance_Center_Right = CheckSpaceRight(lakeCenterX, yBoundTop);
		MazeBoundPolygon.Add(new Vector2(lakeCenterX - distance_Center_Left, yBoundTop) * 16);
		for (int dx = lakeCenterX - distance_Center_Left + 5; dx < lakeCenterX + distance_Center_Right; dx += 10)
		{
			int depthOfLake = CheckSpaceDown(dx, yBoundTop);
			MazeBoundPolygon.Add(new Vector2(dx, yBoundTop + depthOfLake) * 16);
		}
		MazeBoundPolygon.Add(new Vector2(lakeCenterX + distance_Center_Right, yBoundTop) * 16);
		PlacePolygonBoundOfBlock(MazeBoundPolygon, ModContent.TileType<YggdrasilBlackRock>(), 40, (int)TileChangeState.Forceful);

		// Random seed Points
		List<Point> seeds = GenerateRandomSeeds(xBoundLeft - 30, xBoundRight + 30, yBoundTop - 30, yBoundBottom + 30, 180, 25);
		for (int x = xBoundLeft; x < xBoundRight; x++)
		{
			for (int y = yBoundTop; y < yBoundBottom; y++)
			{
				// Exist a projection. SeedMap is not TileMap.
				var tile = SafeGetTile(x, y);
				if (!tile.HasTile)
				{
					if (MazeUnderLake_IsEdgePoint(x, y, seeds))
					{
						tile.TileType = (ushort)ModContent.TileType<YggdrasilBlackRock>();
						tile.HasTile = true;
					}
					if (MazeUnderLake_IsEdgePoint(x, y, seeds, 1.8f))
					{
						tile.wall = (ushort)ModContent.WallType<YggdrasilBlackRockWall>();
					}
				}
			}
		}

		for (int x = xBoundLeft; x < xBoundRight; x++)
		{
			for (int y = yBoundTop - 10; y <= yBoundTop + 10; y++)
			{
				// Exist a projection. SeedMap is not TileMap.
				int value = y - yBoundTop;
				var tile = SafeGetTile(x, y);
				if (!tile.HasTile)
				{
					if (value > -GetPerlinPixelG(x, y) * 4f - 4f && value < GetPerlinPixelR(x, y) * 4f + 4f)
					{
						tile.TileType = (ushort)ModContent.TileType<YggdrasilBlackRock>();
						tile.HasTile = true;
						if (value > -GetPerlinPixelG(x, y) * 4f - 3f && value < GetPerlinPixelR(x, y) * 4f + 3f)
						{
							tile.wall = (ushort)ModContent.WallType<YggdrasilBlackRockWall>();
						}
					}
				}
			}
		}
		Dictionary<Point, List<Point>> holes = new Dictionary<Point, List<Point>>();
		Point mediumSeedPos = default;
		float minDisToCenter = new Vector2(xBoundRight - xBoundLeft, yBoundBottom - yBoundTop).Length();
		foreach (var pos in seeds)
		{
			float boundRange = 8;
			Vector2 center = new Vector2(xBoundLeft + xBoundRight, yBoundTop + yBoundBottom) * 0.5f;
			float toCenterDis = new Vector2(pos.X - center.X, pos.Y - center.Y).Length();
			if (toCenterDis < minDisToCenter)
			{
				minDisToCenter = toCenterDis;
				mediumSeedPos = pos;
			}
			if (pos.X >= xBoundLeft + boundRange && pos.X <= xBoundRight - boundRange && pos.Y <= yBoundBottom - boundRange && pos.Y >= yBoundTop + boundRange && !SafeGetTile(pos).HasTile)
			{
				MazeUnderLake_AddNewConnection(pos, holes, seeds);
			}
		}
		if (mediumSeedPos != default)
		{
			int maxStep = 10;
			for (int t = 0; t < maxStep; t++)
			{
				List<Point> connectedWithMediumSeeds = MazeUnderLake_GetAllConnectedPoints(holes, seeds, new List<Point>(), mediumSeedPos, 0);
				foreach (var pos in connectedWithMediumSeeds)
				{
					bool noTileWithin5x5 = true;
					for (int dx = -2; dx <= 2; dx++)
					{
						for (int dy = -2; dy <= 2; dy++)
						{
							Tile surroundTile = SafeGetTile(pos.X + dx, pos.Y + dy);
							if (surroundTile.HasTile)
							{
								noTileWithin5x5 = false;
								break;
							}
						}
						if (!noTileWithin5x5)
						{
							break;
						}
					}
					if (noTileWithin5x5)
					{
						MazeUnderLake_AddNewConnectionWithMediumNet(pos, holes, seeds, connectedWithMediumSeeds);
					}
				}
			}
		}
		foreach (var pos in seeds)
		{
			bool inTheArea = MathUtils.IsPointInPolygon(MazeBoundPolygon, pos.ToWorldCoordinates());
			if (!inTheArea)
			{
				continue;
			}
			bool flag0 = holes.ContainsKey(pos);
			if (flag0)
			{
				continue;
			}
			bool canFill = true;
			foreach (var des in seeds)
			{
				bool flag2 = holes.ContainsKey(des) && holes[des].Contains(pos);
				if (flag2)
				{
					canFill = false;
					break;
				}
			}
			if (canFill)
			{
				List<Point> cellArea = BFSContinueEmpty(pos, false, 1024);
				foreach (var cell_pos in cellArea)
				{
					MazeUnderLake_FillYggdrasilBlackRock(cell_pos.X, cell_pos.Y);
				}
			}
		}
		for (int x = xBoundLeft; x < xBoundRight; x += 5)
		{
			for (int y = yBoundTop; y <= yBoundBottom; y += 5)
			{
				List<Point> cellArea = BFSContinueEmpty(new Point(x, y), false, 25);
				if (cellArea.Count < 25 && cellArea.Count > 0)
				{
					foreach (var pos in cellArea)
					{
						MazeUnderLake_FillYggdrasilBlackRock(pos.X, pos.Y);
					}
				}
			}
		}
		foreach (var pos in seeds)
		{
			bool inTheArea = MathUtils.IsPointInPolygon(MazeBoundPolygon, pos.ToWorldCoordinates());
			if (!inTheArea)
			{
				continue;
			}

			List<Point> tiles = BFSContinueEmpty(pos, false, 1536, WaterDeliveryHoleTiles);
			MazeUnderLake_BuildDesolateRoom(tiles);
		}
	}

	public static void MazeUnderLake_BuildDesolateRoom(List<Point> tiles)
	{
		int maxY = 0;
		foreach (var pos in tiles)
		{
			maxY = Math.Max(maxY, pos.Y);
		}
		foreach (var pos in tiles)
		{
			Tile tile = SafeGetTile(pos);
			if (pos.Y > maxY - 7 + GetPerlinPixelB(pos.X, pos.Y) && !tile.HasTile)
			{
				tile.TileType = (ushort)ModContent.TileType<DarkLakeBottomMud>();
				tile.HasTile = true;
				PlaceWallAround(tile, (ushort)ModContent.WallType<DarkLakeBottomMudWall>(), true, false);
			}
			if (WaterDeliveryHoleTiles.Contains(tile.TileType))
			{
				if (tile == MazeUnderLake_WaterDeliveryHole_GetCenterTile(pos.X, pos.Y))
				{
					int dir = MazeUnderLake_WaterDeliveryHole_GetDirection(tile);
					Vector2 checkTilePos = tile.Center();
					Vector2 normal = new Vector2(8, 0).RotatedBy(MathHelper.PiOver4 * dir);
					checkTilePos += normal * 4;
					List<Vector2> shouldClearTilePos = new List<Vector2>();
					for (int k = 0; k < 32; k++)
					{
						checkTilePos += normal;
						shouldClearTilePos.Add(checkTilePos);
						if (SafeGetTile(checkTilePos.ToTileCoordinates()).TileType != ModContent.TileType<DarkLakeBottomMud>())
						{
							shouldClearTilePos.Add(checkTilePos + normal);
							shouldClearTilePos.Add(checkTilePos + normal * 2);
							shouldClearTilePos.Add(checkTilePos + normal * 3);
							Main.NewText(SafeGetTile(checkTilePos.ToTileCoordinates()).TileType);
							break;
						}
					}
					foreach (var corePos in shouldClearTilePos)
					{
						KillCircleAreaOfBlockWithRandomNoiseInCertainTypeOfTile(corePos.ToTileCoordinates(), 3, new List<int> { ModContent.TileType<DarkLakeBottomMud>(), ModContent.TileType<YggdrasilBlackRock>() });
					}
				}
			}
		}

		// foreach (var pos in tiles)
		// {
		// if (tile.Y() == maxY - 5)
		// {
		// if (GenRand.NextBool(18))
		// {
		// int height = GenRand.Next(1, 7);
		// for (int j = 0; j < height; j++)
		// {
		// var algeeTile = SafeGetTile(tile.X(), tile.Y() - j);
		// algeeTile.TileType = (ushort)ModContent.TileType<JadeLakeGreenAlgae>();
		// algeeTile.HasTile = true;
		// }
		// }
		// }
		// }
	}

	public static int MazeUnderLake_WaterDeliveryHole_GetDirection(Tile tile)
	{
		int targetType = tile.TileType;
		int dir = -1;
		int style = TileObjectData.GetTileStyle(tile);
		if (targetType == ModContent.TileType<WaterDeliveryHole>())
		{
			if (style == 0)
			{
				dir = 6;
			}
			else
			{
				dir = 2;
			}
		}
		if (targetType == ModContent.TileType<WaterDeliveryHole_V>())
		{
			if (style == 0)
			{
				dir = 4;
			}
			else
			{
				dir = 0;
			}
		}
		if (targetType == ModContent.TileType<WaterDeliveryHole_BottomRight>())
		{
			dir = 1;
		}
		if (targetType == ModContent.TileType<WaterDeliveryHole_BottomLeft>())
		{
			dir = 3;
		}
		if (targetType == ModContent.TileType<WaterDeliveryHole_TopLeft>())
		{
			dir = 5;
		}
		if (targetType == ModContent.TileType<WaterDeliveryHole_TopRight>())
		{
			dir = 7;
		}
		return dir;
	}

	public static Tile MazeUnderLake_WaterDeliveryHole_GetCenterTile(int i, int j)
	{
		Tile tile = TileUtils.SafeGetTile(i, j);
		int currentOffsetX = 0;
		int currentOffsetY = 0;
		bool fail = true;
		if (tile.TileType == ModContent.TileType<WaterDeliveryHole_V>())
		{
			currentOffsetX = -tile.TileFrameX / 18 + 1;
			if (tile.TileFrameX >= 36)
			{
				currentOffsetX = -(tile.TileFrameX % 36) / 18;
			}
			currentOffsetY = -tile.TileFrameY / 18 + 2;
			fail = false;
		}
		else if (tile.TileType == ModContent.TileType<WaterDeliveryHole>())
		{
			currentOffsetX = -(tile.TileFrameX % 90) / 18 + 2;
			currentOffsetY = -tile.TileFrameY / 18 + 1;
			fail = false;
		}
		else if (WaterDeliveryHoleTiles.Contains(tile.TileType))
		{
			int currentStyle = TileObjectData.GetTileStyle(tile);
			TileObjectData currentObjectData = TileObjectData.GetTileData(tile.TileType, currentStyle);
			currentOffsetX = -(tile.TileFrameX / 18 - currentStyle * currentObjectData.Width) + currentObjectData.Origin.X;
			currentOffsetY = -tile.TileFrameY / 18 + currentObjectData.Origin.Y;
			fail = false;
		}
		if (fail)
		{
			Main.NewText("Fail to access target", Color.Red);
		}
		return TileUtils.SafeGetTile(i + currentOffsetX, j + currentOffsetY);
	}

	public static void MazeUnderLake_AddNewConnectionWithMediumNet(Point pos, Dictionary<Point, List<Point>> holes, List<Point> seeds, List<Point> mediumNet, int count = 1)
	{
		int connectedCount = 0;
		if (holes.ContainsKey(pos))
		{
			connectedCount += holes[pos].Count;
		}
		List<Point> closeSeeds = new List<Point>();
		foreach (var otherPos in seeds)
		{
			if (otherPos != pos)
			{
				float dis = Vector2.Distance(new Vector2(pos.X, pos.Y), new Vector2(otherPos.X, otherPos.Y));
				Point des = otherPos;
				bool flag1 = holes.ContainsKey(pos) && holes[pos].Contains(des);
				bool flag2 = holes.ContainsKey(des) && holes[des].Contains(pos);
				bool flag3 = mediumNet.Contains(des);
				bool flag4 = des.Y < UnderWaterMazeTopY + 5;
				bool flag5 = BFSContinueEmpty(des, false, 10).Count < 10;
				bool flag6 = BFSContinueEmpty(pos, false, 10).Count < 10;
				if (dis < 50 && !flag1 && !flag2 && !flag3 && !flag4 && !flag5 && !flag6)
				{
					closeSeeds.Add(des);
				}
				if (flag2)
				{
					connectedCount++;
				}
			}
		}
		if (connectedCount >= 3)
		{
			return;
		}
		List<Point> connectedSeeds = new List<Point>();
		if (holes.ContainsKey(pos))
		{
			connectedSeeds = holes[pos];
		}
		for (int k = 0; k < count; k++)
		{
			if (closeSeeds.Count <= 0)
			{
				break;
			}
			int index = GenRand.Next(closeSeeds.Count);
			MazeUnderLake_ConnectDeliveryHole(pos.ToWorldCoordinates(), closeSeeds[index].ToWorldCoordinates());
			connectedSeeds.Add(closeSeeds[index]);
			closeSeeds.RemoveAt(index);
		}
		if (holes.ContainsKey(pos))
		{
			holes[pos] = connectedSeeds;
		}
		else
		{
			holes.Add(pos, connectedSeeds);
		}
	}

	public static void MazeUnderLake_AddNewConnection(Point pos, Dictionary<Point, List<Point>> holes, List<Point> seeds, int count = 1)
	{
		List<Point> closeSeeds = new List<Point>();
		foreach (var otherPos in seeds)
		{
			if (otherPos != pos)
			{
				float dis = Vector2.Distance(new Vector2(pos.X, pos.Y), new Vector2(otherPos.X, otherPos.Y));
				Point des = otherPos;
				bool flag1 = holes.ContainsKey(pos) && holes[pos].Contains(des);
				bool flag2 = holes.ContainsKey(des) && holes[des].Contains(pos);
				bool flag4 = des.Y < UnderWaterMazeTopY + 5;
				bool flag5 = BFSContinueEmpty(des, false, 10).Count < 10;
				bool flag6 = BFSContinueEmpty(pos, false, 10).Count < 10;
				if (dis < 33 && !flag1 && !flag2 && !flag4 && !flag5 && !flag6)
				{
					closeSeeds.Add(des);
				}
			}
		}
		List<Point> connectedSeeds = new List<Point>();
		if (holes.ContainsKey(pos))
		{
			connectedSeeds = holes[pos];
		}
		for (int k = 0; k < count; k++)
		{
			if (closeSeeds.Count <= 0)
			{
				break;
			}
			int index = GenRand.Next(closeSeeds.Count);
			MazeUnderLake_ConnectDeliveryHole(pos.ToWorldCoordinates(), closeSeeds[index].ToWorldCoordinates());
			connectedSeeds.Add(closeSeeds[index]);
			closeSeeds.RemoveAt(index);
		}
		if (holes.ContainsKey(pos))
		{
			holes[pos] = connectedSeeds;
		}
		else
		{
			holes.Add(pos, connectedSeeds);
		}
	}

	public static List<Point> MazeUnderLake_GetAllConnectedPoints(Dictionary<Point, List<Point>> connectMap, List<Point> seedMap, List<Point> checkedList, Point checkPos, int step)
	{
		List<Point> result = checkedList;
		if (step > 8)
		{
			return result;
		}
		List<Point> newPoint = new List<Point>();
		if (connectMap.ContainsKey(checkPos))
		{
			List<Point> subSeed = connectMap[checkPos];
			foreach (var sub in subSeed)
			{
				if (!result.Contains(sub) && !newPoint.Contains(sub))
				{
					newPoint.Add(sub);
				}
			}
		}
		foreach (var parentPos in connectMap.Keys)
		{
			if (connectMap[parentPos].Contains(checkPos))
			{
				if (!result.Contains(parentPos) && !newPoint.Contains(parentPos))
				{
					newPoint.Add(parentPos);
				}
			}
		}
		result.AddRange(newPoint);
		foreach (var point in newPoint)
		{
			List<Point> subNewPoint = MazeUnderLake_GetAllConnectedPoints(connectMap, seedMap, result, point, step + 1);
			foreach (var pos in subNewPoint)
			{
				if (!result.Contains(pos))
				{
					result.Add(pos);
				}
			}
		}
		return result;
	}

	public static void MazeUnderLake_ConnectDeliveryHole(Vector2 pos0, Vector2 pos1)
	{
		if (pos0 == pos1)
		{
			return;
		}
		Vector2 dir = (pos1 - pos0).SafeNormalize(Vector2.Zero);
		float rot = dir.ToRotation();

		int style = -1;
		if (MathF.Abs(rot) <= MathHelper.Pi * 1f / 8f)
		{
			style = 0;
		}
		else if (rot <= MathHelper.Pi * 3f / 8f && rot > MathHelper.Pi * 1f / 8f)
		{
			style = 1;
		}
		else if (rot <= MathHelper.Pi * 5f / 8f && rot > MathHelper.Pi * 3f / 8f)
		{
			style = 2;
		}
		else if (rot <= MathHelper.Pi * 7f / 8f && rot > MathHelper.Pi * 5f / 8f)
		{
			style = 3;
		}
		else if (MathF.Abs(rot) >= MathHelper.Pi * 7f / 8f)
		{
			style = 4;
		}
		else if (rot >= -MathHelper.Pi * 7f / 8f && rot < -MathHelper.Pi * 5f / 8f)
		{
			style = 5;
		}
		else if (rot >= -MathHelper.Pi * 5f / 8f && rot < -MathHelper.Pi * 3f / 8f)
		{
			style = 6;
		}
		else if (rot >= -MathHelper.Pi * 3f / 8f && rot < -MathHelper.Pi * 1f / 8f)
		{
			style = 7;
		}
		Vector2 checkPos = pos0;
		int penetrateState = 0;

		Point origin0 = default;
		Point origin1 = default;
		int penetrate1Count = 0;
		for (int t = 0; t < 400; t++)
		{
			if (t % 2 == 0)
			{
				checkPos.X += dir.X * 4f;
			}
			else
			{
				checkPos.Y += dir.Y * 4f;
			}
			Point tilePos = checkPos.ToTileCoordinates();
			if (penetrateState == 0)
			{
				if (MazeUnderLake_SpecialCollisionWithDeliveryHoleStyle(checkPos, style))
				{
					penetrateState = 1;
					origin0 = tilePos;
				}
			}

			if (penetrateState == 1)
			{ // Second delivery hole.
				penetrate1Count++;
				if (!MazeUnderLake_SpecialCollisionWithDeliveryHoleStyle(checkPos, style) && penetrate1Count >= 16)
				{
					origin1 = tilePos;
					break;
				}
			}
		}
		if (origin0 != default && origin1 != default)
		{
			WaterDeliveryHole_TopLeft waterDeliveryHole_TopLeft = TileLoader.GetTile(ModContent.TileType<WaterDeliveryHole_TopLeft>()) as WaterDeliveryHole_TopLeft;
			WaterDeliveryHole_BottomRight waterDeliveryHole_BottomRight = TileLoader.GetTile(ModContent.TileType<WaterDeliveryHole_BottomRight>()) as WaterDeliveryHole_BottomRight;
			WaterDeliveryHole_TopRight waterDeliveryHole_TopRight = TileLoader.GetTile(ModContent.TileType<WaterDeliveryHole_TopRight>()) as WaterDeliveryHole_TopRight;
			WaterDeliveryHole_BottomLeft waterDeliveryHole_BottomLeft = TileLoader.GetTile(ModContent.TileType<WaterDeliveryHole_BottomLeft>()) as WaterDeliveryHole_BottomLeft;
			switch (style)
			{
				case 0:
					PlaceFrameImportantTiles(origin0.X - 1, origin0.Y - 2, 2, 5, (ushort)ModContent.TileType<WaterDeliveryHole_V>(), 0, 0);
					PlaceFrameImportantTiles(origin1.X, origin1.Y - 2, 2, 5, (ushort)ModContent.TileType<WaterDeliveryHole_V>(), 36, 0);
					break;
				case 1:
					MazeUnderLake_CheckSuitableForSlopeHole(ref origin0, 3);
					waterDeliveryHole_TopLeft.PlaceAtTileObjectDataOrigin(origin0.X, origin0.Y);
					MazeUnderLake_CheckSuitableForSlopeHole(ref origin1, 1);
					waterDeliveryHole_BottomRight.PlaceAtTileObjectDataOrigin(origin1.X, origin1.Y);
					break;
				case 2:
					PlaceFrameImportantTiles(origin0.X - 2, origin0.Y - 1, 5, 2, (ushort)ModContent.TileType<WaterDeliveryHole>(), 0, 0);
					PlaceFrameImportantTiles(origin1.X - 2, origin1.Y, 5, 2, (ushort)ModContent.TileType<WaterDeliveryHole>(), 90, 0);
					break;
				case 3:
					MazeUnderLake_CheckSuitableForSlopeHole(ref origin0, 0);
					waterDeliveryHole_TopRight.PlaceAtTileObjectDataOrigin(origin0.X, origin0.Y);
					MazeUnderLake_CheckSuitableForSlopeHole(ref origin1, 2);
					waterDeliveryHole_BottomLeft.PlaceAtTileObjectDataOrigin(origin1.X, origin1.Y);
					break;
				case 4:
					PlaceFrameImportantTiles(origin0.X, origin0.Y - 2, 2, 5, (ushort)ModContent.TileType<WaterDeliveryHole_V>(), 36, 0);
					PlaceFrameImportantTiles(origin1.X - 1, origin1.Y - 2, 2, 5, (ushort)ModContent.TileType<WaterDeliveryHole_V>(), 0, 0);
					break;
				case 5:
					MazeUnderLake_CheckSuitableForSlopeHole(ref origin0, 1);
					waterDeliveryHole_BottomRight.PlaceAtTileObjectDataOrigin(origin0.X, origin0.Y);
					MazeUnderLake_CheckSuitableForSlopeHole(ref origin1, 3);
					waterDeliveryHole_TopLeft.PlaceAtTileObjectDataOrigin(origin1.X, origin1.Y);
					break;
				case 6:
					PlaceFrameImportantTiles(origin0.X - 2, origin0.Y, 5, 2, (ushort)ModContent.TileType<WaterDeliveryHole>(), 90, 0);
					PlaceFrameImportantTiles(origin1.X - 2, origin1.Y - 1, 5, 2, (ushort)ModContent.TileType<WaterDeliveryHole>(), 0, 0);
					break;
				case 7:
					MazeUnderLake_CheckSuitableForSlopeHole(ref origin0, 2);
					waterDeliveryHole_BottomLeft.PlaceAtTileObjectDataOrigin(origin0.X, origin0.Y);
					MazeUnderLake_CheckSuitableForSlopeHole(ref origin1, 0);
					waterDeliveryHole_TopRight.PlaceAtTileObjectDataOrigin(origin1.X, origin1.Y);
					break;
			}
			List<Vector2> polygon =
			[
				origin0.ToWorldCoordinates() + new Vector2(4, -32).RotatedBy(MathHelper.PiOver4 * style),
				origin1.ToWorldCoordinates() + new Vector2(0, -32).RotatedBy(MathHelper.PiOver4 * style),
				origin1.ToWorldCoordinates() + new Vector2(0, 32).RotatedBy(MathHelper.PiOver4 * style),
				origin0.ToWorldCoordinates() + new Vector2(4, 32).RotatedBy(MathHelper.PiOver4 * style),
			];
			PlacePolygonAreaOfBlock(polygon, ModContent.TileType<YggdrasilBlackRock>(), (int)TileChangeState.NoTileOnly);
		}
	}

	/// <summary>
	/// Check whether the position is suitable for placing a slope hole.
	/// </summary>
	/// <param name="oldPoint"></param>
	/// <param name="slopeType">0: TopRight 1: BottomRight 2: BottomLeft 3: TopLeft</param>
	/// <returns>A suitable point for placement.</returns>
	public static void MazeUnderLake_CheckSuitableForSlopeHole(ref Point oldPoint, int slopeType)
	{
		switch (slopeType)
		{
			case 0:
				for (int c = 0; c < 2; c++)
				{
					if (SafeGetTile(oldPoint.X - 1, oldPoint.Y).HasTile || SafeGetTile(oldPoint.X, oldPoint.Y + 1).HasTile)
					{
						if (c % 2 == 0)
						{
							oldPoint.X += 1;
						}
						else
						{
							oldPoint.Y -= 1;
						}
					}
				}
				break;
			case 1:
				for (int c = 0; c < 2; c++)
				{
					if (SafeGetTile(oldPoint.X - 1, oldPoint.Y).HasTile || SafeGetTile(oldPoint.X, oldPoint.Y - 1).HasTile)
					{
						if (c % 2 == 0)
						{
							oldPoint.X += 1;
						}
						else
						{
							oldPoint.Y += 1;
						}
					}
				}
				break;
			case 2:
				for (int c = 0; c < 2; c++)
				{
					if (SafeGetTile(oldPoint.X + 1, oldPoint.Y).HasTile || SafeGetTile(oldPoint.X, oldPoint.Y - 1).HasTile)
					{
						if (c % 2 == 0)
						{
							oldPoint.X -= 1;
						}
						else
						{
							oldPoint.Y += 1;
						}
					}
				}
				break;
			case 3:
				for (int c = 0; c < 2; c++)
				{
					if (SafeGetTile(oldPoint.X + 1, oldPoint.Y).HasTile || SafeGetTile(oldPoint.X, oldPoint.Y + 1).HasTile)
					{
						if (c % 2 == 0)
						{
							oldPoint.X -= 1;
						}
						else
						{
							oldPoint.Y -= 1;
						}
					}
				}
				break;
		}
	}

	public static bool MazeUnderLake_SpecialCollisionWithDeliveryHoleStyle(Vector2 checkPos, int style)
	{
		switch (style)
		{
			case 0:
				if (Collision.SolidCollision(checkPos - new Vector2(8, 40), 16, 80))
				{
					return true;
				}
				break;
			case 1:
				if (Collision.SolidCollision(checkPos - new Vector2(8), 16, 16) || Collision.SolidCollision(checkPos - new Vector2(8) + new Vector2(32, -32), 16, 16) || Collision.SolidCollision(checkPos - new Vector2(8) + new Vector2(-32, 32), 16, 16))
				{
					return true;
				}
				break;
			case 2:
				if (Collision.SolidCollision(checkPos - new Vector2(40, 8), 80, 16))
				{
					return true;
				}
				break;
			case 3:
				if (Collision.SolidCollision(checkPos - new Vector2(8), 16, 16) || Collision.SolidCollision(checkPos - new Vector2(8) + new Vector2(-32, -32), 16, 16) || Collision.SolidCollision(checkPos - new Vector2(8) + new Vector2(32, 32), 16, 16))
				{
					return true;
				}
				break;
			case 4:
				if (Collision.SolidCollision(checkPos - new Vector2(8, 40), 16, 80))
				{
					return true;
				}
				break;
			case 5:
				if (Collision.SolidCollision(checkPos - new Vector2(8), 16, 16) || Collision.SolidCollision(checkPos - new Vector2(8) + new Vector2(32, -32), 16, 16) || Collision.SolidCollision(checkPos - new Vector2(8) + new Vector2(-32, 32), 16, 16))
				{
					return true;
				}
				break;
			case 6:
				if (Collision.SolidCollision(checkPos - new Vector2(40, 8), 80, 16))
				{
					return true;
				}
				break;
			case 7:
				if (Collision.SolidCollision(checkPos - new Vector2(8), 16, 16) || Collision.SolidCollision(checkPos - new Vector2(8) + new Vector2(-32, -32), 16, 16) || Collision.SolidCollision(checkPos - new Vector2(8) + new Vector2(32, 32), 16, 16))
				{
					return true;
				}
				break;
		}
		return false;
	}

	public static void MazeUnderLake_FillYggdrasilBlackRock(int i, int j)
	{
		Tile tile = SafeGetTile(i, j);
		if (!tile.HasTile)
		{
			tile.TileType = (ushort)ModContent.TileType<YggdrasilBlackRock>();
			tile.HasTile = true;
		}
	}

	/// <summary>
	/// 龙潭
	/// </summary>
	public static void DragonPond()
	{
		int sandLayerTopY = (int)(Main.maxTilesY * 0.893f);
		int sandLayerBottomY = (int)(Main.maxTilesY * 0.9f);
		int leftX = (int)(Main.maxTilesX * 0.08f);
		int RightX = (int)(Main.maxTilesX * 0.26f);
		for (int x = leftX; x <= RightX; x++)
		{
			float xDuration = Math.Abs((x - leftX) / (float)(RightX - leftX));
			float deltaY = Main.maxTilesY * 0.003f * MathF.Pow(xDuration, 0.5f);
			sandLayerTopY = (int)(Main.maxTilesY * 0.893f + deltaY);
			for (int y = sandLayerBottomY; y >= sandLayerTopY; y--)
			{
				Tile tile = TileUtils.SafeGetTile(x, y);
				if (!tile.HasTile)
				{
					tile.TileType = (ushort)ModContent.TileType<DecaySandSoil>();
					tile.HasTile = true;
				}
				if (y > sandLayerTopY + 3)
				{
					tile.wall = (ushort)ModContent.WallType<DecaySandSoilWall>();
				}
			}
		}
		sandLayerTopY = (int)(Main.maxTilesY * 0.893f);
		SmoothTile(leftX, sandLayerTopY, RightX, sandLayerBottomY);
		for (int x = leftX; x <= RightX; x++)
		{
			for (int y = sandLayerTopY - 10; y < sandLayerBottomY; y++)
			{
				Tile tile = TileUtils.SafeGetTile(x, y);
				if (tile.HasTile)
				{
					if (GenRand.NextBool(2) && tile.Slope == SlopeType.Solid)
					{
						for (int algeaY = 0; algeaY < 90; algeaY++)
						{
							if (Main.rand.Next(90) < algeaY)
							{
								break;
							}
							Tile algea = TileUtils.SafeGetTile(x, y - algeaY - 1);
							if (!algea.HasTile)
							{
								algea.TileType = (ushort)ModContent.TileType<JadeLakeSargassum>();
								algea.HasTile = true;
							}
						}
					}
					break;
				}
			}
		}
	}

	/// <summary>
	/// 水下宝库
	/// </summary>
	public static void UnderwaterTreasury()
	{
	}

	/// <summary>
	/// Build a mossy cave(horizental cave, long and flat), with moss side, radius at lease = 10.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="height"></param>
	/// <param name="radius"></param>
	public static void DigAMossyCaveLow(int i, int j, float height, float radius)
	{
		Tile firstCheck = TileUtils.SafeGetTile(i, j);
		if (!firstCheck.HasTile || EmbeddingDepth(i, j, 10) < 10)
		{
			return;
		}
		float hValue = 1f;

		// Right Cave
		int maxStepRight = (int)(radius + 3);
		for (int step = 0; step < maxStepRight; step++)
		{
			if (step > maxStepRight - 13)
			{
				hValue = (maxStepRight - 3 - step) / 10f;
			}
			int x = i + step;
			if (maxStepRight == (int)(radius + 3))
			{
				int xCheck = x + 13;
				float hCheck = 1f;
				if (step + 13 > maxStepRight - 10)
				{
					hCheck = (maxStepRight - step - 13) / 10f;
				}
				for (int h = (int)(-height * hCheck); h <= height * hCheck; h++)
				{
					int y = j + h;
					Tile tile = TileUtils.SafeGetTile(xCheck, y);
					if (!tile.HasTile)
					{
						maxStepRight = step + 13;
						break;
					}
				}
			}
			for (int h = (int)(-height * hValue - 3); h <= height * hValue + 3; h++)
			{
				int y = j + h;
				Tile tile = TileUtils.SafeGetTile(x, y);
				if (MathF.Abs(h) < height * hValue)
				{
					tile.wall = (ushort)ModContent.WallType<OldMossWall>();
					tile.HasTile = false;
				}
				else
				{
					tile.HasTile = true;
					tile.TileType = (ushort)ModContent.TileType<OldMoss>();
					tile.wall = (ushort)ModContent.WallType<OldMossWall>();
				}
			}
		}
		hValue = 1f;

		// Left Cave
		int maxStepLeft = (int)(radius + 3);
		for (int step = 0; step < maxStepLeft + 3; step++)
		{
			if (step > maxStepLeft - 13)
			{
				hValue = (maxStepLeft - 3 - step) / 10f;
			}
			int x = i - step;
			if (maxStepLeft == (int)(radius + 3))
			{
				int xCheck = x - 13;
				float hCheck = 1f;
				if (step + 13 > maxStepLeft - 10)
				{
					hCheck = (maxStepLeft - step - 13) / 10f;
				}
				for (int h = (int)(-height * hCheck); h <= height * hCheck; h++)
				{
					int y = j + h;
					Tile tile = TileUtils.SafeGetTile(xCheck, y);
					if (!tile.HasTile)
					{
						maxStepLeft = step + 13;
						break;
					}
				}
			}
			for (int h = (int)(-height * hValue - 3); h <= height * hValue + 3; h++)
			{
				int y = j + h;
				Tile tile = TileUtils.SafeGetTile(x, y);
				if (MathF.Abs(h) < height * hValue)
				{
					tile.wall = (ushort)ModContent.WallType<OldMossWall>();
					tile.HasTile = false;
				}
				else
				{
					tile.HasTile = true;
					tile.TileType = (ushort)ModContent.TileType<OldMoss>();
					tile.wall = (ushort)ModContent.WallType<OldMossWall>();
				}
			}
		}
	}

	/// <summary>
	/// Create a tunnel with moss side between 2 points.
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="width"></param>
	public static void ConnectMossyTunnel(int x0, int y0, int x1, int y1, float width)
	{
		int maxStep = (int)(new Vector2(x1, y1) - new Vector2(x0, y0)).Length();
		Vector2 dir = Vector2.Normalize(new Vector2(x1, y1) - new Vector2(x0, y0));
		Vector2 checkPoint = new Vector2(x0, y0);
		float halfWidth = width / 2f;
		for (int s = 0; s < maxStep; s++)
		{
			checkPoint += dir;
			for (int x = (int)(-halfWidth); x < halfWidth; x++)
			{
				for (int y = (int)(-halfWidth); y < halfWidth; y++)
				{
					Vector2 checkDir = new Vector2(x, y);
					bool shouldKill = false;
					if (checkDir.Length() < 3f)
					{
						shouldKill = true;
					}
					else
					{
						Vector2 normalCheckDir = checkDir.NormalizeSafe();
						if (MathF.Abs(Vector2.Dot(normalCheckDir, dir)) <= 0.15f)
						{
							shouldKill = true;
						}
					}
					if (shouldKill)
					{
						Tile tile = TileUtils.SafeGetTile((int)(x + checkPoint.X), (int)(y + checkPoint.Y));
						if (tile.HasTile)
						{
							if (checkDir.Length() < halfWidth - 1)
							{
								tile.HasTile = false;
								tile.wall = (ushort)ModContent.WallType<OldMossWall>();
							}
							else
							{
								tile.HasTile = true;
								tile.TileType = (ushort)ModContent.TileType<OldMoss>();
								tile.wall = (ushort)ModContent.WallType<OldMossWall>();
							}
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Create a tunnel with moss side between 2 points.
	/// </summary>
	/// <param name="p0"></param>
	/// <param name="p1"></param>
	/// <param name="width"></param>
	public static void ConnectMossyTunnel(Point p0, Point p1, float width)
	{
		ConnectMossyTunnel(p0.X, p0.Y, p1.X, p1.Y, width);
	}

	/// <summary>
	/// True if the point is close to multiple seeds, which means it's likely to be at the edge of the Voronoi cell and suitable for placing delivery holes.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="seeds"></param>
	/// <param name="edgeThreshold"></param>
	/// <returns></returns>
	public static bool MazeUnderLake_IsEdgePoint(int x, int y, List<Point> seeds, float edgeThreshold = 2.5f)
	{
		// 计算到所有种子点的距离平方（避免开方运算，提高效率）
		List<(int SeedIndex, long DistanceSquared)> distances = new List<(int, long)>();

		for (int i = 0; i < seeds.Count; i++)
		{
			long dx = x - seeds[i].X;
			long dy = y - seeds[i].Y;
			long distSq = dx * dx + dy * dy; // 距离平方
			distances.Add((i, distSq));
		}

		// 排序获取最近的两个种子点
		distances.Sort((a, b) => a.DistanceSquared.CompareTo(b.DistanceSquared));

		// 如果最近两个种子点的距离差小于阈值，则视为边缘
		// 阈值可调整：值越小边缘越细，值越大边缘越粗
		double minDist = Math.Sqrt(distances[0].DistanceSquared);
		double secondMinDist = Math.Sqrt(distances[1].DistanceSquared);

		return (secondMinDist - minDist) < edgeThreshold;
	}

	public static int CheckWaterSurfaceDown(int x, int y)
	{
		int count = 0;
		int x0 = x;
		int y0 = y;
		if (y0 > Main.maxTilesY || y0 < 0)
		{
			return count;
		}
		while (!TileUtils.SafeGetTile(x0, y0).HasTile && TileUtils.SafeGetTile(x0, y0).LiquidAmount <= 0)
		{
			if (y0 > Main.maxTilesY)
			{
				break;
			}
			y0++;
			count++;
		}
		return count;
	}

	/// <summary>
	/// 刺苔庭园/朽木王庭
	/// </summary>
	public static void MattedMossCourt()
	{
	}

	/// <summary>
	/// 2，3层分界
	/// </summary>
	public static void BuildBoundOf23Stratum()
	{
		int startY = (int)(Main.maxTilesY * 0.75);
		Vector2 checkPos = new Vector2(Main.maxTilesX - 20, startY);
		Vector2 checkVel = new Vector2(0, 16);
		float omega = 0f;
		bool joint = false;
		for (int step = 0; step < 264; step++)
		{
			if (checkPos.Y <= Main.maxTilesY * 0.91)
			{
				CircleTile(checkPos, 30, ModContent.TileType<DragonScaleWood>());
				CircleWall(checkPos, 28, ModContent.WallType<DragonScaleWoodWall>());
			}
			KelpCurtainBiome.StratumBoundCurve.Add(checkPos.ToPoint());
			checkPos += checkVel;
			checkVel = checkVel.RotatedBy(omega);
			if (!joint)
			{
				omega += 0.00013f;
				if (omega > 0.02f)
				{
					joint = true;
				}
			}
			else
			{
				if (omega > -0.018f)
				{
					omega -= 0.006f;
				}
			}
		}
	}

	public static void UnforcablePlaceAreaOfTile(int x0, int y0, int x1, int y1, int type)
	{
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile tile = TileUtils.SafeGetTile(x, y);
				if (!tile.HasTile)
				{
					tile.TileType = (ushort)type;
					tile.HasTile = true;
				}
			}
		}
	}
}