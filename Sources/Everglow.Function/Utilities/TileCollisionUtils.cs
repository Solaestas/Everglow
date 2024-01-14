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
}
