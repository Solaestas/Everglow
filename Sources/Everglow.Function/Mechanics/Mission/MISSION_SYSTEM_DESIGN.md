# 任务系统总设计

## 总体目标

Everglow 的任务系统保留两套领域模型：PlayerSide 提供跟随玩家存档的个人、可重复体验；WorldSide 提供跟随世界存档的静态流程和多人同步。两侧共享展示语义，但不合并 MissionBase、Manager、状态机、Objective、节点、生命周期或存档格式。

统一数据流为：

```text
PlayerSide Manager ── Player adapter ──┐
                                       ├── MissionPresentationService ── MissionPresentationEntry
WorldSide Manager  ── World adapter  ──┘
PlayerSide/WorldSide Actions <──────────── MissionPresentationService.TryExecute
```

## 分层职责

- Mission 层保存真实状态并执行解锁、推进、完成、失败、重试、持久化、同步和发奖规则，是唯一事实来源。
- Presentation 层用两侧独立 adapter 生成统一、只读的 `MissionView` 与 `MissionAction` 快照；`MissionPresentationService` 提供查询和执行入口，`MissionPresentationSystem` 管理 Service 的运行期生命周期。View 不包含 Manager、delegate、领域行为或可变领域集合。

## 身份与状态

`MissionIdentity` 由 `Side`、`DefinitionId` 和 `InstanceId` 组成。

- Player：`DefinitionId = Name`；对象构造时生成 N 格式 GUID 作为 `InstanceId`。Available、Accepted、Cancel 回池以及当前对象的 Retry/Reset 不更换 ID；重新创建对象会产生新 ID。Manager 仍按 `Name` 判重，不支持同定义多实例并存。
- World：固定单实例，`DefinitionId = InstanceId = Name`；运行期 `WhoAmI` 不进入展示身份。

统一展示状态为 Locked、Available、Active、Completed、Failed、Overdue。Player 的 Available/Accepted/Completed/Failed/Overdue 分别映射到同名语义（Accepted 映射 Active）；World 的 Locked/Active/Completed/Failed 直接映射。展示状态不推导重试、领奖或按钮能力。

## 文本、Hint 与可见性

DisplayName、Description、Hint、Objective 描述和非物品奖励描述均为 `string`，默认空字符串，并可继续携带现有 StringDrawer 标记。包含非空白内容的 Hint 由 adapter 在快照层替换详情：Description 为空、ObjectiveNodes 与 Rewards 为空、Progress 和 ElapsedTime 为 0、TimeLimit 为 `null`；纯空白 Hint 不触发遮蔽。统一遮蔽文本使用 `MissionHintText.Masked`（`"???"`）。

Hint 与 `Visible` 相互独立；adapter 原样导出 Player `IsVisible` 或 World `Visible`，不新增 `IsListed`，也不把不可见自动转换为 Masked Hint。

## 进度、时间、来源、图标与奖励

- Mission 与 Objective 进度在导出时约束到 `[0, 1]`，NaN 归零；Completed Objective 强制为 1，Skipped Objective 强制为 0。
- 时间统一使用游戏帧。ElapsedTime 来自两侧 `Time`；`TimeLimit <= 0` 归一化为 `null`；RemainingTime 不小于 0。UI 负责换算秒或分钟。
- MissionType 原样导出，仅作为无行为的展示和筛选标签，不推导状态、操作能力或 UI 布局。
- Source 继续使用 `MissionSourceBase`，空值归一化为 `Default`；仅 Player 有 SubSource。来源图标与任务图标分离，UI 可按 Source/SubSource 单独创建来源图标。
- Icons 永不为 `null`。Player adapter 对现有图标结果生成数组快照并过滤 `MissionSourceIcon`，保留普通任务及汇总后的 Objective 图标；World 当前导出空数组。轮播状态和默认占位图属于 UI，`DrawerItem` 不进入 View。
- Rewards 是 `RewardView` 快照。物品奖励保留任务创建的 `Item` 引用，由 UI 只读展示 Terraria 名称、数量和 tooltip；`Item == null` 时使用 Description 表示非物品奖励。领奖与发放仍属于领域层。

## Presentation Objective 树

`MissionView.ObjectiveNodes` 按领域定义顺序保存互斥的只读节点：Leaf、Parallel、AnyOf（映射 Optional）和 Branch。Branch 包含有序 `ObjectiveBranchView`；未选择、已选择、已排除分别为 Candidate、Selected、Skipped。已完成的已选分支仍为 Selected，不另设分支完成态。

`ObjectiveView` 只含实例内 ID、Description、Progress 和 Pending/Active/Completed/Skipped 状态。状态优先级固定为：位于已排除分支、Objective 已完成、Mission 为 Active 且属于 `FindCurrentObjectives()`、其他。Player adapter 通过 `GetObjectivesText(List<string>)` 收集多行描述；World adapter 导出空描述。

两侧具体节点仅向同程序集 adapter 提供最小 `internal` 只读出口：Leaf 的 Objective，Parallel/Optional 的 Objective 序列，Branch 的嵌套分支序列与可空 SelectedBranchIndex。出口使用只读包装，不暴露内部 List、游标、存档键或行为，也不让领域层依赖 Presentation。

## Adapter 与领域边界

adapter 只读取状态、归一化空值与范围并创建数组快照；不得调用 Update、Complete、Reset、Retry、GiveRewards 或 Manager，不得改变任务、Objective、奖励、存档和网络状态，也不得访问 UI。未知领域状态或节点类型应显式失败，避免静默生成错误展示。

Player `InstanceId` 随任务写入玩家存档；加载时只有合法 N 格式 GUID 才覆盖构造 ID，缺失或非法旧数据保留新 ID。World 保持既有权威判断、同步和存档协议；全任务/单任务同步、进度上传及现有 NetSend/NetReceive/Packet 由 WorldSide 维护。

## Presentation 查询与操作

`MissionPresentationEntry` 在 `MissionView` 外并列携带 `IReadOnlyList<MissionAction>`。Service 每次查询都重新读取当前 Manager 中的领域任务，并连续投影 View 与 Actions；不保存 entry 缓存，也不暴露 Manager 集合。旧 entry 及其集合层级都是数组快照，Manager 后续变化不会反向修改旧结果。

`MissionPresentationService` 同时支持 Player 与 World：

- `GetAll()` 投影并返回两侧全部 entry。
- `TryGet` 使用完整 `MissionIdentity` 做 Ordinal 匹配。Player 必须同时匹配定义名与当前实例 GUID；World 必须满足 `DefinitionId == InstanceId == Name`。
- `TryExecute` 根据 `MissionSide` 调用与对应 Manager 成对的既有 Actions；Actions 重新校验完整 Identity、Hint 和当前可用操作，并以 `bool` 表示执行结果。
- 未知 Side、任务缺失或 Player 实例过期时返回 `false`。adapter 的投影异常直接向调用方暴露。

Player 的 `Submit` 只在 Accepted、已完成且未被 Hint 遮蔽时提供；执行时调用任务既有完成入口，并以是否进入 Completed 作为结果。World 不提供 Submit，也不改变重试、领奖、奖励或 Packet 权威。

## 运行期组合

`MissionPresentationSystem` 在 `PostSetupContent` 从 `PlayerMissionSystem` 和 `WorldMissionSystem` 取得各自成对的 Manager/Actions，并创建 `MissionPresentationService`；不使用 Manager 静态 locator，不注册到 `Ins`，也不解析图形或 UI 服务。`Unload` 清空 Service 引用。

`WorldMissionSystem` 在 `Load` 中为自身 Manager 创建 `WorldMissionActions`，并在 `Unload` 中与 Manager 一并清空引用。
