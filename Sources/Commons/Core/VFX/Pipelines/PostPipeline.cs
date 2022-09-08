using Everglow.Sources.Commons.Core.VFX.Visuals;
using ReLogic.Content;

namespace Everglow.Sources.Commons.Core.VFX.Pipelines;

internal abstract class PostPipeline : IPipeline
{
    protected Asset<Effect> effect;

    public virtual void Load()
    {
    }

    public void Render(IEnumerable<IVisual> visuals)
    {
        Debug.Assert(visuals.Count() == 1);
        var rt2D = visuals.Single() as VFXManager.Rt2DVisual;
        Debug.Assert(rt2D != null);
        Render(rt2D.locker.Resource);
        rt2D.Active = false;
        rt2D.locker.Release();
    }

    public abstract void Render(RenderTarget2D rt2D);

    public virtual void Unload()
    {
    }
}