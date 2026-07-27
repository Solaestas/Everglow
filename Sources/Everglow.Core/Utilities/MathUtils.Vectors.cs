using System.Runtime.CompilerServices;

namespace Everglow.Commons.Utilities;

public static partial class MathUtils
{
	/// <summary>
	/// 向量点积
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static float Dot(this Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;

	/// <summary>
	/// 向量自点积
	/// </summary>
	/// <param name="a"></param>
	/// <returns></returns>
	public static float Dot(this Vector2 a) => Dot(a, a);

	/// <summary>
	/// 向量叉积
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Cross(this Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;

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
		if (Dot(diff, diff) < maxMove * maxMove) // 这里用叉积来计算平方距离，避免开方
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

	/// <summary>
	/// Converts a set of spherical coordinates to their equivalent Cartesian coordinates.
	/// </summary>
	/// <remarks>The method assumes that the angles are specified in radians and that the radius is non-negative.
	/// Supplying a negative radius may result in unexpected output.</remarks>
	/// <param name="sphericalCoordinates">A <see cref="Vector3"/> representing the spherical coordinates, where the X component is the radius, the Y
	/// component is the polar angle (theta), and the Z component is the azimuthal angle (phi), all in radians.</param>
	/// <returns>A <see cref="Vector3"/> containing the Cartesian coordinates corresponding to the specified spherical coordinates.</returns>
	public static Vector3 SphericalToCartesian(Vector3 sphericalCoordinates)
	{
		float r = sphericalCoordinates.X;
		float theta = sphericalCoordinates.Y;
		float phi = sphericalCoordinates.Z;

		float z = r * (float)Math.Sin(theta) * (float)Math.Cos(phi);
		float x = r * (float)Math.Sin(theta) * (float)Math.Sin(phi);
		float y = r * (float)Math.Cos(theta);

		return new Vector3(x, y, z);
	}

	/// <summary>
	/// Converts a set of Cartesian coordinates to their equivalent spherical coordinates.
	/// </summary>
	/// <remarks>The conversion assumes a right-handed coordinate system. The polar angle (theta) ranges from 0 to
	/// π, and the azimuthal angle (phi) ranges from -π/2 to π/2. All angles are returned in radians.</remarks>
	/// <param name="cartesianCoordinates">The Cartesian coordinates to convert, represented as a <see cref="Vector3"/> where X, Y, and Z correspond to the
	/// respective axes.</param>
	/// <returns>A <see cref="Vector3"/> containing the spherical coordinates, where the X component is the radius, the Y component
	/// is the polar angle (theta), and the Z component is the azimuthal angle (phi), all in radians.</returns>
	public static Vector3 CartesianToSpherical(Vector3 cartesianCoordinates)
	{
		double x = cartesianCoordinates.X;
		double y = cartesianCoordinates.Y;
		double z = cartesianCoordinates.Z;

		double r = Math.Sqrt(x * x + y * y + z * z);
		double theta = Math.Acos(y / r);

		double phi = MathHelper.PiOver2 - Math.Atan(z / x);
		if (x < 0)
		{
			phi = -Math.Atan(z / x) - MathHelper.PiOver2;
		}
		return new Vector3((float)r, (float)theta, (float)phi);
	}

	/// <summary>
	/// Rotates the specified axis so that it becomes perpendicular to a given fixed axis.
	/// </summary>
	/// <remarks>This method adjusts the orientation of the rotateAxis vector so that it is perpendicular to the
	/// fixAxis vector, preserving the original direction as much as possible. The operation modifies the rotateAxis
	/// parameter directly.</remarks>
	/// <param name="fixAxis">The fixed reference axis to which the rotated axis will be made perpendicular. This vector should be normalized
	/// before calling the method.</param>
	/// <param name="rotateAxis">The axis to rotate. This vector is modified in place to become perpendicular to the fixed axis.</param>
	public static void RotateToPerpendicular(Vector3 fixAxis, ref Vector3 rotateAxis)
	{
		Vector3 perpendicularAxis = Vector3.Normalize(Vector3.Cross(fixAxis, rotateAxis));
		float angle = Vector3.Dot(fixAxis, rotateAxis) / fixAxis.Length() / rotateAxis.Length();
		angle = MathF.Acos(angle);
		Quaternion rotation = Quaternion.CreateFromAxisAngle(perpendicularAxis, MathHelper.PiOver2 - angle);
		rotateAxis = Vector3.Transform(rotateAxis, rotation);
	}
}