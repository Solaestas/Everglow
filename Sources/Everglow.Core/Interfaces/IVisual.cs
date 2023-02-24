using Everglow.Common.Enums;

namespace Everglow.Common.Interfaces;

public interface IVisual
{
	/// <summary>
	/// 判断这个视觉特效是否还处于激活状态。我们需要保证如果它不是激活状态那么以后不会再用到它
	/// </summary>
	public bool Active { get; }

	/// <summary>
	/// 判断这个视觉特效是否需要绘制
	/// </summary>
	public bool Visible { get; }

	public int VisualType => Ins.VFXManager.GetVisualType(this);

	public CodeLayer DrawLayer { get; }

	public void Update();

	public void Draw();

	public void Kill();
}