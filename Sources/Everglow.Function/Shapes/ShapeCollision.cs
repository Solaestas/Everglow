using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Everglow.Commons.Shapes.Shape;

namespace Everglow.Commons.Shapes;
internal static class ShapeCollision
{
	#region Normal
	private static float CrossProduct(Vector2 A, Vector2 B, Vector2 C) => (B.X - A.X) * (C.Y - A.Y) - (B.Y - A.Y) * (C.X - A.X);
	public static List<Vector2> QuickHull(IEnumerable<Vector2> vs)
	{
		if (vs.Count() < 3)
		{
			throw new ArgumentException("The count must not be less than 3.");
		}
		var sortedVertices = vs.OrderBy(v => v.X).ThenBy(v => v.Y).ToList();
		var lowerHull = new List<Vector2>();
		foreach (var v in sortedVertices)
		{
			while (lowerHull.Count >= 2 && CrossProduct(lowerHull[^2], lowerHull[^1], v) <= 0)
			{
				lowerHull.RemoveAt(lowerHull.Count - 1);
			}
			lowerHull.Add(v);
		}
		var upperHull = new List<Vector2>();
		for (int i = sortedVertices.Count - 1; i >= 0; i--)
		{
			var v = sortedVertices[i];
			while (upperHull.Count >= 2 && CrossProduct(upperHull[^2], upperHull[^1], v) <= 0)
			{
				upperHull.RemoveAt(upperHull.Count - 1);
			}
			upperHull.Add(v);
		}
		upperHull.RemoveAt(upperHull.Count - 1);
		lowerHull.RemoveAt(lowerHull.Count - 1);
		lowerHull.AddRange(upperHull);
		return lowerHull;
	}
	public static bool Contains(this IConvex<Vector2> convex, Vector2 point)
	{
		var vs = convex.ConvexVertex();
		var len = vs.Length;
		for (int i = 0; i < len; i++)
		{
			Vector2 v1 = vs[i];
			Vector2 v2 = vs[(i + 1) % len];
			if (CrossProduct(v1, v2, point) < 0)
			{
				return false;
			}
		}
		return true;
	}
	public static bool Intersect(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End, float precision = 0.000001f)
	{
		float
			x1 = line1Start.X, y1 = line1Start.Y,
			x2 = line1End.X, y2 = line1End.Y,
			x3 = line2Start.X, y3 = line1Start.Y,
			x4 = line2End.X, y4 = line2End.Y;
		if (Math.Max(x1, x2) < Math.Min(x3, x4) || Math.Max(x3, x4) < Math.Min(x1, x2) ||
			Math.Max(y1, y2) < Math.Min(y3, y4) || Math.Max(y3, y4) < Math.Min(y1, y2))
		{
			return false;
		}
		float c1 = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
		float c2 = (x4 - x3) * (y2 - y3) - (y4 - y3) * (x2 - x3);
		float c3 = (x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1);
		float c4 = (x2 - x1) * (y4 - y1) - (y2 - y1) * (x4 - x1);

		return c1 * c2 <= 0 && c3 * c4 <= 0;
	}
	#endregion

	#region SAT
	public static bool Intersect_SAT(ISAT<Vector2> sat1, ISAT<Vector2> sat2)
	{
		List<Vector2> axes = new();
		// 获取 sat1 和 sat2 的所有分离轴
		axes.AddRange(sat1.GetAxes());
		axes.AddRange(sat2.GetAxes());
		foreach (Vector2 axis in axes)
		{
			// 在分离轴上投影 sat1 和 sat2 的区间
			sat1.Project(axis, out float min1, out float max1);
			sat2.Project(axis, out float min2, out float max2);

			// 检查投影是否重叠，如果不重叠则碰撞不存在
			if (max1 < min2 || max2 < min1)
			{
				return false;
			}
		}
		return true;
	}
	#endregion

	#region GJK
	public static bool Intersect_GJK(IGJK<Vector2> gjk1, IGJK<Vector2> gjk2)
	{
		Vector2 dir = Vector2.UnitX;
		List<Vector2> simplex = new() { Support(gjk1, gjk2, dir) };
		dir = Vector2.Normalize(Vector2.Zero - simplex[0]);
		while (true)
		{
			Vector2 A = Support(gjk1, gjk2, dir);
			if (Vector2.Dot(A, dir) < 0)
			{
				return false;
			}
			simplex.Add(A);
			if (HandleSimplex(simplex, ref dir))
			{
				return true;
			}
		}
	}
	private static bool HandleSimplex(List<Vector2> simplex, ref Vector2 dir)
	{
		return simplex.Count == 2 ? HandleLine(simplex, ref dir) : HandleTriangle(simplex, ref dir);
	}
	private static bool HandleTriangle(List<Vector2> simplex, ref Vector2 dir)
	{
		Vector2 C = simplex[^3];
		Vector2 B = simplex[^2];
		Vector2 A = simplex[^1];
		Vector2 AB = Vector2.Normalize(B - A);
		Vector2 AC = Vector2.Normalize(C - A);
		Vector2 AO = Vector2.Normalize(Vector2.Zero - A);
		Vector2 ABperp = TripleProd(AC, AB, AB);
		Vector2 ACperp = TripleProd(AB, AC, AC);
		if (Vector2.Dot(ABperp, AO) > 0)
		{
			simplex.RemoveAt(simplex.Count - 3);
			dir = ABperp;
			return false;
		}
		if (Vector2.Dot(ACperp, AO) > 0)
		{
			simplex.RemoveAt(simplex.Count - 2);
			dir = ACperp;
			return false;
		}
		return true;
	}
	private static bool HandleLine(List<Vector2> simplex, ref Vector2 dir)
	{
		Vector2 B = simplex[^2];
		Vector2 A = simplex[^1];
		Vector2 AB = Vector2.Normalize(B - A);
		Vector2 AO = Vector2.Normalize(Vector2.Zero - A);
		dir = TripleProd(AB, AO, AB);
		return false;
	}
	private static Vector2 TripleProd(Vector2 v1, Vector2 v2, Vector2 v3)
	{
		Vector3 v = Vector3.Cross(Vector3.Cross(new Vector3(v1, 0), new Vector3(v2, 0)), new Vector3(v3, 0));
		return Vector2.Normalize(new Vector2(v.X, v.Y));
	}
	private static Vector2 Support(IGJK<Vector2> gjk1, IGJK<Vector2> gjk2, Vector2 dir)
	{
		return gjk1.FurthestPoint(dir) - gjk2.FurthestPoint(-dir);
	}
	#endregion

	#region EPA
	public static bool Intersect_EPA(IEPA<Vector2> epa1, IEPA<Vector2> epa2, out IConvex<Vector2> convex)
	{
		convex = null;
		Vector2 dir = Vector2.UnitX;
		List<Vector2> simplex = new() { Support(epa1, epa2, dir) };
		dir = Vector2.Normalize(Vector2.Zero - simplex[0]);
		while (true)
		{
			Vector2 A = Support(epa1, epa2, dir);
			if (Vector2.Dot(A, dir) < 0)
			{
				return false;
			}
			simplex.Add(A);
			if (HandleSimplex(simplex, ref dir))
			{
				break;
			}
		}
		List<Vector2> polytope = new(simplex);
		while (true)
		{
			Vector2 support = Support(epa1, epa2, dir);
			if (Vector2.Dot(support, dir) < 0)
			{
				return false;
			}
			polytope.Add(support);
			if (UpdateSimplex(polytope, ref dir))
			{
				break;
			}
		}
		convex = new NormalConvex2D(polytope);
		return true;
	}
	private static bool UpdateSimplex(List<Vector2> polytope, ref Vector2 dir)
	{
		Vector2 closestPoint = GetClosestPointToOrigin(polytope);

		if (Vector2.Dot(closestPoint, dir) >= 0)
		{
			return true;
		}
		int index = FindClosestEdge(polytope, closestPoint);
		Vector2 A = polytope[index];
		Vector2 B = polytope[(index + 1) % polytope.Count];
		Vector2 edgeDir = Vector2.Normalize(B - A);
		dir = TripleProd(edgeDir, A - closestPoint, edgeDir);
		return false;
	}
	private static Vector2 GetClosestPointToOrigin(List<Vector2> polytope)
	{
		Vector2 closestPoint = polytope[0];
		float closestDistSq = closestPoint.LengthSquared();

		for (int i = 1; i < polytope.Count; i++)
		{
			Vector2 point = polytope[i];
			float distSq = point.LengthSquared();

			if (distSq < closestDistSq)
			{
				closestPoint = point;
				closestDistSq = distSq;
			}
		}

		return closestPoint;
	}
	private static int FindClosestEdge(List<Vector2> polytope, Vector2 closestPoint)
	{
		int index = 0;
		float closestDist = Vector2.DistanceSquared(closestPoint, GetClosestPointOnEdge(polytope[0], polytope[^1], closestPoint));

		for (int i = 0; i < polytope.Count - 1; i++)
		{
			Vector2 pointA = polytope[i];
			Vector2 pointB = polytope[i + 1];
			float dist = Vector2.DistanceSquared(closestPoint, GetClosestPointOnEdge(pointA, pointB, closestPoint));

			if (dist < closestDist)
			{
				index = i;
				closestDist = dist;
			}
		}

		return index;
	}
	private static Vector2 GetClosestPointOnEdge(Vector2 pointA, Vector2 pointB, Vector2 closestPoint)
	{
		Vector2 edgeDir = Vector2.Normalize(pointB - pointA);
		float t = Vector2.Dot(closestPoint - pointA, edgeDir);
		return pointA + t * edgeDir;
	}
	#endregion
}
