using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public static class TextDefinition
{
	public static string GetPoolTypeText(MissionViewState? type) => type switch
	{
		MissionViewState.Active => "Accepted",
		null => "All",
		_ => type.ToString(),
	};
}
