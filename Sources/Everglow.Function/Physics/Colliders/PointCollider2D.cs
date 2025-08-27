using Everglow.Commons.Physics.Abstracts;
using Everglow.Commons.Physics.DataStructures;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Physics.Colliders;

public class PointCollider2D : ICollider2D
{
	public PointCollider2D(Vector2 position)
	{
		Position = position;
	}

	public PointCollider2D()
	{
	}

	public Vector2 Position { get; set; }

	public AABB AABB => new(Position, 1, 1);

	public float Rotation { get; set; }

	public Vector2 Size { get; set; }

	public bool Intersect(ICollider2D other)
	{
		if (!other.AABB.Contain(Position))
		{
			return false;
		}

		if (other is PointCollider2D)
		{
			return other.Position == Position;
		}

		Ins.Logger.Warn($"未定义的碰撞：{other.GetType()} 与 {GetType()}");
		return false;
	}

	public bool Contains(Vector2 position) => Position == position;

	public float Distance(Vector2 position) => Vector2.Distance(Position, position);

	public Vector3 GetSDFWithGradient(Vector2 position) => throw new NotImplementedException();
}