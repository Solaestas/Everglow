using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem;

public class MissionManager
{
	/// <summary>
	/// 任务池类型
	/// <list type="table">
	///     <item>Acceptd: 已接取</item>
	///     <item>Available: 可接取</item>
	///     <item>Completed: 已完成</item>
	///     <item>Overdue: 已过期</item>
	///     <item>Failed: 已失败</item>
	/// </list>
	/// </summary>
	public enum PoolType
	{
		/// <summary>
		/// 已经被接取的任务池
		/// </summary>
		Accepted,

		/// <summary>
		/// 可以被接取的任务池
		/// </summary>
		Available,

		/// <summary>
		/// 任务完成且已领取奖励的任务池
		/// </summary>
		Completed,

		/// <summary>
		/// 逾期未完成的任务池
		/// </summary>
		Overdue,

		/// <summary>
		/// 任务失败的任务池
		/// </summary>
		Failed,
	}

	/// <summary>
	/// 任务池
	/// </summary>
	private Dictionary<PoolType, List<MissionBase>> _missionPools = [];

	/// <summary>
	/// 任务管理器实例
	/// </summary>
	public static MissionManager Instance => Main.LocalPlayer.GetModPlayer<MissionPlayer>().MissionManager;

	public MissionManager()
	{
		foreach (var missionPoolType in Enum.GetValues<PoolType>())
		{
			_missionPools.Add(missionPoolType, []);
		}
	}

	/// <summary>
	/// 获取某个类型的所有任务
	/// </summary>
	/// <typeparam name="T">任务的类型</typeparam>
	/// <param name="type">任务池类型</param>
	/// <returns>任务池内所有该类型的任务</returns>
	public List<T> GetMissions<T>(PoolType type)
		where T : MissionBase => _missionPools[type].FindAll(m => m is T).ConvertAll(m => (T)m);

	/// <summary>
	/// 获取某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public MissionBase GetMission(string missionName, PoolType type) =>
		_missionPools[type].Find(m => m.Name == missionName);

	/// <summary>
	/// 获取某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <returns></returns>
	public MissionBase GetMission(string missionName)
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
	public bool HasMission<T>(PoolType type = PoolType.Available)
		where T : MissionBase => _missionPools[type].Find(m => m is T) != null;

	/// <summary>
	/// 检查任务池内是否包含某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public bool HasMission(string missionName, PoolType type) =>
		_missionPools[type].Find(m => m.Name == missionName) != null;

	/// <summary>
	/// 向任务池中添加任务
	/// </summary>
	/// <param name="mission">任务</param>
	/// <param name="type">任务池类型</param>
	public void AddMission(MissionBase mission, PoolType type)
	{
		_missionPools[type].Add(mission);
		mission.PoolType = type;
	}

	/// <summary>
	/// 移除任务池内某个任务类型
	/// </summary>
	/// <typeparam name="T">任务类型</typeparam>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public int RemoveMissions<T>(PoolType type)
		where T : MissionBase =>
		_missionPools[type].RemoveAll(m => m is T);

	/// <summary>
	/// 移除任务池内某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public bool RemoveMission(string missionName, PoolType type) =>
		_missionPools[type].Remove(_missionPools[type].Find(m => m.Name == missionName));

	/// <summary>
	/// 将某个任务从目前任务池移到另一个
	/// </summary>
	/// <param name="missionName">任务名称</param>
	/// <param name="fromType">任务目前所处任务池</param>
	/// <param name="toType">目标任务池</param>
	/// <returns>是否成功</returns>
	public bool MoveMission(string missionName, PoolType fromType, PoolType toType)
	{
		var mission = _missionPools[fromType].Find(m => m.Name == missionName);
		if (mission == null)
		{
			return false;
		}

		_missionPools[fromType].Remove(mission);
		_missionPools[toType].Add(mission);
		mission.PoolType = toType;
		return true;
	}

	/// <summary>
	/// 将某个任务从目前任务池移到另一个
	/// </summary>
	/// <param name="mission">任务实例</param>
	/// <param name="fromType">任务目前所处任务池</param>
	/// <param name="toType">目标任务池</param>
	public void MoveMission(MissionBase mission, PoolType fromType, PoolType toType)
	{
		_missionPools[fromType].Remove(mission);
		_missionPools[toType].Add(mission);
		mission.PoolType = toType;
	}

	/// <summary>
	/// 获取任务池内的所有任务
	/// <para/>注: 该方法返回的是任务池的副本，修改该副本不会造成任何影响
	/// </summary>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public List<MissionBase> GetMissionPool(PoolType type) => _missionPools[type][..];

	/// <summary>
	/// 为任务每帧更新
	/// </summary>
	public void Update()
	{
		foreach (var (_, missionPool) in _missionPools)
		{
			missionPool.ForEach(m => m.Update());
		}

		// 更新结束后检测所有进度100%的Accpted任务，触发其OnFinish()方法
		_missionPools[PoolType.Accepted].Where(x => x.CheckFinish()).ToList().ForEach(x => x.OnFinish());
	}

	/// <summary>
	/// 清除所有任务池中的任务
	/// </summary>
	public void Clear()
	{
		foreach (var (_, missionPool) in _missionPools)
		{
			missionPool.Clear();
		}
	}

	/// <summary>
	/// 保存任务
	/// </summary>
	/// <param name="tag"></param>
	public void Save(TagCompound tag)
	{
		foreach (var (poolType, missionPool) in _missionPools)
		{
			tag.Add(
				$"Everglow.MissionManage.{poolType}.Type",
				missionPool.ConvertAll(m => m.GetType().FullName));
			tag.Add(
				$"Everglow.MissionManage.{poolType}.Missions",
				missionPool.ConvertAll(m =>
				{
					TagCompound mt = new TagCompound();
					m.Save(mt);
					return mt;
				}));
		}
	}

	/// <summary>
	/// 加载任务
	/// </summary>
	/// <param name="tag"></param>
	public void Load(TagCompound tag)
	{
		foreach (var (poolType, missionPool) in _missionPools)
		{
			missionPool.Clear();
			if (tag.TryGet<IList<string>>($"Everglow.MissionManage.{poolType}.Type", out var mt) &&
				tag.TryGet<IList<TagCompound>>($"Everglow.MissionManage.{poolType}.Missions", out var m))
			{
				for (int j = 0; j < mt.Count; j++)
				{
					var mission = (MissionBase)Activator.CreateInstance(Type.GetType(mt[j]));
					mission.Load(m[j]);
					missionPool.Add(mission);
					mission.PoolType = poolType;
				}
			}
		}
	}
}