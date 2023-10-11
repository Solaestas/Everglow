namespace Everglow.Commons.Collider.EntityCollider;

public interface IEntityCollider<T> : IBox where T : Entity
{
	public T Entity { get; }

	public RigidEntity Ground { get; set; }

	public float OffsetY { get; set; }
}