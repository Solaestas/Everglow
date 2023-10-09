using Everglow.Commons.CustomTiles;
using Everglow.Commons.CustomTiles.Collide;
using Everglow.Commons.CustomTiles.DataStructures;
using Everglow.Commons.CustomTiles.EntityCollider;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.CustomTiles.Tiles;

public abstract class DPlatform : RigidEntity, IHookable
{
	private static readonly Dictionary<(Direction, Direction), Direction> hitEdges = new()
	{
		[(Direction.Up, Direction.Right)] = Direction.UpRight,
		[(Direction.Right, Direction.Down)] = Direction.DownRight,
		[(Direction.Down, Direction.Left)] = Direction.DownLeft,
		[(Direction.Up, Direction.Left)] = Direction.UpLeft,
		[(Direction.Up, Direction.Down)] = Direction.In,
		[(Direction.Right, Direction.Left)] = Direction.In,
		[(Direction.Up, Direction.None)] = Direction.Up,
		[(Direction.Right, Direction.None)] = Direction.Right,
		[(Direction.Down, Direction.None)] = Direction.Down,
		[(Direction.Left, Direction.None)] = Direction.Left,
	};
	public DPlatform()
	{

	}
	public DPlatform(Vector2 position, Vector2 velocity, float width)
	{
		this.position = position;
		this.velocity = velocity;
		this.width = width;
	}
	public DPlatform(Vector2 position, Vector2 velocity, float width, Rotation rotation, float miu)
	{
		this.position = position;
		this.velocity = velocity;
		this.width = width;
		this.rotation = rotation;
		this.miu = miu;
	}
	public DPlatform(Vector2 position, Vector2 velocity, float width, Rotation rotation, Rotation angularVelocity, float miu)
	{
		this.position = position;
		this.velocity = velocity;
		this.width = width;
		this.rotation = rotation;
		this.angularVelocity = angularVelocity;
		this.miu = miu;
	}

	/// <summary>
	/// 宽度 
	/// </summary>
	public float width;
	/// <summary>
	/// 角度，面朝右为0
	/// </summary>
	public Rotation rotation;
	/// <summary>
	/// 转速
	/// </summary>
	public Rotation angularVelocity;
	/// <summary>
	/// 摩擦系数
	/// </summary>
	public float miu;
	/// <summary>
	/// 碰撞后可能站上去的加速度缓存
	/// </summary>
	private Vector2 cache;
	public Edge Edge => new(position - rotation.YAxis * width / 2, position + rotation.YAxis * width / 2);
	public override bool IsGrabbable => false;
	/// <summary>
	/// 能否通过按住上下键来下平台
	/// </summary>
	public virtual bool IsControllable => true;
	public override Collider Collider => new CEdge(Edge);
	public override Direction MoveCollision(AABB aabb, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
	{
		if (ignorePlats && IsControllable && rotation < 0)
			return Direction.None;

		var smallBox = aabb;
		smallBox.TopLeft += Vector2.One;
		smallBox.BottomRight -= Vector2.One;
		Edge edge = Edge;
		if (smallBox.Intersect(edge))
			return Direction.None;
		bool onGround = velocity.Y == 0;
		Vector2 rvel = move - this.velocity;
		Vector2 pos0 = aabb.position;
		Vector2 target = aabb.position + rvel;
		Direction result = Direction.None;
		if (Vector2.Dot(rotation.XAxis, aabb.Center - position) < 0)
			return Direction.None;
		aabb.position.Y = MathUtils.Approach(aabb.position.Y, target.Y, 1);

		do
		{
			aabb.position = aabb.position.Approach(target, 1);
			if (edge.Intersect(aabb, out Array2<Direction> directions))
			{
				result = hitEdges[directions.Tuple];

				switch (result)
				{
					case Direction.Left:
						if (!(aabb.Left.Distance(edge.begin.X) < 1) && !(aabb.Left.Distance(edge.end.X) < 1))
							result = rotation.Angle < 0 ? Direction.Down : Direction.Up;
						break;
					case Direction.Right:
						if (!(aabb.Right.Distance(edge.begin.X) < 1) && !(aabb.Right.Distance(edge.end.X) < 1))
							result = rotation.Angle < 0 ? Direction.Down : Direction.Up;
						break;
					default:
						break;
				}
				if (result == Direction.In)
				{
					if (-MathHelper.PiOver4 * 3 < rotation.Angle && rotation.Angle <= -MathHelper.PiOver4 ||
						MathHelper.PiOver4 < rotation.Angle && rotation.Angle <= MathHelper.PiOver4 * 3)
					{
						result = aabb.Center.Y > position.Y ? Direction.Up : Direction.Down;
						float w;
						float h;
						if (result == Direction.Down)
						{
							w = (rotation.Angle < -MathHelper.PiOver2 ? aabb.Right : aabb.Left) - position.X;
							h = (rotation.Angle < -MathHelper.PiOver2 ? aabb.Right : aabb.Left) * edge.K + edge.B;
							aabb.position.Y = h - aabb.Height;
						}
						else
						{
							w = (rotation.Angle > MathHelper.PiOver2 ? aabb.Right : aabb.Left) - position.X;
							h = (rotation.Angle > MathHelper.PiOver2 ? aabb.Right : aabb.Left) * edge.K + edge.B;
							aabb.position.Y = h;
						}
						aabb.position.X = target.X;
						cache = angularVelocity.YAxis * w * Math.Abs(angularVelocity.Angle) * Vector2.UnitY;
						velocity.Y = this.velocity.Y + cache.Y;
					}
					else
					{
						result = aabb.Center.X > position.X ? Direction.Left : Direction.Right;
						float w;
						float h;
						if (result == Direction.Right)
						{
							w = (rotation.Angle < -MathHelper.PiOver2 ? aabb.Right : aabb.Left) - position.X;
							h = (rotation.Angle < -MathHelper.PiOver2 ? aabb.Right : aabb.Left) * edge.K + edge.B;
							aabb.position.X = h - aabb.Width;
						}
						else
						{
							w = (rotation.Angle > 0 ? aabb.Top : aabb.Bottom) - position.Y;
							h = (rotation.Angle > 0 ? aabb.Top : aabb.Bottom - edge.B) / edge.K;
							aabb.position.X = h;
						}
						aabb.position.Y = target.Y;
						cache = angularVelocity.YAxis * w * Math.Abs(angularVelocity.Angle) * Vector2.UnitX;
						velocity.X = this.velocity.X + cache.X;
					}
				}
				else if (result == Direction.Down)
				{
					float h = position.X < aabb.Center.X ? edge.end.Y : edge.begin.Y;
					aabb.position.Y = h - aabb.Height;
					aabb.position.X = target.X;
					cache = angularVelocity.YAxis * width * Math.Abs(angularVelocity.Angle) * Vector2.UnitY;
					velocity.Y = this.velocity.Y + cache.Y;
				}
				else if (result == Direction.Up)
				{
					float h = position.X > aabb.Center.X ? edge.end.Y : edge.begin.Y;
					aabb.position.Y = h;
					aabb.position.X = target.X;
					cache = angularVelocity.YAxis * width * Math.Abs(angularVelocity.Angle) * Vector2.UnitY;
					velocity.Y = this.velocity.Y + cache.Y;
				}
				else if (result == Direction.Right)
				{
					float min = Math.Min(edge.begin.X, edge.end.X);
					aabb.position.X = min - aabb.Width;
					aabb.position.Y = target.Y;
					velocity.X = this.velocity.X + angularVelocity.YAxis.X * width * Math.Abs(angularVelocity.Angle);
				}
				else if (result == Direction.Left)
				{
					float max = Math.Max(edge.begin.X, edge.end.X);
					aabb.position.X = max;
					aabb.position.Y = target.Y;
					velocity.X = this.velocity.X + angularVelocity.YAxis.X * width * Math.Abs(angularVelocity.Angle);
				}
				else if (Math.Tan(Math.Abs(rotation.Angle + MathHelper.PiOver2)) <= miu)
				{
					Direction h = result & (Direction.Right | Direction.Left);
					float w = Vector2.Dot(result.ToVector2() * aabb.size / 2 + aabb.Center - position, rotation.YAxis);
					result = result & ~Direction.Right & ~Direction.Left;
					if (rotation.Angle < 0 && result == Direction.Up || rotation.Angle > 0 && result == Direction.Down)
						result = Direction.None;
					else
					{
						aabb.position.X = target.X;
						aabb.position.Y = (h == Direction.Left ? aabb.Left : aabb.Right) * edge.K + edge.B - (rotation.Angle > 0 ? 0 : aabb.Height);
						cache = angularVelocity.YAxis * w * Math.Abs(angularVelocity.Angle) * Vector2.UnitY;
						velocity.Y = this.velocity.Y + cache.Y;
					}
				}
				else
				{
					float w = 0;
					Vector2 offset = Vector2.Zero;
					switch (result)
					{
						case Direction.UpLeft:
							w = Vector2.Dot(aabb.TopLeft - position, rotation.YAxis);
							offset = position + w * rotation.YAxis - aabb.TopLeft;
							break;
						case Direction.UpRight:
							w = Vector2.Dot(aabb.TopRight - position, rotation.YAxis);
							offset = position + w * rotation.YAxis - aabb.TopRight;
							break;
						case Direction.DownLeft:
							w = Vector2.Dot(aabb.BottomLeft - position, rotation.YAxis);
							offset = position + w * rotation.YAxis - aabb.BottomLeft;
							break;
						case Direction.DownRight:
							w = Vector2.Dot(aabb.BottomRight - position, rotation.YAxis);
							offset = position + w * rotation.YAxis - aabb.BottomRight;
							break;
					}
					aabb.position += offset + Vector2.Dot(target - aabb.position, rotation.YAxis) * rotation.YAxis;
					velocity = Vector2.Dot(velocity, rotation.YAxis) * rotation.YAxis + this.velocity + angularVelocity.YAxis * w * Math.Abs(angularVelocity.Angle);
				}
				move = aabb.position - pos0 + this.velocity;
				if (velocity.Y == 0)
					velocity.Y = onGround ? 0 : TileSystem.AirSpeed;
				break;
			}
		} while (aabb.position != target);
		return result;
	}
	public override void Draw()
	{
		Main.spriteBatch.Draw(Texture.Value,
			position - Main.screenPosition,
			new Rectangle(0, 0, 1, (int)width),
			Color.White,
			rotation.Angle,
			new Vector2(0, width / 2f), 1,
			SpriteEffects.None, 0);
	}
	public override void UpdatePosition()
	{
		base.UpdatePosition();
		rotation += angularVelocity;
	}
	public void SetHookPosition(Projectile hook)
	{
		float proj = Vector2.Dot(hook.Center - position, rotation.YAxis);
		hook.Center = position + rotation.YAxis * proj;
	}
	public override void Stand(IEntityCollider collider, bool newStand)
	{
		collider.DeltaVelocity = cache + velocity;
	}
}