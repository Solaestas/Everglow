using Everglow.Commons.Physics.Abstracts;
using Everglow.Commons.Physics.DataStructures;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Physics.Colliders;

public class AABBCollider2D : IPolygonalCollider2D
{
	public AABBCollider2D()
	{
	}

	public AABBCollider2D(Vector2 position, Vector2 size)
	{
		aabb = new AABB(position, size);
	}

	public AABBCollider2D(AABB aabb)
	{
		this.aabb = aabb;
	}

	public AABB aabb;

	public Vector2 Position { get => aabb.position; set => aabb.position = value; }

	public Vector2 Center { get; set; }

	public Vector2 Size { get; set; }

	public float Rotation { get; set; }

	public AABB AABB => aabb;

	public bool Contains(Vector2 position)
	{
		return position.X > Center.X - Size.X && position.X < Center.X + Size.X
			&& position.Y > Center.Y - Size.Y && position.Y < Center.Y + Size.Y;
	}

	public bool Intersect(ICollider2D other)
	{
		if (!other.AABB.Intersect(aabb))
		{
			return false;
		}

		if (other is PointCollider2D point)
		{
			return aabb.Contain(point.Position);
		}
		else if (other is AABBCollider2D)
		{
			return true;
		}

		return false;
	}

	public float Distance(Vector2 position) => aabb.Distance(position);

	public Polygon GetPolygon() => new Polygon([
		Center - Size,
		Center + new Vector2(-Size.X, Size.Y),
		Center + Size,
		Center + new Vector2(Size.X, -Size.Y),
	]);

	public Vector3 GetSDFWithGradient(Vector2 pos) => SDFUtils.SdgBox(pos - Center, Size);
}