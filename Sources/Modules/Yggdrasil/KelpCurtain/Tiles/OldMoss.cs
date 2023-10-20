namespace Everglow.Yggdrasil.KelpCurtain.Tiles;

public class OldMoss : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlendAll[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DragonScaleWood>()] = true;
		Main.tileMerge[Type][ModContent.TileType<MossProneSandSoil>()] = true;
		Main.tileMerge[Type][TileID.Stone] = true;
		Main.tileMerge[TileID.Stone][Type] = true;
		Main.ugBackTransition = 1000;
		DustType = DustID.BrownMoss;
		MinPick = 50;
		HitSound = SoundID.Dig;
		AddMapEntry(new Color(68, 91, 27));
	}
	public override void RandomUpdate(int i, int j)
	{
		if (Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i + 1, j].Slope == SlopeType.Solid && Main.tile[i - 1, j].Slope == SlopeType.Solid && Main.tile[i + 2, j].Slope == SlopeType.Solid && Main.tile[i - 2, j].Slope == SlopeType.Solid &&
				Main.tile[i, j + 1].Slope == SlopeType.Solid && Main.tile[i + 1, j + 1].Slope == SlopeType.Solid && Main.tile[i - 1, j + 1].Slope == SlopeType.Solid && Main.tile[i + 2, j + 1].Slope == SlopeType.Solid && Main.tile[i - 2, j + 1].Slope == SlopeType.Solid)//树木
		{
			int MaxHeight = 0;
			for (int x = -2; x < 3; x++)
			{
				for (int y = -1; y > -8; y--)
				{
					if (j + y > 20)
					{
						if (Main.tile[i + x, j + y].HasTile || Main.tile[i + x, j + y].LiquidAmount > 3)
							return;
					}
					MaxHeight = -y;
				}
			}
			if (MaxHeight > 3)
				BuildCyatheaTree(i, j - 1, MaxHeight);
		}
	}
	public static void BuildCyatheaTree(int i, int j, int height = 0)
	{
		if (j < 30)
			return;
		int trueHeight = Main.rand.Next(3, height);

		for (int g = 0; g < trueHeight; g++)
		{
			Tile tile = Main.tile[i, j - g];
			if (g == 0)
			{
				tile.TileType = (ushort)ModContent.TileType<YggdrasilCyathea>();
				tile.TileFrameY = 0;
				tile.TileFrameX = (short)Main.rand.Next(2);
				tile.HasTile = true;
				continue;
			}
			if (g == trueHeight - 1)
			{
				tile.TileType = (ushort)ModContent.TileType<YggdrasilCyathea>();
				tile.TileFrameY = 2;
				tile.TileFrameX = (short)Main.rand.Next(2);
				tile.HasTile = true;
				continue;
			}
			tile.TileType = (ushort)ModContent.TileType<YggdrasilCyathea>();
			tile.TileFrameY = 1;
			tile.TileFrameX = (short)Main.rand.Next(4);
			tile.HasTile = true;
		}
	}
}
