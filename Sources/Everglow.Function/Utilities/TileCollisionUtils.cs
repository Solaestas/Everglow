namespace Everglow.Commons.Utilities;

public static class TileCollisionUtils
{
	public static bool PlatformCollision(Vector2 checkPoint)
	{
		if (checkPoint.X < 320 || checkPoint.X > Main.maxTilesX * 16 || checkPoint.Y < 320 || checkPoint.Y > Main.maxTilesY * 16)
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

	/// <summary>
	/// Get a terrain normal toward the air.
	/// </summary>
	/// <param name="position"></param>
	/// <param name="maxRange"></param>
	/// <returns></returns>
	public static Vector2 GetTopographicGradient(Vector2 position, int maxRange)
	{
		if (maxRange <= 0)
		{
			maxRange = 1;
		}
		Vector2 normal = Vector2.zeroVector;
		for (int i = -maxRange; i <= maxRange; i++)
		{
			for (int j = -maxRange; j <= maxRange; j++)
			{
				float length = new Vector2(i, j).Length();
				if (length <= maxRange && length > 0)
				{
					if (Collision.SolidCollision(position + new Vector2(i, j) * 16, 0, 0))
					{
						normal -= Utils.SafeNormalize(new Vector2(i, j), Vector2.zeroVector) / (length + 5);
					}
					else
					{
						normal += Utils.SafeNormalize(new Vector2(i, j), Vector2.zeroVector) / (length + 5);
					}
				}
			}
		}
		if (normal.Length() < 0.01)
		{
			return Vector2.zeroVector;
		}
		normal = Utils.SafeNormalize(normal, Vector2.zeroVector);
		return normal;
	}
}