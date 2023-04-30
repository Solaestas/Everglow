namespace Everglow.Myth.TheFirefly.Tiles;

public class DarkCocoonSpawn : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkCocoonMoss>()] = true;
		MinPick = 17500;
		DustType = 191;
		ItemDrop = ModContent.ItemType<Items.DarkCocoon>();
		AddMapEntry(new Color(17, 16, 17));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}