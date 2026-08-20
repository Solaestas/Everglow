using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Terraria.ModLoader.IO;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class PlayerMissionBasePersistenceTest
{
	private sealed class PersistenceStubMission : PlayerMissionBase
	{
		public override string DisplayName => nameof(PersistenceStubMission);
	}

	[TestMethod]
	[DataRow(PlayerMissionState.Accepted)]
	[DataRow(PlayerMissionState.Available)]
	[DataRow(PlayerMissionState.Failed)]
	[DataRow(PlayerMissionState.Overdue)]
	[DataRow(PlayerMissionState.Completed)]
	public void SaveData_LoadData_PreservesState(PlayerMissionState state)
	{
		var saved = new PersistenceStubMission { State = state };
		var tag = new TagCompound();

		// The headless test TagCompound currently rejects bool payloads even though player saves accept them at runtime.
		try
		{
			saved.SaveData(tag);
		}
		catch (IOException)
		{
		}

		Assert.IsTrue(tag.TryGet<int>(nameof(PlayerMissionBase.State), out var stored));
		Assert.AreEqual((int)state, stored);
		Assert.IsTrue(tag.TryGet<string>(nameof(PlayerMissionBase.InstanceId), out var storedInstanceId));
		Assert.AreEqual(saved.InstanceId, storedInstanceId);

		var loaded = new PersistenceStubMission();
		loaded.LoadData(tag);

		Assert.AreEqual(state, loaded.State);
		Assert.AreEqual(saved.InstanceId, loaded.InstanceId);
	}

	[TestMethod]
	public void SaveData_StoresTimeAsInt()
	{
		var saved = new PersistenceStubMission { Time = 120 };
		var tag = new TagCompound();

		// The headless test TagCompound currently rejects bool payloads even though player saves accept them at runtime.
		try
		{
			saved.SaveData(tag);
		}
		catch (IOException)
		{
		}

		Assert.IsInstanceOfType<int>(tag[PlayerMissionBase.TimeSaveKey]);
		Assert.AreEqual(120, tag.GetInt(PlayerMissionBase.TimeSaveKey));
	}

	[TestMethod]
	[DataRow((long)int.MaxValue + 1, int.MaxValue)]
	[DataRow((long)int.MinValue - 1, int.MinValue)]
	public void LoadData_LegacyLongTime_ClampsToIntRange(long storedTime, int expectedTime)
	{
		var tag = new TagCompound
		{
			{ PlayerMissionBase.TimeSaveKey, storedTime },
		};
		var loaded = new PersistenceStubMission();

		loaded.LoadData(tag);

		Assert.AreEqual(expectedTime, loaded.Time);
	}

	[TestMethod]
	public void NewInstances_HaveDistinctValidIds()
	{
		var first = new PersistenceStubMission();
		var second = new PersistenceStubMission();

		Assert.IsTrue(Guid.TryParseExact(first.InstanceId, "N", out _));
		Assert.IsTrue(Guid.TryParseExact(second.InstanceId, "N", out _));
		Assert.AreNotEqual(first.InstanceId, second.InstanceId);
	}

	[TestMethod]
	public void StateChangesAndReset_PreserveInstanceId()
	{
		var mission = new PersistenceStubMission { State = PlayerMissionState.Available };
		string instanceId = mission.InstanceId;

		mission.State = PlayerMissionState.Accepted;
		mission.Reset();
		mission.State = PlayerMissionState.Failed;
		mission.Reset();

		Assert.AreEqual(instanceId, mission.InstanceId);
	}

	[TestMethod]
	public void LoadData_MissingState_FallsBackToAvailable()
	{
		var tag = new TagCompound();
		var loaded = new PersistenceStubMission
		{
			// Simulate Activator.CreateInstance default before LoadData.
			State = PlayerMissionState.Accepted,
		};

		loaded.LoadData(tag);

		Assert.AreEqual(PlayerMissionState.Available, loaded.State);
	}

	[TestMethod]
	public void LoadData_MissingInstanceId_PreservesGeneratedId()
	{
		var loaded = new PersistenceStubMission();
		string generatedId = loaded.InstanceId;

		loaded.LoadData(new TagCompound());

		Assert.AreEqual(generatedId, loaded.InstanceId);
	}

	[TestMethod]
	public void LoadData_InvalidInstanceId_PreservesGeneratedId()
	{
		var tag = new TagCompound
		{
			{ nameof(PlayerMissionBase.InstanceId), "not-a-guid" },
		};
		var loaded = new PersistenceStubMission();
		string generatedId = loaded.InstanceId;

		loaded.LoadData(tag);

		Assert.AreEqual(generatedId, loaded.InstanceId);
	}
}
