namespace Everglow.Commons.Utilities;

public static partial class MathUtils
{
	/// <summary>
	/// 若 <paramref name="val" /> 与 <paramref name="target" /> 距离小于 <paramref name="maxMove" />
	/// 则返回target <br> </br> 否则返回 <paramref name="val" /> 向 <paramref name="target" /> 移动
	/// <paramref name="maxMove" /> 距离后的值
	/// </summary>
	/// <param name="val"> </param>
	/// <param name="target"> </param>
	/// <param name="maxMove"> </param>
	/// <returns> </returns>
	public static float Approach(float val, float target, float maxMove)
	{
		if (val <= target)
		{
			return Math.Min(val + maxMove, target);
		}

		return Math.Max(val - maxMove, target);
	}

	/// <summary>
	/// :[)
	/// </summary>
	/// <param name="value"> </param>
	/// <param name="from"> </param>
	/// <param name="to"> </param>
	/// <returns> </returns>
	public static float Wrap(this float value, float from, float to)
	{
		while (value >= to)
		{
			value -= to - from;
		}

		while (value < from)
		{
			value += to - from;
		}

		return value;
	}

	/// <summary>
	/// <paramref name="value" /> &lt; <paramref name="from" /> 返回 <paramref name="from" /><br>
	/// </br><paramref name="value" /> &gt; <paramref name="to" /> 返回 <paramref name="to" /><br>
	/// </br> 否则返回value
	/// </summary>
	/// <param name="value"> </param>
	/// <param name="from"> </param>
	/// <param name="to"> </param>
	/// <returns> </returns>
	public static float Clamp(this float value, float from, float to)
	{
		return Math.Clamp(value, from, to);
	}

	/// <summary>
	/// 若from &lt;= to，则返回1，否则返回0
	/// </summary>
	/// <param name="from"> </param>
	/// <param name="to"> </param>
	/// <returns> </returns>
	public static float Step(this float from, float to)
	{
		return from <= to ? 1 : 0;
	}

	/// <summary>
	/// 返回 <paramref name="a" /> 与 <paramref name="b" /> 中绝对值较大的值
	/// </summary>
	/// <param name="a"> </param>
	/// <param name="b"> </param>
	/// <returns> </returns>
	public static float AbsMax(float a, float b)
	{
		if (Math.Abs(a) > Math.Abs(b))
		{
			return a;
		}

		return b;
	}

	/// <summary>
	/// 返回 <paramref name="a" /> 与 <paramref name="b" /> 中绝对值较小的值
	/// </summary>
	/// <param name="a"> </param>
	/// <param name="b"> </param>
	/// <returns> </returns>
	public static float AbsMin(float a, float b)
	{
		if (Math.Abs(a) > Math.Abs(b))
		{
			return b;
		}

		return a;
	}

	public static float Sqrt(this float num)
	{
		return (float)Math.Sqrt(num);
	}

	public static float Cos(this float num)
	{
		return (float)Math.Cos(num);
	}

	public static float Sin(this float num)
	{
		return (float)Math.Sin(num);
	}

	/// <summary>
	/// n的阶乘
	/// </summary>
	/// <param name="n"></param>
	/// <returns></returns>
	public static int Factorial(int n)
	{
		if (n <= 0)
		{
			return 1;
		}
		else
		{
			return n * Factorial(n - 1);
		}
	}

	/// <summary>
	/// 组合数
	/// </summary>
	/// <param name="n"></param>
	/// <param name="m"></param>
	/// <returns></returns>
	public static int Combination(int n, int m)
	{
		return Factorial(n) / (Factorial(m) * Factorial(n - m));
	}
}