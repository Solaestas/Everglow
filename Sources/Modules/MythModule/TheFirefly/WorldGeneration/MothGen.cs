using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Tiles;
using Everglow.Sources.Modules.MythModule.TheFirefly.Pylon;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration
{
    public class MothLand : ModSystem
    {
        public override void PostUpdateEverything()
        {
            if (Main.mouseRight && Main.mouseRightRelease)
            {
                //TestCode;
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
        public static void BuildShabbyCastle()
        {
            Point16 sbpp = ShabbyPylonPos();
            string Path = "MapIOResources/ShabbyCastle0" + (Main.rand.Next(7) + 1) + ".mapio";
            MapIO mapIO = new MapIO(sbpp.X, sbpp.Y);
            int Height = mapIO.ReadHeight(Everglow.Instance.GetFileStream("Sources/Modules/MythModule/" + Path));
            QuickBuild(sbpp.X, sbpp.Y - Height / 2, Path);

            Point pylonBottom = new Point(sbpp.X + Main.rand.Next(8, 16), sbpp.Y - Height / 2 + 8);
            ushort PylonType = (ushort)ModContent.TileType<ShabbyPylon>();
            PylonSystem.Instance.shabbyPylonEnable = false;
            for (int a = 0; a < 12; a++)
            {
                pylonBottom.Y++;
                if (Main.tile[pylonBottom.X, pylonBottom.Y].HasTile)
                {
                    pylonBottom.Y -= 1;
                    break;
                }
            }
            for (int i = -1; i <= 1; i++)
            {
                var PylonTile = Main.tile[pylonBottom.X + i, pylonBottom.Y + 1];
                PylonTile.TileType = TileID.GrayBrick;
                PylonTile.HasTile = true;
                WorldGen.TileFrame(pylonBottom.X + i, pylonBottom.Y + 1);
            }

            TileObject.CanPlace(pylonBottom.X, pylonBottom.Y, PylonType, 0, 0, out var tileObject);
            TileObject.Place(tileObject);
            TileObjectData.CallPostPlacementPlayerHook(pylonBottom.X, pylonBottom.Y, PylonType, 0, 0, 0, tileObject);
            //switch (Main.rand.Next(5))
            //{
            //    case 0:
            //        QuickBuild(sbpp.X, sbpp.Y - 13, "MapIOResources/ShabbyPylonWithCastle20x23Style2.mapio");
            //        break;
            //    case 1:
            //        QuickBuild(sbpp.X, sbpp.Y - 13, "MapIOResources/ShabbyPylonWithCastle21x26Style1.mapio");
            //        break;
            //    case 2:
            //        QuickBuild(sbpp.X, sbpp.Y - 13, "MapIOResources/ShabbyPylonWithCastle22x22Style0.mapio");
            //        break;
            //    case 3:
            //        QuickBuild(sbpp.X, sbpp.Y - 13, "MapIOResources/ShabbyPylonWithCastle22x26Style3.mapio");
            //        break;
            //    case 4:
            //        QuickBuild(sbpp.X, sbpp.Y - 13, "MapIOResources/ShabbyPylonWithCastle22x26Style4.mapio");
            //        break;
            //}
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
                Main.spawnTileX = 723;
                Main.spawnTileY = 226;
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
                BuildShabbyCastle();
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

            //using (MemoryStream stream = new())
            //{
            //    using (BinaryWriter writer = new(stream))
            //    {
            //        var ropeinfos = ModContent.GetInstance<FluorescentTree>().GetRopeStyleList();
            //        writer.Write(ropeinfos.Count);
            //        ropeinfos.ForEach(info =>
            //        {
            //            writer.Write(info.x);
            //            writer.Write(info.y);
            //            writer.Write(info.style);
            //        });
            //        tag.Set("Ropes", stream.GetBuffer());
            //    }
            //}

            //if (Main.ActiveWorldFileData == SubWorldModule.SubworldSystem.root && SubWorldModule.SubworldSystem.current is not null)
            //{
            //    tag.Add("ExitTo", SubWorldModule.SubworldSystem.current.FullName);
            //    tag.Add("ExitPosX", Main.LocalPlayer.Center.X);
            //    tag.Add("ExitPosY", Main.LocalPlayer.Center.Y);
            //}

            //tag["DepartX"] = (int)(Main.LocalPlayer.Center.X - Main.LocalPlayer.velocity.X);
            //tag["DepartY"] = (int)(Main.LocalPlayer.Center.Y - Main.LocalPlayer.velocity.Y);
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
            
            //if (tag.TryGet("Ropes", out byte[] ropedata))
            //{
            //    using (MemoryStream stream = new(ropedata))
            //    {
            //        using (BinaryReader reader = new(stream))
            //        {
            //            List<(int, int, int)> ropes = new();
            //            int count = reader.ReadInt32();
            //            for (int i = 0; i < count; i++)
            //            {
            //                ropes.Add((reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()));
            //            }
            //            ModContent.GetInstance<FluorescentTree>().InitTreeRopes(ropes);
            //        }
            //    }
            //}

            //if (tag.TryGet("ExitTo", out string subworldname) && SubWorldModule.SubworldSystem.cache is not null && SubWorldModule.SubworldSystem.cache.Name == subworldname)
            //{
            //    Main.LocalPlayer.Center = new(tag.Get<int>("ExitPosX"), tag.Get<int>("ExitPosY"));
            //}
            
            //if(tag.ContainsKey("DepartX") && tag.ContainsKey("DepartY"))
            //{
            //    Main.LocalPlayer.position = new Vector2(tag.GetAsInt("DepartX"), tag.GetAsInt("DepartY"));
            //}
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

                            case 3:
                                if (pixel.R == 165 && pixel.G == 0 && pixel.B == 255)
                                {
                                    MythUtils.PlaceFrameImportantTiles(a + x, b + y, 5, 7, ModContent.TileType<Tiles.MothWorldDoor>());
                                }
                                if (pixel.R == 45 && pixel.G == 49 && pixel.B == 255)
                                {
                                    MythUtils.PlaceFrameImportantTiles(a + x, b + y, 3, 4, ModContent.TileType<Pylon.FireflyPylon>());
                                    TEModdedPylon moddedPylon = ModContent.GetInstance<FireflyPylonTileEntity>();
                                    moddedPylon.Position = new Point16(a + x, b + y);
                                    //TODO:I need help to generate map Icon;

                                    ushort PylonType = (ushort)ModContent.TileType<Pylon.FireflyPylon>();
                                    var bottom = new Point(a + x, b + y);
                                    for (int i = -1; i <= 1; i++)
                                    {
                                        var PylonTile = Main.tile[bottom.X + i, bottom.Y + 1];
                                        PylonTile.TileType = (ushort)ModContent.TileType<DarkCocoon>();
                                        PylonTile.HasTile = true;
                                        PylonTile.IsHalfBlock = false;
                                        PylonTile.Slope = 0;
                                        WorldGen.TileFrame(bottom.X + i, bottom.Y + 1);
                                    }

                                    TileObject.CanPlace(bottom.X, bottom.Y, PylonType, 0, 0, out var tileObject);
                                    TileObject.Place(tileObject);
                                    TileObjectData.CallPostPlacementPlayerHook(bottom.X, bottom.Y, PylonType, 0, 0, 0, tileObject);
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
            Point16 AB = CocoonPos();
            int a = AB.X;
            int b = AB.Y;
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            mothLand.fireflyCenterX = a;
            mothLand.fireflyCenterY = b;
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

        /// <summary>
        /// 获取一个出生地附近的平坦地面
        /// </summary>
        /// <returns></returns>
        private static Point16 ShabbyPylonPos()
        {
            int PoX = (int)(Main.rand.Next(40, 160) * (Main.rand.Next(2) - 0.5f) * 2 + Main.maxTilesX / 2);
            int PoY = 20;

            while (!IsTileSmooth(new Point(PoX, PoY)))
            {
                PoX = (int)(Main.rand.Next(40, 240) * (Main.rand.Next(2) - 0.5f) * 2 + Main.maxTilesX / 2);
                for (int y = 20; y < Main.maxTilesY / 3; y++)
                {
                    if (Main.tile[PoX, y].HasTile)
                    {
                        PoY = y;
                        break;
                    }
                }
            }
            return new Point16(PoX, PoY);
        }
        private static bool IsTileSmooth(Point point, int Width = 22)
        {
            if (point.X > Main.maxTilesX - 20 || point.Y > Main.maxTilesY - 20 || point.X < 20 || point.Y < 20)
            {
                return false;
            }
            int x = point.X;
            int y = point.Y;
            var LeftTile = Main.tile[x, y];
            var RightTile = Main.tile[x + Width, y];
            var LeftTileUp = Main.tile[x, y - 1];
            var RightTileUp = Main.tile[x + Width, y - 1];
            if (!LeftTileUp.HasTile && !RightTileUp.HasTile)
            {
                if (LeftTile.HasTile && RightTile.HasTile)
                {
                    return true;
                }
            }
            return false;
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
