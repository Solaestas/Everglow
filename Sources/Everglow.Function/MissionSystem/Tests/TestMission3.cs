using Everglow.Commons.MissionSystem.Enums;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Templates;

namespace Everglow.Commons.MissionSystem.Tests;

public class TestMission3 : CollectItemMission
{
	public override List<CollectItemRequirement> DemandCollectItems { get; init; } = [
		CollectItemRequirement.Create([ItemID.IronOre], 1000)];

	public override List<Item> RewardItems => [
		new Item(ItemID.Zenith, 1000)];

	public override string DisplayName => "获取10个铁矿";

	public override MissionType MissionType => MissionType.MainStory;

	public static TestMission3 Create() => new TestMission3();
}