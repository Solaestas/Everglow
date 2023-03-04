using Everglow.Commons.CustomTiles.DataStructures;
using Everglow.Commons.CustomTiles.Tiles;

namespace Everglow.Commons.CustomTiles.EntityColliding;

public enum AttachType
{
	None,

	Stand,

	Grab
}

public abstract class EntityHandler
{
	public abstract Entity GetEntity();

	public Vector2 position;

	public Vector2 extraVelocity;

	public Vector2 trueVelocity;

	public CustomTile attachTile;

	public AttachType attachType;

	public Direction attachDir;

	public AABB HitBox => new(position, GetEntity().Size);
}

public abstract class EntityHandler<TEntity> : EntityHandler where TEntity : Entity
{
	private TEntity entity;

	public override Entity GetEntity() => entity;

	public TEntity Entity => entity;

	protected EntityHandler(TEntity entity)
	{
		this.entity = entity;
		position = entity.position;
		extraVelocity = entity.velocity;
		attachTile = null;
	}

	public virtual bool CanAttach() => true;

	public virtual Direction Ground => Direction.Bottom;

	public virtual void OnCollision(CustomTile tile, Direction dir, ref CustomTile newAttach)
	{ }

	public virtual void OnAttach()
	{ }

	public virtual void OnLeave()
	{ }

	public virtual void Update(bool ignorePlats = false)
	{
		if (position == Vector2.Zero)
		{
			position = entity.position;
			extraVelocity = trueVelocity = entity.velocity;
			return;
		}

		Vector2 move = entity.position - position + extraVelocity;//计算获得物块推动的强制位移
		trueVelocity = entity.velocity + extraVelocity;//当前帧实体速度的变化

		CustomTile newAttach = null;
		if (attachTile != null && !attachTile.Active)
			attachTile = null;
		foreach (var (tile, dir) in TileSystem.Instance.MoveCollision(this, move, ignorePlats))
		{
			OnCollision(tile, dir, ref newAttach);
			if (dir == Ground && CanAttach())
			{
				newAttach = tile;
				attachType = AttachType.Stand;
				attachDir = dir;
			}
		}

		//处理站在方块上
		if (newAttach == null)
		{
			if (attachTile != null)
			{
				attachTile.Leave(this);
				OnLeave();
				extraVelocity *= 0;
				attachTile = null;
				attachType = AttachType.None;
			}
		}
		else if (attachTile != newAttach)
		{
			if (attachTile != null)
			{
				attachTile.Leave(this);
				OnLeave();
			}
			extraVelocity *= 0;
			attachTile = newAttach;
			newAttach.Stand(this, true);
		}
		else if (attachTile != null && attachTile == newAttach)
		{
			newAttach.Stand(this, false);
		}

		//同步位置
		entity.position = position;
		entity.velocity = trueVelocity - extraVelocity;
		if (attachTile != null)
			OnAttach();
	}
}