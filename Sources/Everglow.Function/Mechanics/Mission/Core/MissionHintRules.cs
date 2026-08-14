namespace Everglow.Commons.Mechanics.Mission.Core;

public static class MissionHintRules
{
	public static bool HasContent(string hint) => !string.IsNullOrWhiteSpace(hint);
}
