namespace Everglow.Commons.MissionSystem.Core;

public abstract class MissionCondition
{
	protected abstract bool GetCheck();

	public bool IsTrue => GetCheck();

	public bool IsFalse => !GetCheck();
}