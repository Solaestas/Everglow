namespace Everglow.Commons.CustomTiles;

[DebuggerDisplay("position = ({position.X}, {position.Y}) size = ({size.X}, {size.Y})")]
public struct AABB
{
	public Vector2 position;

	public Vector2 size;

	public AABB(Vector2 position, Vector2 size)
	{
		this.position = position;
		this.size = size;
	}

	public AABB(Vector2 position, float sizeX, float sizeY)
	{
		this.position = position;
		size = new Vector2(sizeX, sizeY);
	}

	public AABB(float x, float y, float sizeX, float sizeY)
	{
		position = new Vector2(x, y);
		size = new Vector2(sizeX, sizeY);
	}

	public override readonly string ToString()
	{
		return $"({position.X}, {position.Y}, {size.X}, {size.Y})";
	}

	public readonly Vector2 TopLeft => position;

	public readonly Vector2 TopRight => position + new Vector2(size.X, 0);

	public readonly Vector2 BottomLeft => position + new Vector2(0, size.Y);

	public readonly Vector2 BottomRight => position + size;

	public readonly Vector2 Center => position + size / 2;

	public readonly float Top => position.Y;
	public readonly float Bottom => position.Y + size.Y;
	public readonly float Left => position.X;
	public readonly float Right => position.X + size.X;


	public static implicit operator Rectangle(AABB aabb)
	{
		return new(
		(int)aabb.position.X,
		(int)aabb.position.Y,
		(int)aabb.size.X,
		(int)aabb.size.Y);
	}
}