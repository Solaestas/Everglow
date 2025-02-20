using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Templates;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class TestMission1 : CollectItemMission
{
	public override List<CollectItemRequirement> DemandCollectItems { get; init; } = [
		CollectItemRequirement.Create([ItemID.DirtBlock], 10, true)];

	public override List<Item> RewardItems => [
		new Item(ItemID.Wood, 10)];

	public override string DisplayName => "获取10个土块";

	public override MissionType MissionType => MissionType.Daily;

	public override bool IsVisible { get; set; } = false;

	public override bool AutoComplete => true;
}