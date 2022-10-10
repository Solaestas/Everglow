using Terraria.Localization;

namespace Everglow.Sources.Modules.YggdrasilModule.KelpCurtain.Tiles
{
    public class OldMoss : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlendAll[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileShine2[Type] = false;
            Main.tileMoss[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<DragonScaleWood>()] = true;
            //Main.tileFrameImportant[(int)base.Type] = false;
            Main.ugBackTransition = 1000;
            DustType = DustID.BrownMoss;
            MinPick = 150;
            HitSound = SoundID.Grass;
            
            ItemDrop = ModContent.ItemType<Items.OldMoss>();
            //Main.tileSpelunker[(int)base.Type] = true;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(81, 107, 18), modTranslation);
            modTranslation.SetDefault("");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "");
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            bool[,] Bound = new bool[9, 9];
            Vector2 Pin = Vector2.Zero;
            for (int a = -4; a < 5; a++)
            {
                for (int b = -4; b < 5; b++)
                {
                    if (Main.tile[i + a, j + b].TileType == Main.tile[i, j].TileType)
                    {
                        int C = 0;
                        if (Main.tile[i + a + 1, j + b].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (Main.tile[i + a - 1, j + b].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (Main.tile[i + a, j + b - 1].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (Main.tile[i + a, j + b + 1].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (C == 4)
                        {
                            Bound[a + 4, b + 4] = false;
                        }
                        else
                        {
                            Bound[a + 4, b + 4] = true;
                        }
                    }
                }
            }
            for (int a = -3; a < 4; a++)
            {
                for (int b = -3; b < 4; b++)
                {
                    Pin = Vector2.Zero;
                    if (Main.tile[i + a, j + b].TileType == Main.tile[i, j].TileType)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            for (int y = -1; y < 2; y++)
                            {
                                if (Main.tile[i + a + x, j + b + y].TileType == Main.tile[i, j].TileType)
                                {
                                    if (Bound[a + x + 4, b + y + 4])
                                    {
                                        Pin += new Vector2(x, y);
                                    }
                                }
                            }
                        }
                        Pin.X = Math.Sign(Pin.X);
                        Pin.Y = Math.Sign(Pin.Y);
                        if (!Bound[a + 4, b + 4])
                        {
                            if (Pin == new Vector2(0, 0))
                            {
                                int C2 = 0;
                                for (int z = -1; z < 2; z++)
                                {
                                    for (int w = -1; w < 2; w++)
                                    {
                                        if (Bound[a + z + 4, b + w + 4])
                                        {
                                            C2++;
                                        }
                                    }
                                }
                                if (C2 == 0)
                                {
                                    if ((Main.tile[i + a, j + b].TileFrameX != 18 && Main.tile[i + a, j + b].TileFrameX != 36 && Main.tile[i + a, j + b].TileFrameX != 54) || Main.tile[i + a, j + b].TileFrameY != 18)
                                    {
                                        Main.tile[i + a, j + b].TileFrameX = (short)(Main.rand.Next(1, 3) * 18);
                                        Main.tile[i + a, j + b].TileFrameY = 18;
                                    }
                                }
                                else
                                {
                                    if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 12 * 18)
                                    {
                                        Main.tile[i + a, j + b].TileFrameX = 12 * 18;
                                        Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                    }
                                }
                            }
                            if (Pin == new Vector2(0, 1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 0 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 0 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(1, 0))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 2 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 2 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(1, 1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 4 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 4 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(1, -1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 6 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 6 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(-1, 1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 5 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 5 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(0, -1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 1 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 1 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(-1, 0))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 3 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 3 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(-1, -1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 7 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 7 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void RandomUpdate(int i, int j)
        {
            bool[,] Bound = new bool[9, 9];
            Vector2 Pin = Vector2.Zero;
            for (int a = -4; a < 5; a++)
            {
                for (int b = -4; b < 5; b++)
                {
                    if (Main.tile[i + a, j + b].TileType == Main.tile[i, j].TileType)
                    {
                        int C = 0;
                        if (Main.tile[i + a + 1, j + b].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (Main.tile[i + a - 1, j + b].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (Main.tile[i + a, j + b - 1].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (Main.tile[i + a, j + b + 1].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (C == 4)
                        {
                            Bound[a + 4, b + 4] = false;
                        }
                        else
                        {
                            Bound[a + 4, b + 4] = true;
                        }
                    }
                }
            }
            for (int a = -3; a < 4; a++)
            {
                for (int b = -3; b < 4; b++)
                {
                    Pin = Vector2.Zero;
                    if (Main.tile[i + a, j + b].TileType == Main.tile[i, j].TileType)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            for (int y = -1; y < 2; y++)
                            {
                                if (Main.tile[i + a + x, j + b + y].TileType == Main.tile[i, j].TileType)
                                {
                                    if (Bound[a + x + 4, b + y + 4])
                                    {
                                        Pin += new Vector2(x, y);
                                    }
                                }
                            }
                        }
                        Pin.X = Math.Sign(Pin.X);
                        Pin.Y = Math.Sign(Pin.Y);
                        if (!Bound[a + 4, b + 4])
                        {
                            if (Pin == new Vector2(0, 0))
                            {
                                int C2 = 0;
                                for (int z = -1; z < 2; z++)
                                {
                                    for (int w = -1; w < 2; w++)
                                    {
                                        if (Bound[a + z + 4, b + w + 4])
                                        {
                                            C2++;
                                        }
                                    }
                                }
                                if (C2 == 0)
                                {
                                    if ((Main.tile[i + a, j + b].TileFrameX != 18 && Main.tile[i + a, j + b].TileFrameX != 36 && Main.tile[i + a, j + b].TileFrameX != 54) || Main.tile[i + a, j + b].TileFrameY != 18)
                                    {
                                        Main.tile[i + a, j + b].TileFrameX = (short)(Main.rand.Next(1, 3) * 18);
                                        Main.tile[i + a, j + b].TileFrameY = 18;
                                    }
                                }
                                else
                                {
                                    if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 12 * 18)
                                    {
                                        Main.tile[i + a, j + b].TileFrameX = 12 * 18;
                                        Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                    }
                                }
                            }
                            if (Pin == new Vector2(0, 1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 0 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 0 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(1, 0))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 2 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 2 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(1, 1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 4 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 4 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(1, -1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 6 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 6 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(-1, 1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 5 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 5 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(0, -1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 1 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 1 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(-1, 0))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 3 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 3 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(-1, -1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 7 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 7 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            bool[,] Bound = new bool[9, 9];
            Vector2 Pin = Vector2.Zero;
            for (int a = -4; a < 5; a++)
            {
                for (int b = -4; b < 5; b++)
                {
                    if (Main.tile[i + a, j + b].TileType == Main.tile[i, j].TileType)
                    {
                        int C = 0;
                        if (Main.tile[i + a + 1, j + b].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (Main.tile[i + a - 1, j + b].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (Main.tile[i + a, j + b - 1].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (Main.tile[i + a, j + b + 1].TileType == Main.tile[i, j].TileType)
                        {
                            C++;
                        }
                        if (C == 4)
                        {
                            Bound[a + 4, b + 4] = false;
                        }
                        else
                        {
                            Bound[a + 4, b + 4] = true;
                        }
                    }
                }
            }
            for (int a = -3; a < 4; a++)
            {
                for (int b = -3; b < 4; b++)
                {
                    Pin = Vector2.Zero;
                    if (Main.tile[i + a, j + b].TileType == Main.tile[i, j].TileType)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            for (int y = -1; y < 2; y++)
                            {
                                if (Main.tile[i + a + x, j + b + y].TileType == Main.tile[i, j].TileType)
                                {
                                    if (Bound[a + x + 4, b + y + 4])
                                    {
                                        Pin += new Vector2(x, y);
                                    }
                                }
                            }
                        }
                        Pin.X = Math.Sign(Pin.X);
                        Pin.Y = Math.Sign(Pin.Y);
                        if (!Bound[a + 4, b + 4])
                        {
                            if (Pin == new Vector2(0, 0))
                            {
                                int C2 = 0;
                                for (int z = -1; z < 2; z++)
                                {
                                    for (int w = -1; w < 2; w++)
                                    {
                                        if (Bound[a + z + 4, b + w + 4])
                                        {
                                            C2++;
                                        }
                                    }
                                }
                                if (C2 == 0)
                                {
                                    if ((Main.tile[i + a, j + b].TileFrameX != 18 && Main.tile[i + a, j + b].TileFrameX != 36 && Main.tile[i + a, j + b].TileFrameX != 54) || Main.tile[i + a, j + b].TileFrameY != 18)
                                    {
                                        Main.tile[i + a, j + b].TileFrameX = (short)(Main.rand.Next(1, 3) * 18);
                                        Main.tile[i + a, j + b].TileFrameY = 18;
                                    }
                                }
                                else
                                {
                                    if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 12 * 18)
                                    {
                                        Main.tile[i + a, j + b].TileFrameX = 12 * 18;
                                        Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                    }
                                }
                            }
                            if (Pin == new Vector2(0, 1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 0 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 0 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(1, 0))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 2 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 2 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(1, 1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 4 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 4 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(1, -1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 6 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 6 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(-1, 1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 5 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 5 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(0, -1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 1 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 1 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(-1, 0))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 3 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 3 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                            if (Pin == new Vector2(-1, -1))
                            {
                                if ((Main.tile[i + a, j + b].TileFrameY != 6 * 18 && Main.tile[i + a, j + b].TileFrameY != 7 * 18 && Main.tile[i + a, j + b].TileFrameY != 8 * 18) || Main.tile[i + a, j + b].TileFrameX != 7 * 18)
                                {
                                    Main.tile[i + a, j + b].TileFrameX = 7 * 18;
                                    Main.tile[i + a, j + b].TileFrameY = (short)(Main.rand.Next(6, 8) * 18);
                                }
                            }
                        }
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
