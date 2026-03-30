任务系统分为世界侧与玩家侧2个部分，世界侧提供完整的多人游戏同步（跟随世界存档），玩家提供可重复的个人体验（跟随玩家存档）。当前仅世界侧。

The world-side mission system aims to deliver a structured static experience through several key features.

(Types) All world-side missions fall into 3 categories: Main Story, Side Story, and Legendary. Main story missions are the core of the game, designed to guide players through the main storyline. Side story missions are optional, offering additional content and rewards. Legendary missions are a special subset of side story missions. They feature unique content and exclusive rewards, setting them apart from regular side story missions.

(Features-Main) Missions are locked before players reach certain milestones, though the unlocking requirements is visible. Some missions only become visible after specific conditions are met. Some of the missions are time-limited. Once the time limit expires, the mission becomes unavailable until a player chooses to restart it.

(UI) Mission UI consists of two parts: mission list and mission detail. In the mission list, all missions are displayed, and each mission is represented by a card. The card shows the mission name, type, state, and the time left (if applicable). Players can click on a card to view the mission detail, which includes the full description, objectives, rewards, and other information. If a mission is invisible, the card is grayed out and only shows "???" as the name, and the detail page remains empty. The UI appearance is selectable, with the marble plate technical style as the default and new themes becoming available over game progress.

(Features-Sub) Each mission is a combination of several parts, including mission name, type, state, description, objectives, rewards, and other information. The mission state can be one of the following: locked, available, completed, or failed. Objectives are the tasks that players must complete to finish the mission. The rewards are granted upon mission completion, and they include items, world state changes, or other benefits.

(Features-Objectives) Objectives can be of various types, including but not limited to: kill NPCs, collect items, consume items, reach certain locations, move for a certain distance, talk to an NPC, give items to an NPC, and complete an invasion. Two special structures are also available: parallel and branching objectives. An objective can also give players rewards that are required to complete subsequent objective(s), but these rewards might be removed after the objective is completed.

## 世界侧任务系统

世界侧任务系统目标提供结构化的静态的任务流程

### 任务构成

- 基础信息: 名称、描述、类型
- 功能信息: 状态、目标、奖励

### 任务机制

- 锁定与可见性: 任务在达到特定条件处于锁定状态，详情内容不可见。大部分任务解锁条件可见；隐藏任务解锁条件不可见，仅显示"???"作为任务名
- 限时任务: 部分任务有时间限制，可无条件重试

### 任务类型

总共包含3个类型，仅用作分类显示，不影响游玩体验:

- 主线任务: 游戏核心，引导玩家推进主线剧情
- 支线任务: 可选内容，提供额外的剧情内容和奖励
- 传说任务: 内容更为丰富的支线任务，独特内容和高级奖励

### 任务状态 & 生命周期

##### 锁定

大部分任务在初始状态下处于锁定状态，需要达成特定条件后才能解锁
解锁后不会再次锁定

##### 进行中

任务解锁后自动接取，进入此状态

##### 已完成

任务完成后进入此状态

##### 失败

标记任务因为某种原因失败，可无条件重试返回进行中状态
可能的原因有：任务计时结束、目标计时结束、任务限制等

### 任务目标

##### 目标类型

- 击杀NPC:击败一定数量的指定NPC，多人游戏下计算所有玩家的击杀（未实现在特定条件下击杀，因BOSS可能需要全伤害计算）
- 收集物品: 在背包中拥有一定数量的指定物品，多人游戏下计算所有玩家的仓库
- 消耗物品: 消耗一定数量的指定物品，多人游戏下由全体玩家贡献进度，需C/S交互
- 到达地点: 检测位置、环境、地层
- 行走距离: 可附带条件
- 对话NPC: 检测玩家对话NPC
- 提交物品: 同上，对话后自动提交，需C/S交互
- 完成事件: 略。等待事件系统实现同步后启用

##### 特殊结构

- 分支目标: 完成其中一项后自动进入该分支
- 并行目标: 完成任意/全部后可到达后继目标

注：特殊结构之间不可互相包含

##### 目标限时

同任务限时，可单独重试该目标

##### 目标内奖励

完成某些目标可能获得奖励，作为后续目标的前置条件。这些奖励可能在完成目标后被移除，类似天国魔力

### 任务奖励

完成任务后向全体玩家发放，包含物品、世界状态变化等

### 网络同步

世界侧任务系统采用服务器权威模型，大部分逻辑均在服务器完成，部分操作由客户端采集信息并上报，在服务器端验证后生效。同步方式分为三类：全量同步、增量同步、数据上传。

#### 全量同步

包含所有任务的状态与进度。

基于`ModSystem.NetSend()`和`ModSystem.NetReceive`实现。与terraria本体相同，在特定时间点全量同步世界的信息。

#### 增量同步

仅包含单个任务/目标的状态与进度。

一般由**数据上传**触发，在数据上传后对所有客户端进行一次增量同步。

#### 数据上传

单个玩家的任务进度贡献量，比如行走的距离，消耗的物品等。

由`Main.OnTickForInternalCodeOnly`触发，每帧调用可增量同步的任务目标的对应方法，通过`PacketResolver`发送特定的数据包到服务器来发送数据。

仅发送有变化的目标数据，同时使用计时器来限制发送频率。
