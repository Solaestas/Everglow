using Everglow.Commons.Mechanics.Mission.PlayerMission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Enums;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Primitives;

namespace Everglow.Commons.Mechanics.Mission.PlayerMission.Tests;

public class OpenPanelMissionTest : MissionBase
{
	public override string DisplayName => nameof(OpenPanelMissionTest);

	public override MissionIconGroup Icon => null;

	public override MissionType MissionType => MissionType.Challenge;
}