using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Commons.Utilities;

public partial class TileUtils
{
	public static bool IsType<T>(this Tile tile)
		where T : ModTile
	{
		var modTile = TileLoader.GetTile(tile.type);
		return modTile is not null && modTile is T;
	}

	public static int ToTileCoordinate(this float value) => (int)value >> 4;

	public static int ToTileCoordinate(this double value) => (int)value >> 4;

	public static Tile SafeGetTile(int i, int j) =>
		Main.tile[Math.Clamp(i, 20, Main.maxTilesX - 20), Math.Clamp(j, 20, Main.maxTilesY - 20)];

	public static Tile SafeGetTile(Point point) => SafeGetTile(point.X, point.Y);

	/// <summary>
	/// The vector is expected to be the tile coordinate, not world coordinate. (i.e. vector.X is expected to be i, and vector.Y is expected to be j)
	/// </summary>
	/// <param name="vector"></param>
	/// <returns></returns>
	public static Tile SafeGetTile(Vector2 vector) => SafeGetTile((int)vector.X, (int)vector.Y);

	public static void DefaultToMultiTileAnchorBottom(int width, int height)
	{
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = height;
		TileObjectData.newTile.Width = width;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.CoordinateHeights = new int[height];
		Array.Fill(TileObjectData.newTile.CoordinateHeights, 16);
		TileObjectData.newTile.CoordinateHeights[^1] = 18;
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.Origin = new Point16(width / 2, height - 1);
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, width, 0);
	}

	public static void DefaultToMultiTileAnchorTop(int width, int height)
	{
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = height;
		TileObjectData.newTile.Width = width;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.CoordinateHeights = new int[height];
		Array.Fill(TileObjectData.newTile.CoordinateHeights, 16);
		TileObjectData.newTile.CoordinateHeights[^1] = 18;
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.Origin = new Point16(width / 2, height - 1);
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, width, 0);
	}

	public static void DefaultToMultiTileWall(int width, int height)
	{
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
		TileObjectData.newTile.Height = height;
		TileObjectData.newTile.Width = width;
		TileObjectData.newTile.CoordinateHeights = new int[height];
		Array.Fill(TileObjectData.newTile.CoordinateHeights, 16);
		TileObjectData.newTile.Origin = new Point16(width / 2, height / 2);
		TileObjectData.newTile.StyleHorizontal = true;
	}

	public static int GetFixedRandomNumber(int x, int y, int max = 1024)
	{
		Random random = new Random(SafeGetTile(x, y).GetHashCode());
		return random.Next(0, max);
	}

	public static int GetFixedRandomNumber(Point point, int max = 1024)
	{
		Random random = new Random(SafeGetTile(point).GetHashCode());
		return random.Next(0, max);
	}

	public static int GetFixedRandomNumber(Tile tile, int max = 1024)
	{
		Random random = new Random(tile.GetHashCode());
		return random.Next(0, max);
	}

	public static int GetFixedRandomNumber(int seed, int max = 1024)
	{
		Random random = new Random(seed);
		return random.Next(0, max);
	}

	public static bool AreaHasTile(int x, int y, int width, int height, Func<Tile, bool> prediction = null)
	{
		for (int i = x; i < x + width; i++)
		{
			for (int j = y; j < y + height; j++)
			{
				var tile = SafeGetTile(i, j);
				if (tile.HasTile && (prediction is null || prediction(tile)))
				{
					return true;
				}
			}
		}

		return false;
	}
}