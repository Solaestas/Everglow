using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class KillNPCMissionTest : MissionBase
{
	public KillNPCMissionTest()
	{
		var objective = new KillNPCObjective(KillNPCRequirement.Create([NPCID.CursedSkull, NPCID.DemonEye], 10, true));
		Objectives.Add(objective);

		Icon.AddRange(objective.DemandNPC.NPCs.Select(i => NPCMissionIcon.Create(i)));

		RewardItems.Add(new(ItemID.DirtBlock, 10));
	}

	public override string DisplayName => nameof(KillNPCMissionTest);

	public override MissionType MissionType => MissionType.MainStory;
}