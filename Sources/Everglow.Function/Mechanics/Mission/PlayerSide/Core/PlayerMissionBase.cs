using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstracts;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Enums;
using Everglow.Commons.Mechanics.Mission.PlayerSide.MissionStructure;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Primitives;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Shared.Icons;
using Everglow.Commons.Mechanics.Mission.UI.UIElements;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Core;

/// <summary>
/// 任务基类
/// </summary>
/// <remarks>
/// NOTE: 继承后必须保证存在一个无参构造函数
/// </remarks>
public abstract class PlayerMissionBase : ITagCompoundEntity
{
	/// <summary>
	/// 任务计时器存储键
	/// </summary>
	public const string TimeSaveKey = "MissionTime";

	/// <summary>
	/// 任务奖励物品来源
	/// </summary>
	public const string RewardItemsSourceContext = "Everglow.MissionSystem";

	protected PlayerMissionBase()
	{
		Objectives = new PlayerStructuralObjectiveContainer();
		RewardItems = [];
		Time = 0;
	}

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
	public virtual string Description { get; } = string.Empty;

	/// <summary>
	/// 任务来源
	/// </summary>
	public virtual MissionSourceBase Source { get; private set; } = MissionSourceBase.Default;

	/// <summary>
	/// 次级任务来源
	/// </summary>
	public virtual MissionSourceBase SubSource { get; private set; } = null;

	/// <summary>
	/// 任务图标
	/// <br>!为null时不显示</br>
	/// </summary>
	public virtual MissionIconGroup Icon => GetIcons(new());

	/// <summary>
	/// 绑定的UI显示
	/// <br>类型必须继承自<see cref="UIMissionItem"/></br>
	/// <br>类型必须存在一个仅有一个参数为目前任务类型或父类的构造函数</br>
	/// </summary>
	public virtual Type BindingUIItem => typeof(UIMissionItem);

	/// <summary>
	/// 任务目标数据
	/// </summary>
	public PlayerStructuralObjectiveContainer Objectives { get; set; }

	/// <summary>
	/// 当前任务目标
	/// </summary>
	public MissionObjectiveBase CurrentObjective => Objectives.CurrentObjective;

	/// <summary>
	/// 任务进度，最大应为 1f
	/// </summary>
	public virtual float Progress
	{
		get => Objectives.Progress;
	}

	/// <summary>
	/// 任务时限
	/// <br>单位为帧</br>
	/// <br>86400帧为泰拉内一天</br>
	/// <br>值为-1时即不限时</br>
	/// </summary>
	public virtual long TimeLimit => -1;

	/// <summary>
	/// 是否启用计时器
	/// </summary>
	public bool EnableTime => TimeLimit > 0;

	/// <summary>
	/// 任务计时器
	/// <br>单位为帧</br>
	/// </summary>
	public virtual long Time { get; set; }

	/// <summary>
	/// Mission state, managed by <see cref="PlayerMissionManager"/>.
	/// </summary>
	/// <remarks>
	/// Should only be changed in <see cref="PlayerMissionManager"/> to keep the mission syncing to its pool collection.
	/// </remarks>
	public PlayerMissionState State { get; set; }

	/// <summary>
	/// 任务类型
	/// </summary>
	public virtual MissionType Type => MissionType.None;

	/// <summary>
	/// 任务奖励物品
	/// </summary>
	public virtual List<Item> RewardItems { get; init; }

	/// <summary>
	/// 是否显示在任务列表中
	/// </summary>
	public virtual bool IsVisible { get; set; } = true;

	/// <summary>
	/// 是否由任务管理器自动检测完成并提交
	/// </summary>
	public virtual bool AutoComplete => false;

	/// <summary>
	/// 是否可放弃任务
	/// </summary>
	public virtual bool Cancellable => false;

	/// <summary>
	/// 任务可提交状态的旧状态
	/// <para/>该属性不需要持久化，保证每次重新进入世界时都会发送信息
	/// </summary>
	public bool OldCheckComplete { get; internal set; } = false;

	/// <summary>
	/// 检查任务是否完成
	/// </summary>
	/// <returns></returns>
	public virtual bool CheckComplete() => Objectives.Completed;

	/// <summary>
	/// 检查任务是否过期
	/// </summary>
	/// <returns></returns>
	public virtual bool CheckExpire() => TimeLimit > 0 ? Time >= TimeLimit : false;

	/// <summary>
	/// 任务可提交状态改变后HOOK
	/// </summary>
	public virtual void OnCheckCompleteChange()
	{
		PlayerMissionManager.NeedRefresh = true;
	}

	/// <summary>
	/// 每帧更新
	/// </summary>
	/// <param name="gt"></param>
	public virtual void Update()
	{
		UpdateTime();

		if (Objectives.Update(this))
		{
			Main.NewText($"[{Name}]任务当前目标已完成", 250, 250, 150);
			PlayerMissionManager.NeedRefresh = true;
		}
	}

	/// <summary>
	/// 检查任务是否过期
	/// <para/>重写该方法以扩展过期条件
	/// </summary>
	protected void UpdateTime()
	{
		if (EnableTime)
		{
			Time += PlayerMissionManager.UpdateInterval;

			if (Time > TimeLimit)
			{
				Time = TimeLimit;
			}
		}
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

		PlayerMissionManager.MoveMission(this, PlayerMissionState.Accepted, PlayerMissionState.Completed);

		IsVisible = true;
		PlayerMissionManager.NeedRefresh = true;

		Main.NewText($"[{Name}]任务已完成", 150, 250, 150);

		PostComplete();
	}

	/// <summary>
	/// 任务完成 <see cref="OnComplete"/> 前HOOK.
	/// </summary>
	/// <returns></returns>
	public virtual bool PreComplete() => true;

	/// <summary>
	/// 任务完成 <see cref="OnComplete"/> 后HOOK.
	/// </summary>
	public virtual void PostComplete()
	{
		foreach (var item in RewardItems)
		{
			Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(RewardItemsSourceContext), item, item.stack);
		}
	}

	/// <summary>
	/// 任务到期时
	/// </summary>
	public virtual void OnExpire()
	{
		PlayerMissionManager.MoveMission(this, PlayerMissionState.Accepted, PlayerMissionState.Overdue);
	}

	/// <summary>
	/// 任务失败时
	/// </summary>
	public virtual void OnFail()
	{
		PlayerMissionManager.MoveMission(this, PlayerMissionState.Accepted, PlayerMissionState.Failed);
	}

	/// <summary>
	/// 任务接取时
	/// </summary>
	public virtual void Activate()
	{
		Objectives.Activate(this);
	}

	/// <summary>
	/// 任务取消接取时
	/// </summary>
	public virtual void Deactivate()
	{
		Objectives.Deactivate();
	}

	/// <summary>
	/// 重置任务进度
	/// </summary>
	public virtual void Reset()
	{
		Objectives.ResetProgress();
	}

	public virtual MissionIconGroup GetIcons(MissionIconGroup iconGroup)
	{
		iconGroup.Add(MissionSourceIcon.Create(Source, SubSource));
		Objectives.GetObjectivesIcon(iconGroup);

		return iconGroup;
	}

	/// <summary>
	/// 获取任务目标文本
	/// </summary>
	/// <returns></returns>
	public virtual IEnumerable<string> GetObjectives()
	{
		var mainIndex = 1;
		var lines = new List<string>();
		foreach (var (completed, objectiveLines) in Objectives.GetObjectivesText())
		{
			int subIndex = 1;
			for (int i = 0; i < objectiveLines.Count; i++)
			{
				if (completed)
				{
					objectiveLines[i] = $"[TextDrawer,Text='(已完成)',Color='100,100,100,255']" + " " + objectiveLines[i];
				}

				objectiveLines[i] = $"{mainIndex}.{subIndex++} " + objectiveLines[i];
			}

			lines.AddRange(objectiveLines);
			mainIndex++;
		}

		return lines;
	}

	/// <summary>
	/// 获取奖励文本
	/// </summary>
	/// <returns></returns>
	public virtual string GetRewards() => string.Join(' ', RewardItems.ConvertAll(i => ItemDrawer.Create(i.type, i.stack, new Color(196, 241, 255))));

	/// <summary>
	/// 获取时间文本
	/// </summary>
	/// <returns></returns>
	public string GetTime() => EnableTime
		? $"[TimerIconDrawer,MissionName='{Name}'] 剩余时间:[TimerStringDrawer,MissionName='{Name}']\n"
		: string.Empty;

	/// <summary>
	/// 保存任务
	/// </summary>
	/// <param name="tag"></param>
	public virtual void SaveData(TagCompound tag)
	{
		tag.Add(nameof(State), (int)State);
		tag.Add(TimeSaveKey, Time);
		tag.Add(nameof(IsVisible), IsVisible);

		Objectives.SaveData(tag);
	}

	/// <summary>
	/// 保存任务目标
	/// </summary>
	/// <param name="tag"></param>
	/// <param name="objectives"></param>
	public static void SaveObjectives(TagCompound tag, IEnumerable<MissionObjectiveBase> objectives, string key = nameof(Objectives))
	{
		var oTags = new List<TagCompound>();
		foreach (var o in objectives)
		{
			var ot = new TagCompound();
			o.SaveData(ot);
			oTags.Add(ot);
		}
		tag.Add(key, oTags);
	}

	/// <summary>
	/// 加载任务
	/// </summary>
	/// <param name="tag"></param>
	public virtual void LoadData(TagCompound tag)
	{
		// Legacy flat `_missions` saves (after pool→list, before State persistence) omit this key.
		// Enum default is Accepted (= 0); leaving it would wrongly Activate() via ApplyData.
		// Missing State → Available (not activated on load). Pre-flat partitioned keys
		// (`Everglow.MissionManage.{pool}.*`) are a manager-level format and not handled here.
		if (tag.TryGet<int>(nameof(State), out var state) && Enum.IsDefined(typeof(PlayerMissionState), state))
		{
			State = (PlayerMissionState)state;
		}
		else
		{
			State = PlayerMissionState.Available;
		}

		if (tag.TryGet<long>(TimeSaveKey, out var mt))
		{
			Time = mt;
		}

		if (tag.TryGet<bool>(nameof(IsVisible), out var isVisible))
		{
			IsVisible = isVisible;
		}

		Objectives.LoadData(tag);
		if (Objectives.RecoveredInvalidState)
		{
			Reset();
			Time = 0;
			OldCheckComplete = false;
			if (State == PlayerMissionState.Completed)
			{
				State = PlayerMissionState.Accepted;
			}
		}

		AssetUtils.LoadVanillaItemTextures(RewardItems.Select(x => x.type));
	}

	/// <summary>
	/// 加载任务目标
	/// </summary>
	/// <param name="tag"></param>
	/// <param name="objectives"></param>
	public static void LoadObjectives(TagCompound tag, IEnumerable<MissionObjectiveBase> objectives, string key = nameof(Objectives), bool useObjectiveID = true)
	{
		if (tag.TryGet<IList<TagCompound>>(key, out var oTags))
		{
			int index = 0;
			foreach (var o in objectives)
			{
				int tagIndex = useObjectiveID ? o.ObjectiveID : index++;
				if (oTags.Count <= tagIndex)
				{
					break;
				}

				o.LoadData(oTags[tagIndex]);
			}
		}
	}
}
