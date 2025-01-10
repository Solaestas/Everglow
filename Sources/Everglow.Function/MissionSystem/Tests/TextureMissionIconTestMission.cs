using Everglow.Commons.MissionSystem.MissionIcons;
using Everglow.Commons.MissionSystem.MissionTemplates;
using ReLogic.Content;

namespace Everglow.Commons.MissionSystem.TestMissions;

public class TextureMissionIconTestMission : GainItemMission
{
	public override MissionIconGroup Icon => new MissionIconGroup([
		TextureMissionIcon.Create(
			Commons.ModAsset.BurningFrozenHeart.Value,
			animation: new(5, 7)),
		NPCMissionIcon.Create(NPCID.BlueSlime),
		ItemMissionIcon.Create(ItemID.LargeRuby),
		]);

	public static TextureMissionIconTestMission Create()
	{
		LoadVanillaNPCTextures([NPCID.BlueSlime]);
		LoadVanillaItemTextures([ItemID.LargeRuby]);
		var mission = new TextureMissionIconTestMission();
		mission.SetInfo("TestMissionIconTest1", "测试TextureMissionIcon", "测试介绍3");
		mission.DemandItems.AddRange([
			GainItemRequirement.Create([ItemID.IronOre], 1000)]);
		mission.RewardItems.AddRange([
			new Item(ItemID.Zenith, 1000)]);
		mission.MissionType = MissionType.MainStory;
		return mission;
	}
}