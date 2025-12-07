namespace Everglow.Commons.Utilities;

public static partial class MathUtils
{
	/// <summary>
	/// 将 <paramref name="vector" /> 投影到 <paramref name="axis" /> 上的长度
	/// </summary>
	/// <param name="axis"> </param>
	/// <param name="vector"> </param>
	/// <returns> </returns>
	public static float Projection(this Vector2 axis, Vector2 vector)
	{
		return Vector2.Dot(axis, vector) / axis.Length();
	}

	/// <summary>
	/// 返回vector的角度，若vector为Zero则返回defaultValue
	/// </summary>
	/// <param name="vector"> </param>
	/// <param name="defaultValue"> </param>
	/// <returns> </returns>
	public static float ToRotationSafe(this Vector2 vector, float defaultValue = default)
	{
		float len = (vector.X * vector.X + vector.Y * vector.Y).Sqrt();
		if (len == 0)
		{
			return defaultValue;
		}
		else
		{
			return (float)Math.Atan2(vector.Y, vector.X);
		}
	}

	/// <summary>
	/// 判断点是否在多边形内（含边界），同时校验多边形有效性（无相交边）
	/// </summary>
	/// <param name="polygon">多边形顶点列表（按索引连接，最后一个连0号）</param>
	/// <param name="point">目标点</param>
	/// <returns>true=在内部/边界，false=在外部</returns>
	public static bool IsPointInPolygon(List<Vector2> polygon, Vector2 point)
	{
		// 1. 基础校验（顶点数≥3才是有效多边形）
		if (polygon == null || polygon.Count < 3)
		{
			return false;
		}

		// 2. 校验多边形边是否相交（相邻边除外）
		// CheckPolygonEdges(polygon);

		// 3. 射线法判断点是否在多边形内
		int vertexCount = polygon.Count;
		bool isInside = false;
		for (int i = 0, j = vertexCount - 1; i < vertexCount; j = i++)
		{
			Vector2 p1 = polygon[i];
			Vector2 p2 = polygon[j];

			// 边界判定：点在边上直接返回true
			if (IsPointOnLineSegment(point, p1, p2))
			{
				return true;
			}

			// 射线与边相交判断（核心逻辑）
			if (((p1.Y > point.Y) != (p2.Y > point.Y)) // 点在边的Y范围之间
				&& (point.X < (p2.X - p1.X) * (point.Y - p1.Y) / (p2.Y - p1.Y) + p1.X)) // 射线与边相交
			{
				isInside = !isInside;
			}
		}
		return isInside;
	}

	/// <summary>
	/// 判断点是否在线段上
	/// </summary>
	public static bool IsPointOnLineSegment(Vector2 point, Vector2 p1, Vector2 p2)
	{
		// 1. 点在直线上（叉积为0）
		float cross = (point.X - p1.X) * (p2.Y - p1.Y) - (point.Y - p1.Y) * (p2.X - p1.X);
		if (Math.Abs(cross) > 1e-6)
		{
			return false; // 浮点数误差容忍
		}

		// 2. 点的坐标在segments的边界内
		float minX = Math.Min(p1.X, p2.X);
		float maxX = Math.Max(p1.X, p2.X);
		float minY = Math.Min(p1.Y, p2.Y);
		float maxY = Math.Max(p1.Y, p2.Y);
		return point.X >= minX - 1e-6 && point.X <= maxX + 1e-6
			   && point.Y >= minY - 1e-6 && point.Y <= maxY + 1e-6;
	}

	/// <summary>
	/// 确定点 <paramref name="r"/> 相对于由点 <paramref name="p"/> 和 <paramref name="q"/> 构成的有向线段的方向。
	/// <br/>此方法基于二维叉积，用于判断从 <paramref name="p"/> 经过 <paramref name="q"/> 转向 <paramref name="r"/> 的方向。
	/// </summary>
	/// <param name="p">起点</param>
	/// <param name="q">中间点</param>
	/// <param name="r">终点</param>
	/// <returns>
	/// 表示 <paramref name="p"/> -> <paramref name="q"/> -> <paramref name="r"/> 转向的符号值。
	/// <list type="table">
	/// <listheader>
	/// 	<term>Return Value</term>
	/// 	<description>Meaning</description>
	/// </listheader>
	/// <item>
	/// 	<term>-1</term>
	/// 	<description>逆时针 (Counter-Clockwise) 或 点 r 在线段 pq 的左侧</description>
	/// </item>
	/// <item>
	/// 	<term>0</term>
	/// 	<description>共线 (Collinear)</description>
	/// </item>
	/// <item>
	/// 	<term>1</term>
	/// 	<description>顺时针 (Clockwise) 或 点 r 在线段 pq 的右侧</description>
	/// </item>
	/// </list>
	/// </returns>
	public static int Orientation(Vector2 p, Vector2 q, Vector2 r)
	{
		float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
		if (Math.Abs(val) < 1e-6)
		{
			return 0; // 共线
		}

		return Math.Sign(val); // 顺时针/逆时针
	}

	/// <summary>
	/// 判断两条线段是否相交（含端点）
	/// </summary>
	public static bool DoLineSegmentsIntersectByOrientation(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
	{
		// 跨立实验+叉积判断
		int o1 = Orientation(a1, a2, b1);
		int o2 = Orientation(a1, a2, b2);
		int o3 = Orientation(b1, b2, a1);
		int o4 = Orientation(b1, b2, a2);

		// 普通相交（非共线）
		if (o1 != o2 && o3 != o4)
		{
			return true;
		}

		// 共线情况：判断端点是否在线段上
		if (o1 == 0 && IsPointOnLineSegment(b1, a1, a2))
		{
			return true;
		}

		if (o2 == 0 && IsPointOnLineSegment(b2, a1, a2))
		{
			return true;
		}

		if (o3 == 0 && IsPointOnLineSegment(a1, b1, b2))
		{
			return true;
		}

		if (o4 == 0 && IsPointOnLineSegment(a2, b1, b2))
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// 判断两条线段是否相交
	/// </summary>
	/// <param name="p1"></param>
	/// <param name="p2"></param>
	/// <param name="p3"></param>
	/// <param name="p4"></param>
	/// <returns></returns>
	public static bool DoLineSegmentsIntersectByAlgebra(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
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

	/// <summary>
	/// 校验多边形边是否相交（相邻边除外，避免自相交多边形）(未完成)
	/// </summary>
	[Obsolete]
	public static void DoPolygonEdgesIntersect(List<Vector2> polygon)
	{
		int n = polygon.Count;
		for (int i = 0; i < n; i++)
		{
			// 当前边：i -> (i+1)%n
			int i1 = i;
			int i2 = (i + 1) % n;
			Vector2 a1 = polygon[i1];
			Vector2 a2 = polygon[i2];

			// 检查与后续非相邻边是否相交
			for (int j = i + 2; j < n; j++)
			{
				// 跳过最后一条边与第一条边的重复检查（i=n-1时，j会到n，无需处理）
				if (i == n - 1 && j == n)
				{
					continue;
				}

				int j1 = j;
				int j2 = (j + 1) % n;
				Vector2 b1 = polygon[j1];
				Vector2 b2 = polygon[j2];

				// 检测两条线段是否相交
				// if (CheckLinesIntersectByOrientation(a1, a2, b1, b2))
				// {
				// 	return false;
				// }
			}
		}
	}
}