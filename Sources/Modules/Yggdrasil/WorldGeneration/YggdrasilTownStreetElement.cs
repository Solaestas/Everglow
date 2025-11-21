using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.WorldGeneration;

/// <summary>
/// 从左到右摆放的街道元件
/// </summary>
public class YggdrasilTownStreetElement
{
	/// <summary>
	/// 元件宽度
	/// </summary>
	public virtual int Width { get; set; }

	/// <summary>
	/// 元件高度
	/// </summary>
	public virtual int Height { get; set; }

	/// <summary>
	/// 稀有度,1为最常见的标准值,根据稀有度的倒数算取该建筑物被构建的可能性权重
	/// 非常不建议小于1,否则降低其它建筑物的生成数量
	/// </summary>
	public virtual float Rare { get; set; }

	/// <summary>
	/// 该种元件最短间距,一般情况下最好大于宽度
	/// </summary>
	public virtual int MinimumDistance { get; set; }

	/// <summary>
	/// 摆放冷却
	/// </summary>
	public int Cooling;

	public virtual void Build(ref int x, int y)
	{
		x += Width;
		Cooling += MinimumDistance;
	}

	public void Update(int times = 1)
	{
		Cooling -= times;
		if (Cooling <= 0)
		{
			Cooling = 0;
		}
	}
}

/// <summary>
/// 路灯柱
/// </summary>
public class Lamppost : YggdrasilTownStreetElement
{
	public override int Width => 1;

	public override int Height => 6;

	public override int MinimumDistance => 8;

	public override float Rare => 1.0f;

	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y - Height, 1, 6, TileID.Lampposts);
		base.Build(ref x, y);
	}
}

/// <summary>
/// 长椅
/// </summary>
public class Bench : YggdrasilTownStreetElement
{
	public override int Width => 3;

	public override int Height => 2;

	public override int MinimumDistance => 8;

	public override float Rare => 1.0f;

	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y - Height, 3, 2, TileID.Benches);
		base.Build(ref x, y);
	}
}

/// <summary>
/// 板条箱
/// </summary>
public class Crate : YggdrasilTownStreetElement
{
	public override int Width => 2;

	public override int Height => 2;

	public override int MinimumDistance => 8;

	public override float Rare => 3.0f;

	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y - Height, 2, 2, TileID.FishingCrate);
		base.Build(ref x, y);
	}
}

/// <summary>
/// 三个板条箱
/// </summary>
public class ThreeCrate : YggdrasilTownStreetElement
{
	public override int Width => 4;

	public override int Height => 4;

	public override int MinimumDistance => 16;

	public override float Rare => 12.0f;

	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y - 2, 2, 2, TileID.FishingCrate);
		TileUtils.PlaceFrameImportantTiles(x + 2, y - 2, 2, 2, TileID.FishingCrate);
		TileUtils.PlaceFrameImportantTiles(x + 1, y - Height, 2, 2, TileID.FishingCrate);
		base.Build(ref x, y);
	}
}

/// <summary>
/// 中式民居
/// </summary>
public class FolkHouseofChineseStyle : YggdrasilTownStreetElement
{
	public override int Width => 28;

	public override int Height => 11;

	public override int MinimumDistance => 32;

	public override float Rare => 12.0f;

	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		if (WorldGen.genRand.NextBool(2))
		{
			QuickBuild(x, y - Height, ModAsset.MapIOs_1FolkHouseofChineseStyleTypeB28x11_Path);
		}
		else
		{
			QuickBuild(x, y - Height, ModAsset.MapIOs_1FolkHouseofChineseStyleTypeA28x11_Path);
		}
		base.Build(ref x, y);
	}
}

/// <summary>
/// 木石结构民居
/// </summary>
public class FolkHouseofWoodStoneStruture : YggdrasilTownStreetElement
{
	public override int Width => 28;

	public override int Height => 11;

	public override int MinimumDistance => 32;

	public override float Rare => 12.0f;

	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		switch (WorldGen.genRand.Next(4))
		{
			case 0:
				QuickBuild(x, y - Height, ModAsset.MapIOs_2FolkHouseofWoodAndStoneStrutureTypeA28x11_Path);
				break;
			case 1:
				QuickBuild(x, y - Height, ModAsset.MapIOs_2FolkHouseofWoodAndStoneStrutureTypeB28x11_Path);
				break;
			case 2:
				QuickBuild(x, y - Height, ModAsset.MapIOs_2FolkHouseofWoodStoneStrutureTypeA28x11_Path);
				break;
			case 3:
				QuickBuild(x, y - Height, ModAsset.MapIOs_2FolkHouseofWoodStoneStrutureTypeB28x11_Path);
				break;
		}
		base.Build(ref x, y);
	}
}

/// <summary>
/// Smithy建筑
/// </summary>
public class SmithyType : YggdrasilTownStreetElement
{
	public override int Width => 22;

	public override int Height => 8;

	public override int MinimumDistance => 26;

	public override float Rare => 12.0f;

	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		if (WorldGen.genRand.NextBool(2))
		{
			QuickBuild(x, y - Height, ModAsset.MapIOs_3SmithyTypeA22x8_Path);
		}
		else
		{
			QuickBuild(x, y - Height, ModAsset.MapIOs_3SmithyTypeB22x8_Path);
		}
		base.Build(ref x, y);
	}
}

/// <summary>
/// 木结构民居
/// </summary>
public class FolkHouseofWoodStruture : YggdrasilTownStreetElement
{
	public override int Width => 22;

	public override int Height => 10;

	public override int MinimumDistance => 26;

	public override float Rare => 12.0f;

	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		switch (WorldGen.genRand.Next(4))
		{
			case 0:
				QuickBuild(x, y - Height, ModAsset.MapIOs_4FolkHouseofWoodStrutureTypeA22x10_Path);
				break;
			case 1:
				QuickBuild(x, y - Height, ModAsset.MapIOs_4FolkHouseofWoodStrutureTypeB22x10_Path);
				break;
			case 2:
				QuickBuild(x, y - Height, ModAsset.MapIOs_4FolkHouseofWoodStrutureTypeC22x10_Path);
				break;
			case 3:
				QuickBuild(x, y - Height, ModAsset.MapIOs_4FolkHouseofWoodStrutureTypeD22x10_Path);
				break;
		}
		base.Build(ref x, y);
	}
}

/// <summary>
/// 木结构双层民居
/// </summary>
public class TwoStoriedFolkHouse : YggdrasilTownStreetElement
{
	public override int Width => 23;

	public override int Height => 13;

	public override int MinimumDistance => 27;

	public override float Rare => 12.0f;

	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		switch (WorldGen.genRand.Next(3))
		{
			case 0:
				QuickBuild(x, y - Height, ModAsset.MapIOs_5TwoStoriedFolkHouseTypeA23x13_Path);
				break;
			case 1:
				QuickBuild(x, y - Height, ModAsset.MapIOs_5TwoStoriedFolkHouseTypeB23x13_Path);
				break;
			case 2:
				QuickBuild(x, y - Height, ModAsset.MapIOs_5TwoStoriedFolkHouseTypeC23x13_Path);
				break;
		}
		base.Build(ref x, y);
	}
}

/// <summary>
/// 铁匠铺
/// </summary>
public class BlacksmithForge : YggdrasilTownStreetElement
{
	public override int Width => 23;

	public override int Height => 13;

	public override int MinimumDistance => 27;

	public override float Rare => 12.0f;

	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		base.Build(ref x, y);
	}
}