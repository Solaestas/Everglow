namespace Everglow.Commons.MissionSystem.MissionAbstracts;

public abstract class MissionCondition
{
	protected abstract bool GetCheck();

	public bool IsTrue => GetCheck();

	public bool IsFalse => !GetCheck();
}