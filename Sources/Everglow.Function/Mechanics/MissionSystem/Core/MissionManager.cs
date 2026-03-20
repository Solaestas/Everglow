using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using MathNet.Numerics;

using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Core;

public static class MissionManager
{
	public const int UpdateInterval = 20;

	/// <summary>
	/// 任务池
	/// </summary>
	private static Dictionary<PoolType, List<MissionBase>> _missionPools;

	/// <summary>
	/// 历史杀怪计数
	/// </summary>
	private static Dictionary<int, int> _nPCKillCounter;

	/// <summary>
	/// 已接受任务的任务池
	/// </summary>
	private static List<MissionBase> AcceptedMissionPool => _missionPools[PoolType.Accepted];

	/// <summary>
	/// 历史杀怪计数
	/// </summary>
	public static IReadOnlyDictionary<int, int> NPCKillCounter => _nPCKillCounter;

	/// <summary>
	/// 任务列表是否需要更新
	/// </summary>
	public static bool NeedRefresh { get; set; } = false;

	#region TML Integration

	public static void Load()
	{
		if (!Main.dedServ)
		{
			_missionPools = Enum.GetValues<PoolType>()
				.ToDictionary(t => t, _ => new List<MissionBase>());
			_nPCKillCounter = [];

			Main.OnTickForInternalCodeOnly += Update;
			Ins.HookManager.AddHook(Commons.Enums.CodeLayer.PostSaveAndQuit, Clear);
			MissionGlobalNPC.OnKillNPCEvent += MissionGlobalNPC_SpecialOnKill_CountKill;
		}
	}

	public static void UnLoad()
	{
		if (!Main.dedServ)
		{
			_missionPools = null;
			_nPCKillCounter = null;
			MissionGlobalNPC.OnKillNPCEvent -= MissionGlobalNPC_SpecialOnKill_CountKill;
		}
	}

	/// <summary>
	/// Initialize mission manager with player mission data
	/// </summary>
	/// <param name="data"></param>
	public static void ApplyData(MissionManagerData data)
	{
		if (data == null)
		{
			return;
		}

		_nPCKillCounter = data.NPCKillCounter.ToDictionary();
		_missionPools = data.MissionPools.ToDictionary();

		AcceptedMissionPool.ForEach(x => x.Activate());
	}

	/// <summary>
	/// 清除所有任务池中的任务
	/// </summary>
	public static void Clear()
	{
		// Clear all events.
		foreach (var m in _missionPools.SelectMany(x => x.Value))
		{
			m.Deactivate();
		}

		_nPCKillCounter.Clear();
		foreach (var (_, missionPool) in _missionPools)
		{
			missionPool.Clear();
		}
	}

	#endregion

	#region In-Game Update

	/// <summary>
	/// 为任务每帧更新
	/// </summary>
	public static void Update()
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
		AcceptedMissionPool.ForEach(m => m.Update());

		// 处理自动提交任务
		var autoCommitMissions = AcceptedMissionPool.Where(m => m.CheckComplete() && m.AutoComplete).ToList();
		if (autoCommitMissions.Count > 0)
		{
			autoCommitMissions.ForEach(m => m.OnComplete());
			NeedRefresh = true;
		}

		// 处理过期任务
		var expiredMissions = AcceptedMissionPool.Where(m => m.CheckExpire()).ToList();
		if (expiredMissions.Count > 0)
		{
			expiredMissions.ForEach(m => m.OnExpire());
			NeedRefresh = true;
		}

		// 检测可提交状态改变的任务，将状态改变为可提交的任务抛出信息
		foreach (var m in AcceptedMissionPool.ToList())
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
	public static void MissionGlobalNPC_SpecialOnKill_CountKill(NPC npc)
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
	/// 获取某个类型的所有任务
	/// </summary>
	/// <typeparam name="T">任务的类型</typeparam>
	/// <param name="type">任务池类型</param>
	/// <returns>任务池内所有该类型的任务</returns>
	public static List<T> GetMissions<T>(PoolType type)
		where T : MissionBase => _missionPools[type].FindAll(m => m is T).ConvertAll(m => (T)m);

	/// <summary>
	/// 获取某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public static MissionBase GetMission(string missionName, PoolType type) =>
		_missionPools[type].Find(m => m.Name == missionName);

	/// <summary>
	/// 获取某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <returns></returns>
	public static MissionBase GetMission(string missionName)
	{
		foreach (var mp in _missionPools)
		{
			var m = mp.Value.Find(m => m.Name == missionName);
			if (m != null)
			{
				return m;
			}
		}
		return null;
	}

	/// <summary>
	/// Checks if a mission exists by type
	/// </summary>
	public static bool HasMission<T>(PoolType? type = null)
		where T : MissionBase =>
		HasMission(m => m is T, type);

	/// <summary>
	/// Checks if a mission exists by name
	/// </summary>
	public static bool HasMission(string missionName, PoolType? type = null) =>
		HasMission(m => m.Name == missionName, type);

	/// <summary>
	/// Internal implementation for mission checking
	/// </summary>
	private static bool HasMission(Func<MissionBase, bool> predicate, PoolType? type = null)
	{
		if (type.HasValue)
		{
			return _missionPools.TryGetValue(type.Value, out var missions)
				? missions.Any(predicate)
				: false;
		}
		else
		{
			foreach (var pool in _missionPools.Values)
			{
				if (pool.Any(predicate))
				{
					return true;
				}
			}

			return false;
		}
	}

	/// <summary>
	/// 向任务池中添加任务
	/// </summary>
	/// <param name="mission">任务</param>
	/// <param name="type">任务池类型</param>
	public static void AddMission(MissionBase mission, PoolType type, bool showText = true)
	{
		if (!HasMission(mission.Name))
		{
			_missionPools[type].Add(mission);
			mission.PoolType = type;

			if (showText)
			{
				Main.NewText($"新的任务任务已添加[{mission.DisplayName}]", 250, 250, 150);
			}

			if (type == PoolType.Accepted)
			{
				mission.Activate();
			}
		}
	}

	/// <summary>
	/// 移除任务池内指定条件的所有任务
	/// </summary>
	/// <typeparam name="T">任务类型</typeparam>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	private static bool RemoveMission(Func<MissionBase, bool> predicate, PoolType? type = null)
	{
		if (type.HasValue)
		{
			var missions = _missionPools[type.Value];
			foreach (var m in missions.Where(predicate))
			{
				m.Deactivate();
			}

			var removed = missions.RemoveAll(m => predicate(m));

			if (removed > 0)
			{
				NeedRefresh = true;
			}

			return removed > 0;
		}
		else
		{
			var removed = 0;
			foreach (var pool in _missionPools.Values)
			{
				foreach (var m in pool.Where(predicate))
				{
					m.Deactivate();
				}

				removed += pool.RemoveAll(m => predicate(m));
			}

			if (removed > 0)
			{
				NeedRefresh = true;
			}

			return removed > 0;
		}
	}

	/// <summary>
	/// 移除任务池内某个任务名的任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public static bool RemoveMission(string missionName, PoolType? type = null) =>
		RemoveMission(m => m.Name == missionName, type);

	/// <summary>
	/// 移除任务池内某个任务
	/// </summary>
	/// <param name="mission"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public static bool RemoveMission<T>(PoolType? type = null)
		where T : MissionBase =>
		RemoveMission(m => m is T, type);

	/// <summary>
	/// 将某个任务从目前任务池移到另一个
	/// </summary>
	/// <param name="missionName">任务名称</param>
	/// <param name="fromType">任务目前所处任务池</param>
	/// <param name="toType">目标任务池</param>
	/// <returns>是否成功</returns>
	public static bool MoveMission(string missionName, PoolType fromType, PoolType toType)
	{
		var mission = _missionPools[fromType].Find(m => m.Name == missionName);
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
	public static void MoveMission(MissionBase mission, PoolType fromType, PoolType toType)
	{
		_missionPools[fromType].Remove(mission);
		_missionPools[toType].Add(mission);
		mission.PoolType = toType;

		if (toType == PoolType.Accepted)
		{
			mission.Activate();
		}
		else
		{
			mission.Deactivate();
		}
	}

	/// <summary>
	/// 获取任务池内的所有任务
	/// <para/>注: 该方法返回的是任务池的副本，修改该副本不会造成任何影响
	/// </summary>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public static List<MissionBase> GetMissionPool(PoolType type) => _missionPools[type][..];

	#endregion

	#region Data Persistance

	/// <summary>
	/// Save missions to player file data.
	/// <br/>Should only be called by <see cref="ModPlayer.SaveData(TagCompound)"/>.
	/// </summary>
	/// <param name="tag">Provided by <see cref="ModPlayer.SaveData(TagCompound)"/>.</param>
	public static void SaveData(TagCompound tag)
	{
		tag.Add(nameof(_nPCKillCounter), _nPCKillCounter.ToList());

		foreach (var (poolType, missionPool) in _missionPools)
		{
			tag.Add(
				$"Everglow.MissionManage.{poolType}.Type",
				missionPool.ConvertAll(m => m.GetType().FullName));
			tag.Add(
				$"Everglow.MissionManage.{poolType}.Missions",
				missionPool.ConvertAll(m =>
				{
					TagCompound t = [];
					m.SaveData(t);
					return t;
				}));
		}
	}

	/// <summary>
	/// Load missions from player file data.
	/// <br/>Should only be called by <see cref="ModPlayer.LoadData(TagCompound)"/>.
	/// </summary>
	/// <param name="tag">Provided by <see cref="ModPlayer.LoadData(TagCompound)"/>.</param>
	public static MissionManagerData LoadData(TagCompound tag)
	{
		// Load npc kill counter.
		var nPCKillCounter = new Dictionary<int, int>();
		tag.TryGet<List<KeyValuePair<int, int>>>(nameof(NPCKillCounter), out var nPCKillCounterStorage);
		if (nPCKillCounterStorage != null && nPCKillCounterStorage.Count > 0)
		{
			nPCKillCounter = nPCKillCounterStorage.ToDictionary();
		}

		// Load missions.
		var missionPools = new Dictionary<PoolType, List<MissionBase>>();
		foreach (var missionPoolType in Enum.GetValues<PoolType>())
		{
			missionPools.Add(missionPoolType, []);
		}

		foreach (var (poolType, missionPool) in missionPools)
		{
			string poolTypeKey = $"Everglow.MissionManage.{poolType}.Type";
			string poolMissionKey = $"Everglow.MissionManage.{poolType}.Missions";
			if (tag.TryGet<IList<string>>(poolTypeKey, out var missionTypes)
				&& tag.TryGet<IList<TagCompound>>(poolMissionKey, out var missions))
			{
				for (int i = 0; i < missionTypes.Count; i++)
				{
					var type = Ins.ModuleManager.Types.FirstOrDefault(t => t.FullName == missionTypes[i]);
					if (type != null
						&& Activator.CreateInstance(type) is MissionBase m)
					{
						m.LoadData(missions[i]);
						missionPool.Add(m);
						m.PoolType = poolType;
					}
					else
					{
						Ins.Logger.Warn($"Invalid type {missionTypes[i]} detected from player file.");
					}
				}
			}
		}

		return new MissionManagerData(nPCKillCounter, missionPools);
	}

	#endregion
}