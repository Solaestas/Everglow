using Everglow.Commons.Physics.DataStructures;

namespace Everglow.Commons.CustomTiles;

public class BoxImpl : IBox
{
	public Vector2 Position { get; set; }

	public Vector2 Size { get; set; }

	public Vector2 Velocity { get; set; }

	public AABB Box => new(Position, Size);

	public int Quantity => 0;

	public float Gravity { get; set; }

	public bool Ignore(RigidEntity entity) => false;
}