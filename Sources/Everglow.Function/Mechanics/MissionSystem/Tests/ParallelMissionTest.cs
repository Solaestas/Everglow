using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class ParallelMissionTest : MissionBase
{
	public ParallelMissionTest()
	{
		var objective1_1 = new KillNPCObjective();
		objective1_1.DemandNPCs.AddRange([
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

		var objective1_2 = new ConsumeItemObjective();
		objective1_2.DemandConsumeItems.AddRange([
			CountItemRequirement.Create([ItemID.LifeCrystal], 2)]);

		var objective1_3 = new KillNPCObjective();
		objective1_3.DemandNPCs.AddRange([
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

		Objectives.AddParallel(objective1_1, objective1_2, objective1_3);

		var objective2 = new KillNPCObjective();
		objective2.DemandNPCs.AddRange([
			KillNPCRequirement.Create(
				[
					NPCID.EyeofCthulhu,
				], 2, true),
			KillNPCRequirement.Create(
				[
					NPCID.Zombie,
				], 3, true)]);

		Objectives.Add(objective2);

		Icon.AddRange(objective1_1.DemandNPCs.SelectMany(s => s.NPCs).Select(i => NPCMissionIcon.Create(i)));
		Icon.AddRange(objective1_2.DemandConsumeItems.SelectMany(s => s.Items).Select(i => ItemMissionIcon.Create(i)));
	}

	public override string DisplayName => nameof(ParallelMissionTest);
}