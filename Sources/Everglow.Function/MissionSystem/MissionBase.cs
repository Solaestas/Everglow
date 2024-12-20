using Everglow.Commons.UI.UIContainers.Mission.UIElements;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem;

/// <summary>
/// 任务基类
/// <br>!继承后必须保证存在一个无参构造函数</br>
/// </summary>
public abstract class MissionBase
{
	/// <summary>
	/// 用于进行内部标识的名字，作用类似 ID
	/// </summary>
	public abstract string Name { get; }

	/// <summary>
	/// 用于外部显示的名字
	/// </summary>
	public abstract string DisplayName { get; }

	/// <summary>
	/// 任务介绍
	/// </summary>
	public abstract string Description { get; }

	/// <summary>
	/// 任务图标
	/// <br>!为null时不显示</br>
	/// </summary>
	public abstract Texture2D Icon { get; }

	/// <summary>
	/// 绑定的UI显示
	/// <br>类型必须继承自<see cref="UIMissionItem"/></br>
	/// <br>类型必须存在一个仅有一个参数为目前任务类型或父类的构造函数</br>
	/// </summary>
	public virtual Type BindingUIItem => typeof(UIMissionItem);

	/// <summary>
	/// 任务进度，最大应为 1f
	/// </summary>
	public abstract float Progress { get; }

	/// <summary>
	/// 任务时限
	/// <br>单位为帧</br>
	/// <br>86400帧为泰拉内一天</br>
	/// <br>值为-1时即不限时</br>
	/// </summary>
	public virtual long TimeMax => -1;

	/// <summary>
	/// 任务计时器
	/// <br>单位为帧</br>
	/// </summary>
	public virtual long Time
	{
		get;
		set;
	}
	= 0;

	/// <summary>
	/// 任务所在的任务池类型
	/// </summary>
	public MissionManager.PoolType PoolType;

	/// <summary>
	/// 检查任务是否完成
	/// </summary>
	/// <returns></returns>
	public virtual bool CheckFinish() => Progress >= 1f;

	/// <summary>
	/// 更新任务进度
	/// </summary>
	/// <param name="objs"></param>
	public abstract void UpdateProgress(params object[] objs);

	/// <summary>
	/// 每帧更新
	/// </summary>
	/// <param name="gt"></param>
	public virtual void Update()
	{
		if (PoolType == MissionManager.PoolType.BeenTaken && TimeMax > 0)
		{
			if (Time < TimeMax)
			{
				Time++;
			}
			else
			{
				Time = 0;
				OnExpire();
			}
		}
	}

	/// <summary>
	/// 任务完成时
	/// </summary>
	public virtual void OnFinish()
	{
		MissionManager.Instance.MoveMission(this, MissionManager.PoolType.BeenTaken, MissionManager.PoolType.Finish);
	}

	/// <summary>
	/// 任务到期时
	/// </summary>
	public virtual void OnExpire()
	{
		MissionManager.Instance.MoveMission(this, MissionManager.PoolType.BeenTaken, MissionManager.PoolType.Overdue);
	}

	/// <summary>
	/// 任务失败时
	/// </summary>
	public virtual void OnFail()
	{
		MissionManager.Instance.MoveMission(this, MissionManager.PoolType.BeenTaken, MissionManager.PoolType.Fail);
	}

	/// <summary>
	/// 保存任务
	/// </summary>
	/// <param name="tag"></param>
	public virtual void Save(TagCompound tag)
	{
		tag.Add("MissionTime", Time);
	}

	/// <summary>
	/// 加载任务
	/// </summary>
	/// <param name="tag"></param>
	public virtual void Load(TagCompound tag)
	{
		if (tag.TryGet<long>("MissionTime", out var mt))
		{
			Time = mt;
		}
	}
}