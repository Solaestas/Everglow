namespace Everglow.Commons.MEAC;

public abstract partial class MeleeProj_3D : ModProjectile, IWarpProjectile_warpStyle2, IBloomProjectile
{
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

	public float GetSizeZ(float coordZ)
	{
		return CenterZ / coordZ;
	}
}