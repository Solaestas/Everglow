using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;
using Terraria;
using Terraria.ID;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
[DoNotParallelize]
public class WorldMissionActionTest
{
	private sealed class StubMission : WorldMissionBase
	{
		public string HintValue { get; set; } = string.Empty;

		public StubMission()
		{
			Objectives.Add(new StubObjective());
		}

		public void SetState(WorldMissionState state) => State = state;

		public void SetTime(int time) => Time = time;

		public override string Hint => HintValue;
	}

	private sealed class StubObjective : WorldObjectiveBase
	{
		public override bool CheckCompletion() => false;

		public override string GetObjectiveText() => string.Empty;

		public override void GetObjectivesIcon(MissionIconGroup iconGroup)
		{
		}
	}

	private sealed class StubGameStateProvider : IGameStateProvider
	{
		public double TimeForVisualEffects => 0;

		public bool GameMenu => false;

		public bool GameInactive => false;

		public bool GamePaused => false;
	}

	[TestInitialize]
	public void Initialize()
	{
		Terraria.Program.SavePath = string.Empty;
		Main.netMode = NetmodeID.SinglePlayer;
		Main.myPlayer = 0;
		Main.player[Main.myPlayer] = new Player { name = "ActionTester" };
	}

	[TestMethod]
	public void FailedSinglePlayerMission_ExportsOnlyRetryAction()
	{
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Failed);

		IReadOnlyList<MissionAction> actions = WorldMissionActionAdapter.GetActions(mission);

		Assert.HasCount(1, actions);
		Assert.AreEqual(MissionActionType.Retry, actions[0].Type);
		Assert.AreEqual(mission.Name, actions[0].Mission.DefinitionId);
		Assert.AreEqual(mission.Name, actions[0].Mission.InstanceId);
	}

	[TestMethod]
	public void CompletedUnclaimedSinglePlayerMission_ExportsOnlyClaimRewardAction()
	{
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Completed);

		IReadOnlyList<MissionAction> actions = WorldMissionActionAdapter.GetActions(mission);

		Assert.HasCount(1, actions);
		Assert.AreEqual(MissionActionType.ClaimReward, actions[0].Type);
	}

	[TestMethod]
	public void RetryAction_ChangesStateOnceAndKeepsWorldIdentity()
	{
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Failed);
		mission.SetTime(120);
		var manager = new WorldMissionManager(new StubGameStateProvider());
		manager.AddMission(mission);
		var actions = new WorldMissionActions(manager);
		MissionAction action = WorldMissionActionAdapter.GetActions(mission).Single();

		bool applied = actions.TryExecute(action);
		bool repeated = actions.TryExecute(action);

		Assert.IsTrue(applied);
		Assert.IsFalse(repeated);
		Assert.AreEqual(WorldMissionState.Active, mission.State);
		Assert.AreEqual(0, mission.Time);
		Assert.AreEqual(mission.Name, action.Mission.InstanceId);
	}

	[TestMethod]
	public void RetryAction_PublishesStatusAndObjectiveUpdates()
	{
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Failed);
		var manager = new WorldMissionManager(new StubGameStateProvider());
		manager.AddMission(mission);
		var actions = new WorldMissionActions(manager);
		int statusUpdateCount = 0;
		int objectiveUpdateCount = 0;
		manager.MissionStatusUpdated += _ => statusUpdateCount++;
		manager.MissionObjectiveUpdated += _ => objectiveUpdateCount++;
		MissionAction action = WorldMissionActionAdapter.GetActions(mission).Single();

		bool applied = actions.TryExecute(action);

		Assert.IsTrue(applied);
		Assert.AreEqual(1, statusUpdateCount);
		Assert.AreEqual(1, objectiveUpdateCount);
	}

	[TestMethod]
	public void RetryAction_PublishesRestartedNotification()
	{
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Failed);
		var manager = new WorldMissionManager(new StubGameStateProvider());
		manager.AddMission(mission);
		var actions = new WorldMissionActions(manager);
		MissionAction action = WorldMissionActionAdapter.GetActions(mission).Single();
		MissionNotification? notification = null;
		WorldMissionManager.NotificationRequested += CaptureNotification;

		try
		{
			bool applied = actions.TryExecute(action);

			Assert.IsTrue(applied);
			Assert.AreEqual(
				new MissionNotification(action.Mission, MissionNotificationType.Restarted),
				notification);
		}
		finally
		{
			WorldMissionManager.NotificationRequested -= CaptureNotification;
		}

		void CaptureNotification(MissionNotification value) => notification = value;
	}

	[TestMethod]
	public void ClaimRewardAction_ClaimsOnceForLocalPlayer()
	{
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Completed);
		var manager = new WorldMissionManager(new StubGameStateProvider());
		manager.AddMission(mission);
		var actions = new WorldMissionActions(manager);
		MissionAction action = WorldMissionActionAdapter.GetActions(mission).Single();

		bool applied = actions.TryExecute(action);
		bool repeated = actions.TryExecute(action);

		Assert.IsTrue(applied);
		Assert.IsFalse(repeated);
		Assert.IsTrue(mission.RewardClaimed);
		Assert.IsTrue(mission.RewardClaimedPlayers.Contains("ActionTester"));
	}

	[TestMethod]
	[DataRow("Follow the trail")]
	[DataRow(MissionHintText.Masked)]
	public void HintedFailedSinglePlayerMission_ExportsNoActions(string hint)
	{
		var mission = new StubMission { HintValue = hint };
		mission.SetState(WorldMissionState.Failed);

		IReadOnlyList<MissionAction> actions = WorldMissionActionAdapter.GetActions(mission);

		Assert.IsEmpty(actions);
	}

	[TestMethod]
	[DataRow(" ")]
	[DataRow("\t")]
	public void WhitespaceHint_DoesNotHideOrRejectRetryAction(string hint)
	{
		var mission = new StubMission { HintValue = hint };
		mission.SetState(WorldMissionState.Failed);
		var manager = new WorldMissionManager(new StubGameStateProvider());
		manager.AddMission(mission);
		var worldActions = new WorldMissionActions(manager);

		MissionAction action = WorldMissionActionAdapter.GetActions(mission).Single();
		bool applied = worldActions.TryExecute(action);

		Assert.IsTrue(applied);
		Assert.AreEqual(WorldMissionState.Active, mission.State);
	}

	[TestMethod]
	public void MultiplayerMission_ExportsNoActionsAndRejectsStaleSinglePlayerAction()
	{
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Failed);
		MissionAction singlePlayerAction = WorldMissionActionAdapter.GetActions(mission).Single();
		var manager = new WorldMissionManager(new StubGameStateProvider());
		manager.AddMission(mission);
		var worldActions = new WorldMissionActions(manager);
		Main.netMode = NetmodeID.MultiplayerClient;

		IReadOnlyList<MissionAction> actions = WorldMissionActionAdapter.GetActions(mission);
		bool applied = worldActions.TryExecute(singlePlayerAction);

		Assert.IsEmpty(actions);
		Assert.IsFalse(applied);
		Assert.AreEqual(WorldMissionState.Failed, mission.State);
	}

	[TestMethod]
	public void WorldActionWithMismatchedIdentity_DoesNotChangeMission()
	{
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Failed);
		var manager = new WorldMissionManager(new StubGameStateProvider());
		manager.AddMission(mission);
		var actions = new WorldMissionActions(manager);
		var action = new MissionAction(
			new(MissionSide.World, mission.Name, "runtime-index"),
			MissionActionType.Retry);

		bool applied = actions.TryExecute(action);

		Assert.IsFalse(applied);
		Assert.AreEqual(WorldMissionState.Failed, mission.State);
	}
}
