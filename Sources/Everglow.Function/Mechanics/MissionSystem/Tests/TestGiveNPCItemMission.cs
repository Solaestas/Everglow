using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Templates;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class TestGiveNPCItemMission : GiveNPCItemMission
{
	public override string DisplayName => "测试提交NPC物品";

	public override List<GiveItemRequirement> DemandGiveItems { get; init; } = [
		GiveItemRequirement.Create([ItemID.DirtBlock], 10)];

	public override List<Item> RewardItems => [new Item(ItemID.BloodbathDye, 10)];

	public override int NPCType => NPCID.Guide;

	public override string NPCText => "Have a good day, have you got enough items?";

	public override string CompleteText => "Thank you!";
}