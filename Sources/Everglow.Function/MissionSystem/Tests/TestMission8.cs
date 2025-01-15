using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionTemplates;

namespace Everglow.Commons.MissionSystem.Tests;

internal class TestMission8 : ConsumeItemMission
{
	public override List<ItemRequirement> DemandConsumeItems { get; init; } = [
		ItemRequirement.Create([ItemID.BattlePotion], 10)];

	public override List<Item> RewardItems => [
		new Item(ItemID.Wood, 10)];

	public override string DisplayName => "测试消耗";

	public override string Description => "测试[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']";

	public override MissionType MissionType => MissionType.Daily;
}