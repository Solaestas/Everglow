using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;
using Terraria;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
[DoNotParallelize]
public class WorldMissionManagerTest
{
	private int _originalNetMode;

	[TestInitialize]
	public void Initialize()
	{
		Terraria.Program.SavePath = string.Empty;
		_originalNetMode = Main.netMode;
		Main.netMode = Terraria.ID.NetmodeID.SinglePlayer;
	}

	[TestCleanup]
	public void Cleanup()
	{
		Main.netMode = _originalNetMode;
	}

	private class TestStateProvider : IGameStateProvider
	{
		public double TimeForVisualEffects { get; set; }

		public bool GameMenu => false;

		public bool GameInactive => false;

		public bool GamePaused => false;
	}

	private sealed class CheckingMission : WorldMissionBase
	{
		public string NameValue { get; init; }

		public override string Name => NameValue;

		public CheckingMission()
		{
			Objectives.Add(new PassiveObjective());
		}

		public void SetState(WorldMissionState state) => State = state;
	}

	private sealed class PassiveObjective : WorldObjectiveBase
	{
		public override bool CheckCompletion() => false;

		public override string GetObjectiveText() => string.Empty;

		public override void GetObjectivesIcon(MissionIconGroup iconGroup)
		{
		}
	}

	[TestMethod]
	public void Unload_ClearsMissionEventSubscriptions()
	{
		var manager = new WorldMissionManager(new TestStateProvider());
		var mission = new CheckingMission { NameValue = "Unload" };
		manager.AddMission(mission);
		int statusUpdateCount = 0;
		manager.MissionStatusUpdated += _ => statusUpdateCount++;

		manager.Unload();
		manager.OnMissionStatusUpdated(mission);

		Assert.AreEqual(0, statusUpdateCount);
	}

	[TestMethod]
	public void NetReceive_PublishesAfterSnapshotApplied()
	{
		var source = new CheckingMission { NameValue = "Snapshot" };
		source.SetState(WorldMissionState.Failed);
		var target = new CheckingMission { NameValue = "Snapshot" };
		var manager = new WorldMissionManager(new TestStateProvider());
		manager.AddMission(target);
		var statusUpdates = new List<MissionIdentity>();
		var objectiveUpdates = new List<MissionIdentity>();
		manager.MissionStatusUpdated += identity =>
		{
			statusUpdates.Add(identity);
			Assert.AreEqual(WorldMissionState.Failed, target.State);
		};
		manager.MissionObjectiveUpdated += objectiveUpdates.Add;

		using var stream = new MemoryStream();
		using (var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
		{
			source.NetSend(writer);
		}
		stream.Position = 0;
		using (var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen: true))
		{
			manager.NetReceive(reader);
		}

		var expectedIdentity = new MissionIdentity(MissionSide.World, target.Name, target.Name);
		Assert.HasCount(1, statusUpdates);
		Assert.HasCount(1, objectiveUpdates);
		Assert.AreEqual(expectedIdentity, statusUpdates[0]);
		Assert.AreEqual(expectedIdentity, objectiveUpdates[0]);
	}

	[TestMethod]
	public void LoadData_PublishesAfterSnapshotApplied()
	{
		var target = new CheckingMission { NameValue = "Snapshot" };
		var missing = new CheckingMission { NameValue = "Missing" };
		missing.SetState(WorldMissionState.Active);
		var manager = new WorldMissionManager(new TestStateProvider());
		manager.AddMission(target);
		manager.AddMission(missing);
		var statusUpdates = new List<MissionIdentity>();
		var objectiveUpdates = new List<MissionIdentity>();
		manager.MissionStatusUpdated += identity =>
		{
			statusUpdates.Add(identity);
			Assert.AreEqual(WorldMissionState.Failed, target.State);
		};
		manager.MissionObjectiveUpdated += objectiveUpdates.Add;
		var missionData = new Terraria.ModLoader.IO.TagCompound
		{
			[nameof(WorldMissionBase.State)] = (int)WorldMissionState.Failed,
		};
		var managerData = new Terraria.ModLoader.IO.TagCompound
		{
			[target.Name] = missionData,
		};

		manager.LoadData(managerData);

		MissionIdentity[] expectedIdentities =
		[
			new(MissionSide.World, target.Name, target.Name),
			new(MissionSide.World, missing.Name, missing.Name),
		];
		CollectionAssert.AreEqual(expectedIdentities, statusUpdates);
		CollectionAssert.AreEqual(expectedIdentities, objectiveUpdates);
		Assert.AreEqual(WorldMissionState.Locked, missing.State);
	}

}
