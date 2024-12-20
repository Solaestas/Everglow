using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem;

public class MissionManager
{
	public enum PoolType
	{
		/// <summary>
		/// 已经被接取的任务池
		/// </summary>
		BeenTaken,

		/// <summary>
		/// 可以被接取的任务池
		/// </summary>
		CanTaken,

		/// <summary>
		/// 任务完成且已领取奖励的任务池
		/// </summary>
		Finish,

		/// <summary>
		/// 逾期未完成的任务池
		/// </summary>
		Overdue,

		/// <summary>
		/// 任务失败的任务池
		/// </summary>
		Fail,
	}

	/// <summary>
	/// 任务池
	/// </summary>
	private List<MissionBase>[] _missionPools;

	/// <summary>
	/// 任务管理器实例
	/// </summary>
	public static MissionManager Instance => Main.LocalPlayer.GetModPlayer<MissionPlayer>().MissionManager;

	public MissionManager()
	{
		_missionPools = new List<MissionBase>[Enum.GetValues<PoolType>().Length];
		for (int i = 0; i < _missionPools.Length; _missionPools[i++] = [])
			;
	}

	/// <summary>
	/// 获取某个类型的所有任务
	/// </summary>
	/// <typeparam name="T">任务的类型</typeparam>
	/// <param name="type">任务池类型</param>
	/// <returns>任务池内所有该类型的任务</returns>
	public List<T> GetMissions<T>(PoolType type)
		where T : MissionBase => _missionPools[(int)type].FindAll(m => m is T).ConvertAll(m => (T)m);

	/// <summary>
	/// 获取某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public MissionBase GetMission(string missionName, PoolType type) =>
		_missionPools[(int)type].Find(m => m.Name == missionName);

	/// <summary>
	/// 获取某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <returns></returns>
	public MissionBase GetMission(string missionName)
	{
		foreach (var mp in _missionPools)
		{
			var m = mp.Find(m => m.Name == missionName);
			if (m != null)
				return m;
		}
		return null;
	}

	/// <summary>
	/// 检查任务池内是否包含某个任务类型
	/// </summary>
	/// <typeparam name="T">任务类型</typeparam>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public bool HasMission<T>(PoolType type = PoolType.CanTaken)
		where T : MissionBase => _missionPools[(int)type].Find(m => m is T) != null;

	/// <summary>
	/// 检查任务池内是否包含某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public bool HasMission(string missionName, PoolType type) =>
		_missionPools[(int)type].Find(m => m.Name == missionName) != null;

	/// <summary>
	/// 向任务池中添加任务
	/// </summary>
	/// <param name="mission">任务</param>
	/// <param name="type">任务池类型</param>
	public void AddMission(MissionBase mission, PoolType type)
	{
		_missionPools[(int)type].Add(mission);
		mission.PoolType = type;
	}

	/// <summary>
	/// 移除任务池内某个任务类型
	/// </summary>
	/// <typeparam name="T">任务类型</typeparam>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public int RemoveMissions<T>(PoolType type)
		where T : MissionBase => _missionPools[(int)type].RemoveAll(m => m is T);

	/// <summary>
	/// 移除任务池内某个任务
	/// </summary>
	/// <param name="missionName">任务名字，或者说 ID</param>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public bool RemoveMission(string missionName, PoolType type) =>
		_missionPools[(int)type].Remove(_missionPools[(int)type].Find(m => m.Name == missionName));

	/// <summary>
	/// 将某个任务从目前任务池移到另一个
	/// </summary>
	/// <param name="missionName">任务名称</param>
	/// <param name="fromType">任务目前所处任务池</param>
	/// <param name="toType">目标任务池</param>
	/// <returns>是否成功</returns>
	public bool MoveMission(string missionName, PoolType fromType, PoolType toType)
	{
		var mission = _missionPools[(int)fromType].Find(m => m.Name == missionName);
		if (mission == null)
			return false;
		_missionPools[(int)fromType].Remove(mission);
		_missionPools[(int)toType].Add(mission);
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
		_missionPools[(int)fromType].Remove(mission);
		_missionPools[(int)toType].Add(mission);
		mission.PoolType = toType;
	}

	/// <summary>
	/// 获取任务池内的所有任务
	/// </summary>
	/// <param name="type">任务池类型</param>
	/// <returns></returns>
	public List<MissionBase> GetMissionPool(PoolType type) => _missionPools[(int)type][..];

	/// <summary>
	/// 为任务每帧更新
	/// </summary>
	public void Update()
	{
		Array.ForEach(_missionPools, m => m.ForEach(m => m.Update()));
	}

	/// <summary>
	/// 保存任务
	/// </summary>
	/// <param name="tag"></param>
	public void Save(TagCompound tag)
	{
		for (int i = 0; i < _missionPools.Length; i++)
		{
			var missionPool = _missionPools[i];
			tag.Add(
				$"Everglow.MissionManage.{(PoolType)i}.Type",
				missionPool.ConvertAll(m => m.GetType().FullName));
			tag.Add(
				$"Everglow.MissionManage.{(PoolType)i}.Missions",
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
		for (int i = 0; i < _missionPools.Length; i++)
		{
			var missionPool = _missionPools[i];
			missionPool.Clear();
			if (tag.TryGet<IList<string>>($"Everglow.MissionManage.{(PoolType)i}.Type", out var mt) &&
			tag.TryGet<IList<TagCompound>>($"Everglow.MissionManage.{(PoolType)i}.Missions", out var m))
			{
				for (int j = 0; j < mt.Count; j++)
				{
					var mission = (MissionBase)Activator.CreateInstance(Type.GetType(mt[j]));
					mission.Load(m[j]);
					missionPool.Add(mission);
					mission.PoolType = (PoolType)i;
				}
			}
		}
	}
}