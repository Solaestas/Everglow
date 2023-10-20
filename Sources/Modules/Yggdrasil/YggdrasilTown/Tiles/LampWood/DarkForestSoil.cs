using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class DarkForestSoil : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkForestGrass>()] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<StoneDragonScaleWoodDust>();
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(61, 29, 28));
	}
	public override void RandomUpdate(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		if(SubworldSystem.IsActive<YggdrasilWorld>())
		{
			if(j > 10800)
			{
				tile.TileType = (ushort)(ModContent.TileType<DarkForestGrass>());
			}
		}
	}
}
