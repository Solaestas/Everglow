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
}
