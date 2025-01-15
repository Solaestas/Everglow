using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionTemplates;

namespace Everglow.Commons.MissionSystem.Tests;

public class TestMission2 : GainItemMission
{
	public override List<GainItemRequirement> DemandGainItems { get; init; } = [
		GainItemRequirement.Create([ItemID.Wood], 10)];

	public override List<Item> RewardItems => [
		new Item(ItemID.IronOre, 10)];

	public override string DisplayName => "获取10个木头";

	public override string Description => "测试介绍2\n" +
					$"[TimerIconDrawer,MissionName='{Name}'] 剩余时间:[TimerStringDrawer,MissionName='{Name}']";

	public override MissionType MissionType => MissionType.Achievement;

	public override long TimeMax => 30000;
}