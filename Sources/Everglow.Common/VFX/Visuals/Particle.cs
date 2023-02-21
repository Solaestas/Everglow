using Everglow.Core.Enums;

namespace Everglow.Core.VFX.Visuals;

public abstract class Particle : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float scale;
	public float alpha;

	public override void Update()
	{
		AI();
		position += velocity;
	}

	public virtual void AI()
	{
	}
}