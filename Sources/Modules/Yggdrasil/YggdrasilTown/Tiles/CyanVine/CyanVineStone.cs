using Terraria.Localization;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.CyanVine;

public class CyanVineStone : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<Dusts.CyanVine>();
		MineResist = 4f;
		ItemDrop = ModContent.ItemType<Items.CyanVineOre>();
		Main.tileSpelunker[Type] = true;
		var modTranslation = CreateMapEntryName();
		AddMapEntry(new Color(155, 173, 183), modTranslation);
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}
