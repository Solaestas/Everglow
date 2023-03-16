namespace Everglow.Yggdrasil.KelpCurtain.Tiles;

public class YggdrasilDirt : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlendAll[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DragonScaleWood>()] = true;
		Main.tileMerge[Type][ModContent.TileType<OldMoss>()] = true;
		Main.tileMerge[Type][TileID.Stone] = true;
		DustType = DustID.Dirt;
		MinPick = 50;
		HitSound = SoundID.Dig;
		ItemDrop = ModContent.ItemType<Items.YggdrasilDirt>();
		AddMapEntry(new Color(53, 29, 26));
	}
	public override bool CanExplode(int i, int j)
	{
		return true;
	}
}
