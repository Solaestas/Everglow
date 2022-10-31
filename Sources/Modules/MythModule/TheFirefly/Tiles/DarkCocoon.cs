namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class DarkCocoon : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<DarkCocoonMoss>()] = true;
            Main.tileMerge[Type][ModContent.TileType<DarkCocoonSpecial>()] = true;
            MinPick = 175;
            DustType = 191;
            ItemDrop = ModContent.ItemType<Items.DarkCocoon>();
            AddMapEntry(new Color(17, 16, 17));
        }

        private void PlantTree(int i, int j, int style)
        {
            for (int y = -8; y < 0; y++)
            {
                Tile tile = Main.tile[i, j + y];
                tile.TileType = (ushort)ModContent.TileType<Tiles.FireflyTree>();
                tile.HasTile = true;
                tile.TileFrameX = (short)(style * 256);
                tile.TileFrameY = (short)((y + 8) * 16);
            }
            var fireFlyTree = ModContent.GetInstance<FireflyTree>();
            fireFlyTree.InsertOneTreeRope(i, j - 8, style);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            RandomUpdate(i, j);//TODO:为了让这玩意效果正常强行采取的暴力措施，如果sublib更新了就删掉
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.rand.NextBool(6))
            {
                if (Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i + 1, j].Slope == SlopeType.Solid && Main.tile[i - 1, j].Slope == SlopeType.Solid && Main.tile[i + 2, j].Slope == SlopeType.Solid && Main.tile[i - 2, j].Slope == SlopeType.Solid && 
                    Main.tile[i, j + 1].Slope == SlopeType.Solid && Main.tile[i + 1, j + 1].Slope == SlopeType.Solid && Main.tile[i - 1, j + 1].Slope == SlopeType.Solid && Main.tile[i + 2, j + 1].Slope == SlopeType.Solid && Main.tile[i - 2, j + 1].Slope == SlopeType.Solid)//树木
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
                    PlantTree(i, j, FrX);
                }
            }

            if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i, j - 1].LiquidAmount > 0)
            {
                Tile tile = Main.tile[i, j - 1];
                tile.TileType = (ushort)(ModContent.TileType<Tiles.LampLotus>());
                tile.HasTile = true;
                tile.TileFrameX = (short)(28 * Main.rand.Next(8));
            }
            if (Main.rand.NextBool(6))//黑萤藤蔓
            {
                Tile t0 = Main.tile[i, j];

                Tile t2 = Main.tile[i, j + 1];
                if (t0.Slope == SlopeType.Solid && !t2.HasTile)
                {
                    t2.TileType = (ushort)ModContent.TileType<Tiles.BlackVine>();
                    t2.HasTile = true;
                    t2.TileFrameY = (short)(Main.rand.Next(6, 9) * 18);
                }
            }
            if (!Main.tile[i, j - 1].HasTile && !Main.tile[i + 1, j - 1].HasTile && !Main.tile[i - 1, j - 1].HasTile && Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i - 1, j].Slope == SlopeType.Solid && Main.tile[i + 1, j].Slope == SlopeType.Solid)//黑萤苣
            {
                Tile t1 = Main.tile[i, j - 1];
                Tile t2 = Main.tile[i, j - 2];
                Tile t3 = Main.tile[i, j - 3];
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
                if (Main.rand.NextBool(2))
                {
                    switch (Main.rand.Next(1, 10))
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
                            t2.TileFrameY = 0;
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
                            t2.TileFrameY = 0;
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
                            t2.TileFrameY = 16;
                            t3.TileFrameY = 0;
                            break;

                        case 4:
                            t1.TileType = (ushort)ModContent.TileType<Tiles.BlueBlossom>();
                            t2.TileType = (ushort)ModContent.TileType<Tiles.BlueBlossom>();
                            t3.TileType = (ushort)ModContent.TileType<Tiles.BlueBlossom>();
                            t1.HasTile = true;
                            t2.HasTile = true;
                            t3.HasTile = true;
                            short num2 = (short)(Main.rand.Next(0, 10) * 120);
                            t3.TileFrameX = num2;
                            t2.TileFrameX = num2;
                            t1.TileFrameX = num2;
                            t1.TileFrameY = 32;
                            t2.TileFrameY = 16;
                            t3.TileFrameY = 0;
                            break;

                        case 5:
                            WorldGen.Place3x2(i - 1, j - 1, (ushort)ModContent.TileType<Tiles.BlackFrenLarge>(), Main.rand.Next(3));
                            break;

                        case 6:
                            WorldGen.Place2x2(i - 1, j - 1, (ushort)ModContent.TileType<Tiles.BlackFren>(), Main.rand.Next(3));
                            break;

                        case 7:
                            WorldGen.Place3x2(i - 1, j - 1, (ushort)ModContent.TileType<Tiles.BlackFrenLarge>(), Main.rand.Next(3));
                            break;

                        case 8:
                            WorldGen.Place2x2(i - 1, j - 1, (ushort)ModContent.TileType<Tiles.BlackFren>(), Main.rand.Next(3));
                            break;

                        case 9:
                            WorldGen.Place2x1(i - 1, j - 1, (ushort)ModContent.TileType<Tiles.CocoonRock>(), Main.rand.Next(3));
                            break;

                        case 10:
                            WorldGen.Place2x1(i - 1, j - 1, (ushort)ModContent.TileType<Tiles.CocoonRock>(), Main.rand.Next(3));
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