using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.Mechanics.MissionSystem.Shared;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class MissionObjectivesTest : MissionBase
{
	public MissionObjectivesTest()
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
			CountItemRequirement.Create([ItemID.SpikyBall], 10)]);

		Objectives.Add(objective1);
		Objectives.Add(objective2);

		Icon.AddRange(objective1.DemandNPCs.SelectMany(s => s.NPCs).Select(i => NPCMissionIcon.Create(i)));
		Icon.AddRange(objective2.DemandConsumeItems.SelectMany(s => s.Items).Select(i => ItemMissionIcon.Create(i)));

		RewardItems.Add(new Item(ItemID.Zenith, 1000));
	}

	public override string DisplayName => nameof(MissionObjectivesTest);
}