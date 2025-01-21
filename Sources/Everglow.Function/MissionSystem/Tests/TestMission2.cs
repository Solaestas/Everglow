using Everglow.Commons.MissionSystem.Enums;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Templates;

namespace Everglow.Commons.MissionSystem.Tests;

public class TestMission2 : CollectItemMission
{
	public override List<CollectItemRequirement> DemandCollectItems { get; init; } = [
		CollectItemRequirement.Create([ItemID.Wood], 10)];

	public override List<Item> RewardItems => [
		new Item(ItemID.IronOre, 10)];

	public override string DisplayName => "获取10个木头";

	public override MissionType MissionType => MissionType.Achievement;

	public override long TimeMax => 30000;
}