using Everglow.Commons.Utilities;

namespace Everglow.Commons.EliminateLight;

public class EliminateLight
{
	private static List<Point> point_Kill_Light = new List<Point>();

	/// <summary>
	/// List x, y, r
	/// </summary>
	private static List<Vector3> circle_Kill_Light = new List<Vector3>();

	private static List<List<Vector2>> polygon_Kill_Light = new List<List<Vector2>>();

	private static List<Rectangle> rectangle_Kill_Light = new List<Rectangle>();

	public static void Clear()
	{
		point_Kill_Light.Clear();
		circle_Kill_Light.Clear();
		polygon_Kill_Light.Clear();
		rectangle_Kill_Light.Clear();
	}

	public static void AddVirtualWall_Circle(Vector2 center, float r)
	{
		Vector3 circle = new Vector3(center, r);
		if (!circle_Kill_Light.Contains(circle) && circle_Kill_Light.Count < 256)
		{
			circle_Kill_Light.Add(circle);
		}
	}

	public static void AddVirtualWall_Polygon(List<Vector2> polygon)
	{
		if (polygon.Count >= 3)
		{
			if (!polygon_Kill_Light.Contains(polygon) && polygon_Kill_Light.Count < 256)
			{
				polygon_Kill_Light.Add(polygon);
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
		AddVirtualWall_Rectangle_XYWH(x0, y0, x1 - x0, y1 - y0);
	}

	public static void AddVirtualWall_Rectangle_XYWH(int x, int y, int w, int h)
	{
		Rectangle rectangle = new Rectangle(x, y, w, h);
		if(!rectangle_Kill_Light.Contains(rectangle) && rectangle_Kill_Light.Count < 256)
		{
			rectangle_Kill_Light.Add(new Rectangle(x, y, w, h));
		}
	}

	public static void AddVirtualWall(int x, int y)
	{
		AddVirtualWall(new Point(x, y));
	}

	public static void AddVirtualWall(Point pos)
	{
		if (!point_Kill_Light.Contains(pos) && point_Kill_Light.Count < 1024)
		{
			point_Kill_Light.Add(pos);
		}
	}

	public static void CheckEliminateLight(int x, int y, ref Vector3 lightColor)
	{
		foreach(var rectangle in rectangle_Kill_Light)
		{
			bool inRectangle = x >= rectangle.X && x < rectangle.X + rectangle.Width && y >= rectangle.Y && y < rectangle.Y + rectangle.Height;
			if (inRectangle)
			{
				lightColor *= 0;
				return;
			}
		}
		foreach (var circle in circle_Kill_Light)
		{
			bool inCircle = Math.Pow(x - circle.X / 16, 2) + Math.Pow(y - circle.Y / 16, 2) <= Math.Pow(circle.Z / 16, 2);
			if (inCircle)
			{
				lightColor *= 0;
				return;
			}
		}
		foreach (var polygon in polygon_Kill_Light)
		{
			bool inPolygon = MathUtils.IsPointInPolygon(polygon, new Point(x, y).ToWorldCoordinates());
			if (inPolygon)
			{
				lightColor *= 0;
				return;
			}
		}
		if (point_Kill_Light.Contains(new Point(x, y)))
		{
			lightColor *= 0;
			return;
		}
	}
}