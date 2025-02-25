using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class ParallelMissionTest : MissionBase
{
	public ParallelMissionTest()
	{
		var objective1 = new KillNPCObjective();
		objective1.DemandNPCs.AddRange([
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

		var objective2 = new ConsumeItemObjective();
		objective2.DemandConsumeItems.AddRange([
			CountItemRequirement.Create([ItemID.LifeCrystal], 2)]);

		var objective = new ParallelObjective(objective1, objective2);

		Objectives.Add(objective);

		Icon.AddRange(objective1.DemandNPCs.SelectMany(s => s.NPCs).Select(i => NPCMissionIcon.Create(i)));
		Icon.AddRange(objective2.DemandConsumeItems.SelectMany(s => s.Items).Select(i => ItemMissionIcon.Create(i)));
	}

	public override string DisplayName => nameof(ParallelMissionTest);
}