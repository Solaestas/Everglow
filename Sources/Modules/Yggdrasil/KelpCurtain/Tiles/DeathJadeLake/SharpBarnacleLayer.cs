using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

public class SharpBarnacleLayer : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;
		TileID.Sets.TouchDamageImmediate[Type] = 40;
		DustType = ModContent.DustType<SharpBarnacle_Dust>();
		AddMapEntry(new Color(139, 158, 181));
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		Tile tile = Main.tile[i, j];
		if (j % 2 == 0 && tile.TileFrameY < 90)
		{
			tile.TileFrameY += 90;
		}
		if (j % 2 == 1 && tile.TileFrameY >= 90)
		{
			tile.TileFrameY -= 90;
		}
		base.NearbyEffects(i, j, closer);
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}
}