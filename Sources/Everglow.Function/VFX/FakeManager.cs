using Everglow.Common.Interfaces;

namespace Everglow.Common.VFX;

public class FakeManager : IVFXManager
{
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
}