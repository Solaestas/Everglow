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
                if (!Main.tile[i, j].IsHalfBlock)
                {
                    for (int x = -2; x < 3; x++)
                    {
                        for (int y = -12; y < 0; y++)
                        {
                            if (Main.tile[i + x, j + y].HasTile)
                            {
                                return;
                            }
                        }
                    }
                    short FrX = (short)(Main.rand.Next(0, 16));
                    for (int y = -8; y < 0; y++)
                    {
                        Tile tile = Main.tile[i, j + y];
                        tile.TileType = (ushort)ModContent.TileType<Tiles.GlowWood>();
                        tile.HasTile = true;
                        tile.TileFrameX = (short)(FrX * 256);
                        tile.TileFrameY = (short)((y + 8) * 16);
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