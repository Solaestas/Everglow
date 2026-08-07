using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Shared.Icons;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Objectives.Requirements;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Shared.Icons;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Objectives;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Tests;

public class MissionObjectivesTest : PlayerMissionBase
{
	public MissionObjectivesTest()
	{
		var objective1 = new KillNPCObjective(KillNPCRequirement.Create(
			[
				NPCID.BlueSlime,
				NPCID.IceSlime,
				NPCID.SpikedJungleSlime,
				NPCID.MotherSlime,
			], 10, true));

		var objective2 = new ConsumeItemObjective(new ItemRequirement([ItemID.SpikyBall], 10));

		Objectives.Add(objective1);
		Objectives.Add(objective2);

		Icon.AddRange(objective1.DemandNPC.NPCs.Select(i => NPCMissionIcon.Create(i)));
		Icon.AddRange(objective2.DemandConsumeItem.Items.Select(i => ItemMissionIcon.Create(i)));

		RewardItems.Add(new Item(ItemID.Zenith, 1000));
	}

	public override string DisplayName => nameof(MissionObjectivesTest);

	public override MissionType Type => MissionType.SideStory;
}
