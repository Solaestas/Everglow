namespace Everglow.Sources.Modules.TwilightForestModule.Tiles
{
    public class TwilightGrassBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[TileID.Grass][Type] = true;
            Main.tileMerge[TileID.CorruptGrass][Type] = true;
            Main.tileMerge[TileID.CrimsonGrass][Type] = true;
            Main.tileMerge[TileID.HallowedGrass][Type] = true;
            Main.tileMerge[TileID.JungleGrass][Type] = true;
            Main.tileMerge[TileID.MushroomGrass][Type] = true;

            Main.tileMerge[Type][TileID.Grass] = true;
            Main.tileMerge[Type][TileID.CorruptGrass] = true;
            Main.tileMerge[Type][TileID.CrimsonGrass] = true;
            Main.tileMerge[Type][TileID.HallowedGrass] = true;
            Main.tileMerge[Type][TileID.JungleGrass] = true;
            Main.tileMerge[Type][TileID.MushroomGrass] = true;
            Main.tileBlockLight[Type] = true;
            DustType = DustID.Dirt;
            ItemDrop = ItemID.DirtBlock;
            AddMapEntry(new Color(33, 140, 141));
        }
        public override void RandomUpdate(int i, int j)
        {
            var ThisTile = Main.tile[i, j];
            for(int d = 0;d < 32;d++)
            {
                int x = Main.rand.Next(-12, 13);
                int y = Main.rand.Next(-12, 13);
                var tile = Main.tile[i + x, j + y];
                if (tile.TileType == TileID.Grass)
                {
                    int upTileType = Main.tile[i + x, j + y - 1].TileType;
                    int[] Trees = { TileID.Trees, TileID.VanityTreeYellowWillow, TileID.PineTree, TileID.MushroomTrees, TileID.VanityTreeYellowWillow };
                    if (upTileType == 0 || upTileType == TileID.Grass)
                    {
                        Main.NewText(5);
                        WorldGen.TileRunner(i, j, 5, 1, ThisTile.TileType);
                    }   
                }
            }//扩散
            var UpTile = Main.tile[i, j - 1];
            var DownTile = Main.tile[i, j + 1];
            var LeftTile = Main.tile[i - 1, j];
            var RightTile = Main.tile[i + 1, j];
            if(IsDirt(UpTile) && IsDirt(DownTile) && IsDirt(LeftTile) && IsDirt(RightTile))
            {
                ThisTile.TileType = TileID.Dirt;
            }//如果嵌入土中就死亡
        }
        private bool IsDirt(Tile tile)
        {
            if(tile.TileType == TileID.Dirt && tile.HasTile)
            {
                return true;
            }
            return false;
        }
    }
}