using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
[DoNotParallelize]
public class PlayerMissionActionTest
{
	private PlayerMissionManager _manager;
	private PlayerMissionActions _actions;

	private sealed class StubMission : PlayerMissionBase
	{
		public bool CancellableValue { get; set; }

		public bool CompleteValue { get; set; }

		public string HintValue { get; set; } = string.Empty;

		public override string DisplayName => nameof(StubMission);

		public override bool Cancellable => CancellableValue;

		public override string Hint => HintValue;

		public override bool CheckComplete() => CompleteValue;
	}

	[TestInitialize]
	public void Initialize()
	{
		_manager = new PlayerMissionManager();
		_actions = new PlayerMissionActions(_manager);
	}

	[TestCleanup]
	public void Cleanup() => _manager.Clear();

	[TestMethod]
	public void AvailableMission_ExportsOnlyAcceptAction()
	{
		var mission = new StubMission { State = PlayerMissionState.Available };

		IReadOnlyList<MissionAction> actions = PlayerMissionActionAdapter.GetActions(mission);

		Assert.HasCount(1, actions);
		Assert.AreEqual(MissionActionType.Accept, actions[0].Type);
		Assert.AreEqual(mission.Name, actions[0].Mission.DefinitionId);
		Assert.AreEqual(mission.InstanceId, actions[0].Mission.InstanceId);
	}

	[TestMethod]
	public void CancellableIncompleteActiveMission_ExportsOnlyCancelAction()
	{
		var mission = new StubMission
		{
			State = PlayerMissionState.Accepted,
			CancellableValue = true,
		};

		IReadOnlyList<MissionAction> actions = PlayerMissionActionAdapter.GetActions(mission);

		Assert.HasCount(1, actions);
		Assert.AreEqual(MissionActionType.Cancel, actions[0].Type);
	}

	[TestMethod]
	public void AcceptAction_ChangesStateOnceAndPreservesInstanceIdentity()
	{
		var mission = new StubMission { State = PlayerMissionState.Available };
		_manager.ApplyData(new PlayerMissionManagerData([], [mission]));
		MissionAction action = PlayerMissionActionAdapter.GetActions(mission).Single();
		string instanceId = mission.InstanceId;

		bool applied = _actions.TryExecute(action);
		bool repeated = _actions.TryExecute(action);

		Assert.IsTrue(applied);
		Assert.IsFalse(repeated);
		Assert.AreEqual(PlayerMissionState.Accepted, mission.State);
		Assert.AreEqual(instanceId, mission.InstanceId);
		Assert.IsTrue(_manager.NeedRefresh);
	}

	[TestMethod]
	[DataRow("Follow the trail")]
	[DataRow(MissionHintText.Masked)]
	public void HintedAvailableMission_ExportsNoActions(string hint)
	{
		var mission = new StubMission
		{
			State = PlayerMissionState.Available,
			HintValue = hint,
		};

		IReadOnlyList<MissionAction> actions = PlayerMissionActionAdapter.GetActions(mission);

		Assert.IsEmpty(actions);
	}

	[TestMethod]
	[DataRow(" ")]
	[DataRow("\t")]
	public void WhitespaceHint_DoesNotHideOrRejectAcceptAction(string hint)
	{
		var mission = new StubMission
		{
			State = PlayerMissionState.Available,
			HintValue = hint,
		};
		_manager.ApplyData(new PlayerMissionManagerData([], [mission]));

		MissionAction action = PlayerMissionActionAdapter.GetActions(mission).Single();
		bool applied = _actions.TryExecute(action);

		Assert.IsTrue(applied);
		Assert.AreEqual(PlayerMissionState.Accepted, mission.State);
	}

	[TestMethod]
	public void CancelAction_ChangesStateOnce()
	{
		var mission = new StubMission
		{
			State = PlayerMissionState.Accepted,
			CancellableValue = true,
		};
		_manager.ApplyData(new PlayerMissionManagerData([], [mission]));
		MissionAction action = PlayerMissionActionAdapter.GetActions(mission).Single();

		bool applied = _actions.TryExecute(action);
		bool repeated = _actions.TryExecute(action);

		Assert.IsTrue(applied);
		Assert.IsFalse(repeated);
		Assert.AreEqual(PlayerMissionState.Failed, mission.State);
	}

	[TestMethod]
	public void ActionForReplacedPlayerInstance_DoesNotAffectCurrentInstance()
	{
		var replaced = new StubMission { State = PlayerMissionState.Available };
		MissionAction staleAction = PlayerMissionActionAdapter.GetActions(replaced).Single();
		var current = new StubMission { State = PlayerMissionState.Available };
		_manager.ApplyData(new PlayerMissionManagerData([], [current]));

		bool applied = _actions.TryExecute(staleAction);

		Assert.IsFalse(applied);
		Assert.AreEqual(PlayerMissionState.Available, current.State);
		Assert.AreNotEqual(staleAction.Mission.InstanceId, current.InstanceId);
	}

	[TestMethod]
	public void HintAddedAfterExport_PreventsExecution()
	{
		var mission = new StubMission { State = PlayerMissionState.Available };
		_manager.ApplyData(new PlayerMissionManagerData([], [mission]));
		MissionAction action = PlayerMissionActionAdapter.GetActions(mission).Single();
		mission.HintValue = MissionHintText.Masked;

		bool applied = _actions.TryExecute(action);

		Assert.IsFalse(applied);
		Assert.AreEqual(PlayerMissionState.Available, mission.State);
	}

	[TestMethod]
	public void InvisibleAvailableMission_StillExportsAcceptAction()
	{
		var mission = new StubMission
		{
			State = PlayerMissionState.Available,
			IsVisible = false,
		};

		IReadOnlyList<MissionAction> actions = PlayerMissionActionAdapter.GetActions(mission);

		Assert.HasCount(1, actions);
		Assert.AreEqual(MissionActionType.Accept, actions[0].Type);
	}
}
