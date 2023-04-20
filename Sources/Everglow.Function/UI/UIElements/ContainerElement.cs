namespace Everglow.Commons.UI.UIElements;

public abstract class ContainerElement : BaseElement
{
	public virtual string Name { get => GetType().FullName; }

	public virtual bool AutoLoad { get => true; }

	public ContainerElement()
	{
		Info.IsVisible = false;
	}

	public override void OnInitialization()
	{
		base.OnInitialization();
		Info.Width = new PositionStyle(0f, 1f);
		Info.Height = new PositionStyle(0f, 1f);
		Info.CanBeInteract = false;
	}
}
