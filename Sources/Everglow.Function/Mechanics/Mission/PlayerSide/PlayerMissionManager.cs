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
	/// 已接受的任务
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
		_missions = data.Missions.ToList();

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
	/// 清除所有任务
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
	/// 添加任务
	/// </summary>
	/// <param name="mission">任务</param>
	/// <param name="state">任务状态</param>
	public void AddMission(PlayerMissionBase mission, PlayerMissionState state, bool showText = true)
	{
		if (!_missions.Any(m => m.Name == mission.Name))
		{
			_missions.Add(mission);
			mission.State = state;

			if (showText)
			{
				Main.NewText($"新的任务任务已添加[{mission.DisplayName}]", 250, 250, 150);
			}

			if (state == PlayerMissionState.Accepted)
			{
				mission.Activate();
			}

			OnMissionAdded(mission);
		}
	}

	/// <summary>
	/// 移除指定任务名的任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <returns></returns>
	public bool RemoveMission(string missionName)
	{
		List<PlayerMissionBase> removedMissions = _missions.Where(m => m.Name == missionName).ToList();
		foreach (var m in removedMissions)
		{
			m.Deactivate();
		}

		var removed = _missions.RemoveAll(m => m.Name == missionName);

		foreach (PlayerMissionBase mission in removedMissions)
		{
			OnMissionRemoved(mission);
		}

		return removed > 0;
	}

	/// <summary>
	/// 更改指定任务的状态
	/// </summary>
	/// <param name="mission">任务实例</param>
	/// <param name="fromState">任务当前状态</param>
	/// <param name="toState">任务目标状态</param>
	public void ChangeMissionState(PlayerMissionBase mission, PlayerMissionState fromState, PlayerMissionState toState)
	{
		if (fromState == toState)
		{
			return;
		}

		mission.State = toState;

		if (toState == PlayerMissionState.Accepted)
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
	/// 获取指定状态的所有任务
	/// <para/>注: 该方法返回的是任务列表的副本，修改该副本不会造成任何影响
	/// </summary>
	/// <param name="state">任务状态</param>
	/// <returns></returns>
	public List<PlayerMissionBase> GetMissions(PlayerMissionState state) => _missions.Where(m => m.State == state).ToList();

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
