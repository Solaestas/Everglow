namespace Everglow.Commons.Mechanics.Mission.Presentation.Views;

public sealed class ObjectiveView
{
	public int Id { get; init; }

	public string Description { get; init; } = string.Empty;

	public float Progress { get; init; }

	public ObjectiveViewState State { get; init; }
}
