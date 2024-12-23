namespace Everglow.Commons.UI.UIContainers.Sidebar.SidebarElements;

public abstract class SidebarElementBase
{
	/// <summary>
	/// 快捷栏元素的标识
	/// </summary>
	public abstract Texture2D Icon { get; }

	/// <summary>
	/// 快捷栏元素的说明
	/// </summary>
	public virtual string Tooltip { get => string.Empty; }

	/// <summary>
	/// 是否显示该快捷栏元素
	/// </summary>
	public abstract bool Visible { get; }

	/// <summary>
	/// 快捷栏元素被触发的事件
	/// </summary>
	public virtual void Invoke()
	{
	}
}