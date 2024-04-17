using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;

public class TwilightStone_0 : ShapeDataTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileWaterDeath[Type] = false;


		DustType = ModContent.DustType<TwilightStone_Dust>();
		AddMapEntry(new Color(39, 50, 52));
	}
}