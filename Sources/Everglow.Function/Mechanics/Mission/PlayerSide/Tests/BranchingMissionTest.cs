using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Objectives;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Primitives;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Shared.Icons;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Tests;

public class BranchingMissionTest : PlayerMissionBase
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

		var objective3 = new KillNPCObjective(KillNPCRequirement.Create(
				[
					NPCID.EyeofCthulhu,
				], 2, true));

		Objectives.Add(objective1).AddBranch([objective2_1, objective2_2], [objective3]);

		Icon.AddRange(objective1.DemandNPC.NPCs.Select(i => NPCMissionIcon.Create(i)));
		Icon.AddRange(objective2_1.DemandConsumeItem.Items.Select(i => ItemMissionIcon.Create(i)));
	}

	public override string DisplayName => nameof(BranchingMissionTest);

	public override MissionType Type => MissionType.Daily;
}
