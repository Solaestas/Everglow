namespace Everglow.Commons.Utilities;

public static class TileCollisionUtils
{
	public static bool PlatformCollision(Vector2 checkPoint)
	{
		if(checkPoint.X < 320 || checkPoint.X > Main.maxTilesX * 16 || checkPoint.Y < 320 || checkPoint.Y > Main.maxTilesY * 16)
		{
			return false;
		}
		Vector2 coord = checkPoint / 16f;
		Tile tile = Main.tile[(int)coord.X, (int)coord.Y];
		return TileID.Sets.Platforms[tile.TileType] || Collision.SolidCollision(checkPoint, 0, 0);
	}
	public static bool CanPlaceMultiAtTopTowardsUpRight(int i, int j, int width, int height)
	{
		if (i < 20 || i + width > Main.maxTilesX - 20 || j < 20 || j + height > Main.maxTilesY - 20)
		{
			return false;
		}
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height + 1; y++)
			{
				Tile checkTile = Main.tile[i + x, j - y];
				if (checkTile.HasTile && y != 0)
				{
					return false;
				}
				if (y == 0)
				{
					if (!checkTile.HasTile || checkTile.Slope != SlopeType.Solid)
					{
						return false;
					}
				}
			}
		}
		return true;
	}
}
