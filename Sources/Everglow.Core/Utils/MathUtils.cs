using Everglow.Common.DataStructures;

namespace Everglow.Common.Utils
{
	public static class MathUtils
	{
		/// <summary>
		/// 若<paramref name="val"/>与<paramref name="target"/>距离小于<paramref name="maxMove"/>则返回target
		/// <br></br>
		/// 否则返回<paramref name="val"/>向<paramref name="target"/>移动<paramref name="maxMove"/>距离后的值
		/// </summary>
		/// <param name="val"></param>
		/// <param name="target"></param>
		/// <param name="maxMove"></param>
		/// <returns></returns>
		public static float Approach(float val, float target, float maxMove)
		{
			if (val <= target)
			{
				return Math.Min(val + maxMove, target);
			}
			return Math.Max(val - maxMove, target);
		}
		/// <summary>
		/// 若<paramref name="val"/>与<paramref name="target"/>距离小于<paramref name="maxMove"/>则返回target
		/// <br></br>
		/// 否则返回<paramref name="val"/>向<paramref name="target"/>移动<paramref name="maxMove"/>距离后的值
		/// </summary>
		/// <param name="val"></param>
		/// <param name="target"></param>
		/// <param name="maxMove"></param>
		/// <returns></returns>
		public static Vector2 Approach(this Vector2 val, Vector2 target, float maxMove)
		{
			if (Vector2.Distance(val, target) < maxMove)
			{
				return target;
			}
			return val + (target - val).NormalizeSafe() * maxMove;
		}
		/// <summary>
		/// :[)
		/// </summary>
		/// <param name="value"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
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
		/// 对X与Y分别进行Clamp
		/// </summary>
		/// <param name="value"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static Vector2 Wrap(this Vector2 value, Vector2 from, Vector2 to)
		{
			return new Vector2(value.X.Clamp(from.X, to.X), value.Y.Clamp(from.Y, to.Y));
		}
		/// <summary>
		/// 将<paramref name="vector"/>投影到<paramref name="axis"/>上的长度
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static float Projection(this Vector2 axis, Vector2 vector)
		{
			return Vector2.Dot(axis, vector) / axis.Length();
		}
		/// <summary>
		/// <paramref name="value"/> &lt; <paramref name="from"/>返回<paramref name="from"/> 
		/// <br></br>
		/// <paramref name="value"/> &gt; <paramref name="to"/>返回<paramref name="to"/>
		/// <br></br>
		/// 否则返回value
		/// </summary>
		/// <param name="value"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static float Clamp(this float value, float from, float to)
		{
			if (value < from)
			{
				return from;
			}
			else if (value > to)
			{
				return to;
			}

			return value;
		}
		/// <summary>
		/// 若from <= to返回1，否则返回0
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static float Step(this float from, float to)
		{
			return from <= to ? 1 : 0;
		}
		/// <summary>
		/// smoothstep插值
		/// </summary>
		/// <param name="value"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static float SmoothStep(this float value, float from, float to)
		{
			value = Clamp(0, 1, (value - from) / (to - from));
			return value * value * (3 - 2 * value);
		}
		/// <summary>
		/// 两个<seealso cref="SmoothStep"/>相减
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="value"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		public static float SmoothStepMinus(this float value, float from, float to, float width)
		{
			return from.SmoothStep(from + width, value) - (to - width).SmoothStep(to, value);
		}
		/// <summary>
		/// 返回<paramref name="a"/>与<paramref name="b"/>中绝对值较大的值
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float AbsMax(float a, float b)
		{
			if (Math.Abs(a) > Math.Abs(b))
			{
				return a;
			}
			return b;
		}
		/// <summary>
		/// 返回<paramref name="a"/>与<paramref name="b"/>中绝对值较小的值
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float AbsMin(float a, float b)
		{
			if (Math.Abs(a) > Math.Abs(b))
			{
				return b;
			}
			return a;
		}
		/// <summary>
		/// 返回<paramref name="a"/>与<paramref name="b"/>中由x，y分量绝对值最大值构成的新向量
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2 AbsMax(Vector2 a, Vector2 b)
		{
			return new Vector2(AbsMax(a.X, b.X), AbsMax(a.Y, b.Y));
		}
		/// <summary>
		/// 返回<paramref name="a"/>与<paramref name="b"/>中由x，y分量绝对值最小值构成的新向量
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2 AbsMin(Vector2 a, Vector2 b)
		{
			return new Vector2(AbsMin(a.X, b.X), AbsMin(a.Y, b.Y));
		}
		/// <summary>
		/// 返回<paramref name="vector"/>的单位向量，若为零向量则返回UnitX
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
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
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Vector2 NormalLine(this Vector2 vector) => new Vector2(-vector.Y, vector.X).NormalizeSafe();
		/// <summary>
		/// 返回vector的角度，若vector为Zero则返回defaultValue
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
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
		public static float Sqrt(this float num) => (float)Math.Sqrt(num);
		public static float Cos(this float num) => (float)Math.Cos(num);
		public static float Sin(this float num) => (float)Math.Sin(num);
		public static float Lerp(this float value, float from, float to) => (1 - value) * from + to * value;
		public static Vector2 Lerp(this float value, Vector2 from, Vector2 to) => (1 - value) * from + to * value;
		public static Rotation Lerp(this float value, Rotation from, Rotation to) => from.Lerp(to, value);
	}
}
