using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class BranchingMissionTest : MissionBase
{
	public BranchingMissionTest()
	{
		var objective1 = new KillNPCObjective(KillNPCRequirement.Create(
				[
					NPCID.BlueSlime,
					NPCID.IceSlime,
					NPCID.SpikedJungleSlime,
					NPCID.MotherSlime,
				], 10, true));

		var objective2_1 = new ConsumeItemObjective(new ItemRequirement([ItemID.WoodenArrow], 2));
		var objective2_2 = new KillNPCObjective();
		objective2_2.DemandNPC = KillNPCRequirement.Create([NPCID.BlueSlime, NPCID.IceSlime, NPCID.SpikedJungleSlime, NPCID.MotherSlime], 5, true);
		var branch2 = new MissionObjectiveData().Add(objective2_1).Add(objective2_2);

		var objective3 = new KillNPCObjective(KillNPCRequirement.Create(
				[
					NPCID.EyeofCthulhu,
				], 2, true));

		var branch3 = new MissionObjectiveData().Add(objective3);

		Objectives.Add(objective1).AddBranches(branch2, branch3);

		Icon.AddRange(objective1.DemandNPC.NPCs.Select(i => NPCMissionIcon.Create(i)));
		Icon.AddRange(objective2_1.DemandConsumeItem.Items.Select(i => ItemMissionIcon.Create(i)));
	}

	public override string DisplayName => nameof(BranchingMissionTest);

	public override MissionType MissionType => MissionType.Daily;
}