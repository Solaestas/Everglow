using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Primitives;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Tests;

public class OpenPanelMissionTest : PlayerMissionBase
{
	public override string DisplayName => nameof(OpenPanelMissionTest);

	public override MissionIconGroup Icon => null;

	public override MissionType Type => MissionType.Challenge;
}