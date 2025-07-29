using Everglow.Yggdrasil.KelpCurtain;
using Everglow.Yggdrasil.KelpCurtain.Tiles;
using Everglow.Yggdrasil.KelpCurtain.Tiles.GeyserAirBuds;
using Everglow.Yggdrasil.KelpCurtain.Walls;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.WorldGeneration;

public class KelpCurtainGeneration
{
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

		// BuildRainValley();
	}

	/// <summary>
	/// 初始化
	/// </summary>
	public static void Initialize()
	{
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
			Tile tile = SafeGetTile(startX, startY);
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
				while (!SafeGetTile(x, y).HasTile)
				{
					Tile tile = SafeGetTile(x, y);
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
		for (int step = startX + bankWidth; step < Main.maxTilesX - 20; step++)
		{
			for (int y = startY - 20; y < Main.maxTilesY * 0.9; y++)
			{
				int x = step;
				if (!SafeGetTile(x, y).HasTile)
				{
					Tile tile = SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<OldMoss>();
					tile.HasTile = true;
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
				Tile tile = SafeGetTile(x, y);
				tile.TileType = (ushort)ModContent.TileType<OldMoss>();
				tile.HasTile = true;
			}
		}

		// Lake water
		for (int x = 50; x <= startX + bankWidth; x++)
		{
			int y = startY - peakHeight + 7;
			int count = 0;
			while (!SafeGetTile(x, y).HasTile)
			{
				count++;
				if (count > 300)
				{
					break;
				}
				if (x > KelpCurtainBiome.FindClosestStratumBoundPointX(y))
				{
					Tile tile = SafeGetTile(x, y);
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
			var tile = SafeGetTile(x, (int)(checkPos.Y + 2));
			tile.TileType = (ushort)ModContent.TileType<OldMoss>();
			tile.HasTile = true;
		}
		PlaceFrameImportantTiles((int)checkPos.X, (int)checkPos.Y, 2, 2, ModContent.TileType<GeyserAirBudsPlatform>());
		for (int t = 1; t < 13; t++)
		{
			Vector2 addPos = new Vector2((t % 2 - 0.5f) * 20, -t * 30 + 10);
			Vector2 topPos = checkPos + addPos;
			GenerateStalactite(topPos, 6, Main.rand.NextFloat(12, 16), ModContent.TileType<OldMoss>());
			topPos.Y -= 10;
			int deltaYTop = CheckSpaceDown((int)checkPos.X, (int)checkPos.Y);
			topPos.Y += deltaYTop - 2;
			for (int x = (int)(topPos.X - 1); x <= (int)(topPos.X + 2); x++)
			{
				var tile = SafeGetTile(x, (int)(checkPos.Y + 2));
				tile.TileType = (ushort)ModContent.TileType<OldMoss>();
				tile.HasTile = true;
			}
			PlaceFrameImportantTiles((int)topPos.X, (int)topPos.Y, 2, 2, ModContent.TileType<GeyserAirBudsPlatform>());
		}
	}

	public static Point FindSquamousShellTopLeft()
	{
		for (int x = 500; x <= Main.maxTilesX - 500; x++)
		{
			for (int y = (int)(Main.maxTilesY * 0.89f); y <= (int)(Main.maxTilesY * 0.96f); y++)
			{
				Tile tile = SafeGetTile(x, y);
				if (tile.TileType == ModContent.TileType<SquamousShellSeal>())
				{
					return new Point(x - tile.TileFrameX / 18, y - tile.TileFrameY / 18);
				}
			}
		}
		return new Point(0, 0);
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
			Tile tile = SafeGetTile(Main.maxTilesX / 2, startY);
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
					Tile tile = SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<OldMoss>();
					tile.HasTile = true;
				}
			}
		}
	}

	/// <summary>
	/// 刺苔庭园/朽木王庭
	/// </summary>
	public static void MattedMossCourt()
	{
	}

	/// <summary>
	/// 碧绿苔原
	/// </summary>
	public static void GreenTundra()
	{
	}

	/// <summary>
	/// 绯红花园
	/// </summary>
	public static void ScarletGarden()
	{
	}

	/// <summary>
	/// 水下迷宫
	/// </summary>
	public static void MazeUnderLake()
	{
	}

	/// <summary>
	/// 森雨幽谷
	/// </summary>
	public static void RainValley()
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
				Tile tile = SafeGetTile(x, y);
				if (!tile.HasTile)
				{
					tile.TileType = (ushort)type;
					tile.HasTile = true;
				}
			}
		}
	}
}