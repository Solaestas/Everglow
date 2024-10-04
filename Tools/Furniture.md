## 家具生成工具使用说明

### 准备工作

确保安装了 node.js

### 使用说明

使用命令行执行 `node FurnitureGenerator.mjs '$1' '$2' '$3' '$4' '$5' '$6' '$7'`;

其中，七个参数分别表示：

1. 系列家具名称，例如 `Heatproof`，将会生成诸如 `HeatproofBed` 等名称
2. Tile 贴图文件夹，包含了家具的 Tile 贴图，对命名有要求，要求名称必须以 $1 开头，然后后面紧跟家具类型，后面名称随意，例如床就需要是 `HeatproofBed.png`，或者 `HeatproofBed_HighLight.png` 也可以
3. Item 贴图文件夹，包含了家具的 Item 贴图，命名要求同 $2
4. Tile 的命名空间，要求以 `Tiles.Furnitures` 结尾，例如 `Everglow.MyFurnitures.Tiles.Furnitures`
5. Item 的命名空间，命名要求与 $4 类似，不过 `Tiles` 要换成 `Items`
6. Tile 输出文件夹
7. Item 输出文件夹

运行后如果没有报错，说明运行成功。运行时会自动检测贴图文件包含了哪些类型的家具，如果出现了不支持的家具类型，会抛出警告

注意，生成之后的家具代码中会有一些需要自己设置的内容，例如 DustType 等，这些地方会给出报错，需要填写后再生成。同时也不会包含合成表，也需要自己填写。

### 使用样例

```shell
node FurnitureGenerator.mjs 'Heatproof' 'E:/Tiles/Heatproof' 'E:/Items/Heatproof' 'Everglow.MyFurnitures.Tiles.Furnitures' 'Everglow.Myfurnitures.Items.Furnitures' 'C:\Documents\My Games\Terraria\tModLoader\ModSources\Everglow\Sources\MyFurnitures\Tiles\Furnitures' 'C:\Documents\My Games\Terraria\tModLoader\ModSources\Everglow\Sources\MyFurnitures\Items\Furnitures'
```
