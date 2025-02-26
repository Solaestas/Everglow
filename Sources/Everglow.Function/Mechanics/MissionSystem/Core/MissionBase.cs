using Everglow.Commons.Mechanics.MissionSystem.Abstracts;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Core;

/// <summary>
/// 任务基类
/// <br>!继承后必须保证存在一个无参构造函数</br>
/// </summary>
public abstract class MissionBase : ITagCompoundEntity
{
	protected MissionBase()
	{
		Icon = new MissionIconGroup();
		Objectives = new MissionObjectiveData();
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
	/// 任务来源NPC
	/// </summary>
	public virtual int SourceNPC { get; set; } = -1;

	/// <summary>
	/// 任务图标
	/// <br>!为null时不显示</br>
	/// </summary>
	public virtual MissionIconGroup Icon { get; }

	/// <summary>
	/// 绑定的UI显示
	/// <br>类型必须继承自<see cref="UIMissionItem"/></br>
	/// <br>类型必须存在一个仅有一个参数为目前任务类型或父类的构造函数</br>
	/// </summary>
	public virtual Type BindingUIItem => typeof(UIMissionItem);

	/// <summary>
	/// 任务目标数据
	/// </summary>
	public MissionObjectiveData Objectives { get; set; }

	/// <summary>
	/// 当前任务目标
	/// </summary>
	public MissionObjectiveBase CurrentObjective { get; set; }

	/// <summary>
	/// 任务进度，最大应为 1f
	/// </summary>
	public virtual float Progress => Objectives.AllObjectives.Count > 0 ? Objectives.AllObjectives.Average(o => o.Progress) : 1f;

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
	public virtual long Time { get; set; }

	/// <summary>
	/// 任务计时器存储键
	/// </summary>
	public const string TimeSaveKey = "MissionTime";

	private PoolType poolType;

	/// <summary>
	/// Mission status, managed by <see cref="MissionManager"/>.
	/// <para/>Should only be changed in <see cref="MissionManager"/> to keep the sync to its pool collection.
	/// </summary>
	public PoolType PoolType
	{
		get => poolType;
		set
		{
			if (value == PoolType.Accepted)
			{
				Activate();
			}
			else
			{
				Deactivate();
			}

			poolType = value;
		}
	}

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
	/// 任务奖励物品
	/// </summary>
	public virtual List<Item> RewardItems { get; }

	/// <summary>
	/// 任务奖励物品来源
	/// </summary>
	public static string RewardItemsSourceContext => "Everglow.MissionSystem";

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

		// Manage objectives. If the mission is finished but not completed, skip this step.
		if (CurrentObjective is not null)
		{
			CurrentObjective.Update();

			if (!CurrentObjective.Completed
			&& CurrentObjective.CheckCompletion())
			{
				CurrentObjective.Complete();
				CurrentObjective.Deactivate();

				CurrentObjective = CurrentObjective.Next;
				CurrentObjective?.Activate(this);

				Main.NewText($"[{Name}]任务当前目标已完成", 250, 250, 150);
			}
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
			Time += MissionManager.UpdateInterval;

			if (Time > TimeMax)
			{
				Time = TimeMax;
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

		MissionManager.MoveMission(this, PoolType.Accepted, PoolType.Completed);

		IsVisible = true;
		MissionManager.NeedRefresh = true;

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
		MissionManager.MoveMission(this, PoolType.Accepted, PoolType.Overdue);
	}

	/// <summary>
	/// 任务失败时
	/// </summary>
	public virtual void OnFail()
	{
		MissionManager.MoveMission(this, PoolType.Accepted, PoolType.Failed);
	}

	/// <summary>
	/// 任务接取时
	/// </summary>
	public virtual void Activate()
	{
		CurrentObjective = Objectives.First;

		CurrentObjective?.Activate(this);
	}

	/// <summary>
	/// 任务取消接取时
	/// </summary>
	public virtual void Deactivate()
	{
		CurrentObjective?.Deactivate();
	}

	public virtual void Reset()
	{
		foreach (var o in Objectives.AllObjectives)
		{
			o.ResetProgress();
		}
	}

	public virtual IEnumerable<string> GetObjectives()
	{
		var lines = new List<string>();

		foreach (var o in Objectives.AllObjectives)
		{
			var tempLines = new List<string>();
			o.GetObjectivesText(tempLines);
			int index = 1;
			for (int i = 0; i < tempLines.Count; i++)
			{
				if (o.Completed)
				{
					tempLines[i] = $"[TextDrawer,Text='(已完成)',Color='100,100,100,255']" + " " + tempLines[i];
				}

				tempLines[i] = $"{o.ObjectiveID + 1}.{index++} " + tempLines[i];
			}

			lines.AddRange(tempLines);
		}

		return lines;
	}

	public virtual string GetRewards() => string.Join(' ', RewardItems.ConvertAll(i => ItemDrawer.Create(i.type, i.stack, new Color(196, 241, 255))));

	public string GetTime() => EnableTime
		? $"[TimerIconDrawer,MissionName='{Name}'] 剩余时间:[TimerStringDrawer,MissionName='{Name}']\n"
		: string.Empty;

	/// <summary>
	/// 保存任务
	/// </summary>
	/// <param name="tag"></param>
	public virtual void SaveData(TagCompound tag)
	{
		tag.Add(TimeSaveKey, Time);
		tag.Add(nameof(SourceNPC), SourceNPC);
		tag.Add(nameof(IsVisible), IsVisible);

		SaveObjectives(tag, Objectives.AllObjectives);
	}

	public static void SaveObjectives(TagCompound tag, IEnumerable<MissionObjectiveBase> objectives)
	{
		var oTags = new List<TagCompound>();
		foreach (var o in objectives)
		{
			var ot = new TagCompound();
			o.SaveData(ot);
			oTags.Add(ot);
		}
		tag.Add(nameof(Objectives), oTags);
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

		LoadObjectives(tag, Objectives.AllObjectives);

		LoadVanillaItemTextures(RewardItems.Select(x => x.type));
	}

	public static void LoadObjectives(TagCompound tag, IEnumerable<MissionObjectiveBase> objectives)
	{
		if (tag.TryGet<IList<TagCompound>>(nameof(Objectives), out var oTags))
		{
			foreach (var o in objectives)
			{
				if (oTags.Count <= o.ObjectiveID)
				{
					break;
				}

				o.LoadData(oTags[o.ObjectiveID]);
			}
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