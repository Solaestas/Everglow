using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.WorldModule;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Tiles;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration
{
    //TestCode
    internal class MothWorld : World
    {
        public MothWorld() : base(800, 600, SaveType.PerWorld) { }

        public override void CreateWorld(GameTime gameTime)
        {
            BuildMothCave();
            Main.spawnTileX = 723;
            Main.spawnTileY = 226;
        }
        public void BuildMothCave()
        {
            //Point16 AB = CocoonPos();
            int a = 230;//AB.X;
            int b = 200;//AB.Y;
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            mothLand.fireflyCenterX = a + 140;
            mothLand.fireflyCenterY = b + 140;
            Main.statusText = "CocoonStart";
            ShapeTile("CocoonWorld.bmp", 0, 0, 1);
            ShapeTile("CocoonWorldWall.bmp", 0, 0, 2);
            Main.statusText = "CocoonKillStart";
            ShapeTile("CocoonSubKill.bmp", a, b, 0);
            Main.statusText = "CocoonStart";
            ShapeTile("CocoonSub.bmp", a, b, 1);
            Main.statusText = "CocoonWallStart";
            ShapeTile("CocoonSubWall.bmp", a, b, 2);
            Main.statusText = "CocoonAnotherStart";
            ShapeTile("CocoonSub.bmp", a, b, 3);
            SmoothMothTile(a, b);
            for (int x = 20; x < Main.maxTilesX - 20; x++)
            {
                for (int y = 20; y < Main.maxTilesY - 20; y++)
                {
                    RandomUpdate(x, y, ModContent.TileType<DarkCocoon>());
                }
            }
        }
        /// <summary>
        /// type = 0:Kill,type = 1:place Tiles,type = 2:place Walls
        /// </summary>
        /// <param name="Shapepath"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="type"></param>
        public void ShapeTile(string Shapepath, int a, int b, int type)
        {
            var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Sources/Modules/MythModule/TheFirefly/WorldGeneration/" + Shapepath);
            imageData.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    if (y + b < 20)
                    {
                        continue;
                    }
                    if (y + b > Main.maxTilesY - 20)
                    {
                        break;
                    }
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        if (x + a < 20)
                        {
                            continue;
                        }
                        if (x + a > Main.maxTilesX - 20)
                        {
                            break;
                        }
                        ref var pixel = ref pixelRow[x];
                        Tile tile = Main.tile[x + a, y + b];
                        switch (type)//21是箱子
                        {
                            case 0:
                                if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(255, 0, 0))
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.ClearEverything();
                                    }
                                }
                                break;

                            case 1:
                                if (pixel.R == 56 && pixel.G == 48 && pixel.B == 61)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(56, 48, 61))
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.TileType = (ushort)ModContent.TileType<DarkCocoon>();
                                        tile.HasTile = true;
                                    }
                                }
                                if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(56, 48, 61))
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.TileType = (ushort)ModContent.TileType<DarkCocoonSpecial>();
                                        tile.HasTile = true;
                                    }
                                }
                                if (pixel.R == 35 && pixel.G == 49 && pixel.B == 122)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(56, 48, 61))
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.TileType = (ushort)ModContent.TileType<DarkCocoonMoss>();
                                        tile.HasTile = true;
                                    }
                                }
                                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 255)//pixel == new SixLabors.ImageSharp.PixelFormats.Rgb24(0, 0, 255))
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.LiquidType = LiquidID.Water;
                                        tile.LiquidAmount = 200;
                                        tile.HasTile = false;
                                        //WorldGen.PlaceLiquid(x, y, byte.MaxValue, 255);
                                    }
                                }
                                break;

                            case 2:
                                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 5)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(0, 0, 5))
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.WallType = (ushort)ModContent.WallType<Walls.DarkCocoonWall>();
                                    }
                                }
                                break;
                        }
                    }
                }
            });
        }
        public void RandomUpdate(int i, int j, int Type)
        {
            if (Main.tile[i, j].TileType != Type || !Main.tile[i, j].HasTile)
            {
                return;
            }
            if (Main.rand.NextBool(4))
            {
                if (Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i + 1, j].Slope == SlopeType.Solid && Main.tile[i - 1, j].Slope == SlopeType.Solid && Main.tile[i + 2, j].Slope == SlopeType.Solid && Main.tile[i - 2, j].Slope == SlopeType.Solid &&
                    Main.tile[i, j + 1].Slope == SlopeType.Solid && Main.tile[i + 1, j + 1].Slope == SlopeType.Solid && Main.tile[i - 1, j + 1].Slope == SlopeType.Solid && Main.tile[i + 2, j + 1].Slope == SlopeType.Solid && Main.tile[i - 2, j + 1].Slope == SlopeType.Solid)//树木
                {
                    int MaxHeight = 0;
                    for (int x = -2; x < 3; x++)
                    {
                        for (int y = -1; y > -30; y--)
                        {
                            if (j + y > 20)
                            {
                                if (Main.tile[i + x, j + y].HasTile || Main.tile[i + x, j + y].LiquidAmount > 3)
                                {
                                    return;
                                }
                            }
                            MaxHeight = -y;
                        }
                    }
                    if (MaxHeight > 7)
                    {
                        BuildFluorescentTree(i, j - 1, MaxHeight);
                    }
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
            if (Main.rand.NextBool(3))//流萤滴
            {
                int count = 0;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = 1; y <= 3; y++)
                    {
                        Tile t0 = Main.tile[i + x, j + y];
                        if (t0.HasTile)
                        {
                            count++;
                        }
                    }
                }
                if (count == 0)
                {
                    Common.MythUtils.PlaceFrameImportantTiles(i - 1, j + 1, 3, 3, ModContent.TileType<Tiles.Furnitures.GlowingDrop>());
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
                            short num2 = (short)(Main.rand.Next(0, 12) * 120);
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
        public void BuildFluorescentTree(int i, int j, int height = 0)
        {
            if (j < 30)
            {
                return;
            }
            int Height = Main.rand.Next(7, height);

            for (int g = 0; g < Height; g++)
            {
                Tile tile = Main.tile[i, j - g];
                if (g > 3)
                {
                    if (Main.rand.NextBool(5))
                    {
                        Tile tileLeft = Main.tile[i - 1, j - g];
                        tileLeft.TileType = (ushort)ModContent.TileType<FluorescentTree>();
                        tileLeft.TileFrameY = 4;
                        tileLeft.TileFrameX = (short)Main.rand.Next(4);
                        tileLeft.HasTile = true;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        Tile tileRight = Main.tile[i + 1, j - g];
                        tileRight.TileType = (ushort)ModContent.TileType<FluorescentTree>();
                        tileRight.TileFrameY = 5;
                        tileRight.TileFrameX = (short)Main.rand.Next(4);
                        tileRight.HasTile = true;
                    }
                }
                if (g == 0)
                {
                    tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
                    tile.TileFrameY = 0;
                    tile.TileFrameX = 0;
                    tile.HasTile = true;
                    continue;
                }
                if (g == 1)
                {
                    tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
                    tile.TileFrameY = -1;
                    tile.TileFrameX = 0;
                    tile.HasTile = true;
                    continue;
                }
                if (g == 2)
                {
                    tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
                    tile.TileFrameY = 3;
                    tile.TileFrameX = (short)Main.rand.Next(4);
                    tile.HasTile = true;
                    continue;
                }
                if (g == Height - 1)
                {
                    tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
                    tile.TileFrameY = 2;
                    tile.TileFrameX = 0;
                    tile.HasTile = true;
                    continue;
                }
                tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
                tile.TileFrameY = 1;
                tile.TileFrameX = (short)Main.rand.Next(12);
                tile.HasTile = true;
            }
        }
        public void SmoothMothTile(int a, int b, int width = 256, int height = 512)
        {
            for (int y = 0; y < width; y += 1)
            {
                for (int x = 0; x < height; x += 1)
                {
                    if (Main.tile[x + a, y + b].TileType == (ushort)ModContent.TileType<Tiles.DarkCocoon>())
                    {
                        Tile.SmoothSlope(x + a, y + b, false);
                        WorldGen.TileFrame(x + a, y + b, true, false);
                    }
                    else
                    {
                        WorldGen.TileFrame(x + a, y + b, true, false);
                    }
                    WorldGen.SquareWallFrame(x + a, y + b, true);
                }
            }
        }
    }
}