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
}