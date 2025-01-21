using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Enums;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Shared.Icons;
using Everglow.Commons.MissionSystem.Templates;

namespace Everglow.Commons.MissionSystem.Tests;

public class TextureMissionIconTestMission : CollectItemMission
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

	public override MissionType MissionType => MissionType.MainStory;

	public override List<CollectItemRequirement> DemandCollectItems { get; init; } = [CollectItemRequirement.Create([ItemID.IronOre], 1000)];

	public override List<Item> RewardItems => [new Item(ItemID.Zenith, 1000)];

	public static TextureMissionIconTestMission Create()
	{
		LoadVanillaNPCTextures([NPCID.BlueSlime]);
		LoadVanillaItemTextures([ItemID.LargeRuby]);
		return new TextureMissionIconTestMission();
	}
}