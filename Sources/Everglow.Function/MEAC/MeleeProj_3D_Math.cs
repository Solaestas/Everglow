namespace Everglow.Commons.MEAC;

public abstract partial class MeleeProj_3D : ModProjectile, IWarpProjectile_warpStyle2, IBloomProjectile
{
	/// <summary>
	/// Return a 2D position of the weapon tip projected from 3D space, which can be used for player arm rotation and slash effect drawing. The position is relative to the projectile center, so adding Projectile.Center will get the world position.
	/// </summary>
	/// <returns></returns>
	public Vector2 CurrentWeaponTipPosition()
	{
		Vector3 currentPos3D = WeaponAxis + new Vector3(0, 0, CenterZ);
		Vector2 currentPos = Project(currentPos3D, ProjectionMatrix);
		return currentPos;
	}

	public static Vector2 Project(Vector3 point, Matrix ProjectionMatrix)
	{
		Vector4 homogenousPoint = Vector4.Transform(new Vector4(point, 1), ProjectionMatrix);

		// Normalized Device Coordinates
		if (homogenousPoint.W != 0)
		{
			float xNDC = -homogenousPoint.X / homogenousPoint.W;
			float yNDC = -homogenousPoint.Y / homogenousPoint.W;

			float xScreen = xNDC * Main.screenWidth / 2f;
			float yScreen = yNDC * Main.screenHeight / 2f;

			return new Vector2(xScreen, yScreen);
		}
		throw new InvalidOperationException("W component of the projected point is zero.");
	}

	public List<Vector2> Project(List<Vector3> ring, Matrix ProjectionMatrix)
	{
		List<Vector2> projectedRing = new List<Vector2>();

		foreach (var point in ring)
		{
			projectedRing.Add(Project(point, ProjectionMatrix));
		}
		return projectedRing;
	}

	public List<Vector3> ToLongitudeRing(List<Vector2> ring, float longitude)
	{
		Matrix rotationMatrix = Matrix.CreateRotationY(longitude);

		List<Vector3> xozRing = new List<Vector3>(ring.Count);

		foreach (var point in ring)
		{
			Vector3 originalPoint = new Vector3(point, 0);
			Vector3 rotatedPoint = Vector3.Transform(originalPoint, rotationMatrix);
			rotatedPoint.Z += CenterZ;
			xozRing.Add(rotatedPoint);
		}
		return xozRing;
	}

	public List<Vector3> ToLatitudeRing(List<Vector2> ring, float latitude, float radius)
	{
		List<Vector3> newRing = new List<Vector3>(ring.Count);

		foreach (var point in ring)
		{
			Vector3 originalPoint = new Vector3(point.X, 0, point.Y);
			Vector3 resizedPoint = originalPoint * MathF.Cos(latitude);
			Vector3 latitudePoint = resizedPoint + new Vector3(0, MathF.Sin(latitude), 0) * radius;
			latitudePoint.Z += CenterZ;
			newRing.Add(latitudePoint);
		}
		return newRing;
	}

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

	public void RotateToPerpendicular(Vector3 fixAxis, ref Vector3 rotateAxis)
	{
		Vector3 perpendicularAxis = Vector3.Normalize(Vector3.Cross(fixAxis, rotateAxis));
		float angle = Vector3.Dot(fixAxis, rotateAxis) / fixAxis.Length() / rotateAxis.Length();
		angle = MathF.Acos(angle);
		Quaternion rotation = Quaternion.CreateFromAxisAngle(perpendicularAxis, MathHelper.PiOver2 - angle);
		rotateAxis = Vector3.Transform(rotateAxis, rotation);
	}

	public void RotateMainAxis(float angle, Vector3 rotatedAxis, ref Vector3 mainAxis)
	{
		Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.Normalize(rotatedAxis), angle);
		mainAxis = Vector3.Transform(mainAxis, rotation);
		WeaponAxis = Vector3.Transform(WeaponAxis, rotation);
	}

	public float GetSizeZ(float coordZ)
	{
		return CenterZ / coordZ;
	}

	/// <summary>
	/// Solve f(x) = x * e^x = y for x, given y. Only the principal branch is implemented, which is the one used in most cases. The input y must be greater than -1/e.
	/// </summary>
	/// <param name="x"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public float LambertW0(double x)
	{
		const double e = Math.E;
		const double eps = 1e-12;

		if (x < -1 / e - eps)
		{
			throw new ArgumentOutOfRangeException(nameof(x), "x must be greater than -1/e");
		}
		if (x == 0)
		{
			return 0;
		}
		if (Math.Abs(x - (-1 / e)) < eps)
		{
			return -1;
		}

		double w;
		if (x > 0)
		{
			w = Math.Log(1 + x);
		}
		else
		{
			w = -1 + Math.Sqrt(2 * (1 + Math.E * x));
		}

		// 4 times Newton iteration to refine the approximation
		for (int i = 0; i < 4; i++)
		{
			double ew = Math.Exp(w);
			double wew = w * ew;
			double f = wew - x;
			double df = ew * (w + 1);
			w -= f / df;
		}
		return (float)w;
	}

	// Rotation Speed Curve
	public double BaseMeleeSpeed = 2.5;
	public double BaseDecaySpeed = 0.88;

	/// <summary>
	/// A const value, if change the BaseMeleeSpeed, BaseDecaySpeed or MaxAttackTime, this value should be recalculated. It represents the integral of the rotation speed curve over time, which is used to solve for the decay coefficient b when given a new melee speed a.
	/// </summary>
	private double FixedIntegral()
	{
		return (Math.Pow(BaseDecaySpeed, MaxAttackTime) - 1) / Math.Log(BaseDecaySpeed) * BaseMeleeSpeed;
	}

	/// <summary>
	/// Input a new meleeSpeed and output b value.
	/// </summary>
	public float SolveB(double newMeleeSpeed)
	{
		double a = newMeleeSpeed;
		double K = FixedIntegral();

		double t = MaxAttackTime * a / K;
		double arg = -t * Math.Exp(-t);
		double w = LambertW0(arg);
		double b = Math.Exp(-w / MaxAttackTime - a / K);

		return (float)b;
	}
}