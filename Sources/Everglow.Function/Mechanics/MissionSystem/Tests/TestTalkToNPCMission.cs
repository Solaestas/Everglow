using Everglow.Commons.Mechanics.MissionSystem.Templates;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class TestTalkToNPCMission : TalkToNPCMission
{
	public override int NPCType => NPCID.Guide;

	public override string NPCText => "Have a good day";

	public override string DisplayName => "测试对话NPC";

	public override List<Item> RewardItems => [new Item(ItemID.GoldOre, 10)];
}