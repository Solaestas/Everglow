using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Commons.Utilities;

public partial class TileUtils
{
	public static Tile SafeGetTile(int i, int j) =>
		Main.tile[Math.Clamp(i, 20, Main.maxTilesX - 20), Math.Clamp(j, 20, Main.maxTilesY - 20)];

	public static Tile SafeGetTile(Point point) => SafeGetTile(point.X, point.Y);

	public static Tile SafeGetTile(Vector2 vector) => SafeGetTile((int)vector.X, (int)vector.Y);

	/// <summary>
	/// 以[x,y]为左上顶点放置大件连续物块,此类物块必须是18x18(不算分隔线就16x16)一帧的
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
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
}