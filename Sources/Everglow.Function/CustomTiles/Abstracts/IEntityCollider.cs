using Everglow.Commons.CustomTiles.Core;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.CustomTiles.Abstracts;

public interface IEntityCollider<TEntity> : IBox
	where TEntity : Entity
{
	private const float Sqrt2Div2 = 0.707106781186f;

	public TEntity Entity { get; }

	public RigidEntity Ground { get; set; }

	public float OffsetY { get; set; }

	public Vector2 OldPosition { get; set; }

	public void OnCollision(CollisionResult result);

	public void OnLeave();

	public void Prepare()
	{
		if (Ground != null && OffsetY != 0)
		{
			Entity.position.Y += OffsetY;
			OffsetY = 0;
		}
		OldPosition = Entity.position;
	}

	public void Update()
	{
		var stride = Entity.position - OldPosition;
		if (Ground != null) // Keep entity over the ground and move with current rg2d.
		{
			var acc = Ground.StandAccelerate(this);
			if (stride.Y * Gravity < 0)
			{
				Entity.velocity += acc;
				OnLeave();
			}
			stride += acc;
			Ground = null;
		}
		Entity.position = OldPosition;
		foreach (var result in ColliderManager.Instance.Move(this, stride))
		{
			if (IsGround(result.Normal, Gravity))
			{
				Ground = result.Collider;
				Entity.velocity.Y = 0;
			}
			else if (result.Normal == new Vector2(0, -1))
			{
				Entity.velocity.Y = -CollisionUtils.Epsilon;
			}
			OnCollision(result);
		}
	}

	private static bool IsGround(Vector2 normal, float gravity)
	{
		return Vector2.Dot(normal, gravity * Vector2.UnitY) > Sqrt2Div2;
	}
}