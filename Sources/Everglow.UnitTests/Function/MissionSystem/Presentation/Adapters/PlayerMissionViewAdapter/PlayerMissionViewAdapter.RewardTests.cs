using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Terraria;

namespace Everglow.UnitTests.Function.MissionSystem;

public partial class PlayerMissionViewAdapterTest
{
	[TestMethod]
	public void Create_MapsRewardItemsByReferenceAndSnapshotsTheCollection()
	{
		var firstReward = new Item { type = 1, stack = 3 };
		var secondReward = new Item { type = 2, stack = 5 };
		var mission = new StubMission();
		mission.RewardItems.Add(firstReward);

		MissionView view = PlayerMissionViewAdapter.Create(mission);
		mission.RewardItems.Add(secondReward);

		Assert.HasCount(1, view.Rewards);
		Assert.AreSame(firstReward, view.Rewards[0].Item);
		Assert.AreEqual(string.Empty, view.Rewards[0].Description);
	}
}
