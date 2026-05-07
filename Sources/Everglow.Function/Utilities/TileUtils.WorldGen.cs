using Terraria.ObjectData;
using Terraria.Utilities;

namespace Everglow.Commons.Utilities;

public partial class TileUtils
{
	public static int[,] PerlinPixelR = new int[1024, 1024];
	public static int[,] PerlinPixelG = new int[1024, 1024];
	public static int[,] PerlinPixelB = new int[1024, 1024];

	public static UnifiedRandom GenRand = new UnifiedRandom();

	/// <summary>
	/// 0: Forceful<br/>
	/// 1: NoTileOnly<br/>
	/// 2: TileOnly<br/>
	/// 3: WallOnly<br/>
	/// 4: NoWallOnly<br/>
	/// 5: LiquidOnly<br/>
	/// 6: NoLiquidOnly<br/>
	/// 7: TileAndWallOnly<br/>
	/// 8: TileButNoWallOnly<br/>
	/// 9: WallButNoTileOnly<br/>
	/// </summary>
	public enum TileChangeState
	{
		Forceful,
		NoTileOnly,
		TileOnly,
		WallOnly,
		NoWallOnly,
		LiquidOnly,
		NoLiquidOnly,
		TileAndWallOnly,
		TileButNoWallOnly,
		WallButNoTileOnly,
	}

	public static bool CanChangeTile(Tile tile, int state)
	{
		switch (state)
		{
			case (int)TileChangeState.Forceful:
				return true;
			case (int)TileChangeState.NoTileOnly:
				if (!tile.HasTile)
				{
					return true;
				}
				break;
			case (int)TileChangeState.TileOnly:
				if (tile.HasTile)
				{
					return true;
				}
				break;
			case (int)TileChangeState.WallOnly:
				if (tile.wall > 0)
				{
					return true;
				}
				break;
			case (int)TileChangeState.NoWallOnly:
				if (tile.wall <= 0)
				{
					return true;
				}
				break;
			case (int)TileChangeState.LiquidOnly:
				if (tile.LiquidAmount > 0)
				{
					return true;
				}
				break;
			case (int)TileChangeState.NoLiquidOnly:
				if (tile.LiquidAmount <= 0)
				{
					return true;
				}
				break;
			case (int)TileChangeState.TileAndWallOnly:
				if (tile.wall > 0 && tile.HasTile)
				{
					return true;
				}
				break;
			case (int)TileChangeState.TileButNoWallOnly:
				if (tile.wall <= 0 && tile.HasTile)
				{
					return true;
				}
				break;
			case (int)TileChangeState.WallButNoTileOnly:
				if (tile.wall > 0 && !tile.HasTile)
				{
					return true;
				}
				break;
		}

		return false;
	}

	public static void TotalInitialize()
	{
		GenRand = WorldGen.genRand;
		FillPerlinPixel();
	}

	public static void FillPerlinPixel()
	{
		var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>(ModAsset.WorldGen_Noise_rgb_Mod);
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
	}

	/// <summary>
	/// A float value based on the noise texture's red channel.
	/// </summary>
	/// <returns>0f~1f</returns>
	public static float GetPerlinPixelR(float x, float y)
	{
		return PerlinPixelR[(int)Math.Abs(x) % 1024, (int)Math.Abs(y) % 1024] / 255f;
	}

	/// <summary>
	/// A float value based on the noise texture's green channel.
	/// </summary>
	/// <returns>0f~1f</returns>
	public static float GetPerlinPixelG(float x, float y)
	{
		return PerlinPixelG[(int)Math.Abs(x) % 1024, (int)Math.Abs(y) % 1024] / 255f;
	}

	/// <summary>
	/// A float value based on the noise texture's blue channel.
	/// </summary>
	/// <returns>0f~1f</returns>
	public static float GetPerlinPixelB(float x, float y)
	{
		return PerlinPixelB[(int)Math.Abs(x) % 1024, (int)Math.Abs(y) % 1024] / 255f;
	}

	/// <summary>
	/// Use (x, y) as the top left corner to place a frame important tile area with given width and height, and set the frameX and frameY of each tile in this area according to their position in this area.
	/// </summary>
	/// <param name="path"></param>
	public static void PlaceFrameImportantTiles(int x, int y, int width, int height, int type, int startX = 0, int startY = 0)
	{
		if (x > Main.maxTilesX - width || x < 0 || y > Main.maxTilesY - height || y < 0)
		{
			return;
		}

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Tile tile = Main.tile[x + i, y + j];
				tile.TileType = (ushort)type;
				tile.TileFrameX = (short)(i * 18 + startX);
				tile.TileFrameY = (short)(j * 18 + startY);
				tile.HasTile = true;
			}
		}
	}

	/// <summary>
	/// place frame important tiles above the area with (x, y) as the BOTTOM left corner and given width and height, and set the frameX and frameY of each tile in this area according to their position in this area.
	/// </summary>
	/// <param name="startX">TileFrameX at left side, +18 each tile towards right.</param>
	/// <param name="startY">TileFrameX at top side, +18 each tile towards down.</param>
	public static void PlaceFrameImportantTilesAbove(int x, int y, int width, int height, int type, int startX = 0, int startY = 0)
	{
		if (x > Main.maxTilesX - width || x < 0 || y > Main.maxTilesY - height || y < 0)
		{
			return;
		}

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Tile tile = Main.tile[x + i, y + j - height];
				tile.TileType = (ushort)type;
				tile.TileFrameX = (short)(i * 18 + startX);
				tile.TileFrameY = (short)(j * 18 + startY);
				tile.HasTile = true;
			}
		}
	}

	/// <returns>True when the given point can be killed safely(without chest).</returns>
	public static bool ChestSafe(int x, int y)
	{
		Tile tile = SafeGetTile(x, y);
		Tile tileUp = SafeGetTile(x, y - 1);
		if (!TileID.Sets.BasicChest[tile.TileType] && !TileID.Sets.BasicChest[tileUp.TileType])
		{
			return true;
		}
		return false;
	}

	/// <param name="center">Tile coordinate, no world coordinate.</param>
	/// <returns></returns>
	public static bool ChestSafe(Vector2 center)
	{
		return ChestSafe((int)center.X, (int)center.Y);
	}

	/// <returns>True when the given tile can be killed safely(without chest).</returns>
	public static bool ChestSafe(Tile tile)
	{
		return ChestSafe(tile.X(), tile.Y());
	}

	/// <summary>
	/// Fill tiles by given area:(x0:left, y0:top, x1:right, y1:bottom)
	/// </summary>
	/// <param name="x0">Left</param>
	/// <param name="y0">Top</param>
	/// <param name="x1">Right</param>
	/// <param name="y1">Bottom</param>
	/// <param name="type">TileID: place the tile.<br/>
	/// -1: Kill tile.<br/>
	/// -2: ClearEverything</param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlaceRectangleAreaOfBlock(int x0, int y0, int x1, int y1, int type, int force = 0)
	{
		if (x0 > x1)
		{
			(x0, x1) = (x1, x0);
		}
		if (y0 > y1)
		{
			(y0, y1) = (y1, y0);
		}
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile tile = SafeGetTile(x, y);
				ChangeTile(tile, type, force);
			}
		}
	}

	/// <summary>
	/// Fill tiles by given area. The pos is the top left corner and size is the width and height. So the area is (pos.X, pos.Y) to (pos.X + size.X, pos.Y + size.Y).
	/// </summary>
	/// <param name="pos">Tile coord.</param>
	/// <param name="size"></param>
	/// <param name="type">TileID: place the tile.<br/>
	/// -1: Kill tile.<br/>
	/// -2: ClearEverything</param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlaceRectangleAreaOfBlock(Vector2 pos, Vector2 size, int type, int force = 0)
	{
		PlaceRectangleAreaOfBlock((int)pos.X, (int)pos.Y, (int)(size + pos).X, (int)(size + pos).Y, type, force);
	}

	/// <summary>
	/// Set a center and radius of a circle in tile coordinate.
	/// </summary>
	/// <param name="center">Tile coord center in Vector2.</param>
	/// <param name="type">TileID: place the tile.<br/>
	/// -1: Kill tile.<br/>
	/// -2: ClearEverything</param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlaceCircleAreaOfBlock(Vector2 center, float radius, int type, int force = 0)
	{
		int radiusI = (int)radius;
		for (int x = -radiusI; x <= radiusI; x++)
		{
			for (int y = -radiusI; y <= radiusI; y++)
			{
				Tile tile = SafeGetTile(center + new Vector2(x, y));
				if (new Vector2(x, y).Length() <= radius)
				{
					ChangeTile(tile, type, force);
				}
			}
		}
	}

	/// <summary>
	/// Set a center and radius of a circle in tile coordinate.
	/// </summary>
	/// <param name="type">TileID: place the tile.<br/>
	/// -1: Kill tile.<br/>
	/// -2: ClearEverything</param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlaceCircleAreaOfBlock(Point center, float radius, int type, int force = 0)
	{
		PlaceCircleAreaOfBlock(center.ToVector2(), radius, type, force);
	}

	/// <summary>
	/// Transform the tile within the circle(center, radius) to the type, but with a random noise affect on the bound.
	/// </summary>
	/// <param name="center">Tile coord center in Vector2.</param>
	/// <param name="type">TileID: place the tile.<br/>
	/// -1: Kill tile.<br/>
	/// -2: ClearEverything</param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlaceCircleAreaOfBlockWithRandomNoise(Vector2 center, float radius, int type, float noiseSize = 10f, int force = 0)
	{
		int x0CoordPerlin = GenRand.Next(1024);
		int y0CoordPerlin = GenRand.Next(1024);
		int radiusI = (int)radius;
		for (int x = -radiusI; x <= radiusI; x++)
		{
			for (int y = -radiusI; y <= radiusI; y++)
			{
				float aValue = PerlinPixelR[Math.Abs((x + x0CoordPerlin) % 1024), Math.Abs((y + y0CoordPerlin) % 1024)] / 255f;
				if (new Vector2(x, y).Length() <= radius - aValue * noiseSize)
				{
					Tile tile = SafeGetTile(center + new Vector2(x, y));
					ChangeTile(tile, type, force);
				}
			}
		}
	}

	/// <summary>
	/// Transform the tile within the circle(center, radius) to the type, but with a random noise affect on the bound.
	/// </summary>
	/// <param name="type">TileID: place the tile.<br/>
	/// -1: Kill tile.<br/>
	/// -2: ClearEverything</param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlaceCircleAreaOfBlockWithRandomNoise(Point center, float radius, int type, float noiseSize = 10f, int force = 0)
	{
		PlaceCircleAreaOfBlockWithRandomNoise(center.ToVector2(), radius, type, noiseSize, force);
	}

	/// <summary>
	/// Kill the tile within the circle(center, radius), but with a random noise affect on the bound.
	/// </summary>
	/// <param name="center">Tile coord</param>
	/// <param name="radius"></param>
	/// <param name="type_be_killed"></param>
	/// <param name="killStyle">-1: Kill tile.<br/>
	/// -2: ClearEverything</param>
	/// <param name="noiseSize"></param>
	/// <param name="force"></param>
	public static void KillCircleAreaOfBlockWithRandomNoiseInCertainTypeOfTile(Vector2 center, float radius, List<int> type_be_killed, int killStyle = -1, float noiseSize = 10f, int force = 0)
	{
		int x0CoordPerlin = GenRand.Next(1024);
		int y0CoordPerlin = GenRand.Next(1024);
		int radiusI = (int)radius;
		for (int x = -radiusI; x <= radiusI; x++)
		{
			for (int y = -radiusI; y <= radiusI; y++)
			{
				float aValue = GetPerlinPixelR(x + x0CoordPerlin, y + y0CoordPerlin) / 255f;
				Tile tile = SafeGetTile(center + new Vector2(x, y));
				if (new Vector2(x, y).Length() <= radius - aValue * noiseSize && type_be_killed.Contains(tile.TileType))
				{
					ChangeTile(tile, killStyle, force);
				}
			}
		}
	}

	/// <summary>
	/// Kill the tile within the circle(center, radius), but with a random noise affect on the bound.
	/// </summary>
	/// <param name="center">Tile coord</param>
	/// <param name="radius"></param>
	/// <param name="type_be_killed"></param>
	/// <param name="killStyle">-1: Kill tile.<br/>
	/// -2: ClearEverything</param>
	/// <param name="noiseSize"></param>
	/// <param name="force"></param>
	public static void KillCircleAreaOfBlockWithRandomNoiseInCertainTypeOfTile(Point center, float radius, List<int> type_be_killed, int killStyle = -1, float noiseSize = 10f, int force = 0)
	{
		KillCircleAreaOfBlockWithRandomNoiseInCertainTypeOfTile(center.ToVector2(), radius, type_be_killed, killStyle, noiseSize, force);
	}

	/// <summary>
	/// Transform the tile within the polygon to the type. The polygon is a list of WORLD coordinates in Vector2, and the area is determined by the point-in-polygon test.
	/// </summary>
	/// <param name="polygon">WORLD coord</param>
	/// <param name="type"></param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlacePolygonAreaOfBlock(List<Vector2> polygon, int type, int force = 0)
	{
		if (polygon.Count < 3)
		{
			return;
		}
		var bounds = MathUtils.GetPolygonAABBBound_Vector4(polygon);
		for (int x = (int)bounds.X; x <= bounds.Z; x++)
		{
			for (int y = (int)bounds.Y; y <= bounds.W; y++)
			{
				if (MathUtils.IsPointInPolygon(polygon, new Vector2(x, y)))
				{
					Tile tile = SafeGetTile(new Vector2(x, y).ToTileCoordinates());
					ChangeTile(tile, type, force);
				}
			}
		}
	}

	/// <summary>
	/// Transform the tile within the polygon to the type. The polygon is a list of TILE coordinates in Point, and the area is determined by the point-in-polygon test.
	/// </summary>
	/// <param name="polygon">TILE coord</param>
	/// <param name="type"></param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlacePolygonAreaOfBlock(List<Point> polygon, int type, int force = 0)
	{
		List<Vector2> polygon_Vector2 = new List<Vector2>();
		foreach (var point in polygon)
		{
			polygon_Vector2.Add(point.ToWorldCoordinates());
		}
		PlacePolygonAreaOfBlock(polygon_Vector2, type, force);
	}

	/// <summary>
	/// Automatically offset the polygon by the anchorPos, then transform the tile within the polygon to the type. The polygon is a list of WORLD coordinates in Vector2, and the area is determined by the point-in-polygon test.
	/// </summary>
	/// <param name="polygon">WORLD coord</param>
	/// <param name="anchorPos"></param>
	/// <param name="type"></param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlacePolygonAreaOfBlockWithOffset(List<Vector2> polygon, Vector2 anchorPos, int type, int force = 0)
	{
		if (polygon.Count < 3)
		{
			return;
		}
		List<Vector2> newPolygon = polygon;
		for (int i = 0; i < newPolygon.Count; i++)
		{
			newPolygon[i] += anchorPos;
		}
		var bounds = MathUtils.GetPolygonAABBBound_Vector4(newPolygon);
		for (int x = (int)bounds.X; x <= bounds.Z; x++)
		{
			for (int y = (int)bounds.Y; y <= bounds.W; y++)
			{
				if (MathUtils.IsPointInPolygon(newPolygon, new Vector2(x, y)))
				{
					Tile tile = SafeGetTile(x, y);
					ChangeTile(tile, type, force);
				}
			}
		}
	}

	/// <summary>
	/// Automatically offset the polygon by the anchorPos, then transform the tile within the polygon to the type. The polygon is a list of TILE coordinates in Point, and the area is determined by the point-in-polygon test.
	/// </summary>
	/// <param name="polygon">TILE coord</param>
	/// <param name="anchorPos"></param>
	/// <param name="type"></param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlacePolygonAreaOfBlockWithOffset(List<Point> polygon, Vector2 anchorPos, int type, int force = 0)
	{
		List<Vector2> polygon_Vector2 = new List<Vector2>();
		foreach (var point in polygon)
		{
			polygon_Vector2.Add(point.ToWorldCoordinates());
		}
		PlacePolygonAreaOfBlockWithOffset(polygon_Vector2, anchorPos, type, force);
	}

	/// <summary>
	/// Transform the tiles at the edge of the polygon to the type. The polygon is a list of WORLD coordinates in Vector2, and the area is determined by the point-in-polygon test.
	/// </summary>
	/// <param name="polygon">WORLD coord</param>
	/// <param name="type"></param>
	/// <param name="thick"></param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlacePolygonBoundOfBlock(List<Vector2> polygon, int type, float thick, int force = 0)
	{
		if (polygon.Count < 3)
		{
			return;
		}
		for (int k = 0; k < polygon.Count; k++)
		{
			int nextIndex = k + 1;
			if (nextIndex == polygon.Count)
			{
				nextIndex = 0;
			}
			PlaceLineBlock(polygon[k], polygon[nextIndex], thick, type, force);
		}
	}

	/// <summary>
	/// Transform the tiles at the edge of the polygon to the type. The polygon is a list of TILE coordinates in Point, and the area is determined by the point-in-polygon test.
	/// </summary>
	/// <param name="polygon">TILE coord</param>
	/// <param name="type"></param>
	/// <param name="thick"></param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlacePolygonBoundOfBlock(List<Point> polygon, int type, float thick, int force = 0)
	{
		List<Vector2> polygon_Vector2 = new List<Vector2>();
		foreach (var point in polygon)
		{
			polygon_Vector2.Add(point.ToWorldCoordinates());
		}
		PlacePolygonBoundOfBlock(polygon_Vector2, type, thick, force);
	}

	/// <summary>
	/// Transform the tiles at the edge of the line(pos0, pos1) to the type. The area is determined by the distance from the tile to the line. If the distance is smaller than thick, then this tile will be transformed.
	/// </summary>
	/// <param name="pos0">WORLD coord</param>
	/// <param name="pos1">WORLD coord</param>
	/// <param name="thick"></param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlaceLineBlock(Vector2 pos0, Vector2 pos1, float thick, int type, int force = 0)
	{
		Vector2 dir = pos0 - pos1;
		if (dir == Vector2.zeroVector)
		{
			return;
		}
		Vector2 normalizedDir = dir.NormalizeSafe().RotatedBy(MathHelper.PiOver2);
		List<Vector2> tiltRect = new List<Vector2>();
		tiltRect.Add(pos0 + normalizedDir * thick * 0.5f);
		tiltRect.Add(pos1 + normalizedDir * thick * 0.5f);
		tiltRect.Add(pos1 - normalizedDir * thick * 0.5f);
		tiltRect.Add(pos0 - normalizedDir * thick * 0.5f);
		PlacePolygonAreaOfBlock(tiltRect, type, force);
	}

	/// <summary>
	/// Transform the tiles at the edge of the line(pos0, pos1) to the type. The area is determined by the distance from the tile to the line. If the distance is smaller than thick, then this tile will be transformed.
	/// </summary>
	/// <param name="pos0">TILE coord</param>
	/// <param name="pos1">TILE coord</param>
	/// <param name="thick"></param>
	/// <param name="force"><see cref="TileChangeState"/></param>
	public static void PlaceLineBlock(Point pos0, Point pos1, float thick, int type, int force = 0)
	{
		PlaceLineBlock(pos0.ToWorldCoordinates(), pos1.ToWorldCoordinates(), thick, type, force);
	}

	public static void ChangeTile(Tile tile, int type, int force)
	{
		if (ChestSafe(tile) && CanChangeTile(tile, force))
		{
			if (type >= 0)
			{
				tile.TileType = (ushort)type;
				tile.HasTile = true;
			}
			else if (type == -1)
			{
				tile.HasTile = false;
			}
			else if (type == -2)
			{
				tile.ClearEverything();
			}
		}
	}

	/// <summary>
	/// Smooth tiles by given area:(x0:left, y0:top, x1:right, y1:bottom)
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	public static void SmoothTile(int x0, int y0, int x1, int y1)
	{
		x0 = Math.Clamp(x0, 20, Main.maxTilesX - 20);
		x1 = Math.Clamp(x1, 20, Main.maxTilesX - 20);
		y0 = Math.Clamp(y0, 20, Main.maxTilesY - 20);
		y1 = Math.Clamp(y1, 20, Main.maxTilesY - 20);
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				if (!ChestSafe(x, y))
				{
					continue;
				}
				Tile.SmoothSlope(x, y, false, false);
				WorldGen.TileFrame(x, y, true, false);
				WorldGen.SquareWallFrame(x, y, true);
			}
		}
	}

	/// <summary>
	/// Use BFS algorithm get the continue tiles from the check point.
	/// </summary>
	/// <param name="checkPoint"></param>
	/// <param name="includeWall">If true, tile with no block but only wall will also be included.</param>
	/// <param name="maxCount"></param>
	/// <returns></returns>
	public static List<Point> BFSContinueTile(Point checkPoint, bool includeWall = false, int maxCount = 512, List<int> theseTypeOnly = default)
	{
		int maxContinueCount = maxCount;
		(int, int)[] directions =
		{
			(0, 1),
			(1, 0),
			(0, -1),
			(-1, 0),
		};
		Queue<Point> queueChecked = new Queue<Point>();

		// Add first point to the queue.
		queueChecked.Enqueue(checkPoint);
		List<Point> visited = new List<Point>();

		while (queueChecked.Count > 0)
		{
			var tilePos = queueChecked.Dequeue();

			foreach (var (dx, dy) in directions)
			{
				int checkX = tilePos.X + dx;
				int checkY = tilePos.Y + dy;
				Point point = new Point(checkX, checkY);
				Tile tile = SafeGetTile(checkX, checkY);

				// Check bound and obstruction.
				if (checkX >= 20 && checkX < Main.maxTilesX - 20 && checkY >= 20 && checkY < Main.maxTilesY - 20 &&
					(tile.HasTile || (includeWall && tile.WallType > WallID.None)) && !visited.Contains(point))
				{
					if (theseTypeOnly == default)
					{
						queueChecked.Enqueue(point);
						visited.Add(point);
					}
					else if (theseTypeOnly.Contains(tile.type))
					{
						queueChecked.Enqueue(point);
						visited.Add(point);
					}
				}
			}
			if (queueChecked.Count > maxContinueCount || visited.Count > maxContinueCount)
			{
				break;
			}
		}
		return visited;
	}

	/// <summary>
	/// Use BFS algorithm get the continue tiles from the check point.
	/// </summary>
	/// <param name="checkPoint"></param>
	/// <param name="includeWall">If true, tile with wall will NOT be included.</param>
	/// <param name="maxCount"></param>
	/// <returns></returns>
	public static List<Point> BFSContinueEmpty(Point checkPoint, bool includeWall = false, int maxCount = 512, List<int> ignoreTheseType = default)
	{
		int maxContinueCount = maxCount;
		(int, int)[] directions =
		{
			(0, 1),
			(1, 0),
			(0, -1),
			(-1, 0),
		};
		Queue<Point> queueChecked = new Queue<Point>();

		// Add first point to the queue.
		queueChecked.Enqueue(checkPoint);
		List<Point> visited = new List<Point>();

		while (queueChecked.Count > 0)
		{
			var tilePos = queueChecked.Dequeue();

			foreach (var (dx, dy) in directions)
			{
				int checkX = tilePos.X + dx;
				int checkY = tilePos.Y + dy;
				Point point = new Point(checkX, checkY);
				Tile tile = SafeGetTile(checkX, checkY);

				// Check bound and obstruction.
				bool flag0 = !tile.HasTile && (!includeWall || tile.WallType <= WallID.None);
				bool flag1 = tile.HasTile && ignoreTheseType != default && ignoreTheseType.Contains(tile.TileType);
				if (checkX >= 20 && checkX < Main.maxTilesX - 20 && checkY >= 20 && checkY < Main.maxTilesY - 20 &&
					(flag0 || flag1) && !visited.Contains(point))
				{
					queueChecked.Enqueue(point);
					visited.Add(point);
				}
			}
			if (queueChecked.Count > maxContinueCount || visited.Count > maxContinueCount)
			{
				break;
			}
		}
		return visited;
	}

	public static List<Point> BFSSurface(Point checkPoint, int maxCount = 512, bool ignoreFrameImportant = true, List<int> theseTypeOnly = default)
	{
		int maxContinueCount = maxCount;
		(int, int)[] directions =
		{
			(0, 1),
			(1, 0),
			(0, -1),
			(-1, 0),
			(1, 1),
			(1, -1),
			(-1, -1),
			(-1, 1),
		};
		Queue<Point> queueChecked = new Queue<Point>();

		// Add first point to the queue.
		queueChecked.Enqueue(checkPoint);
		List<Point> visited = new List<Point>();

		while (queueChecked.Count > 0)
		{
			var tilePos = queueChecked.Dequeue();

			foreach (var (dx, dy) in directions)
			{
				int checkX = tilePos.X + dx;
				int checkY = tilePos.Y + dy;
				Point point = new Point(checkX, checkY);
				Tile tile = SafeGetTile(checkX, checkY);
				Tile tile_up = SafeGetTile(checkX, checkY - 1);
				Tile tile_bottom = SafeGetTile(checkX, checkY + 1);
				Tile tile_left = SafeGetTile(checkX - 1, checkY);
				Tile tile_right = SafeGetTile(checkX + 1, checkY);
				//Tile tile_upleft = SafeGetTile(checkX - 1, checkY - 1);
				//Tile tile_bottomleft = SafeGetTile(checkX - 1, checkY + 1);
				//Tile tile_upright = SafeGetTile(checkX + 1, checkY - 1);
				//Tile tile_bottomright = SafeGetTile(checkX + 1, checkY - 1);

				//bool flag0 = !tile_upleft.HasTile || !tile_upright.HasTile || !tile_bottomleft.HasTile || !tile_bottomright.HasTile;
				// Check bound and obstruction.
				if (checkX >= 20 && checkX < Main.maxTilesX - 20 && checkY >= 20 && checkY < Main.maxTilesY - 20 &&
					tile.HasTile && (!tile_up.HasTile || !tile_bottom.HasTile || !tile_left.HasTile || !tile_right.HasTile) && (!ignoreFrameImportant || !Main.tileFrameImportant[tile.TileType]) && !visited.Contains(point))
				{
					if (theseTypeOnly == default)
					{
						queueChecked.Enqueue(point);
						visited.Add(point);
					}
					else if (theseTypeOnly.Contains(tile.type))
					{
						queueChecked.Enqueue(point);
						visited.Add(point);
					}
				}
			}
			if (queueChecked.Count > maxContinueCount || visited.Count > maxContinueCount)
			{
				break;
			}
		}
		return visited;
	}

	public static List<Point> GenerateRandomSeeds(int xBoundLeft, int xBoundRight, int yBoundTop, int yBoundBottom, int expect_Count, float minDistance = 20)
	{
		List<Point> result = new List<Point>();
		HashSet<Point> used = new HashSet<Point>();

		int width = xBoundRight - xBoundLeft;
		int height = yBoundBottom - yBoundTop;
		float cellSize = minDistance / (float)Math.Sqrt(2);
		int gridW = (int)Math.Ceiling(width / cellSize) + 2;
		int gridH = (int)Math.Ceiling(height / cellSize) + 2;

		Point?[,] grid = new Point?[gridW, gridH];
		List<Point> active = new List<Point>();

		int startX = (xBoundLeft + xBoundRight) / 2;
		int startY = (yBoundTop + yBoundBottom) / 2;
		result.Add(new Point(startX, startY));
		used.Add(new Point(startX, startY));
		active.Add(new Point(startX, startY));
		SetGrid(grid, cellSize, xBoundLeft, yBoundTop, startX, startY);

		while (active.Count > 0 && result.Count < expect_Count)
		{
			int idx = Main.rand.Next(active.Count);
			var current = active[idx];
			bool found = false;

			for (int i = 0; i < 8; i++)
			{
				float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
				float dist = Main.rand.NextFloat(minDistance, minDistance * 2f);

				int newX = current.X + (int)Math.Round(Math.Cos(angle) * dist);
				int newY = current.Y + (int)Math.Round(Math.Sin(angle) * dist);

				if (newX < xBoundLeft || newX > xBoundRight ||
					newY < yBoundTop || newY > yBoundBottom)
				{
					continue;
				}

				int gx = (int)((newX - xBoundLeft) / cellSize);
				int gy = (int)((newY - yBoundTop) / cellSize);

				bool tooClose = false;

				for (int cx = Math.Max(0, gx - 1); cx <= Math.Min(gridW - 1, gx + 1); cx++)
				{
					for (int cy = Math.Max(0, gy - 1); cy <= Math.Min(gridH - 1, gy + 1); cy++)
					{
						var pt = grid[cx, cy];
						if (pt.HasValue)
						{
							float d = Vector2.Distance(
								new Vector2(newX, newY),
								new Vector2(pt.Value.X, pt.Value.Y));
							if (d < minDistance)
							{
								tooClose = true;
								break;
							}
						}
					}
					if (tooClose)
					{
						break;
					}
				}

				if (!tooClose && !used.Contains(new Point(newX, newY)))
				{
					result.Add(new Point(newX, newY));
					used.Add(new Point(newX, newY));
					active.Add(new Point(newX, newY));
					SetGrid(grid, cellSize, xBoundLeft, yBoundTop, newX, newY);
					found = true;
					break;
				}
			}

			if (!found)
			{
				active.RemoveAt(idx);
			}
		}

		return result;
	}

	private static void SetGrid(Point?[,] grid, float cellSize, int ox, int oy, int x, int y)
	{
		int gx = (int)((x - ox) / cellSize);
		int gy = (int)((y - oy) / cellSize);
		grid[gx, gy] = new Point(x, y);
	}

	public static bool TileAtOriginPos(Tile tile)
	{
		int style = TileObjectData.GetTileStyle(tile);
		TileObjectData tileObjectData = TileObjectData.GetTileData(tile.TileType, style);
		return tile.TileFrameX - style * tileObjectData.Width * 18 == tileObjectData.Origin.X * 18 && tile.TileFrameY == tileObjectData.Origin.Y * 18;
	}

	public static void PlaceWallAround(Tile tile, int wallType, bool middle = true, bool top = true, bool bottom = true, bool left = true, bool right = true)
	{
		PlaceWallAround(tile.X(), tile.Y(), wallType, middle, top, bottom, left, right);
	}

	public static void PlaceWallAround(int x, int y, int wallType, bool middle = true, bool top = true, bool bottom = true, bool left = true, bool right = true)
	{
		Tile tile = SafeGetTile(x, y);
		if (middle)
		{
			tile.wall = (ushort)wallType;
		}
		Tile tile_top = SafeGetTile(x, y - 1);
		if (top)
		{
			tile_top.wall = (ushort)wallType;
		}
		Tile tile_bottom = SafeGetTile(x, y + 1);
		if (bottom)
		{
			tile_bottom.wall = (ushort)wallType;
		}
		Tile tile_left = SafeGetTile(x - 1, y);
		if (left)
		{
			tile_left.wall = (ushort)wallType;
		}
		Tile tile_right = SafeGetTile(x + 1, y);
		if (right)
		{
			tile_right.wall = (ushort)wallType;
		}
	}

	private static readonly (int, int)[] directionsLiquid =
	{
		(1, 0),
		(0, 1),
		(-1, 0),
	};

	/// <summary>
	/// Fill water or other liquid below pos. pos : in World coord.
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="type"></param>
	/// <param name="maxCount"></param>
	public static void FillLiquid(Point pos, int type = 0, int maxCount = 900)
	{
		Queue<Point> queueChecked = new Queue<Point>();

		// 将起始点加入队列
		queueChecked.Enqueue(pos);
		List<Point> visited = new List<Point>();

		while (queueChecked.Count > 0)
		{
			var tilePos = queueChecked.Dequeue();

			foreach (var (dx, dy) in directionsLiquid)
			{
				int checkX = tilePos.X + dx;
				int checkY = tilePos.Y + dy;
				Point point = new Point(checkX, checkY);

				// 检查边界和障碍物
				if (checkX >= 20 && checkX < Main.maxTilesX - 20 && checkY >= 20 && checkY < Main.maxTilesY - 20 &&
					!Collision.IsWorldPointSolid(point.ToWorldCoordinates()) && !visited.Contains(point))
				{
					queueChecked.Enqueue(point);
					visited.Add(point);
				}
			}
			if (queueChecked.Count > maxCount || visited.Count > maxCount)
			{
				break;
			}
		}
		if (visited.Count < maxCount)
		{
			foreach (var checked_pos in visited)
			{
				Tile tile = SafeGetTile(checked_pos);
				tile.LiquidType = (byte)type;
				tile.LiquidAmount = 255;
			}
		}
	}

	/// <summary>
	/// Fill water or other liquid below center.center : in World coord.
	/// </summary>
	/// <param name="center">World coord</param>
	/// <param name="type"></param>
	public static void FillLiquid(Vector2 center, int type = 0, int maxCount = 900)
	{
		FillLiquid(center.ToTileCoordinates(), type, maxCount);
	}

	/// <summary>
	/// Fill water or other liquid below pos.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="type"></param>
	/// <param name="maxCount"></param>
	public static void FillLiquid(int i, int j, int type = 0, int maxCount = 900)
	{
		FillLiquid(new Point(i, j), type, maxCount);
	}

	/// <summary>
	/// Fill water or other liquid below center.center : in tile coord.
	/// </summary>
	/// <param name="center">Tile coord</param>
	public static List<Point> BFSGetCanFillLiquidTiles(Vector2 center, int maxCount = 900)
	{
		return BFSGetCanFillLiquidTiles(center.ToTileCoordinates(), maxCount);
	}

	/// <summary>
	/// Fill water or other liquid below pos. pos : in tile coord.
	/// </summary>
	/// <param name="pos">Tile coord</param>
	public static List<Point> BFSGetCanFillLiquidTiles(Point pos, int maxCount = 900)
	{
		Queue<Point> queueChecked = new Queue<Point>();

		// 将起始点加入队列
		queueChecked.Enqueue(pos);
		List<Point> visited = new List<Point>();

		while (queueChecked.Count > 0)
		{
			var tilePos = queueChecked.Dequeue();

			foreach (var (dx, dy) in directionsLiquid)
			{
				int checkX = tilePos.X + dx;
				int checkY = tilePos.Y + dy;
				Tile Tile = SafeGetTile(checkX, checkY);
				Point point = new Point(checkX, checkY);
				// 检查边界和障碍物
				if (checkX >= 20 && checkX < Main.maxTilesX - 20 && checkY >= 20 && checkY < Main.maxTilesY - 20 &&
					!Collision.IsWorldPointSolid(new Point(checkX, checkY).ToWorldCoordinates()) && !visited.Contains(point))
				{
					queueChecked.Enqueue(new Point(checkX, checkY));
					visited.Add(point);
				}
			}
			if (queueChecked.Count > maxCount || visited.Count > maxCount)
			{
				break;
			}
		}
		return visited;
	}

	public static List<Point> BFSGetCanFillLiquidTiles(int i, int j, int maxCount = 900)
	{
		return BFSGetCanFillLiquidTiles(new Point(i, j), maxCount);
	}
}