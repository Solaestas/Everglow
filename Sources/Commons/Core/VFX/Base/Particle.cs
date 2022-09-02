namespace Everglow.Sources.Commons.Core.VFX.Base;

public abstract class Particle : Visual
{
    public override CallOpportunity DrawLayer => CallOpportunity.PostDrawDusts;
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