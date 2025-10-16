using Everglow.Commons.Physics.Abstracts;
using Everglow.Commons.Physics.DataStructures;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Physics.Colliders;

public class CircleCollider2D : ICollider2D
{
	public Circle circle;

	public CircleCollider2D(Vector2 position, float radius)
	{
		circle = new Circle(position, radius);
	}

	public CircleCollider2D(Circle circle)
	{
		this.circle = circle;
	}

	public Vector2 Position { get => circle.position; set => circle.position = value; }

	public AABB AABB => new(circle.position - new Vector2(circle.radius, circle.radius), new Vector2(circle.radius * 2, circle.radius * 2));

	public float Rotation { get; set; }

	public Vector2 Size { get; set; }

	public bool Intersect(ICollider2D other)
	{
		if (!circle.Intersect(other.AABB))
		{
			return false;
		}

		if (other is PointCollider2D)
		{
			return circle.Contain(other.Position);
		}
		else if (other is AABBCollider2D)
		{
			return true;
		}
		else if (other is EdgeCollider2D edge)
		{
			return edge.edge.Distance(circle.position) < circle.radius;
		}

		return false;
	}

	public bool Contains(Vector2 position) => circle.Contain(position);

	public float Distance(Vector2 position) => circle.Distance(position);

	public Vector3 GetSDFWithGradient(Vector2 position) => throw new NotImplementedException();
}