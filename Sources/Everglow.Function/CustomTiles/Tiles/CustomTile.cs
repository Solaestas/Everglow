using Everglow.Commons.CustomTiles.Collide;
using Everglow.Commons.CustomTiles.DataStructures;
using Everglow.Commons.CustomTiles.EntityColliding;
using ReLogic.Content;

namespace Everglow.Commons.CustomTiles.Tiles;

public interface IGrabbable
{
	public bool CanGrab(Player player);

	public void EndGrab(Player player);

	public void OnGrab(Player player);
}

public interface IHookable
{
	public void SetHookPosition(Projectile hook);
}

/// <summary>
/// 一个基础的运动方块
/// </summary>
public abstract class CustomTile
{
	/// <summary>
	/// 上一帧的速度，为了避免碰撞判定后修改速度导致出现差错，使用 <see cref="oldVelocity" /> 来决定移动距离
	/// </summary>
	public Vector2 oldVelocity;

	/// <summary>
	/// 位置
	/// </summary>
	public Vector2 position;

	/// <summary>
	/// 速度
	/// </summary>
	public Vector2 velocity;

	/// <summary>
	/// 在数组中的下标
	/// </summary>
	public int whoAmI = -1;

	/// <summary>
	/// 是否激活
	/// </summary>
	public bool Active { get; set; } = true;

	/// <summary>
	/// 检验碰撞的碰撞箱
	/// </summary>
	public virtual Collider Collider { get; }

	public virtual bool IsGrabbable => false;

	/// <summary>
	/// 默认地图绘制使用的颜色
	/// </summary>
	public virtual Color MapColor { get; }

	/// <summary>
	/// 速度
	/// </summary>
	public virtual Vector2 Position { get => position; set => position = value; }

	public Asset<Texture2D> Texture { get; }

	/// <summary>
	/// 位移
	/// </summary>
	public virtual Vector2 Velocity { get => velocity; set => velocity = value; }

	/// <summary>
	/// 在数组中的下标
	/// </summary>
	public virtual int WhoAmI
	{
		get
		{
			return whoAmI;
		}
		set
		{
			whoAmI = value;
		}
	}

	/// <summary>
	/// 空的，用来执行物块运动逻辑
	/// </summary>
	public virtual void AI()
	{
	}

	public virtual bool Collision(Collider collider)
	{
		return Collider.Collision(collider);
	}

	public virtual void Draw()
	{
		Main.spriteBatch.Draw(Texture.Value, position - Main.screenPosition, null, Color.White);
	}

	public virtual void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
	{ }

	public virtual void Kill()
	{
		Active = false;
	}

	public virtual void Leave(EntityHandler entity)
	{ }

	public virtual void Move()
	{
		position += oldVelocity;
	}

	public abstract Direction MoveCollision(AABB aabb, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false);

	public virtual void OnCollision(AABB aabb, Direction dir)
	{ }

	public virtual void Stand(EntityHandler entity, bool newStand)
	{ }

	/// <summary>
	/// 更新顺序不固定，可以自己重写
	/// </summary>
	public virtual void Update()
	{
		Move();
		AI();
		oldVelocity = velocity;
	}
}