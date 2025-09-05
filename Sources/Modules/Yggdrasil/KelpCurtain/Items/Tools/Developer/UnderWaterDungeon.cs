using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;
using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.IRProbe;
using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.UnderwaterGuillotine;
using Everglow.Yggdrasil.KelpCurtain.Walls;
using Everglow.Yggdrasil.WorldGeneration;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Tools.Developer;

public class UnderWaterDungeon : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 30;
		Item.useTurn = true;
		Item.useAnimation = 4;
		Item.useTime = 4;
		Item.autoReuse = false;
		Item.useStyle = ItemUseStyleID.Swing;
	}

	public Point OldMousePos = default;

	public override void HoldItem(Player player) => base.HoldItem(player);

	public override bool CanUseItem(Player player)
	{
		Point point = Main.MouseWorld.ToTileCoordinates();
		GenerateUnderWaterDungeon(point.X, point.Y);
		return false;
	}

	public void GenerateUnderWaterDungeon(int i, int j)
	{
		KillRectangleAreaOfTile(i - 800, j - 170, i + 80, j + 70);
		KillRectangleAreaOfWall(i - 800, j - 170, i + 80, j + 70);
		KillRectangleAreaOfLiquid(i - 800, j - 170, i + 80, j + 70);
		KillRectangleAreaOfWire(i - 800, j - 170, i + 80, j + 70);
		int cellHeight = 22;
		int cellWidth = 34;
		int cellDistance = 80;
		int tunnelHeight = 16;
		for (int xRoom = 0; xRoom < 2; xRoom++)
		{
			BrickRoom(i - xRoom * cellDistance, j, cellWidth, cellHeight);
			Point irPos = new Point(i - xRoom * cellDistance - cellWidth + 9, j);
			Tile irProbeTile = SafeGetTile(irPos);
			irProbeTile.TileType = (ushort)ModContent.TileType<IRProbe_Normal>();
			irProbeTile.TileFrameX = 36;
			irProbeTile.HasTile = true;

			Point underwaterGuillotinePos = new Point(i - xRoom * cellDistance - 4, j - cellHeight + 9);
			UnderwaterGuillotine underwaterGuillotine = TileLoader.GetTile(ModContent.TileType<UnderwaterGuillotine>()) as UnderwaterGuillotine;
			if (underwaterGuillotine is not null)
			{
				underwaterGuillotine.PlaceOriginAtTopLeft(underwaterGuillotinePos.X, underwaterGuillotinePos.Y);
			}
			ConnectWire(underwaterGuillotinePos, irPos);
		}
		for (int xRoom = 0; xRoom < 2; xRoom++)
		{
			if (xRoom < 3)
			{
				int y = j + (cellHeight - tunnelHeight / 2) - 3;
				ConnectWaterErodedBrickTunnel(i - xRoom * cellDistance - (cellWidth - 7), y, i - xRoom * cellDistance - cellDistance + (cellWidth - 7), y, tunnelHeight);
			}
		}
		BrickRoom(i - 2 * cellDistance - 20, j - 120, cellWidth + 30, cellHeight);
		ConnectWaterErodedBrickTunnel_Serrated(new Point(i - 2 * cellDistance - 40, j - 94), new Point(i - 1 * cellDistance - 38, j + 9), 12, 8);

	}

	public void ConnectWire(Point point0, Point point1, bool red = true, bool green = false, bool blue = false, bool yellow = false)
	{
		int distanceX = Math.Abs((point1 - point0).X);
		int distanceY = Math.Abs((point1 - point0).Y);
		Point check = point0;
		int dirX = 1;
		int dirY = 1;
		if (point1.X < point0.X)
		{
			dirX = -1;
		}
		if (point1.Y < point0.Y)
		{
			dirY = -1;
		}
		for (int y = 0; y < distanceY; y++)
		{
			Tile tile = SafeGetTile(check);
			tile.RedWire = red;
			tile.GreenWire = green;
			tile.BlueWire = blue;
			tile.YellowWire = yellow;
			check.Y += dirY;
		}
		for (int x = 0; x <= distanceX; x++)
		{
			Tile tile = SafeGetTile(check);
			tile.RedWire = red;
			tile.GreenWire = green;
			tile.BlueWire = blue;
			tile.YellowWire = yellow;
			check.X += dirX;
		}
	}

	public void BrickRoom(int centerX, int centerY, int halfWidth, int halfHeight)
	{
		int thick = 8;
		for (int x = centerX - halfWidth; x <= centerX + halfWidth; x++)
		{
			for (int y = centerY - halfHeight; y <= centerY + halfHeight; y++)
			{
				Tile tile = SafeGetTile(x, y);
				int boundValue = halfWidth - Math.Abs(x - centerX);
				int boundValue2 = halfHeight - Math.Abs(y - centerY);
				boundValue = Math.Min(boundValue, boundValue2);
				if (boundValue <= thick)
				{
					tile.TileType = (ushort)ModContent.TileType<WaterErodedGreenBrick>();
					tile.HasTile = true;
				}
				if (boundValue > thick)
				{
					tile.HasTile = false;
					if (boundValue > 1)
					{
						tile.wall = (ushort)ModContent.WallType<WaterErodedGreenBrickWall>();
						tile.liquid = (byte)LiquidID.Water;
						tile.LiquidAmount = 255;
					}
				}
			}
		}
	}

	/// <summary>
	/// Create a tunnel with water-erodeed brick side between 2 points.
	/// </summary>
	/// <param name="p0"></param>
	/// <param name="p1"></param>
	/// <param name="width"></param>
	public static void ConnectWaterErodedBrickTunnel(Point p0, Point p1, float width)
	{
		ConnectWaterErodedBrickTunnel(p0.X, p0.Y, p1.X, p1.Y, width);
	}

	/// <summary>
	/// Create a tunnel with water-erodeed brick side between 2 points.
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="width"></param>
	public static void ConnectWaterErodedBrickTunnel(int x0, int y0, int x1, int y1, float width)
	{
		int sideThick = 6;
		int maxStep = (int)(new Vector2(x1, y1) - new Vector2(x0, y0)).Length();
		Vector2 dir = Vector2.Normalize(new Vector2(x1, y1) - new Vector2(x0, y0));
		Vector2 checkPoint = new Vector2(x0, y0);
		float halfWidth = width / 2f;
		for (int s = 0; s < maxStep; s++)
		{
			checkPoint += dir;
			for (int x = (int)(-halfWidth); x <= halfWidth; x++)
			{
				for (int y = (int)(-halfWidth); y <= halfWidth; y++)
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
						float distance = checkDir.Length();
						if (distance < halfWidth - (sideThick - 1))
						{
							tile.HasTile = false;
							tile.wall = (ushort)ModContent.WallType<WaterErodedGreenBrickWall>();
							tile.liquid = (byte)LiquidID.Water;
							tile.LiquidAmount = 255;
						}
						if (distance >= halfWidth - (sideThick - 1))
						{
							tile.HasTile = true;
							tile.TileType = (ushort)ModContent.TileType<WaterErodedGreenBrick>();
							if (distance < halfWidth)
							{
								tile.wall = (ushort)ModContent.WallType<WaterErodedGreenBrickWall>();
							}
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Create a tunnel with water-erodeed brick side between 2 points.Traversing altitute by creating a zigzag path.
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="width"></param>
	public void ConnectWaterErodedBrickTunnel_Serrated(int x0, int y0, int x1, int y1, float width, float sideThick)
	{
		ConnectWaterErodedBrickTunnel_Serrated(new Point(x0, y0), new Point(x1, y1), width, sideThick);
	}

	/// <summary>
	/// Create a tunnel with water-erodeed brick side between 2 points.Traversing altitute by creating a zigzag path.
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="width"></param>
	public void ConnectWaterErodedBrickTunnel_Serrated(Point point0, Point point1, float width, float sideThick)
	{
		float centerThick = width - sideThick;
		int distanceY = Math.Abs((point1 - point0).Y);
		Point check = point0;
		int dirY = 1;
		if (point1.Y < point0.Y)
		{
			dirY = -1;
		}
		int jaggedX = 0;
		int jaggedDir = (int)((WorldGen.genRand.Next(2) - 0.5f) * 2);

		List<Point> trailPos = new List<Point>();
		for (int y = 0; y < distanceY; y++)
		{
			trailPos.Add(check + new Point(jaggedX, 0));
			check.Y += dirY;
			jaggedX += jaggedDir * 3;
			if (Math.Abs(jaggedX) > 24 && jaggedX * jaggedDir > 0)
			{
				jaggedDir *= -1;
			}
		}
		check += new Point(jaggedX, 0);
		int dirX = 1;
		if (point1.X < check.X)
		{
			dirX = -1;
		}
		int distanceX = Math.Abs((point1 - check).X);
		for (int x = 0; x <= distanceX; x++)
		{
			trailPos.Add(check);
			check.X += dirX;
		}
		for (int s = 0; s < trailPos.Count; s++)
		{
			PlaceSquareAreaOfBlock(trailPos[s], (int)width, ModContent.TileType<WaterErodedGreenBrick>());
			PlaceSquareAreaOfWall(trailPos[s], (int)width - 1, ModContent.WallType<WaterErodedGreenBrickWall>());
		}
		for (int s = 0; s < trailPos.Count; s++)
		{
			PlaceSquareAreaOfBlock(trailPos[s], (int)centerThick, -1);
			PlaceSquareAreaOfLiquid(trailPos[s], (int)width - 1, LiquidID.Water);
		}
		int overLength = 15;
		if (dirX < 0)
		{
			KillRectangleAreaOfTile(trailPos[^1].X - (int)width - overLength, trailPos[^1].Y - (int)centerThick, trailPos[^1].X, trailPos[^1].Y + (int)centerThick);
			PlaceRectangleAreaOfWall(trailPos[^1].X - (int)width - overLength, trailPos[^1].Y - (int)centerThick, trailPos[^1].X, trailPos[^1].Y + (int)centerThick, ModContent.WallType<WaterErodedGreenBrickWall>());
			PlaceRectangleAreaOfLiquid(trailPos[^1].X - (int)width - overLength, trailPos[^1].Y - (int)centerThick, trailPos[^1].X, trailPos[^1].Y + (int)centerThick, LiquidID.Water);
		}
		else
		{
			KillRectangleAreaOfTile(trailPos[^1].X, trailPos[^1].Y - (int)centerThick, trailPos[^1].X + (int)width + overLength, trailPos[^1].Y + (int)centerThick);
			PlaceRectangleAreaOfWall(trailPos[^1].X, trailPos[^1].Y - (int)centerThick, trailPos[^1].X + (int)width + overLength, trailPos[^1].Y + (int)centerThick, ModContent.WallType<WaterErodedGreenBrickWall>());
			PlaceRectangleAreaOfLiquid(trailPos[^1].X, trailPos[^1].Y - (int)centerThick, trailPos[^1].X + (int)width + overLength, trailPos[^1].Y + (int)centerThick, LiquidID.Water);
		}
		KillRectangleAreaOfTile(trailPos[0].X - (int)centerThick, trailPos[0].Y - (int)width, trailPos[0].X + (int)centerThick, trailPos[0].Y);
		PlaceRectangleAreaOfWall(trailPos[0].X - (int)centerThick, trailPos[0].Y - (int)width, trailPos[0].X + (int)centerThick, trailPos[0].Y, ModContent.WallType<WaterErodedGreenBrickWall>());
		PlaceRectangleAreaOfLiquid(trailPos[0].X - (int)centerThick, trailPos[0].Y - (int)width, trailPos[0].X + (int)centerThick, trailPos[0].Y, LiquidID.Water);
	}

	public static void ClearRectangleAreaExclude(int x0, int y0, int x1, int y1, int excludeType)
	{
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile tile = YggdrasilWorldGeneration.SafeGetTile(x, y);
				if (tile.TileType != excludeType)
				{
					tile.ClearEverything();
				}
			}
		}
	}

	public override bool? UseItem(Player player)
	{
		return base.UseItem(player);
	}
}