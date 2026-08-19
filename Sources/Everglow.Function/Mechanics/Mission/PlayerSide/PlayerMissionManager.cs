using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Hooks;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using MathNet.Numerics;

using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide;

public class PlayerMissionManager
{
	public const int UpdateInterval = 20;
	public static PlayerMissionManager Instance => ModContent.GetInstance<PlayerMissionSystem>().Manager;

	public event Action<MissionIdentity> MissionAdded;
	public event Action<MissionIdentity> MissionRemoved;
	public event Action<MissionIdentity> MissionStatusUpdated;
	public event Action<MissionIdentity> MissionObjectiveUpdated;

	private List<PlayerMissionBase> _missions = [];

	public IReadOnlyList<PlayerMissionBase> Missions => _missions;

	/// <summary>
	/// 历史杀怪计数
	/// </summary>
	private Dictionary<int, int> _nPCKillCounter = [];
	private bool _loaded;

	/// <summary>
	/// 已接受任务的任务池
	/// </summary>
	private IEnumerable<PlayerMissionBase> AcceptedMissions => _missions.Where(m => m.State == PlayerMissionState.Accepted);

	/// <summary>
	/// 历史杀怪计数
	/// </summary>
	public IReadOnlyDictionary<int, int> NPCKillCounter => _nPCKillCounter;

	#region TML Integration

	public void Load()
	{
		if (!Main.dedServ && !_loaded)
		{
			Main.OnTickForInternalCodeOnly += Update;
			Ins.HookManager.AddHook(Commons.Enums.CodeLayer.PostSaveAndQuit, Clear);
			MissionGlobalNPC.OnKillNPCEvent += MissionGlobalNPC_SpecialOnKill_CountKill;
			_loaded = true;
		}
	}

	public void Unload()
	{
		if (_loaded)
		{
			Main.OnTickForInternalCodeOnly -= Update;
			MissionGlobalNPC.OnKillNPCEvent -= MissionGlobalNPC_SpecialOnKill_CountKill;
			_loaded = false;
		}

		Clear();
		MissionAdded = null;
		MissionRemoved = null;
		MissionStatusUpdated = null;
		MissionObjectiveUpdated = null;
	}

	/// <summary>
	/// Initialize mission manager with player mission data
	/// </summary>
	/// <param name="data"></param>
	public void ApplyData(PlayerMissionManagerData data)
	{
		if (data == null)
		{
			return;
		}

		List<PlayerMissionBase> oldMissions = _missions;
		_nPCKillCounter = data.NPCKillCounter.ToDictionary();
		_missions = data.MissionPools.ToList();

		foreach (PlayerMissionBase mission in oldMissions)
		{
			OnMissionRemoved(mission);
		}

		foreach (var m in AcceptedMissions)
		{
			m.Activate();
		}

		foreach (PlayerMissionBase mission in _missions)
		{
			OnMissionAdded(mission);
		}
	}

	/// <summary>
	/// 清除所有任务池中的任务
	/// </summary>
	public void Clear()
	{
		List<PlayerMissionBase> removedMissions = _missions.ToList();
		foreach (var mission in _missions)
		{
			mission.Deactivate();
		}

		_nPCKillCounter.Clear();
		_missions.Clear();

		foreach (PlayerMissionBase mission in removedMissions)
		{
			OnMissionRemoved(mission);
		}
	}

	#endregion

	#region In-Game Update

	/// <summary>
	/// 为任务每帧更新
	/// </summary>
	public void Update()
	{
		// Main.gamePaused always be false here when triggered by Main.OnTickForInternalCodeOnly hook.
		if (Main.gameMenu || Main.gameInactive) // || Main.gamePaused
		{
			return;
		}

		if (Main.timeForVisualEffects % UpdateInterval != 0)
		{
			return;
		}

		// 更新所有任务
		foreach (var m in AcceptedMissions)
		{
			m.Update();
			OnMissionObjectiveUpdated(m);
		}

		// 处理自动提交任务
		var autoCommitMissions = AcceptedMissions.Where(m => m.CheckComplete() && m.AutoComplete).ToList();
		if (autoCommitMissions.Count > 0)
		{
			autoCommitMissions.ForEach(m => m.OnComplete());
		}

		// 处理过期任务
		var expiredMissions = AcceptedMissions.Where(m => m.CheckExpire()).ToList();
		if (expiredMissions.Count > 0)
		{
			expiredMissions.ForEach(m => m.OnExpire());
		}

		// 检测可提交状态改变的任务，将状态改变为可提交的任务抛出信息
		foreach (var m in AcceptedMissions.ToList())
		{
			if (m.CheckComplete() != m.OldCheckComplete)
			{
				m.OldCheckComplete = m.CheckComplete();

				m.OnCheckCompleteChange();

				// 由不可提交改变到可提交状态的任务, 发送消息提示
				if (m.CheckComplete())
				{
					Main.NewText($"[{m.Name}]任务可以提交了", 250, 250, 150);
				}
			}
		}
	}

	/// <summary>
	/// 记录杀怪
	/// </summary>
	/// <param name="npc">被击杀的NPC</param>
	/// <exception cref="InvalidParameterException">参数为空或npc类型错误</exception>
	public void MissionGlobalNPC_SpecialOnKill_CountKill(NPC npc)
	{
		if (npc.type <= NPCID.None)
		{
			Ins.Logger.Warn($"Invalid npc type {npc.type}.");
			return;
		}

		// Update NPC kill history
		if (!_nPCKillCounter.TryAdd(npc.type, 1))
		{
			_nPCKillCounter[npc.type]++;
		}
	}

	#endregion

	#region Manager Logic

	/// <summary>
	/// 获取某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <returns></returns>
	public PlayerMissionBase GetMission(string missionName) =>
		_missions.FirstOrDefault(m => m.Name == missionName);

	/// <summary>
	/// 获取某个类型的所有任务
	/// </summary>
	/// <typeparam name="T">任务的类型</typeparam>
	/// <param name="type">任务池类型</param>
	/// <returns>任务池内所有该类型的任务</returns>
	public List<T> GetMissions<T>()
		where T : PlayerMissionBase =>
		_missions.OfType<T>().ToList();

	/// <summary>
	/// Checks if a mission exists by type
	/// </summary>
	public bool HasMission<T>()
		where T : PlayerMissionBase =>
		HasMission(m => m is T);

	/// <summary>
	/// Checks if a mission exists by name
	/// </summary>
	public bool HasMission(string missionName) =>
		HasMission(m => m.Name == missionName);

	/// <summary>
	/// Internal implementation for mission checking
	/// </summary>
	private bool HasMission(Func<PlayerMissionBase, bool> predicate) =>
		_missions.Any(predicate);

	/// <summary>
	/// 向任务池中添加任务
	/// </summary>
	/// <param name="mission">任务</param>
	/// <param name="type">任务池类型</param>
	public void AddMission(PlayerMissionBase mission, PlayerMissionState type, bool showText = true)
	{
		if (!HasMission(mission.Name))
		{
			_missions.Add(mission);
			mission.State = type;

			if (showText)
			{
				Main.NewText($"新的任务任务已添加[{mission.DisplayName}]", 250, 250, 150);
			}

			if (type == PlayerMissionState.Accepted)
			{
				mission.Activate();
			}

			OnMissionAdded(mission);
		}
	}

	/// <summary>
	/// 移除任务池内指定条件的所有任务
	/// </summary>
	/// <param name="predicate">删除范围</param>
	/// <returns></returns>
	private bool RemoveMission(Func<PlayerMissionBase, bool> predicate)
	{
		List<PlayerMissionBase> removedMissions = _missions.Where(predicate).ToList();
		foreach (var m in removedMissions)
		{
			m.Deactivate();
		}

		var removed = _missions.RemoveAll(m => predicate(m));

		foreach (PlayerMissionBase mission in removedMissions)
		{
			OnMissionRemoved(mission);
		}

		return removed > 0;
	}

	/// <summary>
	/// 移除任务池内某个任务名的任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public bool RemoveMission(string missionName) =>
		RemoveMission(m => m.Name == missionName);

	/// <summary>
	/// 移除任务池内某个任务
	/// </summary>
	/// <typeparam name="T">任务类型</typeparam>
	/// <param name="type"></param>
	/// <returns></returns>
	public bool RemoveMission<T>()
		where T : PlayerMissionBase =>
		RemoveMission(m => m is T);

	/// <summary>
	/// 将某个任务从目前任务池移到另一个
	/// </summary>
	/// <param name="missionName">任务名称</param>
	/// <param name="fromType">任务目前所处任务池</param>
	/// <param name="toType">目标任务池</param>
	/// <returns>是否成功</returns>
	public bool MoveMission(string missionName, PlayerMissionState fromType, PlayerMissionState toType)
	{
		var mission = _missions.FirstOrDefault(m => m.Name == missionName);
		if (mission == null)
		{
			return false;
		}

		MoveMission(mission, fromType, toType);
		return true;
	}

	/// <summary>
	/// 将某个任务从目前任务池移到另一个
	/// </summary>
	/// <param name="mission">任务实例</param>
	/// <param name="fromType">任务目前所处任务池</param>
	/// <param name="toType">目标任务池</param>
	public void MoveMission(PlayerMissionBase mission, PlayerMissionState fromType, PlayerMissionState toType)
	{
		if (fromType == toType)
		{
			return;
		}

		mission.State = toType;

		if (toType == PlayerMissionState.Accepted)
		{
			mission.Activate();
		}
		else
		{
			mission.Deactivate();
		}

		OnMissionStatusUpdated(mission);
	}

	/// <summary>
	/// 获取任务池内的所有任务
	/// <para/>注: 该方法返回的是任务池的副本，修改该副本不会造成任何影响
	/// </summary>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public List<PlayerMissionBase> GetMissionPool(PlayerMissionState type) => _missions.Where(m => m.State == type).ToList();

	#endregion

	#region Data Persistance

	/// <summary>
	/// Save missions to player file data.
	/// <br/>Should only be called by <see cref="ModPlayer.SaveData(TagCompound)"/>.
	/// </summary>
	/// <param name="tag">Provided by <see cref="ModPlayer.SaveData(TagCompound)"/>.</param>
	public void SaveData(TagCompound tag)
	{
		tag.Add(nameof(_nPCKillCounter), _nPCKillCounter.ToList());
		tag.Add(nameof(_missions), _missions.ConvertAll(m =>
		{
			TagCompound mainT = [];
			TagCompound t = [];
			m.SaveData(t);
			mainT.Add("Type", m.GetType().FullName);
			mainT.Add("Data", t);
			return mainT;
		}));
	}

	/// <summary>
	/// Load missions from player file data.
	/// <br/>Should only be called by <see cref="ModPlayer.LoadData(TagCompound)"/>.
	/// </summary>
	/// <param name="tag">Provided by <see cref="ModPlayer.LoadData(TagCompound)"/>.</param>
	public PlayerMissionManagerData LoadData(TagCompound tag)
	{
		// Load npc kill counter.
		var nPCKillCounter = new Dictionary<int, int>();
		tag.TryGet<List<KeyValuePair<int, int>>>(nameof(_nPCKillCounter), out var nPCKillCounterStorage);
		if (nPCKillCounterStorage != null && nPCKillCounterStorage.Count > 0)
		{
			nPCKillCounter = nPCKillCounterStorage.ToDictionary();
		}

		// Load missions.
		var missions = new List<PlayerMissionBase>();

		if (tag.TryGet<IList<TagCompound>>(nameof(_missions), out var missionTags))
		{
			foreach (var mTag in missionTags)
			{
				if (mTag.TryGet<string>("Type", out var typeName)
					&& mTag.TryGet<TagCompound>("Data", out var data))
				{
					var type = Ins.ModuleManager.Types.FirstOrDefault(t => t.FullName == typeName);
					if (type != null
						&& Activator.CreateInstance(type) is PlayerMissionBase m)
					{
						m.LoadData(data);
						missions.Add(m);
					}
					else
					{
						Ins.Logger.Warn($"Invalid type {typeName} detected from player file.");
					}
				}
				else
				{
					Ins.Logger.Warn($"Invalid mission data detected from player file.");
				}
			}
		}

		return new PlayerMissionManagerData(nPCKillCounter, missions);
	}

	#endregion

	public void OnMissionAdded(PlayerMissionBase mission) => MissionAdded?.Invoke(GetIdentity(mission));

	public void OnMissionRemoved(PlayerMissionBase mission) => MissionRemoved?.Invoke(GetIdentity(mission));

	public void OnMissionStatusUpdated(PlayerMissionBase mission) => MissionStatusUpdated?.Invoke(GetIdentity(mission));

	public void OnMissionObjectiveUpdated(PlayerMissionBase mission) => MissionObjectiveUpdated?.Invoke(GetIdentity(mission));

	private static MissionIdentity GetIdentity(PlayerMissionBase mission) => new(MissionSide.Player, mission.Name, mission.InstanceId);
}
