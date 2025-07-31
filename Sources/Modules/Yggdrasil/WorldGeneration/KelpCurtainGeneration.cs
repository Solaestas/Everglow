using Everglow.Yggdrasil.Common.Blocks;
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
		BuildMossyCavesLow();
		BuildMossyCavesHigh();
		GreenTundra();

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
				if (!SafeGetTile(x, y).HasTile && stoneValue >= 0)
				{
					Tile tile = SafeGetTile(x, y);
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

		// Place Geyser Air Buds.
		PlaceFrameImportantTiles((int)checkPos.X, (int)checkPos.Y, 2, 2, ModContent.TileType<GeyserAirBudsPlatform>());
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
				var tile = SafeGetTile(x, (int)(topPos.Y + 2));
				tile.ClearEverything();
				tile.wall = (ushort)ModContent.WallType<OldMossWall>();
				tile.TileType = (ushort)ModContent.TileType<OldMoss>();
				tile.HasTile = true;
				tile.Slope = SlopeType.Solid;
			}
			PlaceFrameImportantTiles((int)topPos.X, (int)topPos.Y, 2, 2, ModContent.TileType<GeyserAirBudsPlatform>());
		}
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
	/// Build a mossy cave(horizental cave, long and flat), with moss side, radius at lease = 10.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="height"></param>
	/// <param name="radius"></param>
	public static void DigAMossyCaveLow(int i, int j, float height, float radius)
	{
		Tile firstCheck = SafeGetTile(i, j);
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
					Tile tile = SafeGetTile(xCheck, y);
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
				Tile tile = SafeGetTile(x, y);
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
					Tile tile = SafeGetTile(xCheck, y);
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
				Tile tile = SafeGetTile(x, y);
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
						Tile tile = SafeGetTile((int)(x + checkPoint.X), (int)(y + checkPoint.Y));
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
				var tile = SafeGetTile(x, y);
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
				var tile = SafeGetTile(x, y);
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
			if (!SafeGetTile(checkPos + vel.NormalizeSafe() * (width + 2)).HasTile && t < maxStep - 5 && t > 20)
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