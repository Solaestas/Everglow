using Everglow.Commons.Interfaces;

namespace Everglow.Commons.VFX;

public class FakeManager : IVFXManager
{
	public RenderTarget2D CurrentRenderTarget => throw new NotImplementedException(nameof(FakeManager));

	public void Add(IVisual visual)
	{
	}

	public void Clear()
	{
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
	}

	public int GetVisualType(IVisual visual)
	{
		return 0;
	}

	public void SwapRenderTarget()
	{
	}
}