using Everglow.Commons.Physics.Abstracts;
using Everglow.Commons.Physics.DataStructures;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Physics.Colliders;

[Obsolete("Not fully implemented.", true)]
public class TriangleCollider2D : IPolygonalCollider2D
{
	public Vector2 Position { get; set; }

	public Vector2 Size { get; set; }

	public Vector2 LineDir { get; set; }

	public Vector2 Center
	{
		get => Position;
		set => Position = value;
	}

	public float Rotation { get; set; }

	public AABB AABB => throw new NotImplementedException();

	public bool Intersect(ICollider2D other) => throw new NotImplementedException();

	public bool Contains(Vector2 position) => GetSDFWithGradient(position).X < 0;

	public float Distance(Vector2 position) => throw new NotImplementedException();

	public Polygon GetPolygon()
	{
		if (LineDir.X * LineDir.Y > 0)
		{
			int sign = Math.Sign(LineDir.X);
			return new Polygon(new List<Vector2>
			{
				Center + sign * Size,
				Center - sign * Size,
				Center + sign * new Vector2(-Size.X, Size.Y),
			});
		}
		else
		{
			int sign = Math.Sign(LineDir.X);
			return new Polygon(new List<Vector2>
			{
				Center + sign * new Vector2(Size.X, -Size.Y),
				Center - sign * Size,
				Center + sign * new Vector2(-Size.X, Size.Y),
			});
		}
	}

	public Vector3 GetSDFWithGradient(Vector2 pos)
	{
		Vector2 p = pos - Center;
		Vector3 box = SDFUtils.SdgBox(p, Size);
		Vector3 line = SDFUtils.SdgLine(p, LineDir);
		if (box.X > line.X)
		{
			return box;
		}
		else
		{
			return line;
		}
	}
}