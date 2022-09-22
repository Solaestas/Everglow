using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.MythModule.TheFirefly.Tiles;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.YggdrasilModule
{
    public class YggdrasilWorldGeneration : ModSystem
    {
        internal class YggdrasilWorldGenPass : GenPass
        {
            public YggdrasilWorldGenPass() : base("MothLand", 500)
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everlow.Common.WorldSystem.BuildMothCave");
                BuildMothCave();
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) => tasks.Add(new YggdrasilWorldGenPass());
        /// <summary>
        /// 地形中心坐标
        /// </summary>
        public int fireflyCenterX = 2000;
        public int fireflyCenterY = 500;

        public override void SaveWorldData(TagCompound tag)
        {
            tag["FIREFLYcenterX"] = fireflyCenterX;
            tag["FIREFLYcenterY"] = fireflyCenterY;

            var fireFlyTree = ModContent.GetInstance<FireflyTree>();
            var list = new List<TagCompound>();
            foreach (var (x, y, style) in fireFlyTree.GetRopeStyleList())
            {
                list.Add(new TagCompound() {
                    { "x", x },
                    { "y", y },
                    { "style", style },
                });
            }
            tag.Set("FIREFLY_FireflyTree", list);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            fireflyCenterX = tag.GetAsInt("FIREFLYcenterX");
            fireflyCenterY = tag.GetAsInt("FIREFLYcenterY");


            if (tag.ContainsKey("FIREFLY_FireflyTree"))
            {
                var fireFlyTree = ModContent.GetInstance<FireflyTree>();
                var listTag = tag.GetList<TagCompound>("FIREFLY_FireflyTree");
                List<(int x, int y, int style)> ropeData = new List<(int x, int y, int style)>();
                foreach (var item in listTag)
                {
                    int x = item.Get<int>("x");
                    int y = item.Get<int>("y");
                    int style = item.GetInt("style");
                    ropeData.Add((x, y, style));
                }
                fireFlyTree.InitTreeRopes(ropeData);
            }
        }

        /// <summary>
        /// type = 0:Kill,type = 1:place Tiles,type = 2:place Walls
        /// </summary>
        /// <param name="Shapepath"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="type"></param>
        public static void ShapeTile(string Shapepath, int a, int b, int type)
        {
            var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Sources/Modules/MythModule/TheFirefly/WorldGeneration/" + Shapepath);
            imageData.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
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
                                        //tile.WallType = (ushort)ModContent.WallType<Walls.DarkCocoonWall>();
                                    }
                                }
                                break;
                        }
                    }
                }
            });
            //int width = colors.GetLength(0);
            //int height = colors.GetLength(1);
            //for (int y = 0; y < height; y += 1)
            //{
            //    for (int x = 0; x < width; x += 1)
            //    {
            //        Tile tile = Main.tile[x + a, y + b];
            //        switch (type)//21是箱子
            //        {
            //            case 0:
            //                if (colors[x, y] == new Color(255, 0, 0, 255))
            //                {
            //                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
            //                    {
            //                        tile.ClearEverything();
            //                    }
            //                }
            //                break;
            //            case 1:
            //                if (colors[x, y] == new Color(56, 48, 61, 255))
            //                {
            //                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
            //                    {
            //                        tile.TileType = (ushort)ModContent.TileType<Tiles.DarkCocoon>();
            //                        tile.HasTile = true;
            //                    }
            //                }
            //                if (colors[x, y] == new Color(0, 0, 255, 255))
            //                {
            //                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
            //                    {
            //                        tile.LiquidType = LiquidID.Water;
            //                        tile.LiquidAmount = 200;
            //                        tile.HasTile = false;
            //                        //WorldGen.PlaceLiquid(x, y, byte.MaxValue, 255);
            //                    }
            //                }
            //                break;
            //            case 2:
            //                if (colors[x, y] == new Color(0, 0, 5, 255))
            //                {
            //                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
            //                    {
            //                        tile.WallType = (ushort)ModContent.WallType<Walls.DarkCocoonWall>();
            //                    }
            //                }
            //                break;
            //        }
            //    }
            //}
        }
        /// <summary>
        /// 建造流萤之茧
        /// </summary>
        public static void BuildMothCave()
        {
            //Point16 AB = CocoonPos();
            int a = 4000;//AB.X;
            int b = 1200;//AB.Y;
            Main.statusText = "CocoonKillStart";
            ShapeTile("CocoonKill.bmp", a, b, 0);
            Main.statusText = "CocoonStart";
            ShapeTile("Cocoon.bmp", a, b, 1);
            Main.statusText = "CocoonWallStart";
            ShapeTile("CocoonWall.bmp", a, b, 2);
            SmoothMothTile(a, b);
        }
        private static int GetCrash(int PoX, int PoY)
        {
            int CrashCount = 0;
            ushort[] DangerTileType = new ushort[]
            {
                41,//蓝地牢砖
                43,//绿地牢砖
                44,//粉地牢砖
                48,//尖刺
                49,//水蜡烛
                50,//书
                137,//神庙机关
                226,//神庙石砖
                232,//木刺
                237,//神庙祭坛
                481,//碎蓝地牢砖
                482,//碎绿地牢砖
                483//碎粉地牢砖
            };
            for (int x = -256; x < 257; x += 8)
            {
                for (int y = -128; y < 129; y += 8)
                {
                    if (Array.Exists(DangerTileType, Ttype => Ttype == Main.tile[x + PoX, y + PoY].TileType))
                    {
                        CrashCount++;
                    }
                }
            }
            return CrashCount;
        }

        private static int GetMergeToJungle(int PoX, int PoY)
        {
            int CrashCount = 0;
            ushort[] MustHaveTileType = new ushort[]
            {
                TileID.JungleGrass,//丛林草方块
                TileID.JunglePlants,//丛林草
                TileID.JungleVines,//丛林藤
                TileID.JunglePlants2,//高大丛林草
                TileID.PlantDetritus//丛林花
            };
            for (int x = -256; x < 257; x += 8)
            {
                for (int y = -128; y < 129; y += 8)
                {
                    if (Array.Exists(MustHaveTileType, Ttype => Ttype == Main.tile[x + PoX, y + PoY].TileType))
                    {
                        CrashCount++;
                    }
                }
            }
            return CrashCount;
        }
        /// <summary>
        /// 获取一个不与原版地形冲突的点
        /// </summary>
        /// <returns></returns>
        private static Point16 CocoonPos()
        {
            int PoX = Main.rand.Next(300, Main.maxTilesX - 600);
            int PoY = Main.rand.Next(500, Main.maxTilesY - 700);

            while (GetCrash(PoX, PoY) > 0 || GetMergeToJungle(PoX, PoY) <= 10)
            {
                PoX = Main.rand.Next(300, Main.maxTilesX - 600);
                PoY = Main.rand.Next(500, Main.maxTilesY - 700);
            }
            return new Point16(PoX, PoY);
        }
        private static void SmoothMothTile(int a, int b)
        {
            for (int y = 0; y < 256; y += 1)
            {
                for (int x = 0; x < 512; x += 1)
                {
                    //if (Main.tile[x + a, y + b].TileType == (ushort)ModContent.TileType<Tiles.DarkCocoon>())
                    //{
                    //    Tile.SmoothSlope(x + a, y + b, false);
                    //    WorldGen.TileFrame(x + a, y + b, true, false);
                    //}
                    //else
                    //{
                    //    WorldGen.TileFrame(x + a, y + b, true, false);
                    //}
                    //WorldGen.SquareWallFrame(x + a, y + b, true);
                }
            }
        }
    }
}

