namespace Everglow.Commons.MissionSystem.MissionAbstracts;

public abstract class MissionCondition
{
	protected abstract bool Can { get; }

	public bool Yes => Can;

	public bool No => !Can;
}