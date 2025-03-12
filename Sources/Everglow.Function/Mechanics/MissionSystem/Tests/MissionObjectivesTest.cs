using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class MissionObjectivesTest : MissionBase
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

		var objective2 = new ConsumeItemObjective(CountItemRequirement.Create([ItemID.SpikyBall], 10));

		Objectives.Add(objective1);
		Objectives.Add(objective2);

		Icon.AddRange(objective1.DemandNPC.NPCs.Select(i => NPCMissionIcon.Create(i)));
		Icon.AddRange(objective2.DemandConsumeItem.Items.Select(i => ItemMissionIcon.Create(i)));

		RewardItems.Add(new Item(ItemID.Zenith, 1000));
	}

	public override string DisplayName => nameof(MissionObjectivesTest);

	public override MissionType MissionType => MissionType.SideStory;
}