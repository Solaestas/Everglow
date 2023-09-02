namespace Everglow.Commons.Utilities;

public class ProjectileUtils
{
	public static bool IsSafeInTheWorld(Projectile projectile)
	{
		return IsSafeInTheWorld(projectile.TopLeft) && IsSafeInTheWorld(projectile.TopRight) && IsSafeInTheWorld(projectile.BottomLeft) && IsSafeInTheWorld(projectile.BottomRight);
	}
	private static bool IsSafeInTheWorld(Vector2 position)
	{
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			return false;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			return false;
		}
		return true;
	}
}