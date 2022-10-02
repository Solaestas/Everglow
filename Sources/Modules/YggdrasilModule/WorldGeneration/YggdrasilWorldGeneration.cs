using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO;
using Terraria.IO;
using Terraria.WorldBuilding;
using Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles;
using Everglow.Sources.Modules.YggdrasilModule.KelpCurtain.Tiles;


namespace Everglow.Sources.Modules.YggdrasilModule.WorldGeneration
{
    public class YggdrasilWorldGeneration : ModSystem
    {
        internal class YggdrasilWorldGenPass : GenPass
        {
            public YggdrasilWorldGenPass() : base("Yggdrasil, the Tree World", 500)
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everlow.Common.WorldSystem.BuildtheTreeWorld");
                BuildtheTreeWorld();
                SmoothTile();
            }
        }

        //public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) => tasks.Add(new YggdrasilWorldGenPass());
        /// <summary>
        /// type = 0:Kill,type = 1:place Tiles,type = 2:place Walls
        /// </summary>
        /// <param name="Shapepath"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="type"></param>
        public static void ShapeTile(string Shapepath, int a, int b, int type)
        {
            var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Sources/Modules/YggdrasilModule/WorldGeneration/" + Shapepath);
            imageData.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        ref var pixel = ref pixelRow[x];
                        Tile tile = Main.tile[x + a, y + b];
                        switch (type)//21 «œ‰◊”
                        {
                            case 0:
                                if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.ClearEverything();
                                    }
                                }
                                break;
                            case 1:
                                if (pixel.R == 44 && pixel.G == 40 && pixel.B == 37)// ØªØ¡˙¡€ƒæ
                                {
                                    tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
                                    tile.HasTile = true;
                                }
                                if (pixel.R == 155 && pixel.G == 173 && pixel.B == 183)//«‡∂–øÛ
                                {
                                    tile.TileType = (ushort)ModContent.TileType<CyanVineStone>();
                                    tile.HasTile = true;
                                }
                                if (pixel.R == 31 && pixel.G == 26 && pixel.B == 45)//∫⁄”Ÿƒ‡
                                {
                                    tile.TileType = (ushort)ModContent.TileType<DarkMud>();
                                    tile.HasTile = true;
                                }
                                if (pixel.R == 255 && pixel.G == 8 && pixel.B == 0)//≤‚ ‘”√∑øŒ›
                                {
                                    MapIO mapIO = new MapIO(x, y);
                                    string pathName = "Everglow/Sources/Modules/YggdrasilModule/WorldGeneration/WoodBridge01.mapio";
                                    using var memoryStream = new MemoryStream(ModContent.GetFileBytes(pathName));
                                    mapIO.Read(memoryStream);

                                    var it = mapIO.GetEnumerator();
                                    while (it.MoveNext())
                                    {
                                        WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
                                        WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
                                    }
                                }



                                if (pixel.R == 82 && pixel.G == 62 && pixel.B == 44)//¡˙¡€ƒæ
                                {
                                    tile.TileType = (ushort)ModContent.TileType<DragonScaleWood>();
                                    tile.HasTile = true;
                                }
                                if (pixel.R == 81 && pixel.G == 107 && pixel.B == 18)//π≈Ã¶ﬁ∫
                                {
                                    tile.TileType = (ushort)ModContent.TileType<OldMoss>();
                                    tile.HasTile = true;
                                }
                                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 255)
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.LiquidType = LiquidID.Water;
                                        tile.LiquidAmount = 200;
                                        tile.HasTile = false;
                                    }
                                }
                                break;
                            case 2:
                                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 5)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(0, 0, 5))
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        //tile.WallType = (ushort)ModContent.WallType<Walls.DarkCocoonWall>();
                                    }
                                }
                                break;
                        }
                    }
                }
            });
           
        }
        /// <summary>
        /// Ω®‘ÏÃÏÒ∑ ˜
        /// </summary>
        public static void BuildtheTreeWorld()
        {
            //Point16 AB = CocoonPos();
            int a = 0;//AB.X;
            int b = 0;//AB.Y;
            Main.statusText = "YggdrasilStart";
            ShapeTile("Tree.bmp", a, b, 1);
        }
        private static void SmoothTile(int a = 0, int b = 0, int c = 0, int d = 0)
        {
            for (int x = 20 + b; x < 980 - d; x += 1)
            {
                for (int y = 20 + a; y < 11980 - c; y += 1)
                {
                
                    Tile.SmoothSlope(x + a, y + b, false);
                    WorldGen.TileFrame(x + a, y + b, true, false);
                    WorldGen.SquareWallFrame(x + a, y + b, true);
                }
            }
        }
    }
}

