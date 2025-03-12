namespace Everglow.Commons.Mechanics.MissionSystem.Primitives;

public abstract class MissionConditionBase
{
	protected abstract bool GetCheck();

	public bool IsTrue => GetCheck();

	public bool IsFalse => !GetCheck();
}