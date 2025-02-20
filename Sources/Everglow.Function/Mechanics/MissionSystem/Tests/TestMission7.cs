using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Templates;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class TestMission7 : CollectItemKillNPCMission
{
	public override List<CollectItemRequirement> DemandCollectItems { get; init; } = [
		CollectItemRequirement.Create([ItemID.Wood], 10),
		CollectItemRequirement.Create([ItemID.DirtBlock], 10),
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
}