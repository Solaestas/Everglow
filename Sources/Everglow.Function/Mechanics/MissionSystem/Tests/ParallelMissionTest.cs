using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class ParallelMissionTest : MissionBase
{
	public ParallelMissionTest()
	{
		var objective1_1 = new KillNPCObjective(KillNPCRequirement.Create(
			[NPCID.BlueArmoredBonesMace, NPCID.HellArmoredBonesMace], 10, true));
		var objective1_2 = new ConsumeItemObjective(CountItemRequirement.Create([ItemID.LifeCrystal], 2));
		var objective1_3 = new KillNPCObjective(KillNPCRequirement.Create([NPCID.DemonEye,], 3, true));

		Objectives.AddParallel(objective1_1, objective1_2, objective1_3);

		var objective2 = new KillNPCObjective(
			KillNPCRequirement.Create([NPCID.ChaosBallTim], 3, true));
		Objectives.Add(objective2);

		Icon.AddRange(objective1_1.DemandNPC.NPCs.Select(i => NPCMissionIcon.Create(i)));
		Icon.AddRange(objective1_2.DemandConsumeItem.Items.Select(i => ItemMissionIcon.Create(i)));
		Icon.AddRange(objective2.DemandNPC.NPCs.Select(i => NPCMissionIcon.Create(i)));
	}

	public override string DisplayName => nameof(ParallelMissionTest);

	public override MissionType MissionType => MissionType.Legendary;
}