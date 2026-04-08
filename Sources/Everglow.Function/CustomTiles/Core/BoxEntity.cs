using Everglow.Commons.CustomTiles.Abstracts;
using Everglow.Commons.Physics.DataStructures;
using Everglow.Commons.Utilities;
using Terraria.GameContent;

namespace Everglow.Commons.CustomTiles.Core;

public class BoxEntity : RigidEntity, IBox, IHookable
{
	public AABB Box => new(Position, Size);

	public int Quantity => 114514;

	public Vector2 Size { get; set; }

	public float Gravity => 1;

	public override bool Collision(IBox other, Vector2 stride, out CollisionResult result)
	{
		result = default;
		result.Collider = this;

		AABB selfBox = Box;
		var otherBox = other.Box;
		Vector2 otherPosition = other.Position;
		Vector2 relativeVelocity = stride - Velocity;

		// 1. Coarse Collision Detection
		if (!otherBox.Scan(relativeVelocity).Intersect(selfBox))
		{
			result.Stride = stride;
			result.Normal = Vector2.Zero;
			return false;
		}

		// 2. Intersect Detection
		const float SmallScale = 7;
		AABB smallBox = selfBox;
		smallBox.TopLeft += new Vector2(SmallScale, SmallScale);
		smallBox.BottomRight -= new Vector2(SmallScale, SmallScale);
		if (smallBox.Intersect(otherBox))
		{
			result.Stride = stride;
			result.Normal = Vector2.Zero;
			return true;
		}

		// 3. Fine Collision Detection
		bool onGround = other.Velocity.Y + Velocity.Y == 0;
		bool? isX = null;
		Vector2 target = otherBox.position + relativeVelocity;
		do
		{
			otherBox.position = otherBox.position.Approach(target, 1);
			if (otherBox.Intersect(selfBox, out var area))
			{
				isX = area.size.X < area.size.Y;
				break;
			}
		}
		while (otherBox.position != target);

		if (isX is null)
		{
			result.Stride = stride;
			result.Normal = Vector2.Zero;

			return false;
		}
		else if (isX.Value)
		{
			otherBox.position.X = otherBox.Left < selfBox.Left ? selfBox.Left - otherBox.size.X : selfBox.Right;
			otherBox.position.Y = target.Y;
			other.Velocity = other.Velocity with { X = Velocity.X };

			result.Stride = otherBox.position - otherPosition + Velocity;
			result.Normal = otherBox.Left < selfBox.Left ? -Vector2.UnitX : Vector2.UnitX;

			return true;
		}
		else
		{
			otherBox.position.Y = otherBox.Top < selfBox.Top ? selfBox.Top - otherBox.size.Y : selfBox.Bottom;
			otherBox.position.X = target.X;
			other.Velocity = other.Velocity with
			{
				Y = Velocity.Y == 0
					? onGround
						? 0
						: CollisionUtils.Epsilon * 10
					: Velocity.Y,
			};

			result.Stride = otherBox.position - otherPosition + Velocity;
			result.Normal = otherBox.Top < selfBox.Top ? Vector2.UnitY : -Vector2.UnitY;

			return true;
		}

		/*result = default;
		result.Collider = this;

		AABB box = other.Box;
		AABB collider = Box;

		if (box.Intersect(collider, true))
		{
			result.Stride = stride;
			result.Normal = Vector2.Zero;
			return true;
		}

		stride -= Velocity;

		float txMax = float.MaxValue, txMin = 0;
		if (stride.X > 0)
		{
			txMin = (collider.Left - box.Right) / stride.X;
			txMax = (collider.Right - box.Left) / stride.X;
		}
		else if (stride.X < 0)
		{
			txMin = (collider.Right - box.Left) / stride.X;
			txMax = (collider.Left - box.Right) / stride.X;
		}
		else if (collider.Right <= box.Left || box.Right <= collider.Left)
		{
			return false;
		}

		float tyMax = float.MaxValue, tyMin = 0;
		if (stride.Y > 0)
		{
			tyMin = (collider.Top - box.Bottom) / stride.Y;
			tyMax = (collider.Bottom - box.Top) / stride.Y;
		}
		else if (stride.Y < 0)
		{
			tyMin = (collider.Bottom - box.Top) / stride.Y;
			tyMax = (collider.Top - box.Bottom) / stride.Y;
		}
		else if (collider.Bottom <= box.Top || box.Bottom <= collider.Top)
		{
			return false;
		}

		float tMin = Math.Max(txMin, tyMin), tMax = Math.Min(txMax, tyMax);
		if (tMin >= 0 && tMin <= 1 && tMin <= tMax)
		{
			if (txMin < tyMin)
			{
				result.Normal = new Vector2(0, Math.Sign(stride.Y));
				result.Stride = stride with { Y = stride.Y * tyMin - 0.001f * Math.Sign(stride.Y) } + Velocity;
				other.Velocity = other.Velocity with { Y = Velocity.Y };
			}
			else if (txMin > tyMin)
			{
				result.Normal = new Vector2(Math.Sign(stride.X), 0);
				result.Stride = stride with { X = stride.X * txMin } + Velocity;
				other.Velocity = other.Velocity with { X = Velocity.X };
			}
			return true;
		}

		return false;*/
	}

	public override void Draw()
	{
		var box = Box;
		box.position -= Main.screenPosition;
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, box, MapColor);
	}

	public override void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
	{
		Vector2 center = Position + new Vector2(16, 16);
		int width = (int)(Size.X / 16f);
		int height = (int)(Size.Y / 16f);
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Vector2 position = (center / 16f + new Vector2(i - 1, j - 1) - mapTopLeft) * mapScale + mapX2Y2AndOff;
				var destination = new Rectangle((int)position.X - 1, (int)position.Y - 1, 2, 2);
				if (mapRect != null ? destination.Intersects(mapRect.Value) : true)
				{
					Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, position, new Rectangle(0, 0, 1, 1), MapColor, 0, Vector2.Zero, mapScale, SpriteEffects.None, 0);
				}
			}
		}
	}

	public override bool Intersect(AABB box)
	{
		return Box.Intersect(box);
	}

	public virtual bool Ignore(RigidEntity entity)
	{
		return false;
	}

	public override Vector2 StandAccelerate(IBox obj)
	{
		return Velocity;
	}

	public void SetHookPosition(Projectile hook)
	{
		if (!PreSetHookPosition(hook))
		{
			return;
		}

		var box = Box;
		hook.position.X = MathHelper.Clamp(hook.position.X, box.Left, box.Right);
		hook.position.Y = MathHelper.Clamp(hook.position.Y, box.Top, box.Bottom);

		PostSetHookPosition(hook);
	}

	public virtual bool PreSetHookPosition(Projectile hook)
	{
		return true;
	}

	public virtual void PostSetHookPosition(Projectile hook)
	{
	}
}