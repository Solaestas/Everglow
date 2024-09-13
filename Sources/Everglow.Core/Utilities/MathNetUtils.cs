using MathNet.Numerics.LinearAlgebra;

namespace Everglow.Commons.Utilities;

public static class MathNetUtils
{
	public static Vector<float> ToMathNetVector(this Vector2 v)
	{
		return Vector<float>.Build.DenseOfArray(new float[] { v.X, v.Y });
	}

	public static Vector<float> ToMathNetVector(this Vector3 v)
	{
		return Vector<float>.Build.DenseOfArray(new float[] { v.X, v.Y, v.Z });
	}

	public static Vector<float> ToMathNetVector(this Vector4 v)
	{
		return Vector<float>.Build.DenseOfArray(new float[] { v.X, v.Y, v.Z, v.W });
	}

	public static Vector2 ToVector2(this Vector<float> v)
	{
		return new Vector2(v[0], v[1]);
	}

	public static Vector3 ToVector3(this Vector<float> v)
	{
		return new Vector3(v[0], v[1], v[2]);
	}

	public static Vector4 ToVector4(this Vector<float> v)
	{
		return new Vector4(v[0], v[1], v[2], v[3]);
	}
}