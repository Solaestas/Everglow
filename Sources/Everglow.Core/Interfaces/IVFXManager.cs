namespace Everglow.Commons.Interfaces;
public interface IVFXManager : IDisposable
{
	public void Add(IVisual visual);
	public void Clear();
	public int GetVisualType(IVisual visual);
}
