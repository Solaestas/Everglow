using Terraria.ObjectData;

namespace Everglow.TwilightForest.Tiles;

public class TwilightGrassBlock : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileFrameImportant[Type] = false;
		Main.tileMergeDirt[Type] = true;
		TileID.Sets.Grass[Type] = true;
		TileID.Sets.ReplaceTileBreakDown[Type] = false;
		TileID.Sets.ReplaceTileBreakUp[Type] = false;

		Main.tileBlockLight[Type] = true;
		DustType = DustID.Dirt;
		ItemDrop = ItemID.DirtBlock;
		AddMapEntry(new Color(33, 140, 141));
	}
	public override void RandomUpdate(int i, int j)
	{
		Tile ThisTile = Main.tile[i, j];
		for (int d = 0; d < 32; d++)
		{
			int x = Main.rand.Next(-12, 13);
			int y = Main.rand.Next(-12, 13);
			var tile = Main.tile[i + x, j + y];
			if (tile.TileType == TileID.Grass && tile.HasTile)
			{
				tile.TileType = ThisTile.TileType;
				//WorldGen.TileRunner(i + x, j + y, 2, 1, ThisTile.TileType);
			}
		}//扩散
		var UpTile = Main.tile[i, j - 1];
		var DownTile = Main.tile[i, j + 1];
		var LeftTile = Main.tile[i - 1, j];
		var RightTile = Main.tile[i + 1, j];
		if (IsDirt(UpTile) && IsDirt(DownTile) && IsDirt(LeftTile) && IsDirt(RightTile))
			ThisTile.TileType = TileID.Dirt;
	}
	private bool IsDirt(Tile tile)
	{
		if (tile.TileType == TileID.Dirt && tile.HasTile)
			return true;
		return false;
	}
}