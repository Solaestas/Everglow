using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
[DoNotParallelize]
public class MissionPresentationServiceTest
{
	private sealed class StubPlayerMission : PlayerMissionBase
	{
		public string NameValue { get; init; } = nameof(StubPlayerMission);

		public bool CompleteValue { get; set; }

		public bool PreCompleteValue { get; set; } = true;

		public override string Name => NameValue;

		public override string DisplayName => NameValue;

		public override bool CheckComplete() => CompleteValue;

		public override bool PreComplete() => PreCompleteValue;
	}

	private sealed class StubWorldMission : WorldMissionBase
	{
		public string NameValue { get; init; } = nameof(StubWorldMission);

		public override string Name => NameValue;

		public override string Hint => MissionHintText.Masked;

		public override float Progress => 0f;
	}

	private sealed class StubGameStateProvider : IGameStateProvider
	{
		public double TimeForVisualEffects => 0;

		public bool GameMenu => false;

		public bool GameInactive => false;

		public bool GamePaused => false;
	}

	[TestMethod]
	public void GetAll_ReturnsCoherentPlayerAndWorldEntries()
	{
		var player = new StubPlayerMission { State = PlayerMissionState.Available };
		var world = new StubWorldMission();
		var context = CreateService([player], [world]);

		IReadOnlyList<MissionPresentationEntry> entries = context.Service.GetAll();

		Assert.HasCount(2, entries);
		MissionPresentationEntry playerEntry = entries.Single(entry => entry.View.Identity.Side == MissionSide.Player);
		MissionPresentationEntry worldEntry = entries.Single(entry => entry.View.Identity.Side == MissionSide.World);
		Assert.AreEqual(player.InstanceId, playerEntry.View.Identity.InstanceId);
		Assert.HasCount(1, playerEntry.Actions);
		Assert.AreEqual(playerEntry.View.Identity, playerEntry.Actions[0].Mission);
		Assert.AreEqual(world.Name, worldEntry.View.Identity.DefinitionId);
		Assert.AreEqual(world.Name, worldEntry.View.Identity.InstanceId);
	}

	[TestMethod]
	public void TryGet_RequiresCompleteCurrentIdentity()
	{
		var player = new StubPlayerMission { State = PlayerMissionState.Available };
		var world = new StubWorldMission();
		var context = CreateService([player], [world]);
		var playerIdentity = new MissionIdentity(MissionSide.Player, player.Name, player.InstanceId);
		var stalePlayerIdentity = playerIdentity with { InstanceId = Guid.NewGuid().ToString("N") };
		var worldIdentity = new MissionIdentity(MissionSide.World, world.Name, world.Name);
		var mismatchedWorldIdentity = worldIdentity with { InstanceId = "runtime-index" };

		bool foundPlayer = context.Service.TryGet(playerIdentity, out MissionPresentationEntry playerEntry);
		bool foundStalePlayer = context.Service.TryGet(stalePlayerIdentity, out _);
		bool foundWorld = context.Service.TryGet(worldIdentity, out MissionPresentationEntry worldEntry);
		bool foundMismatchedWorld = context.Service.TryGet(mismatchedWorldIdentity, out _);
		bool foundWrongCase = context.Service.TryGet(playerIdentity with { DefinitionId = player.Name.ToLowerInvariant() }, out _);

		Assert.IsTrue(foundPlayer);
		Assert.AreEqual(playerIdentity, playerEntry.View.Identity);
		Assert.IsFalse(foundStalePlayer);
		Assert.IsTrue(foundWorld);
		Assert.AreEqual(worldIdentity, worldEntry.View.Identity);
		Assert.IsFalse(foundMismatchedWorld);
		Assert.IsFalse(foundWrongCase);
	}

	[TestMethod]
	public void MissingIdentity_ReturnsNoEntryAndRejectsAction()
	{
		var context = CreateService([], []);
		var identity = new MissionIdentity(MissionSide.Player, "missing", Guid.NewGuid().ToString("N"));

		Assert.IsFalse(context.Service.TryGet(identity, out _));
		Assert.IsFalse(context.Service.TryExecute(new MissionAction(identity, MissionActionType.Accept)));
	}

	[TestMethod]
	public void ReturnedEntry_RemainsUnchangedAfterManagerMutation()
	{
		var player = new StubPlayerMission { State = PlayerMissionState.Available };
		var context = CreateService([player], []);
		MissionPresentationEntry oldEntry = context.Service.GetAll().Single();

		context.PlayerManager.ChangeMissionState(player, PlayerMissionState.Available, PlayerMissionState.Accepted);
		MissionPresentationEntry currentEntry = context.Service.GetAll().Single();

		Assert.AreEqual(MissionViewState.Available, oldEntry.View.State);
		Assert.HasCount(1, oldEntry.Actions);
		Assert.AreEqual(MissionActionType.Accept, oldEntry.Actions[0].Type);
		Assert.AreEqual(MissionViewState.Active, currentEntry.View.State);
		Assert.IsEmpty(currentEntry.Actions);
	}

	[TestMethod]
	public void TryExecute_DispatchesAndRevalidatesAction()
	{
		var executable = new StubPlayerMission
		{
			NameValue = "Executable",
			State = PlayerMissionState.Available,
		};
		var rejected = new StubPlayerMission
		{
			NameValue = "Rejected",
			State = PlayerMissionState.Accepted,
			CompleteValue = true,
			PreCompleteValue = false,
		};
		var context = CreateService([executable, rejected], []);
		MissionAction accept = PlayerMissionActionAdapter.GetActions(executable).Single();
		MissionAction submit = PlayerMissionActionAdapter.GetActions(rejected).Single();
		MissionAction stale = accept with
		{
			Mission = accept.Mission with { InstanceId = Guid.NewGuid().ToString("N") },
		};

		bool executed = context.Service.TryExecute(accept);
		bool staleExecuted = context.Service.TryExecute(stale);
		bool rejectedExecuted = context.Service.TryExecute(submit);

		Assert.IsTrue(executed);
		Assert.AreEqual(PlayerMissionState.Accepted, executable.State);
		Assert.IsFalse(staleExecuted);
		Assert.IsFalse(rejectedExecuted);
		Assert.AreEqual(PlayerMissionState.Accepted, rejected.State);
	}

	private static (
		MissionPresentationService Service,
		PlayerMissionManager PlayerManager) CreateService(
		IEnumerable<PlayerMissionBase> playerMissions,
		IEnumerable<WorldMissionBase> worldMissions)
	{
		var playerManager = new PlayerMissionManager();
		playerManager.ApplyData(new PlayerMissionManagerData([], playerMissions.ToList()));
		var worldManager = new WorldMissionManager(new StubGameStateProvider());
		foreach (WorldMissionBase mission in worldMissions)
		{
			worldManager.AddMission(mission);
		}
		var service = new MissionPresentationService(
			playerManager,
			new PlayerMissionActions(playerManager),
			worldManager,
			new WorldMissionActions(worldManager));
		return (service, playerManager);
	}
}
