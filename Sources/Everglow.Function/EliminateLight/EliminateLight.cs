using Everglow.Commons.Utilities;

namespace Everglow.Commons.EliminateLight;

public class EliminateLight
{
	public static List<Point> Point_BlockLightAsWall = new List<Point>();

	/// <summary>
	/// List x, y, r
	/// </summary>
	public static List<Vector3> Point_BlockLight_Circle = new List<Vector3>();

	public static List<List<Vector2>> Point_BlockLight_Polygon = new List<List<Vector2>>();

	public static void AddVirtualWall_Circle(Vector2 center, float r)
	{
		Vector3 circle = new Vector3(center, r);
		if(!Point_BlockLight_Circle.Contains(circle))
		{
			Point_BlockLight_Circle.Add(circle);
		}
	}

	public static void AddVirtualWall_Polygon(List<Vector2> polygon)
	{
		if(polygon.Count >= 3)
		{
			if(!Point_BlockLight_Polygon.Contains(polygon))
			{
				Point_BlockLight_Polygon.Add(polygon);
			}
		}
	}

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

	public static void WallLightWithFakeBlock(int x, int y, ref Vector3 lightColor)
	{
		foreach(var circle in Point_BlockLight_Circle)
		{
			bool inCircle = Math.Pow(x - circle.X / 16, 2) + Math.Pow(y - circle.Y / 16, 2) <= Math.Pow(circle.Z / 16, 2);
			if(inCircle)
			{
				lightColor *= 0;
				return;
			}
		}
		foreach (var polygon in Point_BlockLight_Polygon)
		{
			bool inPolygon = MathUtils.IsPointInPolygon(polygon, new Point(x, y).ToWorldCoordinates());
			if (inPolygon)
			{
				lightColor *= 0;
				return;
			}
		}
		if (Point_BlockLightAsWall.Contains(new Point(x, y)))
		{
			lightColor *= 0;
			return;
		}
	}
}