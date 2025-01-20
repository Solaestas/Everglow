using Everglow.Commons.MissionSystem.Templates;

namespace Everglow.Commons.MissionSystem.Tests;

public class TestTalkToNPCMission : TalkToNPCMission
{
	public override int NPCType => NPCID.Guide;

	public override string NPCText => "Have a good day";

	public override string DisplayName => "测试对话NPC";

	public override string Description => "和向导对话";

	public override List<Item> RewardItems => [new Item(ItemID.GoldOre, 10)];
}