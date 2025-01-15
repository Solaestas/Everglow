using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionIcons;
using Everglow.Commons.MissionSystem.MissionTemplates;

namespace Everglow.Commons.MissionSystem.Tests;

public class TextureMissionIconTestMission : GainItemMission
{
	public override MissionIconGroup Icon => new MissionIconGroup([
		TextureMissionIcon.Create(
			ModAsset.BurningFrozenHeart.Value,
			animation: new(5, 7)),
		NPCMissionIcon.Create(NPCID.BlueSlime),
		ItemMissionIcon.Create(ItemID.LargeRuby),
		]);

	public override string Name => "TestMissionIconTest1";

	public override string DisplayName => "测试TextureMissionIcon";

	public override string Description => "测试介绍3";

	public override MissionType MissionType => MissionType.MainStory;

	public override List<GainItemRequirement> DemandGainItems { get; init; } = [GainItemRequirement.Create([ItemID.IronOre], 1000)];

	public override List<Item> RewardItems => [new Item(ItemID.Zenith, 1000)];

	public static TextureMissionIconTestMission Create()
	{
		LoadVanillaNPCTextures([NPCID.BlueSlime]);
		LoadVanillaItemTextures([ItemID.LargeRuby]);
		return new TextureMissionIconTestMission();
	}
}