using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Terraria.ID;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
[DoNotParallelize]
public class PlayerMissionManagerTest
{
	private bool _originalDedServ;
	private bool _originalGameMenu;
	private bool _originalGameInactive;
	private double _originalTimeForVisualEffects;

	private sealed class StubMission : PlayerMissionBase
	{
		public string NameValue { get; init; } = nameof(StubMission);

		public int UpdateCount { get; private set; }

		public override string Name => NameValue;

		public override string DisplayName => NameValue;

		public override bool CheckComplete() => false;

		public override void Update() => UpdateCount++;

		public override void OnCheckCompleteChange()
		{
		}
	}

	private sealed class HookMission : PlayerMissionBase
	{
		public int ActivateHookCount { get; private set; }

		public override string DisplayName => nameof(HookMission);

		public override void Activate()
		{
			base.Activate();
			ActivateHookCount++;
		}
	}

	[TestInitialize]
	public void Initialize()
	{
		Terraria.Program.SavePath = string.Empty;
		_originalDedServ = Terraria.Main.dedServ;
		_originalGameMenu = Terraria.Main.gameMenu;
		_originalGameInactive = Terraria.Main.gameInactive;
		_originalTimeForVisualEffects = Terraria.Main.timeForVisualEffects;
		Terraria.Main.dedServ = true;
		Terraria.Main.gameMenu = false;
		Terraria.Main.gameInactive = false;
		Terraria.Main.timeForVisualEffects = PlayerMissionManager.UpdateInterval;
	}

	[TestCleanup]
	public void Cleanup()
	{
		Terraria.Main.dedServ = _originalDedServ;
		Terraria.Main.gameMenu = _originalGameMenu;
		Terraria.Main.gameInactive = _originalGameInactive;
		Terraria.Main.timeForVisualEffects = _originalTimeForVisualEffects;
	}

	[TestMethod]
	public void MoveMission_TransitionsAndActivatesMission()
	{
		var mission = new HookMission();
		var manager = new PlayerMissionManager();
		manager.AddMission(mission, PlayerMissionState.Available, showText: false);

		manager.MoveMission(mission, PlayerMissionState.Available, PlayerMissionState.Accepted);

		Assert.AreEqual(PlayerMissionState.Accepted, mission.State);
		Assert.AreEqual(1, mission.ActivateHookCount);
	}

	[TestMethod]
	public void Unload_ClearsChangedSubscriptions()
	{
		var manager = new PlayerMissionManager();
		int changeCount = 0;
		manager.Changed += () => changeCount++;

		manager.Unload();
		manager.OnChanged();

		Assert.AreEqual(0, changeCount);
	}

	[TestMethod]
	public void Changed_DoesNotSignalWithoutExistingRefreshRequest()
	{
		var first = new StubMission
		{
			NameValue = "First",
			State = PlayerMissionState.Available,
		};
		var second = new StubMission
		{
			NameValue = "Second",
			State = PlayerMissionState.Available,
		};
		var manager = new PlayerMissionManager();
		int changeCount = 0;
		manager.Changed += () => changeCount++;

		manager.ApplyData(new PlayerMissionManagerData([], [first]));
		manager.AddMission(second, PlayerMissionState.Available, showText: false);
		manager.MoveMission(first, PlayerMissionState.Available, PlayerMissionState.Accepted);
		manager.Update();
		manager.Clear();

		Assert.AreEqual(1, first.UpdateCount);
		Assert.AreEqual(0, changeCount);
	}

	[TestMethod]
	public void RemoveMission_SignalsChangedWhenRemovalOccurs()
	{
		var mission = new StubMission();
		var manager = new PlayerMissionManager();
		manager.ApplyData(new PlayerMissionManagerData([], [mission]));
		int changeCount = 0;
		manager.Changed += () => changeCount++;

		bool removed = manager.RemoveMission(mission.Name);
		bool repeated = manager.RemoveMission(mission.Name);

		Assert.IsTrue(removed);
		Assert.IsFalse(repeated);
		Assert.AreEqual(1, changeCount);
	}

	[TestMethod]
	public void MutableState_IsIsolatedAcrossManagerInstances()
	{
		var firstMission = new StubMission();
		var secondMission = new StubMission();
		var first = new PlayerMissionManager();
		var second = new PlayerMissionManager();

		first.ApplyData(new PlayerMissionManagerData(
			new Dictionary<int, int> { [NPCID.BlueSlime] = 3 },
			[firstMission]));
		second.ApplyData(new PlayerMissionManagerData(
			new Dictionary<int, int> { [NPCID.Zombie] = 7 },
			[secondMission]));

		Assert.AreSame(firstMission, first.GetMission(nameof(StubMission)));
		Assert.AreSame(secondMission, second.GetMission(nameof(StubMission)));
		Assert.AreEqual(3, first.NPCKillCounter[NPCID.BlueSlime]);
		Assert.IsFalse(second.NPCKillCounter.ContainsKey(NPCID.BlueSlime));
	}

	[TestMethod]
	public void GlobalAccessors_AreExposedByServicesInsteadOfSystem()
	{
		const System.Reflection.BindingFlags PublicStatic =
			System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static;

		var managerAccessor = typeof(PlayerMissionManager).GetProperty("Instance", PublicStatic);
		var actionsAccessor = typeof(PlayerMissionActions).GetProperty("Instance", PublicStatic);
		var systemAccessor = typeof(PlayerMissionSystem).GetProperty("Instance", PublicStatic);

		Assert.IsNotNull(managerAccessor);
		Assert.AreEqual(typeof(PlayerMissionManager), managerAccessor.PropertyType);
		Assert.IsNotNull(actionsAccessor);
		Assert.AreEqual(typeof(PlayerMissionActions), actionsAccessor.PropertyType);
		Assert.IsNull(systemAccessor);
	}
}
