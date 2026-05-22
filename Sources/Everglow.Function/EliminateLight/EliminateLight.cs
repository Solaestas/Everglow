using Terraria.Graphics.Light;

namespace Everglow.Commons.EliminateLight;

public class EliminateLight
{
	public static List<Point> Point_BlockLightAsWall = new List<Point>();

	public static void AddVirtualWall(int x, int y)
	{
		AddVirtualWall(new Point(x, y));
	}

	public static void AddVirtualWall(Point pos)
	{
		if(!Point_BlockLightAsWall.Contains(pos))
		{
			Point_BlockLightAsWall.Add(pos);
		}
	}

	private static readonly object _lockObj = new object();

	public static void WallLightWithFakeBlock(int x, int y, ref Vector3 lightColor)
	{
		lock (_lockObj)
		{
			for (int t = Point_BlockLightAsWall.Count - 1; t >= 0; t--)
			{
				Point pos = Point_BlockLightAsWall[t];
				if (pos.X == x && pos.Y == y)
				{
					lightColor *= 0;
					Point_BlockLightAsWall.RemoveAt(t);
					break;
				}
			}
		}
	}
}