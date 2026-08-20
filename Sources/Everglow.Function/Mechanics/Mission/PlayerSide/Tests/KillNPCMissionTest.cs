using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Objectives;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Tests;

public class KillNPCMissionTest : PlayerMissionBase
{
	public KillNPCMissionTest()
	{
		var objective = new KillNPCObjective([NPCID.CursedSkull, NPCID.DemonEye], 10, true);
		Objectives.Add(objective);

		RewardItems.Add(new(ItemID.DirtBlock, 10));
	}

	public override MissionSourceBase Source => MissionSourceTest1.Instance;

	public override MissionSourceBase SubSource => MissionSourceTest2.Instance;

	public override string DisplayName => nameof(KillNPCMissionTest);

	public override MissionType Type => MissionType.MainStory;
}
