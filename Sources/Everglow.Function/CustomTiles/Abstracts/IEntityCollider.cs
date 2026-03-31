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
		// Still some issues:
		// When CustomTile embedding with tile, and entity clips inside it by accident, entity will chop into CustomTile.
		bool entityCollideTile = Collision.SolidCollision(Entity.position, Entity.width, Entity.height);
		ColliderManager.EnableHook = true;
		bool entityCollideTile_Next = Collision.SolidCollision(Entity.position + Entity.velocity + new Vector2(0, -0.4f + OffsetY - 1f), Entity.width, Entity.height);
		ColliderManager.EnableHook = false;
		if (Ground != null && OffsetY != 0)
		{
			if (!entityCollideTile && !entityCollideTile_Next)
			{
				// This code is for preventing player from sticking to the ground. Only when player stand on customtile and not on solid tile or try to step up to a solid tile, the player will get unstuck. This code will not cause player to get unstuck when player is standing on solid tile, which is for preventing player from getting unstuck when standing on solid tile and trying to step up to customtile.
				CancelAttachToSolidTile();
			}
		}
		OldPosition = Entity.position;
	}

	public void CancelAttachToSolidTile()
	{
		Entity.position.Y += OffsetY;
		OffsetY = 0;
	}

	public void Update()
	{
		var stride = Entity.position - OldPosition;

		// Apply standing behavior
		if (Ground != null)
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
			else if (result.Normal == -Vector2.UnitY * Gravity
				&& result.Collider.Velocity.Y * Gravity <= 0) // Added this condition for preventing player from sticking to box's bottom when box is moving downward.
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