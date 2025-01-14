using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionTemplates;

namespace Everglow.Commons.MissionSystem.Tests;

public class TestMission7 : GainItemKillNPCMission
{
	public override List<GainItemRequirement> DemandItems { get; init; } = [
		GainItemRequirement.Create([ItemID.Wood], 10),
		GainItemRequirement.Create([ItemID.DirtBlock], 10),
	];

	public override List<KillNPCRequirement> DemandNPCs { get; init; } = [
		KillNPCRequirement.Create(
			[
				NPCID.BlueSlime,
			], 5, true),
		KillNPCRequirement.Create(
			[
				NPCID.DemonEye,
			], 3, true),
		];

	public override List<Item> RewardItems => [
	new Item(ItemID.Zenith, 10)];

	public override string DisplayName => "测试复合任务";

	public override string Description => "XXXXXXXXXXXXXXXXXXXXXXXXX";
}