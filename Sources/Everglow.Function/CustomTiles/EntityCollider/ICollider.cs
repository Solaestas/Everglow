using Everglow.Commons.CustomTiles.Tiles;

namespace Everglow.Commons.CustomTiles.EntityCollider;

public interface IEntityCollider
{
	public Direction AttachDir { get; set; }

	public RigidEntity AttachTile { get; set; }

	public AttachType AttachType { get; set; }

	public bool CanAttach { get; }

	public Entity Entity { get; }

	public Vector2 DeltaVelocity { get; set; }

	public Direction Ground { get; }

	public Vector2 Position { get; set; }

	public Vector2 AbsoluteVelocity { get; set; }

	public void OnAttach();

	public void OnCollision(RigidEntity tile, Direction dir, ref RigidEntity newAttach);

	public void OnUpdate();

	public static void Update(IEntityCollider collider, bool ignorePlats = false)
	{
		if (collider.Position == Vector2.Zero)
		{
			collider.Position = collider.Entity.position;
			collider.DeltaVelocity = collider.AbsoluteVelocity = collider.Entity.velocity;
			return;
		}

		Vector2 move = collider.Entity.position - collider.Position + collider.DeltaVelocity;//计算获得物块推动的强制位移
		collider.AbsoluteVelocity = collider.Entity.velocity + collider.DeltaVelocity;//当前帧实体速度的变化

		RigidEntity newAttach = null;
		if (collider.AttachTile != null && !collider.AttachTile.Active)
		{
			collider.AttachTile = null;
		}

		foreach (var (tile, dir) in TileSystem.Instance.MoveCollision(collider, move, ignorePlats))
		{
			collider.OnCollision(tile, dir, ref newAttach);
			if (dir == collider.Ground && collider.CanAttach)
			{
				newAttach = tile;
				collider.AttachType = AttachType.Stand;
				collider.AttachDir = dir;
			}
		}

		//处理站在方块上
		if (newAttach == null)
		{
			if (collider.AttachTile != null)
			{
				collider.AttachTile.Leave(collider);
				collider.DeltaVelocity *= 0;
				collider.AttachTile = null;
				collider.AttachType = AttachType.None;
			}
		}
		else if (collider.AttachTile != newAttach)
		{
			if (collider.AttachTile != null)
			{
				collider.AttachTile.Leave(collider);
			}
			collider.DeltaVelocity *= 0;
			collider.AttachTile = newAttach;
			newAttach.Stand(collider, true);
		}
		else if (collider.AttachTile != null && collider.AttachTile == newAttach)
		{
			newAttach.Stand(collider, false);
		}

		//同步位置
		collider.Entity.position = collider.Position;
		collider.Entity.velocity = collider.AbsoluteVelocity - collider.DeltaVelocity;
		if (collider.AttachTile != null)
		{
			collider.OnAttach();
		}
	}
}