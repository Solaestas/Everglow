using Everglow.Commons.CustomTiles.DataStructures;
using Everglow.Commons.CustomTiles.EntityCollider;
using ReLogic.Content;

namespace Everglow.Commons.CustomTiles.Tiles;

public abstract class RigidEntity : IEntityCollider
{
	public Vector2 Position { get; set; }

	public Vector2 AbsoluteVelocity { get; set; }

	public Vector2 DeltaVelocity { get; set; }

	/// <summary>
	/// 是否激活
	/// </summary>
	public bool Active { get; set; } = true;

	/// <summary>
	/// 检验碰撞的碰撞箱
	/// </summary>
	public virtual Collider Collider { get; }

	/// <summary>
	/// 在数组中的下标
	/// </summary>
	public int WhoAmI { get; private set; } = -1;
	public Direction AttachDir { get; set; }
	public RigidEntity AttachTile { get; set ; }
	public AttachType AttachType { get ; set; }

	public bool CanAttach => throw new NotImplementedException();

	public Entity Entity => throw new NotImplementedException();

	public Direction Ground => throw new NotImplementedException();

	/// <summary>
	/// 空的，用来执行物块运动逻辑
	/// </summary>
	public abstract void AI();

	public virtual bool Collision(Collider collider)
	{
		return Collider.Collision(collider);
	}

	public abstract void Draw();

	public virtual void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale) { }

	public virtual void Kill()
	{
		Active = false;
	}

	public virtual void Leave(IEntityCollider entity)
	{ }

	public virtual void UpdatePosition()
	{
		position += velocity;
	}

	public abstract Direction MoveCollision(AABB aabb, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false);

	public virtual void OnCollision(AABB aabb, Direction dir)
	{ }

	public virtual void Stand(IEntityCollider entity, bool newStand)
	{ }

	public void Update()
	{
		UpdatePosition();
		AI();
	}

	public void OnAttach()
	{
		throw new NotImplementedException();
	}

	public void OnCollision(RigidEntity tile, Direction dir, ref RigidEntity newAttach)
	{
		throw new NotImplementedException();
	}

	public void OnUpdate()
	{
		throw new NotImplementedException();
	}
}