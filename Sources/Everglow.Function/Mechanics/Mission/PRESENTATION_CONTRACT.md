# Mission Presentation 同步契约

## 目标

PlayerSide 与 WorldSide 只统一面向 Presentation 的可观察语义，不合并内部实现。

本文中的“同步”是指两侧对 Presentation 输出的字段和语义保持一致，不是合并两侧文件，也不是 WorldSide 的网络同步。

统一后的基本数据流如下：

```text
PlayerSide                    WorldSide
  身份、状态、存档、操作        身份、状态、网络、操作
         │                         │
         ├──── 独立 adapter ───────┤
                     │
                 MissionView
                     │
                     UI
```

## 职责与操作边界

1. Mission 层是任务真实状态与业务规则的唯一事实来源。
2. Presentation 层负责将两侧不同模型转换成统一的只读展示对象。
3. 长期目标中的 UI 层只负责展示 View；现有 UI 仍直接使用 PlayerSide 类型，本轮不迁移或修改它。
4. `MissionView` 是只读展示快照，不承载操作接口、delegate、Manager 或领域对象行为。
5. Actions 与 `MissionView` 并列，负责之后商定的操作能力；本契约暂不定义任何 Action 类型、状态、字段或执行入口。
6. 两侧 adapter 只转换数据，不改变两侧任务的生命周期、持久化或网络规则。

## MissionView

当前收敛后的 `MissionView` 轮廓如下：

```csharp
public sealed class MissionView
{
	public MissionIdentity Identity { get; init; }
	public MissionSourceBase Source { get; init; } = MissionSourceBase.Default;
	public MissionSourceBase SubSource { get; init; }
	public MissionType Type { get; init; }
	public string DisplayName { get; init; } = string.Empty;
	public string Description { get; init; } = string.Empty;
	public string Hint { get; init; } = string.Empty;
	public bool Visible { get; init; }
	public IReadOnlyList<MissionIconBase> Icons { get; init; }
		= Array.Empty<MissionIconBase>();
	public MissionViewState State { get; init; }
	public float Progress { get; init; }
	public long ElapsedTime { get; init; }
	public long? TimeLimit { get; init; }
	public long? RemainingTime => TimeLimit is long limit
		? Math.Max(0, limit - ElapsedTime)
		: null;
	public IReadOnlyList<ObjectiveNodeView> ObjectiveNodes { get; init; }
		= Array.Empty<ObjectiveNodeView>();
	public IReadOnlyList<RewardView> Rewards { get; init; }
		= Array.Empty<RewardView>();
}
```

所有集合都必须非 `null`。adapter 没有可导出的内容时使用空集合，不向 UI 暴露两侧的可变集合。

## 任务身份

Presentation 使用稳定身份定位任务：

```csharp
public readonly record struct MissionIdentity(
	MissionSide Side,
	string DefinitionId,
	string InstanceId);
```

- `DefinitionId` 标识任务定义。
- `InstanceId` 标识具体一轮或具体运行实例。
- PlayerSide 的 `DefinitionId = mission.Name`，每个新 Mission 对象在构造时生成一个 N 格式 GUID `InstanceId`。
- PlayerSide 任务在 `Available` 阶段即拥有 `InstanceId`；Available → Accepted、Cancel 回池以及当前实例的 Retry/Reset 都不改变 ID。
- PlayerSide `SaveData` 保存 `InstanceId`；`LoadData` 只在字段是合法 N 格式 GUID 时覆盖构造 ID。旧存档缺少字段或字段非法时保留构造 ID。
- 完成或删除后重新创建同类任务时，新对象生成新的 `InstanceId`。
- 本轮不改变 Manager 当前按 `Name` 判重的行为，也不实现多实例并存、完成历史或重复任务轮次。
- WorldSide 固定单实例任务可以暂时令 `InstanceId == DefinitionId`。
- Definition 级历史、完成次数和是否可重复属于系统层，不进入 `MissionView`。

`MissionView` 不导出以下重复任务信息：

- `IsRepeatable`
- `CompletionCount`
- `CurrentIteration`
- 历史记录
- 冷却或下次可接取时间

这些信息只在系统层使用。系统需要统计时直接查询任务实例集合或历史记录；操作能力留待 Actions 契约表达。

## 来源

不新增 `MissionSourceView`，直接复用现有 `MissionSourceBase`：

```csharp
public MissionSourceBase Source { get; init; } = MissionSourceBase.Default;

public MissionSourceBase SubSource { get; init; }
```

- `Source` 为空时，adapter 映射为 `MissionSourceBase.Default`。
- WorldSide 没有 `SubSource` 时导出 `null`。
- `MissionSourceBase` 现有的 `Name`、`Texture`、`Animation` 和 `Equals` 足够用于展示与筛选。
- 来源图标不自动混入任务 `Icons`。
- UI 需要展示来源图标时，可以使用 `MissionSourceIcon.Create(Source, SubSource)`。

## 任务类型

```csharp
public MissionType Type { get; init; }
```

`MissionType` 只是无行为的筛选标签。不新增 `MissionTypeView`，也不根据类型推导状态、操作能力或 UI 布局。

## 文本与 Hint

```csharp
public string DisplayName { get; init; } = string.Empty;

public string Description { get; init; } = string.Empty;

public string Hint { get; init; } = string.Empty;
```

- `DisplayName` 保持普通 `string`，用于标题、排序和搜索。
- `Description`、`Hint`、Objective 描述和非物品奖励描述都使用 `string`，默认值为 `string.Empty`。
- 展示字符串允许继续包含现有 StringDrawer 标记；本轮不新增另一套文本模型。
- `StringDrawer.Init` 的空字符串残留修复延期，但在 Hint 真正接入 UI 前必须完成。
- `Hint` 是通用提示，不绑定 `Locked` 状态；它既可以是真实提示，也可以全部由 `???` 构成。
- 需要统一遮蔽文本时使用 `MissionHintText.Masked`：

```csharp
public static class MissionHintText
{
	public const string Masked = "???";
}
```

- 两侧 MissionBase 的默认 `Hint` 都是 `string.Empty`；具体任务可以动态覆写。
- 不新增 `IsListed`；adapter 原样导出 `Visible`，现有 UI 是否列出任务仍由旧逻辑决定。
- 不引入 `MissionDisclosureLevel`、`IsUnlockHintVisible` 或 `IsHintMasked`。
- `Visible` 本轮保留为旧 UI 兼容字段，与 `Hint` 相互独立；不得把 `IsVisible == false` 自动映射成 Masked Hint。

### Hint 替换详情

当未来 UI 接入 View 时，`Hint` 非空表示用它替换整个任务详情界面。任务列表内容仍由 UI 根据兼容字段决定。

信息遮蔽必须由 adapter 完成，不能只依赖 UI 自觉隐藏。`Hint` 非空时，adapter 按以下规则导出隐藏详情：

| 字段 | 导出值 |
|---|---|
| `Description` | `string.Empty` |
| `ObjectiveNodes` | 空集合 |
| `Rewards` | 空集合 |
| `Progress` | `0` |
| `ElapsedTime` | `0` |
| `TimeLimit` | `null` |

之后定义的 Actions 也必须遵守同一遮蔽原则，但本契约不定义其具体形式。

隐藏成就可以在未完成时导出 `MissionHintText.Masked`，完成后改为 `string.Empty`，从而显示完整详情。

## 图标

```csharp
public IReadOnlyList<MissionIconBase> Icons { get; init; }
	= Array.Empty<MissionIconBase>();
```

- `MissionView` 不持有可变的 `MissionIconGroup`。
- `CurrentIndex`、`Prev`、`Next` 等轮播状态属于具体 UI。
- `Icons` 永不为 `null`；没有图标时使用空集合。
- 默认占位图由 UI 决定。
- 第一阶段继续复用 `MissionIconBase`，不重写为图标 DTO。
- Objective 不单独导出 Icons；如果仍需在顶部轮播 Objective 图标，由 adapter 将它们汇总进 `MissionView.Icons`。
- 来源图标与普通任务 Icons 保持分离。
- Player adapter 从现有图标结果创建数组快照，过滤 `MissionSourceIcon`；`mission.Icon == null` 时使用空数组。
- WorldSide 暂无图标，World adapter 使用空数组。
- `DrawerItem` 等可变 UI 绘制对象不得进入 `MissionView`。

## 展示状态

Presentation 使用单一、中性的展示状态枚举：

```csharp
public enum MissionViewState
{
	Locked,
	Available,
	Active,
	Completed,
	Failed,
	Overdue,
}
```

映射规则如下：

| PlayerSide | MissionViewState | WorldSide |
|---|---|---|
| `Available` | `Available` | — |
| `Accepted` | `Active` | `Active` |
| `Completed` | `Completed` | `Completed` |
| `Failed` | `Failed` | `Failed` |
| `Overdue` | `Overdue` | — |
| — | `Locked` | `Locked` |

- 不拆分 Phase 与 Outcome。
- 展示状态只用于展示，不推导按钮能力、奖励领取能力或可重复性。
- PlayerSide 任务取消后如果回到任务池，则重新导出为 `Available`。
- PlayerSide 任务删除后，其 instance 从 Catalog 中移除。
- 不新增 `Canceled` 展示状态。

## 任务总进度与计时

```csharp
public float Progress { get; init; }

public long ElapsedTime { get; init; }

public long? TimeLimit { get; init; }

public long? RemainingTime => TimeLimit is long limit
	? Math.Max(0, limit - ElapsedTime)
	: null;
```

- adapter 将 `Progress` 约束到 `[0, 1]`，两侧继续独立计算真实进度。
- 时间单位统一为游戏帧。
- `TimeLimit == null` 表示不限时。
- PlayerSide 与 WorldSide 的 `TimeLimit <= 0` 都只在各自 adapter 中归一化为 `null`，不能泄露给 UI。
- UI 负责将帧格式化为秒或分钟。
- 不导出 `EnableTime`、`CompletedSteps` 或 `TotalSteps`。
- `Hint` 非空时导出 `Progress = 0`、`ElapsedTime = 0`、`TimeLimit = null`。

## Presentation 专用 Objective 树

Objective 不导出为扁平文本列表，也不合并 PlayerSide 与 WorldSide 的领域任务树。两侧 adapter 将领域节点映射到 Presentation 专用只读树。

`MissionView` 持有：

```csharp
public IReadOnlyList<ObjectiveNodeView> ObjectiveNodes { get; init; }
	= Array.Empty<ObjectiveNodeView>();
```

顶层列表表示顺序节点。节点使用互斥类型，避免一个 NodeView 同时携带多组无效字段：

```csharp
public abstract record ObjectiveNodeView;

public sealed record LeafObjectiveNodeView(ObjectiveView Objective)
	: ObjectiveNodeView;

public sealed record ParallelObjectiveNodeView(
	IReadOnlyList<ObjectiveView> Objectives)
	: ObjectiveNodeView;

public sealed record AnyOfObjectiveNodeView(
	IReadOnlyList<ObjectiveView> Objectives)
	: ObjectiveNodeView;

public sealed record ObjectiveBranchView(
	ObjectiveBranchState State,
	IReadOnlyList<ObjectiveView> Objectives);

public sealed record BranchObjectiveNodeView(
	IReadOnlyList<ObjectiveBranchView> Branches)
	: ObjectiveNodeView;
```

`AnyOfObjectiveNodeView` 映射现有 `OptionalNode`；现有 OptionalNode 的实际语义是任一 Objective 完成。

分支状态为：

```csharp
public enum ObjectiveBranchState
{
	Candidate,
	Selected,
	Skipped,
}
```

- `Candidate`：尚未选择的候选分支。
- `Selected`：已经进入的分支。
- `Skipped`：已经排除的分支。
- Selected 且全部 Objective 为 Completed 表示分支完成，不新增 Completed 分支状态。
- Skipped 分支内的 Objective 统一导出 `State = Skipped`、`Progress = 0`。
- 分支展示对象不加入 BranchId、Name、Progress、SelectedIndex、CurrentObjectiveIndex、行为或领域引用。

### 领域节点只读出口

两侧具体节点只向本侧 adapter 开放最小 `internal` 只读状态：

- Leaf：`Objective`。
- Parallel/Optional：`IReadOnlyList<ObjectiveBase> Objectives`。
- Branch：只读分支序列和 `int? SelectedBranchIndex`。

只读出口不得暴露可变领域 `List`、`_indexInBranch`、Save/Load 键或 Complete/Reset 等行为。领域节点不依赖 Presentation，两侧也不建立公共领域节点接口；所有集合保持领域定义顺序。

### ObjectiveView

Objective 的最小展示字段为：

```csharp
public sealed class ObjectiveView
{
	public int Id { get; init; }
	public string Description { get; init; } = string.Empty;
	public float Progress { get; init; }
	public ObjectiveViewState State { get; init; }
}

public enum ObjectiveViewState
{
	Pending,
	Active,
	Completed,
	Skipped,
}
```

- `Id` 在同一个 Mission instance 内唯一；完整定位由 `MissionIdentity + Id` 组成。
- `Description` 可以多行，也可以包含现有 StringDrawer 标记。
- adapter 将 `Progress` 约束到 `[0, 1]`。
- `Completed` 状态统一导出 `Progress = 1`。
- 不同时保留 `Completed`、`IsActive` 等布尔字段。
- 不加入 Objective Name、Hint、TimeLimit、Action、Icons、Rewards 或领域引用。
- `Hint` 非空时，`MissionView.ObjectiveNodes` 为空集合。

Objective 状态按以下优先级推导：

1. 位于 Skipped branch → `Skipped`。
2. `objective.Completed` → `Completed`。
3. Mission 为 Active 且 Objective 位于 `FindCurrentObjectives()` → `Active`。
4. 其他 → `Pending`。

PlayerSide 通过现有 `GetObjectivesText(List<string>)` 收集描述并用换行连接；WorldSide 第一版不调用当前会抛异常的 Objective 文本方法，暂时导出 `string.Empty`。

## 奖励

奖励保持最简展示模型：

```csharp
public sealed class RewardView
{
	public Item Item { get; init; }
	public string Description { get; init; } = string.Empty;
}

public IReadOnlyList<RewardView> Rewards { get; init; }
	= Array.Empty<RewardView>();
```

只存在两种展示路径：

1. `Item` 非 `null`：UI 使用独立物品方块和 Terraria 原生名称、描述、数量及 tooltip，并忽略自定义 `Description`。
2. `Item` 为 `null`：UI 使用 `Description` 显示非物品奖励。

其他规则：

- `Item` 是任务系统自己构造的奖励对象，不要求 Clone。
- UI 将 `Item` 视为只读，不修改它，也不直接将它放入玩家背包。
- 不新增 Reward 层级、`ItemView`、`ItemSnapshot`、`RewardState`、`ObjectiveId`、`CanClaimReward` 或 `ExtraRewards object`。
- 是否领取及发奖行为属于系统层和之后的 Actions 契约。
- `Hint` 非空时，`Rewards` 为空集合。

## 不进入 MissionView 的内容

以下内容继续由两侧或系统层独立维护：

- MissionBase 文件与继承关系。
- Manager 与领域对象行为。
- 内部状态枚举和生命周期转换。
- MissionStructure 与领域节点实现。
- Objective 实现。
- 存档格式与历史记录。
- WorldSide 网络同步和服务器权威规则。
- 奖励领取状态与发放方式。
- PlayerSide 取消、删除和可重复任务逻辑。
- Action 定义、可用性、请求和执行结果。

## 当前验收边界

- PlayerSide 与 WorldSide 均能导出完整、只读的 `MissionView`。
- 现有 UI 文件保持不变；迁移到 View 属于后续工作。
- PlayerSide 可重复任务的一轮对应一个稳定 `InstanceId`。
- adapter 原样保留 `Visible`，隐藏详情独立由 `Hint` 驱动。
- `Hint` 非空时，adapter 不会导出可绕过提示读取的隐藏详情。
- Source、Mission Icons 和 Objective Icons 的职责分离明确。
- 两侧领域任务树保持独立，只映射到 Presentation 专用只读树。
- Reward 只提供物品与非物品描述两种展示路径。
- 现有存档格式、PlayerSide 灵活性与 WorldSide 网络权威不因 View 统一而被削弱。
- Action 契约尚未达成共识，不在本文定义。
