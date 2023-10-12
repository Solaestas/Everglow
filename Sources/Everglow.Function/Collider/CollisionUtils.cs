using System.Runtime.CompilerServices;

namespace Everglow.Commons.Collider;

public static class CollisionUtils
{
	public const float Epsilon = 1e-3f;

	public static bool SignEquals(float a, float b)
	{
		return (a > 0 && b > 0) || (a < 0 && b < 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Cross(this Vector2 a, Vector2 b)
	{
		return a.X * b.Y - a.Y * b.X;
	}

	public static Vector2 Abs(this Vector2 v)
	{
		return new(Math.Abs(v.X), Math.Abs(v.Y));
	}

	/// <summary>
	/// 判断两个AABB是否相交，边缘重叠视为相交
	/// </summary>
	/// <param name="a"> </param>
	/// <param name="b"> </param>
	/// <returns> </returns>
	public static bool Intersect(this AABB a, AABB b)
	{
		return a.position.X < b.position.X + b.size.X && a.position.X + a.size.X > b.position.X &&
			a.position.Y < b.position.Y + b.size.Y && a.position.Y + a.size.Y > b.position.Y;
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

	public static bool Intersect(float u0, float v0, float u1, float v1)
	{
		return !(u1 > v0 || v1 < u0);
	}

	public static bool Contain(this AABB aabb, Vector2 position)
	{
		return aabb.position.X <= position.X && position.X <= aabb.position.X + aabb.size.X &&
			aabb.position.Y <= position.Y && position.Y <= aabb.position.Y + aabb.size.Y;
	}

	public static float Distance(this float a, float b)
	{
		return Math.Abs(a - b);
	}

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

	public static bool FloatEquals(float a, float b)
	{
		return Math.Abs(a - b) <= Epsilon;
	}

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

	public static AABB ToAABB(this Rectangle rectangle)
	{
		return new(rectangle.TopLeft(), rectangle.Size());
	}
}