namespace Everglow.Commons.Collider;

public interface IBox
{
	public AABB Box { get; }

	public float Gravity { get; }

	public Vector2 Position { get; set; }

	public Vector2 Size { get; }

	public Vector2 Velocity { get; set; }

	public bool Ignore(RigidEntity entity);
}