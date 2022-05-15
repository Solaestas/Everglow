# 综述
负责修改世界选择的UI样式，允许通过AddWorld添加虚假的UI，并且通过假UI进入游戏时调用自定义的世界生成


## System
### WorldSystem
WorldSystem继承自IModule会被自动加载
通过对Terraria.Main.LoadWorlds于Terraria.GameContent.UI.Elements.UIWorldListItem.ctor添加On
来实现添加UI以及对UI的操控
静态变量CurrentWorld表示当前所在世界类型对应的World类变量，联机同步已经完成

## 基类
### World
World继承自IModule会被自动加载，通过这个实例来调用相应的虚函数
World里面定义了世界的名词，版本，存档文件，世界生成方法
原版世界名为"Terraria"

## Tip
世界生成不止要包含Tile的修改，还有给Main的地下层等一系列的参数赋值（并且我还没有找全，导致直接背景会炸）
存档版本号为ulong，高位用来存放uint类型的WorldName通过MD5所得的数，低位用来存放uint类型的世界版本号
