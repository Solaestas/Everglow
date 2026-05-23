using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.EliminateLight;

public class EliminateLightManager
{
	private static List<Point> points = [];

	/// <summary>
	/// List x, y, r
	/// </summary>
	private static List<Vector3> circles = [];

	private static List<List<Vector2>> polygons = [];

	private static List<Rectangle> rectangles = [];

	public static void ApplyEliminateLight(int x, int y, ref Vector3 lightColor)
	{
		foreach (var rectangle in rectangles)
		{
			bool inRectangle = x >= rectangle.X && x < rectangle.X + rectangle.Width && y >= rectangle.Y && y < rectangle.Y + rectangle.Height;
			if (inRectangle)
			{
				lightColor *= 0;
				return;
			}
		}
		foreach (var circle in circles)
		{
			bool inCircle = Math.Pow(x - circle.X / 16, 2) + Math.Pow(y - circle.Y / 16, 2) <= Math.Pow(circle.Z / 16, 2);
			if (inCircle)
			{
				lightColor *= 0;
				return;
			}
		}
		foreach (var polygon in polygons)
		{
			bool inPolygon = MathUtils.IsPointInPolygon(polygon, new Point(x, y).ToWorldCoordinates());
			if (inPolygon)
			{
				lightColor *= 0;
				return;
			}
		}
		if (points.Contains(new Point(x, y)))
		{
			lightColor *= 0;
			return;
		}
	}

	public static void Clear()
	{
		points.Clear();
		circles.Clear();
		polygons.Clear();
		rectangles.Clear();
	}

	public static void Add(int x, int y)
	{
		Add(new Point(x, y));
	}

	public static void Add(Point pos)
	{
		if (!points.Contains(pos) && points.Count < 1024)
		{
			points.Add(pos);
		}
	}

	public static void AddCircle(Vector2 center, float r)
	{
		Vector3 circle = new Vector3(center, r);
		if (!circles.Contains(circle) && circles.Count < 256)
		{
			circles.Add(circle);
		}
	}

	public static void AddPolygon(List<Vector2> polygon)
	{
		if (polygon.Count >= 3)
		{
			if (!polygons.Contains(polygon) && polygons.Count < 256)
			{
				polygons.Add(polygon);
			}
		}
	}

	public static void AddRectangleXXYY(int x0, int y0, int x1, int y1)
	{
		if (x0 > x1)
		{
			(x0, x1) = (x1, x0);
		}
		if (y0 > y1)
		{
			(y0, y1) = (y1, y0);
		}
		AddRectangleXYWH(x0, y0, x1 - x0, y1 - y0);
	}

	public static void AddRectangleXYWH(int x, int y, int w, int h)
	{
		Rectangle rectangle = new Rectangle(x, y, w, h);
		if (!rectangles.Contains(rectangle) && rectangles.Count < 256)
		{
			rectangles.Add(new Rectangle(x, y, w, h));
		}
	}
}