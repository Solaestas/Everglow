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
	/// 计算三个点的方向（叉积结果）
	/// </summary>
	/// <returns>0=共线，1=顺时针，2=逆时针</returns>
	public static int Orientation(Vector2 p, Vector2 q, Vector2 r)
	{
		float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
		if (Math.Abs(val) < 1e-6)
		{
			return 0; // 共线
		}

		return val > 0 ? 1 : 2; // 顺时针/逆时针
	}

}