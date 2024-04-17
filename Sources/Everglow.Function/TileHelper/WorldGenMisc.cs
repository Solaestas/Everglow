namespace Everglow.Commons;

public class WorldGenMisc
{
	/// <summary>
	/// 以给定的坐标为左下角，放置内含物品的箱子
	/// </summary>
	public static void PlaceChest(int x, int y, int type, List<Item> itemList)
	{
		if(x >= Main.maxTilesX - 2 || x <= 2 || y >= Main.maxTilesY - 2 || y <= 2)
		{
			throw new Exception("the chest stand out of the world!");
		}
		SmoothTileOfAreaXYWH(x - 1, y + 1, 4, 2);
		WorldGen.PlaceChest(x, y, (ushort)type);
		foreach (Chest chest in Main.chest)
		{
			if (chest != null)
			{
				if (chest.x == x && chest.y + 1 == y)
				{
					for(int t = 0;t < itemList.Count;t++)
					{
						chest.item[t] = itemList[t];
					}
				}
			}
		}
	}
	/// <summary>
	/// 以[x,y]为左上顶点放置大件连续物块,此类物块必须是18x18(不算分隔线就16x16)一帧的
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static void PlaceFrameImportantTiles(int x, int y, int width, int height, int type, int xStartAt = 0, int yStartAt = 0)
	{
		if (x > Main.maxTilesX - width || x < 0 || y > Main.maxTilesY - height || y < 0)
			return;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Tile tile = Main.tile[x + i, y + j];
				tile.TileType = (ushort)type;
				tile.TileFrameX = (short)(i * 18 + xStartAt);
				tile.TileFrameY = (short)(j * 18 + yStartAt);
				tile.HasTile = true;
			}
		}
	}
	/// <summary>
	/// 以[x0,y0]为左上顶点,[x1,y1]为右下顶点平滑物块
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	public static void SmoothTileOfAreaFourPoint(int x0, int y0, int x1, int y1)
	{
		x0 = Math.Clamp(x0, 20, Main.maxTilesX - 20);
		x1 = Math.Clamp(x1, 20, Main.maxTilesX - 20);
		y0 = Math.Clamp(y0, 20, Main.maxTilesY - 20);
		y1 = Math.Clamp(y1, 20, Main.maxTilesY - 20);
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile tile = Main.tile[x, y];
				Tile tileUp = Main.tile[x, y - 1];
				if (tile.TileType == TileID.Containers)
				{
					continue;
				}
				if (tileUp.TileType == TileID.Containers)
				{
					continue;
				}
				Tile.SmoothSlope(x, y, false);
				WorldGen.TileFrame(x, y, true, false);
				WorldGen.SquareWallFrame(x, y, true);
			}
		}
	}
	/// <summary>
	/// 以x, y, width, height的格式确定区域平滑物块
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	public static void SmoothTileOfAreaXYWH(int x, int y, int width, int height)
	{
		x = Math.Clamp(x, 20, Main.maxTilesX - 20);
		y = Math.Clamp(y, 20, Main.maxTilesY - 20);
		if(x + width > Main.maxTilesX - 20)
		{
			x += Main.maxTilesX - 20 - (x + width);
		}
		if (y + height > Main.maxTilesY - 20)
		{
			y += Main.maxTilesY - 20 - (y + height);
		}
		for (int xi = x; xi < x + width; xi += 1)
		{
			for (int yi = y; yi < y + height; yi += 1)
			{
				Tile.SmoothSlope(xi, yi, false);
				WorldGen.TileFrame(xi, yi, true, false);
				WorldGen.SquareWallFrame(xi, yi, true);
			}
		}
	}
}
