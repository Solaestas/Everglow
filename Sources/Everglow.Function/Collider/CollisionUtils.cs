using System.Runtime.CompilerServices;

namespace Everglow.Commons.Collider;

public static class CollisionUtils
{
	public const float Epsilon = 1e-3f;

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
		Vector2 width1 = (p2 - p1).SafeNormalize(Vector2.zeroVector).RotatedBy(MathHelper.PiOver2) * line1Width / 2f;
		Vector2 width2 = (p4 - p3).SafeNormalize(Vector2.zeroVector).RotatedBy(MathHelper.PiOver2) * line2Width / 2f;
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

		// 判定是否存在包含
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
			if (totalRad > 6.2831)
			{
				return true;
			}
		}
		for (int j = 0; j < lines1.Length; j++)
		{
			double totalRad = 0;
			for (int i = 0; i < lines2.Length; i++)
			{
				Vector2 side1 = lines2[j] - lines1[i];
				Vector2 side2 = lines2[(j + 1) % lines2.Length] - lines1[i];
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
		float denominator = (p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y);
		float numerator1 = (p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X);
		float numerator2 = (p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X);

		if (denominator == 0)
		{
			return numerator1 == 0 && numerator2 == 0;
		}

		float r = numerator1 / denominator;
		float s = numerator2 / denominator;

		return r >= 0 && r <= 1 && s >= 0 && s <= 1;
	}
}