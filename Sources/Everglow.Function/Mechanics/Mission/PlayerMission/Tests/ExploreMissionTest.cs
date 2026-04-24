using Everglow.Commons.Mechanics.Mission.PlayerMission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Objectives;
using Terraria.GameContent.Personalities;

namespace Everglow.Commons.Mechanics.Mission.PlayerMission.Tests;

public class ExploreMissionTest : MissionBase
{
	public override string DisplayName => nameof(ExploreMissionTest);

	public ExploreMissionTest()
	{
		Objectives.Add(new ExploreObjective(new ForestBiome(), 2000));
		RewardItems.Add(new Item(ItemID.Zenith, 100));
	}
}