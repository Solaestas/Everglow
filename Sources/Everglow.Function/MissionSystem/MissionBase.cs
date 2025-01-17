using Everglow.Commons.UI.UIContainers.Mission.UIElements;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem;

/// <summary>
/// 任务基类
/// <br>!继承后必须保证存在一个无参构造函数</br>
/// </summary>
public abstract class MissionBase : ITagCompoundEntity
{
	/// <summary>
	/// 用于进行内部标识的名字，作用类似 ID
	/// </summary>
	public virtual string Name => GetType().Name;

	/// <summary>
	/// 用于外部显示的名字
	/// </summary>
	public abstract string DisplayName { get; }

	/// <summary>
	/// 任务介绍
	/// </summary>
	public abstract string Description { get; }

	/// <summary>
	/// 任务来源NPC
	/// </summary>
	public virtual int SourceNPC { get; set; } = -1;

	/// <summary>
	/// 任务图标
	/// <br>!为null时不显示</br>
	/// </summary>
	public abstract MissionIconGroup Icon { get; }

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
	/// 是否启用计时器
	/// </summary>
	public bool EnableTime => TimeMax > 0;

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

	public const string TimeSaveKey = "MissionTime";

	/// <summary>
	/// 任务所在的任务池类型
	/// </summary>
	public MissionManager.PoolType PoolType { get; set; }

	/// <summary>
	/// 任务类型
	/// </summary>
	public virtual MissionType MissionType => MissionType.None;

	/// <summary>
	/// 是否显示在任务列表中
	/// </summary>
	public virtual bool IsVisible { get; set; } = true;

	/// <summary>
	/// 是否由任务管理器自动检测完成并提交
	/// </summary>
	public virtual bool AutoComplete => false;

	/// <summary>
	/// 检查任务是否完成
	/// </summary>
	/// <returns></returns>
	public virtual bool CheckComplete() => Progress >= 1f;

	/// <summary>
	/// 检查任务是否过期
	/// </summary>
	/// <returns></returns>
	public virtual bool CheckExpire() => TimeMax > 0 ? Time >= TimeMax : false;

	/// <summary>
	/// 任务可提交状态的旧状态
	/// <para/>该属性不需要持久化，保证每次重新进入世界时都会发送信息
	/// </summary>
	public bool OldCheckComplete { get; internal set; } = false;

	/// <summary>
	/// 任务可提交状态改变后HOOK
	/// </summary>
	public virtual void OnCheckCompleteChange()
	{
		MissionManager.NeedRefresh = true;
	}

	/// <summary>
	/// 每帧更新
	/// </summary>
	/// <param name="gt"></param>
	public virtual void Update()
	{
		UpdateTime();
		UpdateProgress();
	}

	/// <summary>
	/// 检查任务是否过期
	/// <para/>重写该方法以扩展过期条件
	/// </summary>
	protected void UpdateTime()
	{
		if (EnableTime)
		{
			Time += MissionManager.UpdateInterval;

			if (Time > TimeMax)
			{
				Time = TimeMax;
			}
		}
	}

	/// <summary>
	/// 更新任务进度
	/// </summary>
	/// <param name="objs"></param>
	public virtual void UpdateProgress(params object[] objs)
	{
	}

	/// <summary>
	/// 任务完成时
	/// <para/>对于完成HOOK，请重写<see cref="PostComplete"/>方法
	/// </summary>
	public void OnComplete()
	{
		if (!PreComplete())
		{
			return;
		}

		MissionManager.MoveMission(this, MissionManager.PoolType.Accepted, MissionManager.PoolType.Completed);

		IsVisible = true;
		MissionManager.NeedRefresh = true;

		Main.NewText($"[{Name}]任务已完成", 150, 250, 150);

		PostComplete();
	}

	/// <summary>
	/// 任务完成 <see cref="OnComplete"/> 前HOOK.
	/// </summary>
	/// <returns></returns>
	public virtual bool PreComplete()
	{
		return true;
	}

	/// <summary>
	/// 任务完成 <see cref="OnComplete"/> 后HOOK.
	/// </summary>
	public virtual void PostComplete()
	{
	}

	/// <summary>
	/// 任务到期时
	/// </summary>
	public virtual void OnExpire()
	{
		MissionManager.MoveMission(this, MissionManager.PoolType.Accepted, MissionManager.PoolType.Overdue);
	}

	/// <summary>
	/// 任务失败时
	/// </summary>
	public virtual void OnFail()
	{
		MissionManager.MoveMission(this, MissionManager.PoolType.Accepted, MissionManager.PoolType.Failed);
	}

	/// <summary>
	/// 保存任务
	/// </summary>
	/// <param name="tag"></param>
	public virtual void SaveData(TagCompound tag)
	{
		tag.Add(TimeSaveKey, Time);
		tag.Add(nameof(SourceNPC), SourceNPC);
		tag.Add(nameof(IsVisible), IsVisible);
	}

	/// <summary>
	/// 加载任务
	/// </summary>
	/// <param name="tag"></param>
	public virtual void LoadData(TagCompound tag)
	{
		if (tag.TryGet<long>(TimeSaveKey, out var mt))
		{
			Time = mt;
		}

		if (tag.TryGet<int>(nameof(SourceNPC), out var sourceNPC))
		{
			SourceNPC = sourceNPC;
		}

		if (tag.TryGet<bool>(nameof(IsVisible), out var isVisible))
		{
			IsVisible = isVisible;
		}
	}

	/// <summary>
	/// Load not-loaded textures for vanilla items
	/// </summary>
	/// <param name="types"></param>
	public static void LoadVanillaItemTextures(IEnumerable<int> types)
	{
		foreach (var type in types.Distinct().Where(t => t <= ItemID.Count))
		{
			// The Main.LoadItem function will skip the loaded items
			Main.instance.LoadItem(type);
		}
	}

	/// <summary>
	/// Load not-loaded textures for vanilla NPCs
	/// </summary>
	/// <param name="types"></param>
	public static void LoadVanillaNPCTextures(IEnumerable<int> types)
	{
		foreach (var type in types.Distinct().Where(t => t <= NPCID.Count))
		{
			// The Main.LoadItem function will skip the loaded items
			Main.instance.LoadNPC(type);
		}
	}
}