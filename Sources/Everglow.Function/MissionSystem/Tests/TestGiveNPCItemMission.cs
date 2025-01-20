using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Templates;

namespace Everglow.Commons.MissionSystem.Tests;

public class TestGiveNPCItemMission : GiveNPCItemMission
{
	public override string DisplayName => "测试提交NPC物品";

	public override string Description => "提交10个土块给向导";

	public override List<GainItemRequirement> DemandGainItems { get; init; } = [
		GainItemRequirement.Create([ItemID.DirtBlock], 10)];

	public override List<Item> RewardItems => [new Item(ItemID.BloodbathDye, 10)];

	public override int NPCType => NPCID.Guide;

	public override string NPCText => "Have a good day, have you got enough items?";

	public override string CompleteText => "Thank you!";
}