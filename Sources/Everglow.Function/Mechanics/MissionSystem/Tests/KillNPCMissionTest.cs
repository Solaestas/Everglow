using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class KillNPCMissionTest : MissionBase
{
	public KillNPCMissionTest()
	{
		var objective = new KillNPCObjective();
		objective.DemandNPCs.AddRange([
			KillNPCRequirement.Create(
				[
					NPCID.BlueSlime,
					NPCID.IceSlime,
					NPCID.SpikedJungleSlime,
					NPCID.MotherSlime,
				], 10, true),
			KillNPCRequirement.Create(
				[
					NPCID.DemonEye,
				], 3, true)]);
		Objectives.Add(objective);

		Icon.AddRange(objective.DemandNPCs.SelectMany(s => s.NPCs).Select(i => NPCMissionIcon.Create(i)));

		RewardItems.Add(new(ItemID.DirtBlock, 10));
	}

	public override string DisplayName => nameof(KillNPCMissionTest);

	public override MissionType MissionType => MissionType.MainStory;
}