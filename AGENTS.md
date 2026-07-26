# AGENTS.md

> 面向 AI 编码 agent 的项目指南。修改本仓库代码前请完整阅读；当你改动了本文涉及的流程/结构/约定时，请同步更新本文。
> 约定优先级：本文件 < 各目录下更具体的文档（如 `Sources/Everglow.Function/Mechanics/Mission/CONTRACTS.md`）。

## 项目概述

Everglow（流光无际）是一个 Terraria tModLoader 大型综合内容模组，C# / .NET 8 / FNA，GPL-3.0。

- 解决方案：`Everglow.sln`（仓库根），所有 C# 源码在 `Sources/` 下。
- 采用"单 Mod + 多程序集模块"架构：1 个主项目 + 2 个公共库 + 14 个内容模块，构建时合并产出**单一** `Everglow.tmod`。
- 使用自研 MSBuild 工具链（NuGet 包 `Solaestas.tModLoader.ModBuilder`）构建，**不能**用 tML 游戏内"生成 Mod"按钮构建。
- 依赖 tML 模组 `SubworldLibrary`（`Libraries/SubworldLibrary.dll`，`build.txt` 中 `modReferences = SubworldLibrary`）。
- 主 Mod 入口：`Sources/Everglow/Everglow.cs`（`public class Everglow : Mod`）。
- 官方构建文档：`Documents/源代码编译流程.md`（改动构建流程前先读它）。

## 项目结构

```
Everglow/                        仓库根
├─ Everglow.sln                  解决方案（含激活模块 + UnitTests）
├─ update.bat                    git fetch + rebase origin/master + dotnet clean + dotnet restore
├─ Sources/
│  ├─ Directory.Build.props      ★ 全局构建属性：net8.0、LangVersion=preview、
│  │                             <Modules> 激活模块列表、ModBuilder/StyleCop 包、资源 Pack 白名单
│  ├─ Directory.Build.targets    WriteResource/ReadResource：模块资源清单的写入与汇总
│  ├─ Everglow/                  ★ 主 mod 项目（程序集 Everglow，唯一 EnableModBuilder=true）
│  │  ├─ Everglow.cs             Mod 入口类（见「架构」）
│  │  ├─ build.txt               mod 元数据（version、modReferences）
│  │  ├─ Localization/           ★ 全部本地化：en-US/ zh-Hans/ templates/ 下 Mods.Everglow.*.hjson
│  │  └─ lib/                    FontStashSharp 等随 mod 分发的 dll
│  ├─ Everglow.Core/             ★ 基础库：不引用 Terraria/tML，RootNamespace = Everglow.Commons
│  ├─ Everglow.Function/         ★ 功能库：引用 tML，RootNamespace = Everglow.Commons，资源前缀 Commons
│  ├─ Everglow.UnitTests/        MSTest 测试项目
│  ├─ Everglow.Scripts/          【残留目录】只剩 bin/obj，源码已移至 Tools/Everglow.Scripts
│  └─ Modules/                   ★ 内容模块（每个是独立的 Everglow.<名>.csproj 类库）
│     ├─ Directory.Build.props   模块统一配置：自动引用 Core/Function、PathPrefix=模块名、全局 using
│     ├─ AssetReplace/  CagedDomain/  EternalResolve/  Example/  Food/  MEAC/
│     ├─ Minortopography/  Myth/  Ocean/  Plant/  PlantAndFarm/  SpellAndSkull/
│     ├─ SubSpace/  Yggdrasil/            ← 以上 14 个为激活模块
│     └─ IIID/  TwilightForest/  ZY/      ← 废弃：仅剩 bin/obj，未列入 <Modules> 与 .sln，勿改动
├─ Libraries/                    SubworldLibrary.dll（勿动）
├─ Resources/                    不打包的设计资源（图标、UI 皮肤、旧音乐，勿动）
├─ Tools/                        构建工具：Everglow.Tasks（WriteResource/ReadResource MSBuild 任务源码）、
│                                Everglow.Scripts（模块管理 CLI）、AtlasCutter、ImageRGBAConvert 等
├─ Localization/                 旧版遗留单体 hjson（非现行体系，勿新增内容）
├─ Documents/                    中文设计/流程文档
└─ .github/workflows/            构建 + AI 审查 workflow
   ├─ build-and-test.yml          .NET 构建与测试
   ├─ holistic-review.lock.yml    AI 审查(gh-aw 编译)
   └─ agentics-maintenance.yml    gh-aw 自动维护
```

## 构建

### 环境前置（缺一不可）

1. **.NET SDK 8.0+**（目标框架 net8.0，无 global.json 约束）。
2. **本机安装 tModLoader**（Steam 版；当前目标版本 2026.03 / Terraria 1.4.4.9）。
3. **`tModLoader.targets` 文件**：必须存在于本仓库或其上溯 5 级内的某个父目录，内容指向 tML 安装目录的 `tMLMod.targets`。标准布局（仓库克隆在 `Documents/My Games/Terraria/tModLoader/ModSources/Everglow`）下，`ModSources\tModLoader.targets` 已存在。缺失时构建报错 `Missing tModLoader.targets`。
4. Windows 环境；编译 `.fx` shader 依赖 XNA 运行库组件（本机装过 tML 一般已具备；CI 会显式安装 XNA Framework Redistributable）。

### 常用命令

```powershell
dotnet restore                                              # 还原依赖
dotnet build                                                # Debug 构建全解决方案（含 .tmod 打包部署）
dotnet build /p:Configuration=Release /p:WarningLevel=0     # CI 构建方式
dotnet test --verbosity normal /p:WarningLevel=0            # 运行全部单元测试（CI 方式）
dotnet test --filter "FullyQualifiedName~MissionSystem"     # 按命名空间过滤测试
.\update.bat                                                # 同步上游：git fetch + rebase + clean + restore
```

### 构建产物与部署

- 构建主项目后自动执行：编译各项目 dll → 编译 `.fx` 为 `.xnb` → 汇总模块资源清单 → 生成 **`Everglow.tmod` 直接写入 tML Mods 目录**（`Documents\My Games\Terraria\tModLoader\Mods\`）→ 自动写入 `enabled.json` 启用模组。
- VS 调试：使用 `Terraria` / `TerrariaServer` 启动配置（`Sources/Everglow/Properties/launchSettings.json`），即以 dotnet 启动 tML。
- 构建异常时先删除各项目 `bin/`、`obj/` 再重建（过期缓存是常见问题）。

### 构建系统关键点（改动前必读）

- `Sources/Directory.Build.props`：全局属性 + **`<Modules>` 模块列表**。该列表同时驱动主项目的 ProjectReference 和 `DefineConstants`（模块名即可用于 `#if Myth` 等条件编译），增删模块改这里。
- ⚠️ **CI 会在 restore 前重新生成 `Sources/Directory.Build.props`**（workflow 内 echo 覆盖，内容与仓库版本一致）。修改该文件必须同步更新 `.github/workflows/build-and-test.yml` 中的对应段落，否则 CI 与本地行为不一致。
- `Sources/Modules/Directory.Build.props`：每个模块自动获得——对 `Everglow.Core`、`Everglow.Function`、`Libraries/*.dll` 的引用；`PathPrefix = 模块名`（从项目名 `Everglow.<名>` 截取）；全局 using（`Terraria`、`Terraria.ModLoader`、`Terraria.ID`、`Microsoft.Xna.Framework(.Graphics)`、`Everglow.Commons`）。**模块 csproj 通常保持空壳**，不要重复添加上述引用与 using。
- `Sources/Directory.Build.targets` + `Tools/Everglow.Tasks`（WriteResource/ReadResource）：各模块构建时把 `Pack=true` 的 `AdditionalFiles` 清单写入主项目 obj 下的 `*.resource`，主项目构建时汇总打包。
- **资源打包是白名单制**：`.hjson/.txt/.png` 自动打包，`.ogg/.wav/.mp3/.json/.atlas/.obj/.bmp/.mapio/.ttf` 已在 props 中显式声明；新增其它资源类型必须在对应 csproj 中添加 `<AdditionalFiles Include="..." Pack="true" ModPath="%(Identity)" />`。模块资源没生效时先查这里。
- ModBuilder（NuGet `Solaestas.tModLoader.ModBuilder`）：负责 tML 引用注入、程序集 Publicize、`.fx` shader 编译（`CompileEffect=true`，输出 `.xnb` 到 `Assets\`）、`.tmod` 生成、`ModAsset` 路径类源生成（`EnablePathGenerator=true`）。
- 模组元数据：`Sources/Everglow/build.txt` + `description.txt` + `workshop_description.txt` + `icon*.png`。

### CI

`.github/workflows/build-and-test.yml`（push/PR 到 master，windows-latest）：

1. **检查分支不得落后 origin/master**（behind 即失败）——提交 PR 前先 rebase/merge 最新 master（可用 `update.bat`）。
2. 重新生成 `Directory.Build.props` → `dotnet restore` → 下载 tML 并生成 `tModLoader.targets` → 安装 XNA → `dotnet build /p:Configuration=Release /p:WarningLevel=0` → `dotnet test --verbosity normal /p:WarningLevel=0`。

PR 前请在本地跑通同样的 build 与 test 命令。CI 无 lint/format 检查。

## 单元测试

- 框架：**MSTest 3.10.2**（不是 xUnit/NUnit），测试项目 `Sources/Everglow.UnitTests`，仅引用 `Everglow.Function`（传递获得 Core 与 tML API）。该测试项目单独启用了 `<Nullable>enable</Nullable>`（主项目未启用）。
- 运行：解决方案根目录 `dotnet test`。测试项目构建后自动把 `tModLoader.dll / FNA.dll / ReLogic.dll / SubworldLibrary.dll` 复制到输出目录（csproj 中 `CopyTModLoaderFiles` 目标），保证测试可加载 Terraria 类型。
- 目录组织镜像被测项目：`Core/`（GraphicsUtils、MathUtils）、`Function/Physics/`、`Function/MissionSystem/`、`Function/UI/` 等；新增测试归入对应目录。
- `TestResults/`（根目录及测试项目下）是 MSTest 部署输出，已 gitignore，不要提交。
- **写测试的规则**：
  - 首次触碰任何 `Terraria.Main` 静态成员前，必须在 `[TestInitialize]` 中执行 `Program.SavePath = string.Empty;`（防止静态构造读取存档配置抛异常，范例见 `UnitTest.cs`）。
  - 不要 `new Main()`，不要启动图形设备/内容加载/真实游戏循环；`Main.player`、`Main.npc` 是静态数组，直接填充槽位即可；`Main.LocalPlayer` 即 `Main.player[Main.myPlayer]`。
  - 共享静态状态意味着相关测试类需避免并行执行。
  - `Player.talkNPC` 是私有 setter，优先反射赋值，**不要**调用 `SetTalkNPC`（会触碰 ShopHelper 导致 NRE）。
  - 详细指引见 `.cursor/rules/unit-testing-terraria-main.mdc`，写涉及 `Main` 的测试前必读。
  - 优先测试纯逻辑（数学、物理、状态机、序列化）；UI 绘制、贴图加载类不测。

## 架构

### 分层

```
Everglow.Core        基础层：不引用 Terraria/tML（csproj 中显式 Remove），免疫 tML 更新
   ↓                    定义接口、DI 容器、模块系统、VFX 基元、工具类
Everglow.Function    功能层：引用 tML，实现 Core 的接口
   ↓                    模板基类、VFX 管线、Hook、Netcode、Mechanics、UI 等
Modules（14 个）      内容层：游戏内容（物品/NPC/弹幕/群系/事件…）
   ↓
Everglow（主项目）    装配层：引用全部项目、合并资源、产出 Everglow.tmod
```

- Core 与 Function 的 RootNamespace **均为 `Everglow.Commons`**。
- **Core 禁止新增 tML 引用**；需要 tML 能力时，在 Core 定义接口、在 Function 实现，经 `Ins` 服务定位器取用。

### 主入口与启动流程

`Sources/Everglow/Everglow.cs` 的 `Load()`：

1. `ModIns.Mod = this`（`Everglow.Function/ModIns.cs`：静态 Mod 引用 + `OnPostSetupContent`/`OnUnload` 事件 + `PacketResolver`）。
2. `AddServices()`：向 `Ins`（`Everglow.Core/Ins.cs`，基于 `Microsoft.Extensions.DependencyInjection` 的静态服务定位器）注册单例：`Logger`、`IVisualQualityController`、`ModuleManager`、`IHookManager`、`IMainThreadContext`；**仅客户端**再注册 `GraphicsDevice`、`SpriteBatch`、`RenderTargetPool`、`IVFXManager`、`VFXBatch`（服务端 `IVFXManager` 注册为 `FakeManager`）。
3. `AddContents()`：**跨程序集自动加载内容**——`Ins.ModuleManager.CreateInstances<ModConfig>()` 注册配置；`CreateInstances<ILoadable>()`（尊重 `[Autoload]`、排除 `ModGore`）把所有 `Everglow.*` 程序集中的 tML 内容类（ModItem/ModNPC/ModProjectile/ModTile/ModSystem…）逐一 `AddContent`。**无需也不应再手动 AddContent**。
4. `PostSetupContent()`/`Unload()` 转发到 `ModIns`；`HandlePacket` 转发到 `ModIns.PacketResolver`。

### 模块系统

**编译期**（MSBuild）：
- 激活模块列表 = `Sources/Directory.Build.props` 的 `<Modules>` 属性（分号分隔），同时注入 `DefineConstants`。
- 每个模块自动引用 Core + Function + SubworldLibrary，`PathPrefix = 模块名`，并获全局 using（见「构建系统关键点」）。
- **资源运行期路径规则**：`Everglow/<PathPrefix>/<项目内相对路径>`。模块前缀即模块名（如 `Everglow/Myth/Effects/WaterDisortion`），`Everglow.Function` 的前缀是 `Commons`（`Everglow/Commons/...`）。
- 每个项目源生成自己的 **`ModAsset`** 静态路径常量类：模块内写 `ModAsset.Xxx` 指**本模块**资源；引用公共库资源写 `Commons.ModAsset.Xxx`（即 `Everglow.Commons.ModAsset`）。优先使用 `ModAsset` 而非手写路径字符串。

**运行期**（反射扫描）：
- `Everglow.Core/Modules/ModuleManager.cs`：扫描 AppDomain 中所有名称以 `Everglow.` 开头的程序集（排除主程序集），实例化所有非抽象、`Condition == true` 的 `IModule` 并 `Load()`；卸载时 `Unload()`。
- `IModule`（`Everglow.Core/Modules/IModule.cs`）：`Code` / `Name` / `Condition` / `Load()` / `Unload()`；推荐继承抽象基类 **`EverglowModule`**（`Everglow.Function/Modules/EverglowModule.cs`，命名空间 `Everglow.Commons.Modules`）。
- **`IModule` 是可选的**：只加物品/NPC 等内容类时不需要它（`AddContents()` 会自动注册）。需要挂 On_/IL_ 钩子、预加载 Effect、注册天空/事件时才写，参考 `Modules/Myth/MythModule.cs`、`Modules/Yggdrasil/YggdrasilModule.cs`。
- `[ModuleHideType]`：标注的类不出现在 `ModuleManager.Types` 中（即不被 `CreateInstances<T>()` 扫描到）。
- `DependencyGraph`（同目录）：类型级依赖拓扑排序工具。

### 模块目录约定

以 `Modules/Myth` 为范式：**先按主题/区域分子目录，再按 tML 内容类型分**：

```
Modules/Myth/
├─ MythModule.cs        IModule 入口（可选）
├─ TheFirefly/          区域（群系）：Items/ NPCs/ Projectiles/ Tiles/ Walls/ Buffs/ Dusts/
│                       Gores/ VFXs/ Backgrounds/ WorldGeneration/ Pylon/ …
├─ TheTusk/  LanternMoon/  Acytaea/  Misc/    其他区域
├─ Common/              模块内共享代码（特效、管线）
├─ Effects/             .fx shader（自动编译）
├─ Sounds/  Music/  UIImages/                 纯资源目录
└─ Everglow.Myth.csproj 通常为空壳
```

- 命名空间：`Everglow.<模块名>[.<区域>[.<类型>]]`，如 `Everglow.Myth.TheFirefly.Items`。
- 贴图/音效等资源与 .cs **同名同目录**。
- `Modules/Example` 是官方示范模块，开发新内容前先参考它（示范 Items/Projectiles/Tiles/Pylon/Elevator/Skeleton/VFX/.fx 写法，大量继承 `Everglow.Commons.Templates` 模板类）。

### 新建模块步骤

1. 创建 `Sources/Modules/<Name>/Everglow.<Name>.csproj`（空壳即可，引用由 `Modules/Directory.Build.props` 自动注入）。
2. 把 `<Name>` 加入 `Sources/Directory.Build.props` 的 `<Modules>` 属性（并同步 CI workflow 中的覆盖段落）。
3. 加入 `Everglow.sln`（可选但推荐）。
4. 命名空间用 `Everglow.<Name>`；如需钩子/预加载则新建 `<Name>Module : EverglowModule`。
5. 本地化键写入 `Sources/Everglow/Localization/{en-US,zh-Hans}/`（见「本地化」）。

### 模块依赖

- 所有模块 → `Everglow.Core` + `Everglow.Function` + `SubworldLibrary`（自动）。
- 显式模块间依赖：`Myth → SpellAndSkull`；`Ocean → Myth`；`Yggdrasil → CagedDomain, Food, SpellAndSkull, SubSpace`。
- 新增跨模块类型引用时必须在模块 csproj 中补 `ProjectReference`（不要依赖主项目传递引用"碰巧编译过"）。

### 公共层速览

**Everglow.Core**（`Everglow.Commons.*`）：

| 目录/文件 | 内容 |
|---|---|
| `Ins.cs` | 服务定位器：`Ins.Batch`、`SpriteBatch`、`Device`、`HookManager`、`Logger`、`MainThread`、`ModuleManager`、`RenderTargetPool`、`VFXManager`、`VisualQuality`；`Ins.Add<T>()`/`Get<T>()` |
| `Modules/` | `IModule`、`ModuleManager`、`DependencyGraph`、`ModuleHideTypeAttribute` |
| `Interfaces/` | `IVisual`、`IVFXManager`、`IHookManager`/`IHookHandler`、`IMainThreadContext`、`IVisualQualityController` |
| `VFX/VFXBatch.cs` | 顶点批次绘制器（配合 `Vertex/` 的 Vertex2D/3D） |
| `ObjectPool/` | `RenderTargetPool`（RenderTarget2D 池） |
| `Coroutines/` | 协程（`CoroutineManager`、`WaitForFrames`…），常用于弹幕/VFX 时序 |
| `Utilities/` | `MathUtils` 系列、`GraphicsUtils`、`MathNetUtils` |

**Everglow.Function**（`Everglow.Commons.*`，资源前缀 `Commons`）：

| 目录 | 内容 |
|---|---|
| `Templates/` | ★ 内容模板基类：`Clubs`、`StabbingSwords`、`Whips`、`Yoyos`、`Slingshots`、`TrailingProjectile`；`Furniture/` 全套家具 + 电梯；`Pylon`；`Enemies/`。**新武器/家具优先继承这些模板** |
| `VFX/` | `VFXManager`、`Pipeline`/`PostPipeline`、`Pipelines/`（Bloom/Warp/HeatMap…）、`Visuals/`（Particle、VisualNPC/Projectile…）、`Effects/`（.fx） |
| `Hooks/` | `HookManager`（经 `Ins.HookManager` 注册 On/IL hook） |
| `Netcode/` | `PacketResolver`、`IPacket`/`IPacketHandler`、内置 Packets |
| `Mechanics/` | `Mission/` 任务系统（含 README 与 CONTRACTS.md）、`Cooldown/`、`ElementalDebuff/`、`Events/` |
| `UI/`、`Menu/` | `EverglowUISystem`、Sidebar、`StringDrawer` 富文本；`EverglowModMenu` 主菜单 |
| `TileHelper/`、`CustomTiles/`、`Physics/` | TileAccessor、CableTile、MapIO；自研碰撞；MassSpring 质弹系统 |
| `Skeleton2D/`、`IIID/`、`MEAC/` | Spine 骨骼动画；Obj 模型渲染；MEAC 特效框架（注意：与废弃模块目录无关） |
| `FeatureFlags/` | `CompileTimeFeatureFlags`（编译期开关）、`EverglowConfig`（运行期） |
| `Localization/` | `ExportHjson` 等本地化导出工具（见「本地化」） |
| `DeveloperContent/` | 开发调试用物品 |

### 关键 API 速查

```csharp
Ins.VFXManager.Add(myVisual);                                  // 提交一个 IVisual 特效对象
Ins.Batch.Begin(...)/Draw(...)/End();                          // VFXBatch 顶点绘制
Ins.HookManager / Ins.MainThread / Ins.Logger                  // 钩子 / 主线程调度 / 日志
ModIns.Mod                                                     // 当前 Mod 实例
public override string Texture => ModAsset.Xxx_Mod;            // 本模块贴图（源生成常量）
Texture2D t = Commons.ModAsset.Trail_8.Value;                  // 公共库资源
ModContent.Request<Effect>("Everglow/<模块名>/Effects/Xxx");    // 手写路径时的前缀规则
```

## 代码规范

规则以根目录 `.editorconfig`（`root = true`）为准，全项目启用 `StyleCop.Analyzers.Unstable`（多数文档/排序类规则已关闭）。要点：

- **Tab 缩进（宽度 4）、LF、UTF-8、文件末尾加新行**。禁止把缩进改成空格。
- **大括号 Allman 风格**（开括号独占一行）；控制语句必须带大括号。
- 命名：类型/方法/属性/事件/枚举 PascalCase；接口 `I` 前缀；私有字段 camelCase（跟随所在文件，不加 `_`/`m_` 前缀）；常量 PascalCase。
- namespace 用**文件作用域**形式（`namespace Everglow.Food.Items;`），新文件一律如此。
- using：`System.*` 排最前、放 namespace 外；`var` 仅在类型从右值显而易见时使用。
- `ImplicitUsings` + `LangVersion=preview` + 允许 unsafe；不要求 `this.`；未使用参数会产生 warning。
- **模块项目已注入全局 using**（见「构建系统关键点」），在 `Sources/Modules/**` 下不要重复添加这些 using。
- 主项目未启用 Nullable（仅 UnitTests 启用），不要在主项目新代码中大规模引入 `?` 注解风格。
- 注释**跟随所在文件既有语言**（本仓库中英混用是现状）；`Documents/` 与 Core 的 XML 注释多为中文。
- 部分旧文件为非 UTF-8 编码（如 `.gitignore` 头部注释为 GBK），编辑时注意保留原编码，不要"修复"导致 diff 爆炸。

## tModLoader 开发约定

- **贴图路径**：不要手写路径字符串，用源生成的 `ModAsset.*` 常量（见「模块系统」）。**重名资源文件会导致 ModAsset 生成冲突**，资源文件名须（在生成范围内）唯一。
- **本地化分类**：所有 `ModItem`/`ModNPC`/`ModProjectile`/`ModBuff`/`ModBiome` 应覆写 `LocalizationCategory`，取值用 `LocalizationUtils.Categories.*` 常量（如 `Categories.MeleeWeapons`），这是本地化导出工具正确归档的前提。
- **ai 数组封装**：`NPC.ai[]`/`Projectile.ai[]` 不要散落魔数下标，用私有枚举 + 属性包装（范例：Myth 模块的 NPC 写法）。
- **多人/网络**：自定义包走 `ModIns.PacketResolver`（`Everglow.Commons.Netcode`，实现 `IPacket`/`IPacketHandler`）；状态变更记得 `netUpdate = true`；客户端专属逻辑（绘制、VFX、RenderTarget）必须 `!Main.dedServ` 守卫。
- **基类优先**：新武器/家具优先继承 `Everglow.Commons.Templates` 的模板类（参考 Example 模块），不要从零实现已有模式。
- **FeatureFlags**：实验性功能用 `CompileTimeFeatureFlags`（编译期）或 `EverglowConfig`（运行期）控制，不要直接提交常开的半成品功能。
- 大文件用 `partial` 拆分；变体命名习惯用下划线后缀（如 `Stove_Item`、`TsunamiShark_missile`），跟随同模块惯例。

## 本地化

- 全部集中在 `Sources/Everglow/Localization/`：`en-US/`、`zh-Hans/`（正式语言，**两份都要维护**）与 `templates/`（模板，勿直接填内容）。
- 文件按类别拆分：`Mods.Everglow.Items.Weapons.Melee.hjson`、`Mods.Everglow.NPCs.hjson`……内容类通过 `LocalizationCategory` 归入对应文件。
- **不要手编分类 hjson 文件新增键**：用游戏内开发者物品 `OutputLocalizationHjsonItem`（源码在 `Sources/Everglow.Function/Localization/ExportHjson.cs`）运行导出，它会增量补齐缺失键（只加不删）。
- **本地化键只增不删**；重命名内容类（internal name）会连锁破坏 hjson 键、`ItemID/NPCID.Search` 与存档兼容，必须全局搜索引用后再改。
- 代码中取文本：`Language.GetTextValue("Mods.Everglow.Common.*")`；瓷砖地图名用 `this.GetLocalization("MapEntry" + option)`。
- 仓库根的 `Localization/en-US_Mods.Everglow.hjson` 是旧格式残留，新内容不要往里加。

## 常见陷阱

- **CI 覆盖 `Sources/Directory.Build.props`**：改它必须同步 workflow（见「CI」）。
- **CI 要求分支不落后 origin/master**：PR 前先同步 master。
- `Sources/Everglow.Function/Test/*.*` 被 .gitignore 忽略——放在这里的内容不会入库，别误用。
- 服务端没有图形服务（`SpriteBatch`/`Device`/`RenderTargetPool`/`VFXBatch` 等仅客户端注册），跨端代码注意 `Main.dedServ`。
- `Modules/IIID`、`TwilightForest`、`ZY` 与 `Sources/Everglow.Scripts` 是废弃残留目录，不参与编译，**不要**在其中新增代码或"复活"它们（注意 `Everglow.Function` 内的 `IIID/`、`MEAC/` 命名空间目录是正常共享代码，与废弃模块无关）。
- 模块资源没生效：先检查资源类型是否在 Pack 白名单内（见「构建系统关键点」）。
- 构建报错 `Missing tModLoader.targets`：仓库上溯 5 级内缺少指向 tML 的 targets 文件（见「环境前置」）。
- tML 升级后优先检查 `Everglow.Function` 的编译错误（Core 被隔离正是为此）。

## 提交与 PR 规范

- master 受保护：走分支 + PR，需其他开发者审查后合并。
- 分支命名：`<模块名>/<简述>`（如 `Myth/newRope`、`common/npc_mission`、`hotfix/Localization`）。
- 提交信息：简短英文祈使句；可用 Conventional Commits 前缀（`fix(Myth): …`、`chore(ci): …`）。
- PR 前自查：`dotnet build` 与 `dotnet test` 本地通过；分支已同步最新 master；无二进制/生成物误入暂存区。

## Agent 行为准则

1. **最小改动**：修 bug 就修 bug，不顺手重构；不做与任务无关的批量格式化（历史文件风格混杂，批量格式化会造成巨大 diff，禁止）。
2. **二进制/美术资源只读**：`Resources/`、`Libraries/`、各模块的 `.png/.obj/.ttf/.atlas/.xnb`、`Sources/Everglow/icon*.png`、`Tools/*.exe|dll` 一律不修改；需要新贴图时告知用户，不要生成图片占位。
3. **结构性修改先确认**：`Sources/Directory.Build.props`、各 csproj、`Everglow.sln` 的结构性改动先向用户说明并确认。
4. **不擅自 git 操作**：除非用户明确要求，不执行 commit/push/rebase 等任何 git 变更。
5. **本地化键只增不删**；改 internal name 必须连带更新 hjson 与所有引用（见「本地化」）。
6. **优先阅读再动手**：改动某模块/系统前，先看其目录下的 README/CONTRACTS/TODO（如 `Mechanics/Mission/`、`Modules/Food/FoodModule.md`）与 `Documents/` 下的中文文档。
7. **验证**：改完必须 `dotnet build`；涉及 Function/UnitTests 逻辑时加 `dotnet test`。无法本地验证的 tML 运行时行为，在总结中明确说明未验证项。
8. **不新建文档文件**（md 等），除非用户要求；代码自解释优先。
9. 遇到构建/测试问题先查 `Documents/源代码编译流程.md` 与 `.cursor/rules/*.mdc`，再向用户提问。

## 参考文档索引

| 文档 | 内容 |
|---|---|
| `Documents/源代码编译流程.md` | 自定义构建管线详解 |
| `Documents/子世界.md`、`Documents/ILDoc/` | 子世界与 IL 修改文档 |
| `.cursor/rules/unit-testing-terraria-main.mdc` | 单元测试实施指南（写测试前必读） |
| `Sources/Everglow.Function/Mechanics/Mission/{README,CONTRACTS}.md` | Mission 任务系统契约 |
| `Sources/Everglow.Core/Utilities/Utils.md`、`Sources/Everglow.Function/VFX/VFX.md`、`Sources/Modules/Food/FoodModule.md` | 子系统文档 |
| `Sources/Modules/Example/` | 官方示范模块（新内容写法参考） |
| tModLoader 官方文档与 ExampleMod | tML API 用法的最终参考 |
