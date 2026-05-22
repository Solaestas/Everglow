namespace Everglow.Commons.EliminateLight;

public class EliminateLight
{
	public static List<Point> Point_BlockLightAsWall = new List<Point>();

	public static void AddVirtualWall_Rectangle_XXYY(int x0, int y0, int x1, int y1)
	{
		if (x0 > x1)
		{
			(x0, x1) = (x1, x0);
		}
		if (y0 > y1)
		{
			(y0, y1) = (y1, y0);
		}
		for (int x = x0; x <= x1; x++)
		{
			for (int y = y0; y <= y1; y++)
			{
				AddVirtualWall(x, y);
			}
		}
	}

	public static void AddVirtualWall_Rectangle_XYWH(int x, int y, int w, int h)
	{
		AddVirtualWall_Rectangle_XXYY(x, y, x + w, y + h);
	}

	public static void AddVirtualWall(int x, int y)
	{
		AddVirtualWall(new Point(x, y));
	}

	public static void AddVirtualWall(Point pos)
	{
		if (!Point_BlockLightAsWall.Contains(pos))
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