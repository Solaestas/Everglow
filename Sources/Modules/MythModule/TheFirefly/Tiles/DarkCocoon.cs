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
            if (Main.rand.NextBool(6))
            {

                if (!Main.tile[i, j].IsHalfBlock && !Main.tile[i + 1, j].IsHalfBlock && !Main.tile[i - 1, j].IsHalfBlock && !Main.tile[i + 2, j].IsHalfBlock && !Main.tile[i - 2, j].IsHalfBlock)
                {
                    for (int x = -2; x < 3; x++)
                    {
                        for (int y = -2; y < 0; y++)
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
            if (!Main.tile[i, j - 1].HasTile && !Main.tile[i + 1, j - 1].HasTile && !Main.tile[i - 1, j - 1].HasTile && !Main.tile[i, j].IsHalfBlock && !Main.tile[i - 1, j].IsHalfBlock && !Main.tile[i + 1, j].IsHalfBlock)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -3; y < 4; y++)
                    {
                        if (Main.tile[i + x, j + y].LiquidAmount > 3)
                        {
                            return;
                        }
                    }
                }
                if (Main.rand.NextBool(8))
                {
                    Tile t1 = Main.tile[i, j - 1];
                    Tile t2 = Main.tile[i, j - 2];
                    Tile t3 = Main.tile[i, j - 3];
                    switch (Main.rand.Next(1, 4))
                    {
                        case 1:
                            t1.TileType = (ushort)ModContent.TileType<Tiles.BlackStarShrubSmall>();
                            t2.TileType = (ushort)ModContent.TileType<Tiles.BlackStarShrubSmall>();
                            t1.HasTile = true;
                            t2.HasTile = true;
                            short numa = (short)(Main.rand.Next(0, 6) * 48);
                            t1.TileFrameX = numa;
                            t2.TileFrameX = numa;
                            t1.TileFrameY = 16;
                            t2.TileFrameX = 0;
                            break;
                        case 2:
                            t1.TileType = (ushort)ModContent.TileType<Tiles.BlackStarShrubSmall>();
                            t2.TileType = (ushort)ModContent.TileType<Tiles.BlackStarShrubSmall>();
                            t1.HasTile = true;
                            t2.HasTile = true;
                            short num = (short)(Main.rand.Next(0, 6) * 48);
                            t2.TileFrameX = num;
                            t1.TileFrameX = num;
                            t1.TileFrameY = 16;
                            t2.TileFrameX = 0;
                            break;
                        case 3:
                            t1.TileType = (ushort)ModContent.TileType<Tiles.BlackStarShrub>();
                            t2.TileType = (ushort)ModContent.TileType<Tiles.BlackStarShrub>();
                            t3.TileType = (ushort)ModContent.TileType<Tiles.BlackStarShrub>();
                            t1.HasTile = true;
                            t2.HasTile = true;
                            t3.HasTile = true;
                            short num1 = (short)(Main.rand.Next(0, 6) * 72);
                            t3.TileFrameX = num1;
                            t2.TileFrameX = num1;
                            t1.TileFrameX = num1;
                            t1.TileFrameY = 32;
                            t2.TileFrameX = 16;
                            t3.TileFrameX = 0;
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