using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Terraria;

namespace Everglow.UnitTests.Function.MissionSystem;

public partial class WorldMissionViewAdapterTest
{
	[TestMethod]
	public void Create_MapsRewardItemsByReferenceAndSnapshotsWithoutClaimingThem()
	{
		var firstReward = new Item { type = 1, stack = 3 };
		var secondReward = new Item { type = 2, stack = 5 };
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Completed);
		mission.AddReward(firstReward);

		MissionView view = WorldMissionViewAdapter.Create(mission);
		mission.AddReward(secondReward);

		Assert.HasCount(1, view.Rewards);
		Assert.AreSame(firstReward, view.Rewards[0].Item);
		Assert.AreEqual(string.Empty, view.Rewards[0].Description);
		Assert.IsFalse(mission.RewardClaimed);
		Assert.IsEmpty(mission.RewardClaimedPlayers);
	}
}
