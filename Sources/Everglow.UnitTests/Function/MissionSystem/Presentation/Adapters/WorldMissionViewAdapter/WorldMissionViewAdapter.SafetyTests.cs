using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Terraria;

namespace Everglow.UnitTests.Function.MissionSystem;

public partial class WorldMissionViewAdapterTest
{
	[TestMethod]
	public void Create_DoesNotTriggerObjectiveBehaviorPersistenceNetworkOrRewardClaims()
	{
		var reward = new Item { type = 1, stack = 1 };
		var objective = new StubObjective { ProgressValue = 0.6f };
		var mission = new StubMission
		{
			ProgressValue = 0.6f,
			TimeLimitValue = 300,
		};
		mission.SetState(WorldMissionState.Active);
		mission.SetTime(90);
		mission.Objectives.Add(objective);
		mission.SetRewards(reward);
		SetWhoAmI(mission, 19);

		MissionView view = WorldMissionViewAdapter.Create(mission);

		Assert.AreEqual(WorldMissionState.Active, mission.State);
		Assert.AreEqual(90, mission.Time);
		Assert.AreEqual(19, mission.WhoAmI);
		Assert.IsFalse(mission.RewardClaimed);
		Assert.IsEmpty(mission.RewardClaimedPlayers);
		Assert.IsFalse(objective.Completed);
		Assert.AreEqual(0, objective.CheckCompletionCalls);
		Assert.AreEqual(0, objective.UpdateCalls);
		Assert.AreEqual(0, objective.CompleteCalls);
		Assert.AreEqual(0, objective.ActivateCalls);
		Assert.AreEqual(0, objective.DeactivateCalls);
		Assert.AreEqual(0, objective.ResetCalls);
		Assert.AreEqual(0, objective.PersistenceCalls);
		Assert.AreEqual(0, objective.NetworkCalls);
		Assert.HasCount(1, view.Rewards);
		Assert.AreSame(reward, view.Rewards[0].Item);
	}
}
