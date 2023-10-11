namespace Everglow.Commons.Collider;

public abstract class RigidEntity
{
	public Vector2 Position { get; set; }

	public Vector2 Velocity { get; set; }

	public bool Active { get; set; } = true;

	public virtual void AI() { }

	public abstract bool Intersect(AABB box);

	public abstract void Draw();

	public abstract void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale);

	public virtual void UpdatePosition()
	{
		Position += Velocity;
	}

	public abstract bool Collision(IBox obj, Vector2 step, out CollisionResult result);

	public abstract Vector2 StandAccelerate(IBox obj);

	public void Update()
	{
		UpdatePosition();
		AI();
	}
}