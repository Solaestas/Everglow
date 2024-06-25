namespace Everglow.Commons.Physics.MassSpringSystem;

public class Mass(float mass, Vector2 position, bool isStatic)
{
	public bool IsStatic = isStatic;

	public float Value = mass;

	public Vector2 Position = position;

	public Vector2 Velocity = Vector2.Zero;

	public Vector2 Force = Vector2.Zero;

	public Vector2 OldPos = Vector2.Zero;

	public Vector2 DeltaPos = Vector2.Zero;

	public Vector2 Gradient = Vector2.Zero;

	public float HessianDiag = 0;
}