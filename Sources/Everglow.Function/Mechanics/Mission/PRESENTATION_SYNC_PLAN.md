# Mission Presentation 有限同步实施计划

## 目标

本轮只统一 PlayerSide 与 WorldSide 向 Presentation 导出的只读数据：

```text
PlayerSide ── PlayerMissionViewAdapter ──┐
                                         ├── MissionView
WorldSide  ── WorldMissionViewAdapter  ──┘
```

本轮不迁移 UI，不定义 Action，不合并两侧领域类型，也不改变任务生命周期、PlayerSide 灵活性或 WorldSide 网络权威。

实施过程拆分为八个可独立构建、测试和回退的提交。代码批次必须携带对应测试，不在最后一个提交集中补测。

## 实施原则

1. `MissionView` 只包含不可变的展示快照，不持有领域行为、Manager、delegate 或操作接口。
2. PlayerSide 与 WorldSide 分别保留自己的 MissionBase、Manager、状态、Objective 和节点实现。
3. 两侧 adapter 分别读取本侧领域模型，输出相同的 Presentation 类型。
4. 领域节点只开放 adapter 所需的最小 `internal` 只读状态，不依赖 Presentation。
5. 现有 UI 行为保持不变，`Visible` 在本轮作为兼容字段保留。
6. 所有 Action 相关设计和实现延后。
7. 每个提交完成后先验证再提交，禁止混入相邻批次或无关重构。

## 提交 1：同步 Presentation 临时文档

建议提交信息：

```text
docs(mission): finalize presentation synchronization docs
```

### 修改范围

将以下两份实施期间使用的临时文档纳入同一提交：

- `PRESENTATION_CONTRACT.md`
- `PRESENTATION_SYNC_PLAN.md`

更新 `PRESENTATION_CONTRACT.md`：

- 将全部展示文本统一为 `string`。
- 明确项目继续使用现有 StringDrawer 标记字符串。
- 加入 `MissionHintText.Masked`。
- 写明任意非空 Hint 都触发详情遮蔽。
- 写明 `Visible` 暂时保留为兼容字段，与 Hint 相互独立。
- 加入 PlayerSide GUID `InstanceId` 的生成、持久化和旧存档规则。
- 补齐 `ObjectiveBranchView` 的最小结构。
- 写明 Objective 状态推导优先级。
- 写明具体节点只开放 `internal` 最小只读出口。
- 删除本轮迁移 UI 或立即修复 StringDrawer 的表述。
- Action 只作为边界出现，不定义任何具体类型、状态、字段或入口。

更新 `PRESENTATION_SYNC_PLAN.md`：

- 记录八个提交的依赖顺序、修改边界和测试要求。
- 将临时契约、代码实施和最终文档收敛串成完整流程。
- 明确第 8 个提交会用长期总设计文档替换这两份临时文档。

`ObjectiveBranchView` 使用：

```csharp
public sealed record ObjectiveBranchView(
	ObjectiveBranchState State,
	IReadOnlyList<ObjectiveView> Objectives);
```

### 验证

- 文档中的展示文本类型全部为 `string`。
- 文档中不存在具体 `MissionAction` 定义。
- Hint、Visible 和 InstanceId 规则没有互相冲突。
- 两份临时文档都已被 Git 跟踪，便于第 8 个提交显式删除。
- `git diff --check` 通过。
- BOM 检查通过。
- 完整构建通过。

## 提交 2：PlayerSide 实例身份

建议提交信息：

```text
feat(mission): persist player mission instance ids
```

### 修改范围

为 `PlayerMissionBase` 增加：

```csharp
public string InstanceId { get; private set; }
	= Guid.NewGuid().ToString("N");
```

持久化规则：

- `SaveData` 随任务保存 `InstanceId`。
- `LoadData` 仅在字段是合法 N 格式 GUID 时覆盖构造时生成的 ID。
- 旧存档缺失字段时保留构造时生成的 ID，并在下次保存时写入。
- 非法字段不覆盖构造时生成的 ID。

生命周期规则：

- `DefinitionId = mission.Name`。
- 创建新的 Mission 对象时产生新的 `InstanceId`。
- Available 转 Accepted 时 ID 不变。
- Cancel 回池时 ID 不变。
- 对当前实例 Retry 或 Reset 时 ID 不变。
- 完成或删除后重新创建同类任务时，由新对象生成新 ID。
- 本轮不修改 Manager 当前按 `Name` 判重的行为。
- 本轮不实现多实例并存、完成历史或重复任务轮次。

### 测试

- 两个新 Mission 对象产生不同 ID。
- ID 是合法的 N 格式 GUID。
- 状态变化和 Reset 不改变 ID。
- Save/Load 恢复原 ID。
- 缺失字段时保留构造 ID。
- 非法字段不会覆盖构造 ID。
- 新对象代表新一轮时产生新 ID。

### 验证

- MissionSystem 定向测试通过。
- 完整构建通过。
- 暂存内容只包含身份、持久化和对应测试。

## 提交 3：Hint 元数据

建议提交信息：

```text
refactor(mission): add mission hint metadata
```

### 修改范围

新增：

```csharp
public static class MissionHintText
{
	public const string Masked = "???";
}
```

PlayerSide 与 WorldSide 的 MissionBase 分别增加：

```csharp
public virtual string Hint => string.Empty;
```

规则：

- 不新增其他文本包装、`MissionHint`、Requirement 或 Formatter。
- 具体任务可以动态覆写 Hint。
- 需要遮蔽文本时统一使用 `MissionHintText.Masked`。
- 不改变 `PlayerMissionBase.IsVisible`。
- 不改变 `WorldMissionBase.Visible`。
- 不修改现有 UI 或 StringDrawer。
- 领域层只提供 Hint，不执行信息遮蔽。

### 测试

- 两侧默认 Hint 均为空。
- 派生任务可以动态覆写 Hint。
- `MissionHintText.Masked` 的值为 `"???"`。

### 验证

- MissionSystem 定向测试通过。
- 完整构建通过。
- UI 文件没有变更。

## 提交 4：Objective 节点最小只读出口

建议提交信息：

```text
refactor(mission): expose objective presentation state
```

### Leaf 节点

PlayerSide 提供：

```csharp
internal PlayerObjectiveBase Objective { get; }
```

WorldSide 提供对应的 `WorldObjectiveBase` 属性。

### Parallel 与 Optional 节点

PlayerSide 提供：

```csharp
internal IReadOnlyList<PlayerObjectiveBase> Objectives { get; }
```

WorldSide 提供对应的 `WorldObjectiveBase` 集合。

### Branch 节点

PlayerSide 提供：

```csharp
internal IReadOnlyList<IReadOnlyList<PlayerObjectiveBase>> Branches { get; }

internal int? SelectedBranchIndex { get; }
```

WorldSide 提供对应类型的属性。

规则：

- `_selected == -1` 映射为 `null`。
- 不暴露 `_indexInBranch`。
- 不暴露可修改的领域 `List`。
- 不暴露 Save/Load 键、Complete、Reset 等行为。
- 节点不引用 Presentation。
- 不建立 PlayerSide 与 WorldSide 公共领域节点接口。
- 两侧 adapter 之后分别 switch 本侧具体节点类型。
- 分支和 Objective 保持领域定义顺序。

### Objective 状态推导规则

adapter 之后按以下优先级推导 `ObjectiveViewState`：

1. Objective 位于 Skipped branch：`Skipped`。
2. `objective.Completed == true`：`Completed`。
3. Mission 为 Active，且 Objective 位于 `FindCurrentObjectives()`：`Active`。
4. 其他情况：`Pending`。

不需要暴露 `_indexInBranch`；当前 Objective 可以从容器的活动 Objective 集合推导。

### Objective 文本过渡规则

- Player adapter 后续通过现有 `GetObjectivesText(List<string>)` 收集文本，并用换行连接为一个描述字符串。
- WorldSide 当前的 `GetObjectivesText()` 尚未实现，adapter 不得调用会抛出异常的方法。
- 第一版允许 World Objective 描述导出 `string.Empty`。
- 补齐具体 World Objective 文本属于内容完善，不阻塞 Presentation 结构同步。

### 测试

- Leaf、Parallel、Optional 保持 Objective 定义顺序。
- Branch 保持分支及分支内 Objective 定义顺序。
- 未选分支导出 `null`。
- 选中后导出正确索引。
- 只读出口不能修改领域内部集合。
- 原有节点生命周期、持久化和网络测试继续通过。

## 提交 5：定义只读 Presentation 模型

建议提交信息：

```text
refactor(mission): define mission presentation views
```

### 新增或重写的类型

- `MissionSide`
- `MissionIdentity`
- `MissionViewState`
- `ObjectiveViewState`
- `ObjectiveBranchState`
- `ObjectiveView`
- `ObjectiveNodeView`
- `LeafObjectiveNodeView`
- `ParallelObjectiveNodeView`
- `AnyOfObjectiveNodeView`
- `ObjectiveBranchView`
- `BranchObjectiveNodeView`
- `RewardView`
- `MissionView`

所有展示文本均使用 `string`：

```csharp
public string Description { get; init; } = string.Empty;

public string Hint { get; init; } = string.Empty;
```

过渡期保留：

```csharp
public bool Visible { get; init; }
```

`Visible` 不是最终统一语义，仅用于后续兼容旧 UI。

### 清理旧模型

删除未被其他代码使用的旧类型：

- `NodeView`
- `NodeType`
- `UIMissionState`

重写旧 `MissionView`：

- 删除接收 PlayerMissionBase 或 WorldMissionBase 的构造器。
- 删除可变公开字段。
- 删除 `Retriable`、步骤计数、领取状态和 `ExtraRewards`。
- 删除领域状态映射方法。

本轮保留：

- `PlayerMissionComparer`
- `MissionIconGroup`
- 现有 UI 使用的 PlayerSide 类型

### 模型约束

- `MissionView` 不引用两侧 MissionBase。
- 所有集合默认空且永不为 `null`。
- adapter 必须传入数组快照。
- `RemainingTime` 不小于零。
- UI 将 `RewardView.Item` 视为只读。
- Objective 节点不支持特殊节点嵌套。
- 不包含 Action、Manager、delegate 或领域节点引用。

### 验证

- 完整构建通过。
- Presentation 模型可以独立构造。
- 旧 UI 仍能编译且行为未变。

## 提交 6：PlayerSide adapter

建议提交信息：

```text
refactor(mission): export player mission views
```

### 入口

```csharp
public static class PlayerMissionViewAdapter
{
	public static MissionView Create(PlayerMissionBase mission);
}
```

### 字段映射

- `MissionSide.Player`
- `DefinitionId = mission.Name`
- `InstanceId = mission.InstanceId`
- Source 空值归一化为 `MissionSourceBase.Default`
- SubSource
- MissionType
- DisplayName
- Description
- Hint
- `Visible = mission.IsVisible`
- Player 状态映射
- Progress clamp 到 `[0, 1]`
- `ElapsedTime = mission.Time`
- `TimeLimit <= 0` 映射为 `null`
- Objective 树
- Reward 数组快照
- Icon 数组快照

### 图标过渡策略

- 保持现有 `MissionIconGroup` 和 UI 行为不变。
- adapter 从当前图标结果创建数组快照。
- 过滤 `MissionSourceIcon`，确保来源图标不进入 `MissionView.Icons`。
- Objective 图标继续包含在普通 Icons 中。
- `mission.Icon == null` 时导出空数组。

### Hint 遮蔽

```csharp
string hint = mission.Hint ?? string.Empty;
bool hidesDetails = hint.Length > 0;
```

任何非空 Hint 都强制导出：

- `Description = string.Empty`
- `ObjectiveNodes = []`
- `Rewards = []`
- `Progress = 0`
- `ElapsedTime = 0`
- `TimeLimit = null`

Hint 不改变 `Visible`。

### 测试

- 全部 Player 状态映射。
- GUID 身份映射。
- 时间和进度归一化。
- Source 默认值。
- Visible 保持原值。
- 普通 Hint 与 Masked Hint 都完整遮蔽。
- Leaf、Parallel、AnyOf、Branch 映射。
- Branch Candidate、Selected、Skipped 状态。
- Skipped Objective 的 Progress 为 0。
- Completed Objective 统一导出 Progress 1。
- Source icon 被排除。
- 导出数组不随领域 List 后续变化。
- Reward Item 保持相同引用，但集合是快照。

### 验证

- PlayerSide MissionSystem 测试通过。
- 完整构建通过。
- UI 文件没有变更。

## 提交 7：WorldSide adapter

建议提交信息：

```text
refactor(mission): export world mission views
```

### 入口

```csharp
public static class WorldMissionViewAdapter
{
	public static MissionView Create(WorldMissionBase mission);
}
```

### 字段映射

- `MissionSide.World`
- `DefinitionId = mission.Name`
- `InstanceId = mission.Name`
- Source 空值归一化为 `MissionSourceBase.Default`
- `SubSource = null`
- MissionType
- DisplayName
- Description
- Hint
- `Visible = mission.Visible`
- World 状态映射
- Progress clamp 到 `[0, 1]`
- `ElapsedTime = mission.Time`
- `TimeLimit <= 0` 映射为 `null`
- Objective 树
- Reward 数组快照
- 暂无图标时导出空数组

Objective 状态使用与 Player adapter 相同的优先级，但 Active 判断使用 World Mission Active 状态和 `FindCurrentObjectives()`。

### 禁止修改

- `WhoAmI`
- NetSend/NetReceive
- Packet
- Retry
- Reward claim
- WorldMissionManager
- 服务器权威判断

### 测试

- 全部 World 状态映射。
- 固定单实例身份。
- `TimeLimit == 0` 映射为 `null`。
- Visible 与 Hint 相互独立。
- 任意非空 Hint 都完整遮蔽。
- Leaf、Parallel、AnyOf、Branch 映射。
- Branch Candidate、Selected、Skipped 状态。
- Reward 集合是快照。
- adapter 不触发任务行为、网络或奖励逻辑。

### 验证

- WorldSide MissionSystem 测试通过。
- 完整构建通过。
- 网络和 Manager 文件没有变更。

## 提交 8：收敛任务系统总设计文档

建议提交信息：

```text
docs(mission): consolidate mission system design
```

### 目标

在代码实施和验证全部完成后，将本轮已经落地的稳定结论整理成长期维护的任务系统总设计文档：

```text
MISSION_SYSTEM_DESIGN.md
```

总设计文档描述最终存在的架构和约束，不保留临时实施顺序、提交信息或已经完成的迁移步骤。

### 内容范围

总设计文档结合以下来源：

- `README.md` 中的任务系统目标和 WorldSide 设计。
- `PRESENTATION_CONTRACT.md` 中已经落地的 Presentation 契约。
- `PRESENTATION_SYNC_PLAN.md` 中经过实施验证的边界和验收结果。
- 实施完成后的实际代码结构。

文档至少覆盖：

1. 任务系统总体目标和双侧模型。
2. Mission、Presentation、UI 三层职责。
3. PlayerSide 与 WorldSide 必须保留的差异。
4. Mission 身份、状态、Hint、时间、来源、图标和奖励语义。
5. Presentation Objective 树及分支状态。
6. 两侧 adapter 的职责和禁止行为。
7. PlayerSide 持久化与 WorldSide 网络权威边界。
8. Action 尚未定稿的边界和未来扩展点。

### 字数限制

`MISSION_SYSTEM_DESIGN.md` 必须不超过 5000 个字符，Markdown 标记、空白和代码片段也计入长度。使用以下 PowerShell 检查：

```powershell
$design = Get-Content -Raw -LiteralPath 'Sources/Everglow.Function/Mechanics/Mission/MISSION_SYSTEM_DESIGN.md'
if ($design.Length -gt 5000)
{
	throw "MISSION_SYSTEM_DESIGN.md exceeds 5000 characters: $($design.Length)"
}
```

### 删除临时文档

总设计文档完成并核对后，在同一提交中删除：

- `PRESENTATION_CONTRACT.md`
- `PRESENTATION_SYNC_PLAN.md`

删除前必须确认总设计文档已经吸收所有仍然有效的约束。不得删除 `README.md` 或 `TODO.md`。

### 验证

- `MISSION_SYSTEM_DESIGN.md` 不超过 5000 个字符。
- 文档描述与最终代码一致，不包含尚未实现的能力。
- Action 明确标记为后续设计，不出现虚构的具体 API。
- 仓库中不存在对两份已删除临时文档的悬空引用。
- `git diff --check` 通过。
- BOM 检查通过。
- 完整构建与完整单元测试通过。
- 该提交只包含总设计文档、临时文档删除及必要的文档引用更新。

## 最终验证

所有提交完成后执行：

```powershell
dotnet build
dotnet test --filter "FullyQualifiedName~MissionSystem"
dotnet test --verbosity normal /p:WarningLevel=0
```

同时完成：

- `git diff --check`
- AGENTS.md 要求的原始字节 BOM 检查
- 逐提交检查文件边界
- 检查工作区没有生成物或意外文件
- 检查 `MissionView` 不引用两侧领域行为
- 检查 UI 文件无变更
- 检查不存在 Action 类型
- 检查 WorldSide 网络文件无变更

## 延期内容

以下内容不属于本轮八个提交：

- MissionAction 及按钮可用性。
- Catalog、刷新事件和统一查询入口。
- UI 列表、详情、图标轮播与按钮迁移。
- 将 `PlayerMissionComparer` 改为比较 `MissionView`。
- 移除 `Visible`。
- 将 `IsVisible` 自动转换为 Masked Hint。
- StringDrawer 空字符串残留修复。
- 可重复任务多实例并存及历史 UI。
- World Objective 文本内容补全。
- WorldSide 网络与奖励领取改造。

StringDrawer 的空字符串残留修复虽然延期，但在 Hint 真正接入 UI 前属于强制前置批次。

## 完成标准

- PlayerSide 与 WorldSide 都能导出只读 `MissionView`。
- 两侧 adapter 不改变领域对象状态。
- PlayerSide 实例身份可以跨存档恢复。
- Objective 树和分支状态可以完整映射。
- 任意非空 Hint 都在 adapter 层完成详情遮蔽。
- Visible 旧行为保持不变。
- Presentation 集合不暴露领域可变 List。
- 现有 UI、PlayerSide 生命周期和 WorldSide 网络行为保持兼容。
- 八个批次均为独立提交，且每个提交都可构建和回退。
- 最终只保留不超过 5000 个字符的 `MISSION_SYSTEM_DESIGN.md`，不再保留临时契约和实施计划。
