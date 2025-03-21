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
	/// 已接受任务的任务池
	/// </summary>
	private static List<MissionBase> AcceptedMissionPool => _missionPools[PoolType.Accepted];

	/// <summary>
	/// 历史杀怪计数
	/// </summary>
	public static Dictionary<int, int> NPCKillCounter { get; private set; }

	/// <summary>
	/// 任务列表是否需要更新
	/// </summary>
	public static bool NeedRefresh { get; set; } = false;

	public static void Load()
	{
		if (!Main.dedServ)
		{
			_missionPools = [];
			NPCKillCounter = [];

			foreach (var missionPoolType in Enum.GetValues<PoolType>())
			{
				_missionPools.Add(missionPoolType, []);
			}

			Main.OnTickForInternalCodeOnly += Update;
			Player.Hooks.OnEnterWorld += ClearAllEvents;
			MissionPlayer.OnKillNPCEvent += MissionPlayer_OnKillNPC_CountKill;
		}
	}

	public static void UnLoad()
	{
		if (!Main.dedServ)
		{
			_missionPools = null;
			NPCKillCounter = null;

			Main.OnTickForInternalCodeOnly -= Update;
			Player.Hooks.OnEnterWorld -= ClearAllEvents;
			MissionPlayer.OnKillNPCEvent -= MissionPlayer_OnKillNPC_CountKill;
		}
	}

	public static void ClearAllEvents(Player player)
	{
		foreach (var m in _missionPools.SelectMany(x => x.Value))
		{
			m.Deactivate();
		}
	}

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
	/// 检查任务池内是否包含某个任务类型
	/// </summary>
	/// <typeparam name="T">任务类型</typeparam>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public static bool HasMission<T>(PoolType type = PoolType.Available)
		where T : MissionBase => _missionPools[type].Find(m => m is T) != null;

	/// <summary>
	/// 检查任务池内是否包含某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public static bool HasMission(string missionName, PoolType type) =>
		_missionPools[type].Find(m => m.Name == missionName) != null;

	/// <summary>
	/// 向任务池中添加任务
	/// </summary>
	/// <param name="mission">任务</param>
	/// <param name="type">任务池类型</param>
	public static void AddMission(MissionBase mission, PoolType type)
	{
		_missionPools[type].Add(mission);
		mission.PoolType = type;

		if (type == PoolType.Accepted)
		{
			mission.Activate();
		}
	}

	/// <summary>
	/// 移除任务池内某个任务类型
	/// </summary>
	/// <typeparam name="T">任务类型</typeparam>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public static int RemoveMissions<T>(PoolType type, Func<MissionBase, bool> predicate = default)
		where T : MissionBase
	{
		foreach (var m in _missionPools[type].Where(predicate))
		{
			m.Deactivate();
		}

		return _missionPools[type].RemoveAll(m => m is T && predicate(m));
	}

	/// <summary>
	/// 移除任务池内某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public static bool RemoveMission(string missionName, PoolType type)
	{
		return RemoveMission(_missionPools[type].Find(m => m.Name == missionName), type);
	}

	public static bool RemoveMission(MissionBase mission, PoolType type)
	{
		mission.Deactivate();
		return _missionPools[type].Remove(mission);
	}

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

	/// <summary>
	/// 为任务每帧更新
	/// </summary>
	public static void Update()
	{
		if (Main.gameMenu || Main.gamePaused || Main.gameInactive)
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
	/// 清除所有任务池中的任务
	/// </summary>
	public static void Clear()
	{
		ClearAllEvents(null);
		NPCKillCounter.Clear();
		foreach (var (_, missionPool) in _missionPools)
		{
			missionPool.Clear();
		}
	}

	/// <summary>
	/// 记录杀怪
	/// </summary>
	/// <param name="npc">被击杀的NPC</param>
	/// <exception cref="InvalidParameterException">参数为空或npc类型错误</exception>
	public static void MissionPlayer_OnKillNPC_CountKill(NPC npc)
	{
		if (npc is null || npc.type < NPCID.None)
		{
			throw new InvalidParameterException();
		}

		// Update NPC kill history
		if (!NPCKillCounter.TryAdd(npc.type, 1))
		{
			NPCKillCounter[npc.type]++;
		}
	}

	/// <summary>
	/// 保存任务
	/// </summary>
	/// <param name="tag"></param>
	public static void SaveData(TagCompound tag)
	{
		tag.Add(nameof(NPCKillCounter), NPCKillCounter.ToList());

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
	/// 加载任务
	/// </summary>
	/// <param name="tag"></param>
	public static MissionManagerInfo LoadData(TagCompound tag)
	{
		NPCKillCounter.Clear();
		tag.TryGet<List<KeyValuePair<int, int>>>(nameof(NPCKillCounter), out var nPCKillCounter);
		if (nPCKillCounter != null && nPCKillCounter.Count > 0)
		{
			NPCKillCounter = nPCKillCounter.ToDictionary();
		}

		foreach (var (poolType, missionPool) in _missionPools)
		{
			missionPool.Clear();

			string poolTypeKey = $"Everglow.MissionManage.{poolType}.Type";
			string poolMissionKey = $"Everglow.MissionManage.{poolType}.Missions";
			if (tag.TryGet<IList<string>>(poolTypeKey, out var mt) &&
				tag.TryGet<IList<TagCompound>>(poolMissionKey, out var m))
			{
				for (int j = 0; j < mt.Count; j++)
				{
					var type = Ins.ModuleManager.Types.FirstOrDefault(t => t.FullName == mt[j]);
					var mission = Activator.CreateInstance(type) as MissionBase;
					mission.LoadData(m[j]);
					missionPool.Add(mission);
					mission.PoolType = poolType;
				}
			}
		}

		return MissionManagerInfo.Create();
	}

	/// <summary>
	/// Initialize mission manager with player mission pool data
	/// </summary>
	/// <param name="poolData"></param>
	public static void OnEnterWorld(MissionManagerInfo info)
	{
		ClearAllEvents(null);
		Clear();

		if (info != null)
		{
			NPCKillCounter = info.NPCKillCounter;
			_missionPools = info.MissionPools;
			AcceptedMissionPool.ForEach(x => x.Activate());
		}
	}

	public class MissionManagerInfo
	{
		public Dictionary<int, int> NPCKillCounter { get; private set; }

		public Dictionary<PoolType, List<MissionBase>> MissionPools { get; private set; }

		public static MissionManagerInfo Create()
		{
			var info = new MissionManagerInfo
			{
				NPCKillCounter = MissionManager.NPCKillCounter.ToDictionary(),
				MissionPools = [],
			};

			foreach (var (type, pool) in _missionPools)
			{
				info.MissionPools.Add(type, pool.ToList());
			}

			return info;
		}
	}
}