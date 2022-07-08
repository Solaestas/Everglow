namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class DarkCocoon : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            MinPick = 175;
            DustType = 191;
            ItemDrop = ModContent.ItemType<Items.DarkCocoon>();
            AddMapEntry(new Color(17, 16, 17));
        }
        public override void RandomUpdate(int i, int j)
        {
            if(Main.rand.NextBool(6))
            {
                if (!Main.tile[i, j].HasTile && !Main.tile[i + 1, j].HasTile && !Main.tile[i - 1, j].HasTile)
                {
                    if (!Main.tile[i, j].IsHalfBlock && !Main.tile[i + 1, j].IsHalfBlock && !Main.tile[i - 1, j].IsHalfBlock && !Main.tile[i + 2, j].IsHalfBlock && !Main.tile[i - 2, j].IsHalfBlock)
                    {
                        for (int x = -2; x < 3; x++)
                        {
                            for (int y = -12; y < 0; y++)
                            {
                                if (Main.tile[i + x, j + y].HasTile || Main.tile[i + x, j + y].LiquidAmount > 3)
                                {
                                    return;
                                }
                            }
                        }
                        short FrX = (short)(Main.rand.Next(0, 16));
                        for (int y = -8; y < 0; y++)
                        {
                            Tile tile = Main.tile[i, j + y];
                            tile.TileType = (ushort)ModContent.TileType<Tiles.FireflyTree>();
                            tile.HasTile = true;
                            tile.TileFrameX = (short)(FrX * 256);
                            tile.TileFrameY = (short)((y + 8) * 16);
                        }
                    }
                }
            }
            if (!Main.tile[i, j - 1].HasTile && !Main.tile[i + 1, j - 1].HasTile && !Main.tile[i - 1, j - 1].HasTile)
            {
                if (Main.rand.NextBool(8))
                {
                    switch (Main.rand.Next(1, 4))
                    {
                        case 1:
                            WorldGen.PlaceTile(i, j - 2, (ushort)ModContent.TileType<Tiles.BlackStarShrubSmall>());
                            short numa = (short)(Main.rand.Next(0, 6) * 48);
                            Main.tile[i, j - 2].TileFrameX = numa;
                            Main.tile[i, j - 1].TileFrameX = numa;
                            break;
                        case 2:
                            WorldGen.PlaceTile(i, j - 2, (ushort)ModContent.TileType<Tiles.BlackStarShrubSmall>());
                            short num = (short)(Main.rand.Next(0, 6) * 48);
                            Main.tile[i, j - 2].TileFrameX = num;
                            Main.tile[i, j - 1].TileFrameX = num;
                            break;
                        case 3:
                            WorldGen.PlaceTile(i, j - 3, (ushort)ModContent.TileType<Tiles.BlackStarShrub>());
                            short num1 = (short)(Main.rand.Next(0, 6) * 72);
                            Main.tile[i, j - 3].TileFrameX = num1;
                            Main.tile[i, j - 2].TileFrameX = num1;
                            Main.tile[i, j - 1].TileFrameX = num1;
                            break;
                    }
                }
            }
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}