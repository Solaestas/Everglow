using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Tiles;
using Everglow.Sources.Modules.MythModule.TheFirefly.Pylon;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;


namespace Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration
{
    public class MothLand : ModSystem
    {
        public override void PostUpdateEverything()
        {
            if(Main.mouseRight && Main.mouseRightRelease)
            {
                //QuickBuild((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, "MapIOResources/ShabbyPylonWithCastle22x22Style0.mapio");
                for (int i = 0; i < 7;i++)
                {
                    //QuickBuild((int)Main.MouseWorld.X / 16 + i * 40, (int)Main.MouseWorld.Y / 16, "MapIOResources/ShabbyCastle0" + (i + 1).ToString() + ".mapio");
                }

                //MythUtils.PlaceFrameImportantTiles((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, 3, 9, ModContent.TileType<BoCPostAndBead>());


            }
            if (!PylonSystem.Instance.shabbyPylonEnable && NPC.downedBoss2)
            {
                PylonSystem.Instance.shabbyPylonEnable = true;
                PylonSystem.Instance.firstEnableAnimation = true;

                Main.NewText("Repaired");
            }
        }
        public static void QuickBuild(int x, int y, string Path)
        {
            MapIO mapIO = new MapIO(x, y);

            mapIO.Read(Everglow.Instance.GetFileStream("Sources/Modules/MythModule/" + Path));

            var it = mapIO.GetEnumerator();
            while (it.MoveNext())
            {
                WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
                WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
            }
        }
        private void BuildLivingFluorescentTree()
        {

        }


        internal class MothLandGenPass : GenPass
        {
            public MothLandGenPass() : base("MothLand", 500)
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everglow.Common.WorldSystem.BuildMothCave");
                BuildMothCave();
            }
        }

        internal class WorldMothLandGenPass : GenPass
        {
            public WorldMothLandGenPass() : base("MothLand", 500)
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everglow.Common.WorldSystem.BuildWorldMothCave");
                BuildWorldMothCave();
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) => tasks.Add(new WorldMothLandGenPass());

        /// <summary>
        /// 地形中心坐标
        /// </summary>
        public int fireflyCenterX = 400;

        public int fireflyCenterY = 300;

        public override void SaveWorldData(TagCompound tag)
        {
            tag["FIREFLYcenterX"] = fireflyCenterX;
            tag["FIREFLYcenterY"] = fireflyCenterY;

            var fireFlyTree = ModContent.GetInstance<FluorescentTree>();
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
                var fireFlyTree = ModContent.GetInstance<FluorescentTree>();
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
                    if (y + b < 20)
                    {
                        continue;
                    }
                    if(y + b > Main.maxTilesY - 20)
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

                            case 3:
                                if (pixel.R == 165 && pixel.G == 0 && pixel.B == 255)
                                {
                                    MythUtils.PlaceFrameImportantTiles(a + x, b + y, 5, 7, ModContent.TileType<Tiles.MothWorldDoor>());
                                }
                                if (pixel.R == 45 && pixel.G == 49 && pixel.B == 255)
                                {
                                    MythUtils.PlaceFrameImportantTiles(a + x, b + y, 3, 4, ModContent.TileType<Pylon.FireflyPylon>());
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
            int a = 230;//AB.X;
            int b = 200;//AB.Y;
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            mothLand.fireflyCenterX = a + 140;
            mothLand.fireflyCenterY = b + 140;
            Main.statusText = "CocoonStart";
            ShapeTile("CocoonWorld.bmp", 0, 0, 1);
            ShapeTile("CocoonWorldWall.bmp", 0, 0, 2);
            Main.statusText = "CocoonKillStart";
            ShapeTile("CocoonKill.bmp", a, b, 0);
            Main.statusText = "CocoonStart";
            ShapeTile("Cocoon.bmp", a, b, 1);
            Main.statusText = "CocoonWallStart";
            ShapeTile("CocoonWall.bmp", a, b, 2);
            Main.statusText = "CocoonAnotherStart";
            ShapeTile("Cocoon.bmp", a, b, 3);
            SmoothMothTile(a, b);
        }

        public static void BuildWorldMothCave()
        {
            //Point16 AB = CocoonPos();
            int a = 2000;//AB.X;
            int b = 600;//AB.Y;
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            mothLand.fireflyCenterX = a + 140;
            mothLand.fireflyCenterY = b + 140;
            Main.statusText = "CocoonKillStart";
            ShapeTile("WorldCocoonKill.bmp", a, b, 0);
            Main.statusText = "CocoonStart";
            ShapeTile("WorldCocoon.bmp", a, b, 1);
            Main.statusText = "CocoonWallStart";
            ShapeTile("WorldCocoonWall.bmp", a, b, 2);
            Main.statusText = "CocoonAnotherStart";
            ShapeTile("WorldCocoon.bmp", a, b, 3);
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

        private static void SmoothMothTile(int a, int b, int width = 256, int height = 512)
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