namespace Everglow.Commons.Utilities;

public static partial class MathUtils
{
	/// <summary>
 /// smoothstep插值
 /// </summary>
 /// <param name="value"> </param>
 /// <param name="from"> </param>
 /// <param name="to"> </param>
 /// <returns> </returns>
	public static float SmoothStep(this float value, float from, float to)
	{
		value = Clamp((value - from) / (to - from), 0, 1);
		return value * value * (3 - 2 * value);
	}

	/// <summary>
	/// 两个 <seealso cref="SmoothStep" /> 相减
	/// </summary>
	/// <param name="from"> </param>
	/// <param name="to"> </param>
	/// <param name="value"> </param>
	/// <param name="width"> </param>
	/// <returns> </returns>
	public static float SmoothStepMinus(this float value, float from, float to, float width)
	{
		return from.SmoothStep(from + width, value) - (to - width).SmoothStep(to, value);
	}
}