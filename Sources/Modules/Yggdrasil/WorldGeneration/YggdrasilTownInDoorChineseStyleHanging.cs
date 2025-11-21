using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;
namespace Everglow.Yggdrasil.WorldGeneration;
/// <summary>
/// 筒状吊灯组
/// </summary>
public class CylinderChandelierGroup : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 1.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		//TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, ModContent.TileType<HangingSkyLantern>());
		base.Build(ref x, y);
	}
}
/// <summary>
/// 六角宫灯
/// </summary>
public class HexagonalCeilingChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 1.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		//TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, ModContent.TileType<HexagonalCeilingLamp>());
		base.Build(ref x, y);
	}
}
/// <summary>
/// 王朝吊灯
/// </summary>
public class DynasticChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 1.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, TileID.Chandeliers, 0, 1188);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 金吊灯
/// </summary>
public class GoldenChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 30.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, TileID.Chandeliers, 0, 1080);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 水晶吊灯
/// </summary>
public class CrystalChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 12.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, TileID.Chandeliers, 108, 0);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 金属吊灯
/// </summary>
public class MetalChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 2.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, TileID.Chandeliers, 0, 54 * WorldGen.genRand.Next(6));
		base.Build(ref x, y);
	}
}
/// <summary>
/// 竹吊灯
/// </summary>
public class BambooChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 6.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, TileID.Chandeliers, 108, 432);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 红木吊灯
/// </summary>
public class RichMahoganyChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 6.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, TileID.Chandeliers, 0, 648);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 棕榈木吊灯
/// </summary>
public class PalmChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 7.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, TileID.Chandeliers, 0, 1242);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 绿地牢吊灯
/// </summary>
public class GreenDungeonChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 13.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, TileID.Chandeliers, 0, 1242);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 恶魔吊灯
/// </summary>
public class EvilChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 10.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, TileID.Chandeliers, 0, 1728);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 花岗岩吊灯
/// </summary>
public class GraniteChandelier : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 10.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 3, 3, TileID.Chandeliers, 0, 1890);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 中式灯笼
/// </summary>
public class ChineseLantern : YggdrasilTownStreetElement
{
	public override int Width => 2;
	public override int Height => 2;
	public override int MinimumDistance => 6;
	public override float Rare => 4.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 2, 2, TileID.ChineseLanterns);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 迪斯科球
/// </summary>
public class DiscoBall : YggdrasilTownStreetElement
{
	public override int Width => 2;
	public override int Height => 2;
	public override int MinimumDistance => 6;
	public override float Rare => 14.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 2, 2, TileID.DiscoBall);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 萤火虫瓶
/// </summary>
public class FireflyBottle : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 14.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.FireflyinaBottle);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 萤光虫瓶
/// </summary>
public class LightningBugBottle : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 14.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.LightningBuginaBottle);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 王朝灯笼
/// </summary>
public class DynasticLantern : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 3.0f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.HangingLanterns, 0, 936);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 3种金属灯笼
/// </summary>
public class MetalLantern : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 5.2f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.HangingLanterns, 0, 36 * WorldGen.genRand.Next(2, 5));
		base.Build(ref x, y);
	}
}
/// <summary>
/// 符文灯笼
/// </summary>
public class SpellLantern : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 7f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.HangingLanterns, 0, 180);
		base.Build(ref x, y);
	}
}
/// <summary>
/// 吊碗灯笼
/// </summary>
public class BowlLantern : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 5.6f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.HangingLanterns, 0, 216);
		base.Build(ref x, y);
	}
}
public class GlassLantern : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 8f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.HangingLanterns, 0, 252);
		base.Build(ref x, y);
	}
}
public class FleshLantern : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 12.4f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.HangingLanterns, 0, 828);
		base.Build(ref x, y);
	}
}
public class LivingWoodLantern : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 14.3f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.HangingLanterns, 0, 792);
		base.Build(ref x, y);
	}
}
public class CrystalLantern : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 24.6f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.HangingLanterns, 0, 1332);
		base.Build(ref x, y);
	}
}
public class BambooLantern : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 4.8f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.HangingLanterns, 0, 1620);
		base.Build(ref x, y);
	}
}
public class LavaFlyBottle : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 22.8f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.LavaflyinaBottle);
		base.Build(ref x, y);
	}
}
public class SoulBottle : YggdrasilTownStreetElement
{
	public override int Width => 1;
	public override int Height => 2;
	public override int MinimumDistance => 3;
	public override float Rare => 45f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 1, 2, TileID.SoulBottles, 0, WorldGen.genRand.Next(6) * 36);
		base.Build(ref x, y);
	}
}
public class BurningBowl : YggdrasilTownStreetElement
{
	public override int Width => 2;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 8f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 2, 3, 592);
		base.Build(ref x, y);
	}
}
public class PlantBowl : YggdrasilTownStreetElement
{
	public override int Width => 2;
	public override int Height => 3;
	public override int MinimumDistance => 6;
	public override float Rare => 5f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		TileUtils.PlaceFrameImportantTiles(x, y, 2, 3, 591, 36 * WorldGen.genRand.Next(9));
		base.Build(ref x, y);
	}
}
public class EmptyAnchoredTop : YggdrasilTownStreetElement
{
	public override int Width => 3;
	public override int Height => 1;
	public override int MinimumDistance => 0;
	public override float Rare => 0.2f;
	public override void Build(ref int x, int y)
	{
		if (Cooling > 0)
		{
			return;
		}
		base.Build(ref x, y);
	}
}