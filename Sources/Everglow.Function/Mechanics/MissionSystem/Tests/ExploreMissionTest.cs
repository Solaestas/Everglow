using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Terraria.GameContent.Personalities;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class ExploreMissionTest : MissionBase
{
	public override string DisplayName => nameof(ExploreMissionTest);

	public ExploreMissionTest()
	{
		Objectives.Add(new ExploreObjective(new ForestBiome(), 2000));
		RewardItems.Add(new Item(ItemID.Zenith, 100));
	}
}