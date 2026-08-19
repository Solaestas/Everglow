using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public sealed record MissionPresentationEntry(
	MissionView View,
	IReadOnlyList<MissionAction> Actions);
