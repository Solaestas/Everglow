using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Objectives;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Tests;

public class CancellableKillNPCMissionTest : PlayerMissionBase
{
	public CancellableKillNPCMissionTest()
	{
		var objective = new KillNPCObjective([NPCID.WallofFlesh], 1, true);
		Objectives.Add(objective);

		RewardItems.Add(new(ItemID.GoldBar, 1000));
	}

	public override bool Cancellable => true;

	public override string DisplayName => nameof(CancellableKillNPCMissionTest);

	public override MissionType Type => MissionType.Daily;
}
