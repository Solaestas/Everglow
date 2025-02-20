using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Templates;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

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