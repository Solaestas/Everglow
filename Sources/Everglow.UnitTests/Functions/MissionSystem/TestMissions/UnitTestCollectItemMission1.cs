using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Templates;
using Terraria;

namespace Everglow.UnitTests.Functions.MissionSystem.TestMissions;

public class UnitTestCollectItemMission1 : CollectItemMission
{
	public UnitTestCollectItemMission1()
	{
		DemandCollectItems = [];
	}

	public UnitTestCollectItemMission1(List<CollectItemRequirement> requires)
	{
		DemandCollectItems = requires;
	}

	public override List<CollectItemRequirement> DemandCollectItems { get; init; }

	public override List<Item> RewardItems => [];

	public override string DisplayName => nameof(UnitTestCollectItemMission1);

	public override string Description => nameof(UnitTestCollectItemMission1);
}
