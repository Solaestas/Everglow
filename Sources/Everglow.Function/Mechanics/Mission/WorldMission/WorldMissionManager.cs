using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Utilities;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission;

public class WorldMissionManager
{
	// Add a config option for update interval if necessary.
#if DEBUG
	public const int UpdateInterval = 1;
#else
	public const int UpdateInterval = 30;
#endif

	public static WorldMissionManager Instance => ModContent.GetInstance<WorldMissionSystem>().Manager;

	private IGameStateProvider _gameState;

	private List<WorldMissionBase> _missions = [];

	private int UpdateTimer => (int)_gameState.TimeForVisualEffects;

	public WorldMissionManager()
	{
		_gameState = GameStateProvider.Default;
	}

	/// <summary>
	/// For test only
	/// </summary>
	/// <param name="gameStateProvider"></param>
	/// <param name="missions"></param>
	public WorldMissionManager(IGameStateProvider gameStateProvider)
	{
		_gameState = gameStateProvider;
	}

	public void Load()
	{
		// Initialize mission manager: load missions, set up main hooks, etc.
		_missions = [];

		var source = ModLoader.Mods
			.Select(m => (m.Name, AssemblyManager.GetLoadableTypes(m.Code).AsEnumerable()))
			.Concat([(nameof(Everglow), Ins.ModuleManager.Types)])
			.Distinct();
		foreach (var (modName, modTypes) in source)
		{
			foreach (var mT in modTypes.Where(t => t.IsSubclassOf(typeof(WorldMissionBase)) && !t.IsAbstract))
			{
				var mission = Activator.CreateInstance(mT);
				_missions.Add(mission as WorldMissionBase);
			}
		}
		Main.OnTickForInternalCodeOnly += Update;
	}

	public void Unload()
	{
		// Clean up mission manager: clear mission data, remove hooks, etc.
		Main.OnTickForInternalCodeOnly -= Update;
		Clear();
		_missions = null;
	}

	public void Initialize()
	{
		foreach (var m in _missions)
		{
			m.Initialize();
		}
	}

	public void Clear()
	{
		_missions.ForEach(m => m.Reset());
	}

	public void Update()
	{
		// Main.gamePaused always be false here when triggered by Main.OnTickForInternalCodeOnly hook.
		// Main.gameInactive always be true on the server
		if (_gameState.GameMenu)
		{
			return;
		}

		if (UpdateTimer % UpdateInterval != 0)
		{
			return;
		}

		// Check locked
		foreach (var m in _missions.Where(m => m.State == WorldMissionState.Locked))
		{
			if (m.CanUnlock())
			{
				// Unlock mission
				// 1. activate (set first + attach hooks)
				// 2. set state to active
				m.Unlock();
			}
		}

		// Check active
		foreach (var m in _missions.Where(m => m.State == WorldMissionState.Active))
		{
			m.Update();

			if (NetUtils.IsClient)
			{
				m.OnMPSync();
			}
		}
	}

	public WorldMissionBase GetMission(int whoAmI) =>
		_missions.FirstOrDefault(m => m.WhoAmI == whoAmI);

	public WorldMissionBase GetMission(string name) =>
		_missions.FirstOrDefault(m => m.Name == name);

	public WorldMissionBase GetMission<T>()
		where T : WorldMissionBase =>
		_missions.OfType<T>().FirstOrDefault();

	public IEnumerable<WorldMissionBase> GetMissions(WorldMissionState state) =>
		_missions.Where(m => m.State == state);

	public void AddMission(WorldMissionBase mission)
	{
		if (_missions.Any(m => m.Name == mission.Name))
		{
			throw new InvalidOperationException($"Mission with name {mission.Name} already exists.");
		}
		_missions.Add(mission);
	}

	/// <summary>
	/// Use this method to activate a mission by its WhoAmI,
	/// which is useful when receiving a request from server to activate a mission.
	/// </summary>
	/// <param name="whoAmI"></param>
	/// <returns></returns>
	public bool ActivateMission(int whoAmI)
	{
		if (whoAmI < 0 || whoAmI >= _missions.Count)
		{
			Ins.Logger.Warn($"Attempted to activate mission with invalid WhoAmI: {whoAmI}");
			return false;
		}

		var mission = _missions[whoAmI];
		ActivateMission(mission);

		return true;
	}

	public bool ActivateMission(WorldMissionBase mission)
	{
		if (mission.State == WorldMissionState.Active)
		{
			Ins.Logger.Warn($"Attempted to activate mission {mission.Name} which is already active.");
			return false;
		}

		// This method is called in two cases:
		// 1. Receving a request to 'unlock' a mission from server.
		// 2. Receving a request to 'retry' a mission from server.
		// If the call aiming to 'retry', we also need to reset the mission progress.
		// Later, we may want to separate the 'unlock' and 'retry' logic into two methods if they have significant differences.

		// 1. Activate (Set first objective + attach hooks)
		// 2. Set state to active
		// 3. (Optional) reset progress if it's a retry request
		throw new NotImplementedException();

		return true;
	}

	public void GiveRewards(WorldMissionBase mission)
	{
		// The request for giving rewards should be sent to server first for validation
		// before the actual method to give rewards is called.
		throw new NotImplementedException();
	}

	public bool ResetMission(WorldMissionBase mission)
	{
		throw new NotImplementedException();
	}

	#region Persistence & Netcode

	public void NetSend(BinaryWriter writer)
	{
		foreach (var m in _missions)
		{
			m.NetSend(writer);
		}
	}

	public void NetReceive(BinaryReader reader)
	{
		foreach (var m in _missions)
		{
			m.NetReceive(reader);
		}
	}

	public void SaveData(TagCompound tag)
	{
		foreach (var m in _missions)
		{
			var mTag = new TagCompound();
			m.SaveData(mTag);
			tag.Add(m.Name, mTag);
		}
	}

	public void LoadData(TagCompound tag)
	{
		Clear();

		foreach (var m in _missions)
		{
			if (tag.TryGet<TagCompound>(m.Name, out var mTag))
			{
				m.LoadData(mTag);
			}
			else
			{
				// Handle missing mission data if necessary.
			}
		}
	}

	#endregion
}