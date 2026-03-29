using ModLiquidLib.Utils;
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
		PlaceCircleAreaOfBlockWithRandomNoise(center.ToVector2(), radius, type, force);
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
	/// Transform the tile within the polygon to the type. The polygon is a list of tile coordinates in Vector2, and the area is determined by the point-in-polygon test.
	/// </summary>
	/// <param name="polygon">All the Vector2 is in Tile coord.</param>
	/// <param name="type"></param>
	/// <param name="force"></param>
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
					Tile tile = SafeGetTile(x, y);
					ChangeTile(tile, type, force);
				}
			}
		}
	}

	/// <summary>
	/// Automatically offset the polygon by the anchorPos, then transform the tile within the polygon to the type. The polygon is a list of tile coordinates in Vector2, and the area is determined by the point-in-polygon test.
	/// </summary>
	/// <param name="polygon"></param>
	/// <param name="anchorPos"></param>
	/// <param name="type"></param>
	/// <param name="force"></param>
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
}