using Everglow.Commons.MissionSystem.Enums;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Templates;

namespace Everglow.Commons.MissionSystem.Tests;

internal class TestMission8 : ConsumeItemMission
{
	public override List<CountItemRequirement> DemandConsumeItems { get; init; } = [
		CountItemRequirement.Create([ItemID.BattlePotion], 10)];

	public override List<Item> RewardItems => [
		new Item(ItemID.Wood, 10)];

	public override string DisplayName => "测试消耗";

	public override MissionType MissionType => MissionType.Daily;
}