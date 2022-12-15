using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Tiles;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration
{
    public class MothLand : ModSystem
    {
        internal class MothLandGenPass : GenPass
        {
            public MothLandGenPass() : base("MothLand", 500)
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everlow.Common.WorldSystem.BuildMothCave");
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
                Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everlow.Common.WorldSystem.BuildWorldMothCave");
                BuildWorldMothCave();
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) => tasks.Add(new WorldMothLandGenPass());

        /// <summary>
        /// ������������
        /// </summary>
        public int fireflyCenterX = 400;

        public int fireflyCenterY = 300;

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
                        switch (type)//21������
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
                                    //MythUtils.PlaceFrameImportantTiles(a + x, b + y, 3, 4, ModContent.TileType<Pylon.FireflyPylon>());
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
            //        switch (type)//21������
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
        /// ������ө֮��
        /// </summary>
        public static void BuildMothCave()
        {
            //Point16 AB = CocoonPos();
            int a = 230;//AB.X;
            int b = 200;//AB.Y;
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            mothLand.fireflyCenterX = a + 140;
            mothLand.fireflyCenterY = b + 140;
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
                41,//������ש
                43,//�̵���ש
                44,//�۵���ש
                48,//���
                49,//ˮ����
                50,//��
                137,//�������
                226,//����ʯש
                232,//ľ��
                237,//�����̳
                481,//��������ש
                482,//���̵���ש
                483//��۵���ש
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
                TileID.JungleGrass,//���ֲݷ���
                TileID.JunglePlants,//���ֲ�
                TileID.JungleVines,//������
                TileID.JunglePlants2,//�ߴ���ֲ�
                TileID.PlantDetritus//���ֻ�
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
        /// ��ȡһ������ԭ����γ�ͻ�ĵ�
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