using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class DarkForestSoil : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkForestGrass>()] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<DarkForestSoil_Dust>();
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(63, 53, 62));
	}
	public override void RandomUpdate(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		if (Main.rand.NextBool(25))
		{
			for (int x = -2; x < 3; x++)
			{
				for (int y = -2; y < 3; y++)
				{
					Tile checkTile = Main.tile[i + x, j + y];
					if (checkTile.TileType == (ushort)(ModContent.TileType<DarkForestGrass>()))
					{
						tile.TileType = (ushort)(ModContent.TileType<DarkForestGrass>());
						return;
					}
				}
			}
		}
	}
}
