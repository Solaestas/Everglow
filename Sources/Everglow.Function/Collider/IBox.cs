namespace Everglow.Commons.Collider;

public interface IBox
{
	public Vector2 Position { get; set; }
	public Vector2 Velocity { get; set; }
	public Vector2 Size { get; }
	public AABB Box { get; }
	public int Quantity { get; }
	public float Gravity { get; }
	public bool Ignore(RigidEntity entity);
}
