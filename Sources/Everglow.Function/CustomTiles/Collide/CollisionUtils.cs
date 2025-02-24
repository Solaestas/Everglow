using System.Runtime.CompilerServices;
using Everglow.Commons.CustomTiles.Collide;
using Everglow.Commons.CustomTiles.DataStructures;

namespace Everglow.Commons.CustomTiles.Collide;

public static class CollisionUtils
{
	public const float Epsilon = 1e-4f;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Cross(this Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;

	public static Vector2 Abs(this Vector2 v) => new(Math.Abs(v.X), Math.Abs(v.Y));

	public static AABB ToAABB(this Edge line)
	{
		var min = Vector2.Min(line.begin, line.end);
		var max = Vector2.Max(line.begin, line.end);
		return new AABB(min, max - min);
	}

	public static AABB ToAABB(this Vertices vertices)
	{
		var min = new Vector2(float.MaxValue, float.MaxValue);
		var max = new Vector2(float.MinValue, float.MinValue);
		foreach (var v in vertices)
		{
			min = Vector2.Min(min, v);
			max = Vector2.Max(max, v);
		}
		return new AABB(min, max - min);
	}

	/// <summary>
	/// 判断两个AABB是否相交，边缘重叠视为相交
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static bool Intersect(this AABB a, AABB b)
	{
		if (a.position.X > b.position.X + b.size.X || a.position.X + a.size.X < b.position.X ||
			a.position.Y > b.position.Y + b.size.Y || a.position.Y + a.size.Y < b.position.Y)
		{
			return false;
		}
		return true;
	}

	public static bool Intersect(this AABB a, AABB b, out AABB area)
	{
		if (a.position.X > b.position.X + b.size.X || a.position.X + a.size.X < b.position.X ||
			a.position.Y > b.position.Y + b.size.Y || a.position.Y + a.size.Y < b.position.Y)
		{
			area = default;
			return false;
		}
		var topLeft = Vector2.Max(a.TopLeft, b.TopLeft);
		var bottomRight = Vector2.Min(a.BottomRight, b.BottomRight);
		area = new AABB(topLeft, bottomRight - topLeft);
		return true;
	}

	public static bool Intersect(this Edge a, Edge b)
	{
		if (!a.ToAABB().Intersect(b.ToAABB()))
			return false;
		float factor = (b.end - b.begin).Cross(a.begin - b.begin);
		float u = (a.end - a.begin).Cross(a.begin - b.begin);
		float denom = (a.end - a.begin).Cross(b.end - b.begin);
		if (Math.Abs(denom) < Epsilon)
			return false;

		factor /= denom;
		u /= denom;
		if (0 <= factor && factor <= 1 && 0 <= u && u <= 1)
			return true;
		return false;
	}

	public static bool Intersect(this Edge a, Edge b, out float factor)
	{
		if (!a.ToAABB().Intersect(b.ToAABB()))
		{
			factor = 0;
			return false;
		}
		factor = (b.end - b.begin).Cross(a.begin - b.begin);
		float u = (a.end - a.begin).Cross(a.begin - b.begin);
		float denom = (b.end - b.begin).Cross(a.end - a.begin);
		if (Math.Abs(denom) < Epsilon)
			return false;

		factor /= denom;
		u /= denom;
		if (0 <= factor && factor <= 1 && 0 <= u && u <= 1)
			return true;
		return false;
	}

	public static bool Intersect(this Edge line, AABB aabb)
	{
		if (aabb.Contain(line.begin) || aabb.Contain(line.end))
			return true;
		if (line.Intersect(new Edge(aabb.TopLeft, aabb.BottomRight)) || line.Intersect(new Edge(aabb.TopRight, aabb.BottomLeft)))
			return true;
		return false;
	}

	public static bool Intersect(this AABB aabb, Edge line)
	{
		if (aabb.Contain(line.begin) || aabb.Contain(line.end))
			return true;
		if (line.Intersect(new Edge(aabb.TopLeft, aabb.BottomRight)) || line.Intersect(new Edge(aabb.TopRight, aabb.BottomLeft)))
			return true;
		return false;
	}

	public static bool Intersect(this Edge edge, AABB aabb, out Array2<Direction> directions)
	{
		directions = new Array2<Direction>();
		var it = directions.GetEnumerator();
		it.MoveNext();
		bool result = false;
		if (new Edge(aabb.TopLeft, aabb.TopRight).Intersect(edge))
		{
			it.Current = Direction.Top;
			if (!it.MoveNext())
				return true;
			result = true;
		}
		if (new Edge(aabb.TopRight, aabb.BottomRight).Intersect(edge))
		{
			it.Current = Direction.Right;
			if (!it.MoveNext())
				return true;
			result = true;
		}
		if (new Edge(aabb.BottomRight, aabb.BottomLeft).Intersect(edge))
		{
			it.Current = Direction.Bottom;
			if (!it.MoveNext())
				return true;
			result = true;
		}
		if (new Edge(aabb.BottomLeft, aabb.TopLeft).Intersect(edge))
		{
			it.Current = Direction.Left;
			if (!it.MoveNext())
				return true;
			result = true;
		}
		return result;
	}

	public static bool Intersect(this Circle circle, AABB aabb)
	{
		Vector2 or = (circle.position - aabb.Center).Abs();
		if (or.X > aabb.Width / 2)
		{
			if (or.Y > aabb.Height / 2)
				return or.Distance(aabb.size / 2) <= circle.radius;
			else
			{
				return or.X - aabb.Width / 2 <= circle.radius;
			}
		}
		else
		{
			return or.Y - aabb.Height / 2 <= circle.radius;
		}
	}

	public static bool Intersect(this Circle circle, AABB aabb, out Direction dir)
	{
		Vector2 or = circle.position - aabb.Center;
		bool flipX = or.X < 0;
		bool flipY = or.Y < 0;
		or = or.Abs();
		dir = Direction.None;
		bool result;
		if (or.X > aabb.Width / 2)
		{
			if (or.Y > aabb.Height / 2)
			{
				result = or.Distance(aabb.size / 2) <= circle.radius;
				if (result)
				{
					dir = (flipX, flipY) switch
					{
						(true, true) => Direction.TopLeft,
						(false, true) => Direction.TopRight,
						(true, false) => Direction.BottomLeft,
						(false, false) => Direction.BottomRight
					};
				}
				return result;
			}
			else
			{
				result = or.X - aabb.Width / 2 <= circle.radius;
				if (result)
					dir = flipX ? Direction.Left : Direction.Right;
				return result;
			}
		}
		else
		{
			result = or.Y - aabb.Height / 2 <= circle.radius;
			if (result)
				dir = flipY ? Direction.Top : Direction.Bottom;
			return result;
		}
	}

	public static bool Intersect(float u0, float v0, float u1, float v1)
	{
		return !(u1 > v0 || v1 < u0);
	}

	public static bool Contain(this AABB aabb, Vector2 position)
	{
		return aabb.position.X <= position.X && position.X <= aabb.position.X + aabb.size.X &&
			aabb.position.Y <= position.Y && position.Y <= aabb.position.Y + aabb.size.Y;
	}

	public static bool Contain(this Circle circle, Vector2 position) => position.Distance(circle.position) <= circle.radius;

	public static float Distance(this Edge edge, Vector2 point)
	{
		if (edge.begin == edge.end)
			return point.Distance(edge.begin);

		Vector2 v = edge.end - edge.begin;
		Vector2 w = point - edge.begin;
		float c1 = Vector2.Dot(w, v);
		if (c1 <= 0)
			return point.Distance(edge.begin);

		float c2 = Vector2.Dot(v, v);
		if (c2 <= c1)
			return point.Distance(edge.end);

		return point.Distance(edge.begin + v * c1 / c2);
	}

	public static float Distance(this float a, float b) => Math.Abs(a - b);

	public static AABB Scan(this AABB aabb, Vector2 move)
	{
		var vs = new Vector2[4] { aabb.TopLeft, aabb.BottomRight, aabb.TopLeft + move, aabb.BottomRight + move };
		Vector2 min = new(float.MaxValue, float.MaxValue), max = new(float.MinValue, float.MinValue);
		foreach (var v in vs)
		{
			min = Vector2.Min(v, min);
			max = Vector2.Max(v, max);
		}
		return new AABB(min, max - min);
	}

	public static bool FloatEquals(float a, float b) => Math.Abs(a - b) <= Epsilon;

	public static float Projection(Vector2 dir, Vector2 vec)
	{
		return Vector2.Dot(Vector2.Normalize(dir), vec);
	}

	public static List<float> Projection(Vector2 dir, params Vector2[] vec)
	{
		var vs = new List<float>(vec.Length);
		for (int i = 0; i < vec.Length; i++)
		{
			vs.Add(Projection(dir, vec[i]));
		}
		return vs;
	}

	public static List<float> Projection(Vector2 dir, IEnumerable<Vector2> vec)
	{
		var vs = new List<float>();
		foreach (var v in vec)
		{
			vs.Add(Projection(dir, v));
		}
		return vs;
	}

	public static Vector2 MinValue(this IEnumerable<Vector2> vector2s)
	{
		var v = new Vector2(float.MaxValue, float.MaxValue);
		foreach (var it in vector2s)
		{
			v = Vector2.Min(it, v);
		}
		return v;
	}

	public static Vector2 MaxValue(this IEnumerable<Vector2> vector2s)
	{
		var v = new Vector2(0, 0);
		foreach (var it in vector2s)
		{
			v = Vector2.Max(it, v);
		}
		return v;
	}

	public static AABB ToAABB(this Rectangle rectangle) => new(rectangle.TopLeft(), rectangle.Size());
	/// <summary>
	/// 两个斜矩形
	/// </summary>
	/// <param name="p1"></param>
	/// <param name="p2"></param>
	/// <param name="line1Width"></param>
	/// <param name="p3"></param>
	/// <param name="p4"></param>
	/// <param name="line2Width"></param>
	/// <returns></returns>
	public static bool Intersect(Vector2 p1, Vector2 p2, float line1Width, Vector2 p3, Vector2 p4, float line2Width)
	{
		// 计算四边形的四条边
		Vector2 width1 = Utils.SafeNormalize(p2 - p1, Vector2.zeroVector).RotatedBy(MathHelper.PiOver2) * line1Width / 2f;
		Vector2 width2 = Utils.SafeNormalize(p4 - p3, Vector2.zeroVector).RotatedBy(MathHelper.PiOver2) * line2Width / 2f;
		Vector2[] lines1 = { p1 - width1, p2 - width1, p2 + width1, p1 + width1 };
		Vector2[] lines2 = { p3 - width2, p4 - width2, p4 + width2, p3 + width2 };

		// 检查每条边是否与另一矩形相交
		for (int i = 0; i < lines1.Length; i++)
		{
			for (int j = 0; j < lines2.Length; j++)
			{
				if (DoLinesIntersect(lines1[i], lines1[(i + 1) % lines1.Length], lines2[j], lines2[(j + 1) % lines2.Length]))
				{
					return true;
				}
			}
		}
		//判定是否存在包含
		for (int i = 0; i < lines1.Length; i++)
		{
			double totalRad = 0;
			for (int j = 0; j < lines2.Length; j++)
			{
				Vector2 side1 = lines2[j] - lines1[i];
				Vector2 side2 = lines2[(j + 1) % lines2.Length] - lines1[i];
				double cosTheta = Vector2.Dot(side1, side2) / (side2.Length() * side1.Length());
				double rad = Math.Acos(cosTheta);
				totalRad += rad;
			}
			if(totalRad > 6.2831)
			{
				return true;
			}
		}
		for (int j = 0; j < lines1.Length; j++)
		{
			double totalRad = 0;
			for (int i = 0; i < lines2.Length; i++)
			{
				Vector2 side1 = lines1[i] - lines2[j];
				Vector2 side2 = lines1[(i + 1) % lines1.Length] - lines2[j];
				double cosTheta = Vector2.Dot(side1, side2) / (side2.Length() * side1.Length());
				double rad = Math.Acos(cosTheta);
				totalRad += rad;
			}
			if (totalRad > 6.2831)
			{
				return true;
			}
		}
		return false;
	}
	/// <summary>
	/// 两线相交
	/// </summary>
	/// <param name="p1"></param>
	/// <param name="p2"></param>
	/// <param name="p3"></param>
	/// <param name="p4"></param>
	/// <returns></returns>
	public static bool DoLinesIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
	{
		float denominator = ((p4.Y - p3.Y) * (p2.X - p1.X)) - ((p4.X - p3.X) * (p2.Y - p1.Y));
		float numerator1 = ((p4.X - p3.X) * (p1.Y - p3.Y)) - ((p4.Y - p3.Y) * (p1.X - p3.X));
		float numerator2 = ((p2.X - p1.X) * (p1.Y - p3.Y)) - ((p2.Y - p1.Y) * (p1.X - p3.X));

		if (denominator == 0)
		{
			return numerator1 == 0 && numerator2 == 0;
		}

		float r = numerator1 / denominator;
		float s = numerator2 / denominator;

		return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
	}
}