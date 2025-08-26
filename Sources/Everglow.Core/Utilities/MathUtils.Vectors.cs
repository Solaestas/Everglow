using System.Runtime.CompilerServices;

namespace Everglow.Commons.Utilities;

public static partial class MathUtils
{
	/// <summary>
	/// 向量叉积
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Cross(this Vector2 a, Vector2 b) => a.X * b.Y + a.Y * b.X;

	/// <summary>
	/// 向量绝对值
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public static Vector2 Abs(this Vector2 v) => new(Math.Abs(v.X), Math.Abs(v.Y));

	public static Vector2 Lerp(this float value, Vector2 from, Vector2 to)
	{
		return (1 - value) * from + to * value;
	}

	/// <summary>
	/// 若 <paramref name="val" /> 与 <paramref name="target" /> 距离小于 <paramref name="maxMove" />
	/// 则返回target <br> </br> 否则返回 <paramref name="val" /> 向 <paramref name="target" /> 移动
	/// <paramref name="maxMove" />距离后的值
	/// </summary>
	/// <param name="val"></param>
	/// <param name="target"></param>
	/// <param name="maxMove"></param>
	/// <returns> </returns>
	public static Vector2 Approach(this Vector2 val, Vector2 target, float maxMove)
	{
		var diff = target - val;
		if (Cross(diff, diff) < maxMove * maxMove) // 这里用叉积来计算平方距离，避免开方
		{
			return target;
		}

		return val + diff.NormalizeSafe() * maxMove;
	}

	/// <summary>
	/// 对X与Y分别进行Clamp
	/// </summary>
	/// <param name="value"></param>
	/// <param name="from"></param>
	/// <param name="to"></param>
	/// <returns> </returns>
	public static Vector2 Clamp(this Vector2 value, Vector2 from, Vector2 to)
	{
		return new Vector2(value.X.Clamp(from.X, to.X), value.Y.Clamp(from.Y, to.Y));
	}

	/// <summary>
	/// 返回 <paramref name="a" /> 与 <paramref name="b" /> 中由x，y分量绝对值最大值构成的新向量
	/// </summary>
	/// <param name="a"> </param>
	/// <param name="b"> </param>
	/// <returns> </returns>
	public static Vector2 AbsMax(Vector2 a, Vector2 b)
	{
		return new Vector2(AbsMax(a.X, b.X), AbsMax(a.Y, b.Y));
	}

	/// <summary>
	/// 返回 <paramref name="a" /> 与 <paramref name="b" /> 中由x，y分量绝对值最小值构成的新向量
	/// </summary>
	/// <param name="a"> </param>
	/// <param name="b"> </param>
	/// <returns> </returns>
	public static Vector2 AbsMin(Vector2 a, Vector2 b)
	{
		return new Vector2(AbsMin(a.X, b.X), AbsMin(a.Y, b.Y));
	}

	/// <summary>
	/// 返回 <paramref name="vector" /> 的单位向量，若为零向量则返回UnitX
	/// </summary>
	/// <param name="vector"> </param>
	/// <returns> </returns>
	public static Vector2 NormalizeSafe(this Vector2 vector)
	{
		float len = (vector.X * vector.X + vector.Y * vector.Y).Sqrt();
		if (len == 0)
		{
			return Vector2.UnitX;
		}
		else
		{
			return new Vector2(vector.X / len, vector.Y / len);
		}
	}

	/// <summary>
	/// 返回vector顺时针旋转90度并且标准化的法向量
	/// </summary>
	/// <param name="vector"> </param>
	/// <returns> </returns>
	public static Vector2 NormalLine(this Vector2 vector)
	{
		return new Vector2(-vector.Y, vector.X).NormalizeSafe();
	}
}