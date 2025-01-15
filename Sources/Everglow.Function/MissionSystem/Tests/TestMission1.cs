using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionTemplates;

namespace Everglow.Commons.MissionSystem.Tests;

public class TestMission1 : GainItemMission
{
	public override List<GainItemRequirement> DemandItems { get; init; } = [
		GainItemRequirement.Create([ItemID.DirtBlock], 10, true)];

	public override List<Item> RewardItems => [
		new Item(ItemID.Wood, 10)];

	public override string DisplayName => "获取10个土块";

	public override string Description => "测试[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']";

	public override MissionType MissionType => MissionType.Daily;

	public override bool IsVisible { get; set; } = false;

	public override bool AutoComplete => true;

	public override bool SubmitItemsOnComplete => true;
}