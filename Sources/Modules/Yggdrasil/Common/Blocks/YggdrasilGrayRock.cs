namespace Everglow.Yggdrasil.Common.Blocks;

public class YggdrasilGrayRock : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<YggdrasilTown.Tiles.StoneScaleWood>()] = true;
		Main.tileMerge[(ushort)ModContent.TileType<YggdrasilTown.Tiles.StoneScaleWood>()][Type] = true;

		Main.tileMerge[Type][(ushort)ModContent.TileType<YggdrasilTown.Tiles.LampWood.DarkForestSoil>()] = true;
		Main.tileMerge[(ushort)ModContent.TileType<YggdrasilTown.Tiles.LampWood.DarkForestSoil>()][Type] = true;

		Main.tileMerge[Type][(ushort)ModContent.TileType<YggdrasilTown.Tiles.LampWood.DarkForestGrass>()] = true;
		Main.tileMerge[(ushort)ModContent.TileType<YggdrasilTown.Tiles.LampWood.DarkForestGrass>()][Type] = true;

		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;

		DustType = DustID.BorealWood;
		MinPick = 180;
		HitSound = SoundID.Dig;
		AddMapEntry(new Color(96, 97, 99));
	}
	public override void PostSetupTileMerge()
	{
		base.PostSetupTileMerge();
	}
}
