using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.CagedDomain.Tiles;

public class PierWithSlabsTop : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileSolid[Type] = true;
		Main.tileBlockLight[Type] = false;
		DustType = DustID.Stone;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 8;
		TileObjectData.newTile.Width = 13;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16
		};
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
		TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)Type, (int)ModContent.TileType<PierWithSlabs>() };
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.newTile.Origin = new Point16(1, 0);
		TileObjectData.addTile(Type);
		// Etc
		AddMapEntry(new Color(86, 86, 86));
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}
}
