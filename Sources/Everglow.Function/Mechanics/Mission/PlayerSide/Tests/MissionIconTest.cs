using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Tests;

public class MissionIconTest : PlayerMissionBase
{
	public override string DisplayName => GetType().Name;

	public override MissionType Type => MissionType.MainStory;
}
