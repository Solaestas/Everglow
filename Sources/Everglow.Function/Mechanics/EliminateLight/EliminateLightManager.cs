using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.EliminateLight;

public class EliminateLightManager
{
	// ========== Capacity Limit Constants ==========
	private const int MAX_POINTS = 1024;
	private const int MAX_CIRCLES = 256;
	private const int MAX_POLYGONS = 256;
	private const int MAX_RECTANGLES = 256;

	// ========== Spatial Partitioning Constants ==========
	private const int BUCKET_SIZE = 8;           // Spatial partition bucket size (in tiles)
	private const int CIRCLE_BUCKET_STEP = 8;    // Circle spatial index step size
	private const int TILE_TO_WORLD_SCALE = 16;  // Scale factor from tile coordinates to world coordinates (Terraria 1 tile = 16 pixels)

	// ========== Data Structures ==========
	private static HashSet<Point> points = [];
	private static List<Vector3> circles = []; // x, y, r (world coordinates and radius)
	private static List<List<Vector2>> polygons = [];
	private static List<Rectangle> rectangles = [];

	// Spatial lookup structures for O(1) point queries
	private static HashSet<(int X, int Y)> pointLookup = [];
	private static Dictionary<int, List<int>> rectangleLookupX = []; // x -> rectangle indices
	private static Dictionary<(int X, int Y), List<int>> circleLookup = [];

	private static bool spatialIndexDirty = true;

	// Precompute bounding boxes for polygons
	private struct PolygonBounds
	{
		public List<Vector2> Polygon;
		public float MinX;
		public float MinY;
		public float MaxX;
		public float MaxY;
	}

	private static List<PolygonBounds> polygonBounds = [];

	public static void ApplyEliminateLight(int x, int y, ref Vector3 lightColor)
	{
		// Quick O(1) point check
		if (pointLookup.Contains((x, y)))
		{
			lightColor *= 0;
			return;
		}

		// Rectangle check with spatial indexing
		if (rectangleLookupX.TryGetValue(x, out var indices))
		{
			foreach (int idx in indices)
			{
				var rect = rectangles[idx];
				if (y >= rect.Y && y < rect.Y + rect.Height)
				{
					lightColor *= 0;
					return;
				}
			}
		}

		// Circle check with spatial bucketing
		var bucketKey = (x / BUCKET_SIZE, y / BUCKET_SIZE);
		if (circleLookup.TryGetValue(bucketKey, out var circleIndices))
		{
			foreach (int idx in circleIndices)
			{
				var circle = circles[idx];

				// Convert circle center and radius from world coordinates to tile coordinates for comparison
				float dx = x - circle.X / TILE_TO_WORLD_SCALE;
				float dy = y - circle.Y / TILE_TO_WORLD_SCALE;
				float radius = circle.Z / TILE_TO_WORLD_SCALE;
				if (dx * dx + dy * dy <= radius * radius)
				{
					lightColor *= 0;
					return;
				}
			}
		}

		// Polygon check with bounding box culling
		Vector2 worldPos = new Point(x, y).ToWorldCoordinates();
		foreach (var polyBounds in polygonBounds)
		{
			if (worldPos.X >= polyBounds.MinX && worldPos.X <= polyBounds.MaxX &&
				worldPos.Y >= polyBounds.MinY && worldPos.Y <= polyBounds.MaxY)
			{
				if (MathUtils.IsPointInPolygon(polyBounds.Polygon, worldPos))
				{
					lightColor *= 0;
					return;
				}
			}
		}
	}

	public static void Clear()
	{
		points.Clear();
		circles.Clear();
		polygons.Clear();
		rectangles.Clear();
		pointLookup.Clear();
		rectangleLookupX.Clear();
		circleLookup.Clear();
		polygonBounds.Clear();
		spatialIndexDirty = true;
	}

	public static void Add(int x, int y) => Add(new Point(x, y));

	public static void Add(Point pos)
	{
		if (points.Count < MAX_POINTS && points.Add(pos))
		{
			pointLookup.Add((pos.X, pos.Y));
		}
	}

	public static void AddCircle(Vector2 center, float r)
	{
		if (circles.Count < MAX_CIRCLES)
		{
			Vector3 circle = new Vector3(center, r);
			if (!circles.Contains(circle))
			{
				circles.Add(circle);
				spatialIndexDirty = true;
			}
		}
	}

	public static void AddPolygon(List<Vector2> polygon)
	{
		if (polygon.Count >= 3 && polygons.Count < MAX_POLYGONS && !polygons.Contains(polygon))
		{
			polygons.Add(polygon);

			// Precompute bounds
			float minX = float.MaxValue, minY = float.MaxValue;
			float maxX = float.MinValue, maxY = float.MinValue;
			foreach (var v in polygon)
			{
				if (v.X < minX)
				{
					minX = v.X;
				}

				if (v.Y < minY)
				{
					minY = v.Y;
				}

				if (v.X > maxX)
				{
					maxX = v.X;
				}

				if (v.Y > maxY)
				{
					maxY = v.Y;
				}
			}
			polygonBounds.Add(new PolygonBounds
			{
				Polygon = polygon,
				MinX = minX,
				MinY = minY,
				MaxX = maxX,
				MaxY = maxY,
			});
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
		if (rectangles.Count < MAX_RECTANGLES && !rectangles.Contains(rectangle))
		{
			rectangles.Add(rectangle);

			// Build spatial index for rectangles
			for (int ix = x; ix < x + w; ix++)
			{
				if (!rectangleLookupX.TryGetValue(ix, out var list))
				{
					list = [];
					rectangleLookupX[ix] = list;
				}
				list.Add(rectangles.Count - 1);
			}
		}
	}

	public static void RebuildSpatialIndex()
	{
		if (!spatialIndexDirty)
		{
			return;
		}

		// Rebuild circle spatial index
		circleLookup.Clear();
		for (int i = 0; i < circles.Count; i++)
		{
			var circle = circles[i];
			int centerX = (int)(circle.X / TILE_TO_WORLD_SCALE);
			int centerY = (int)(circle.Y / TILE_TO_WORLD_SCALE);
			int radius = (int)(circle.Z / TILE_TO_WORLD_SCALE) + 1;

			for (int dx = -radius; dx <= radius; dx += CIRCLE_BUCKET_STEP)
			{
				for (int dy = -radius; dy <= radius; dy += CIRCLE_BUCKET_STEP)
				{
					var bucket = (centerX / BUCKET_SIZE + dx, centerY / BUCKET_SIZE + dy);
					if (!circleLookup.TryGetValue(bucket, out var list))
					{
						list = [];
						circleLookup[bucket] = list;
					}
					list.Add(i);
				}
			}
		}

		spatialIndexDirty = false;
	}
}