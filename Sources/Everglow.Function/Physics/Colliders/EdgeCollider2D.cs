using Everglow.Commons.Collider;
using Everglow.Commons.Physics.Abstracts;
using Everglow.Commons.Physics.DataStructures;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Physics.Colliders;

public class EdgeCollider2D : ICollider2D
{
	public Edge edge;

	public EdgeCollider2D()
	{
	}

	public EdgeCollider2D(Edge edge)
	{
		this.edge = edge;
	}

	public Vector2 Position { get => edge.begin; set => edge.begin = value; }

	public Physics.DataStructures.AABB AABB => edge.ToAABB();

	public Vector2 Begin { get => edge.begin; set => edge.begin = value; }

	public Vector2 End { get => edge.end; set => edge.end = value; }

	public float Rotation { get; set; }

	public Vector2 Size { get; set; }

	public bool Contains(Vector2 position) => edge.Contain(position);

	public float Distance(Vector2 position) => edge.Distance(position);

	public bool Intersect(ICollider2D other)
	{
		if (!other.AABB.Intersect(edge))
		{
			return false;
		}

		if (other is PointCollider2D)
		{
			float min = Math.Min(edge.begin.X, edge.end.X);
			float max = Math.Max(edge.begin.X, edge.end.X);
			return min <= other.Position.X && other.Position.X <= max &&
				(edge.begin - other.Position).Cross(edge.end - other.Position) < CollisionUtils.Epsilon;
		}
		else if (other is AABBCollider2D)
		{
			return true;
		}
		else if (other is EdgeCollider2D edge)
		{
			return edge.edge.Intersect(this.edge);
		}

		return false;
	}

	public Vector3 GetSDFWithGradient(Vector2 position) => throw new NotImplementedException();
}