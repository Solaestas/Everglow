using Everglow.Commons.Interfaces;
using ReLogic.Content;

namespace Everglow.Commons.VFX;

public abstract class PostPipeline : IPipeline
{
	protected Asset<Effect> effect;

	public virtual void Load()
	{
	}

	public void Render(IEnumerable<IVisual> visuals)
	{
		if (visuals.FirstOrDefault() is VFXManager.Rt2DVisual rt2D)
		{
			Render(rt2D.locker.Resource);
			rt2D.Active = false;
			rt2D.locker.Release();
		}
		else
		{
			Render((RenderTarget2D)null);
		}
	}

	public abstract void Render(RenderTarget2D rt2D);

	public virtual void Unload()
	{
	}
}