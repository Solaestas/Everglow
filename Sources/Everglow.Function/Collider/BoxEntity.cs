using Terraria.GameContent;

namespace Everglow.Commons.Collider;

public class BoxEntity : RigidEntity, IBox, IHookable
{
	public AABB Box => new(Position, Size);

	public int Quantity => 114514;

	public Vector2 Size { get; set; }

	public float Gravity => 1;

	public override bool Collision(IBox obj, Vector2 stride, out CollisionResult result)
	{
		result = default;
		result.Collider = this;

		AABB box = obj.Box;
		AABB collider = Box;

		if (box.Intersect(collider))
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
		if ((0 <= tMin && tMin <= 1) && tMin <= tMax)
		{
			if (txMin < tyMin)
			{
				result.Normal = new Vector2(0, Math.Sign(stride.Y));
				result.Stride = stride with { Y = stride.Y * tyMin - 0.001f * Math.Sign(stride.Y) } + Velocity;
				obj.Velocity = obj.Velocity with { Y = Velocity.Y };
			}
			else
			{
				result.Normal = new Vector2(Math.Sign(stride.X), 0);
				result.Stride = stride with { X = stride.X * txMin } + Velocity;
				obj.Velocity = obj.Velocity with { X = Velocity.X };
			}
			return true;
		}

		return false;
	}

	public override void Draw()
	{
		var box = Box;
		box.position -= Main.screenPosition;
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, box, Color.White);
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
					Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, position, new Rectangle(0, 0, 1, 1), Color.White, 0, Vector2.Zero, mapScale, SpriteEffects.None, 0);
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
		var box = Box;
		hook.position.X = MathHelper.Clamp(hook.position.X, box.Left, box.Right);
		hook.position.Y = MathHelper.Clamp(hook.position.Y, box.Top, box.Bottom);
	}
}