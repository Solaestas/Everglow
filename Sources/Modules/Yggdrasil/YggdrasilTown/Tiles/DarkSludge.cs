using Terraria;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class DarkSludge : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<StoneScaleWood>()] = true;
		Main.tileMerge[(ushort)ModContent.TileType<StoneScaleWood>()][Type] = true;
		Main.tileBlockLight[Type] = true;

		DustType = DustID.BorealWood;
		MinPick = 250;
		HitSound = SoundID.Dig;
		AddMapEntry(new Color(31, 26, 45));

	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
	public override void FloorVisuals(Player player)
	{
		player.velocity *= 0.1f;
		//player.position += new Vector2(0, player.gravDir * 15f);
		player.AddBuff(BuffID.Shimmer, 60);
	}
	public override void RandomUpdate(int i, int j)
	{
		Tile thisTile = Main.tile[i, j];
		updateTileFrameX(thisTile);
		if(i >= 24 && i <= Main.maxTilesX - 24)
		{
			for(int x = -5;x <=5;x+=2)
			{
				Tile otherTile = Main.tile[i + x, j];
				updateTileFrameX(otherTile);
			}
		}
		void updateTileFrameX(Tile tile)
		{
			if (tile.TileFrameY == 0)
			{
				if (tile.TileFrameX >= 18 && tile.TileFrameX <= 54)
				{
					tile.TileFrameX = (short)(Main.rand.Next(1, 4) * 18);
				}
			}
		}
	}
}
