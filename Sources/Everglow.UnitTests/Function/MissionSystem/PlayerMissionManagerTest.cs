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
		public override string DisplayName => nameof(StubMission);
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
		first.NeedRefresh = false;
		second.NeedRefresh = false;
		first.NeedRefresh = true;

		Assert.AreSame(firstMission, first.GetMission(nameof(StubMission)));
		Assert.AreSame(secondMission, second.GetMission(nameof(StubMission)));
		Assert.AreEqual(3, first.NPCKillCounter[NPCID.BlueSlime]);
		Assert.IsFalse(second.NPCKillCounter.ContainsKey(NPCID.BlueSlime));
		Assert.IsTrue(first.NeedRefresh);
		Assert.IsFalse(second.NeedRefresh);
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
