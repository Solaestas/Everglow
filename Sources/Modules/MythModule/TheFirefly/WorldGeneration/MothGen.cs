using Everglow.Sources.Modules.MythModule.Common;
using System.Drawing;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.IO;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration
{
    //TODO 这种小东西可以合起来丢到一个ModPlayer里面
    public class MothLandSyncPlayer : ModPlayer
    {
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if(newPlayer)
            {
                //新玩家进入世界是发送请求
                Everglow.PacketResolver.Send(new MothPositionPacket());
            }
        }
    }

    public class MothLand : ModSystem
    {
        private class MothLandGenPass : GenPass
        {
            public MothLandGenPass() : base("MothLand", 500)
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                BuildMothCave();
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) => tasks.Add(new MothLandGenPass());
        /// <summary>
        /// 地形中心坐标
        /// </summary>
        public int fireflyCenterX = 2000;
        public int fireflyCenterY = 500;

        public override void SaveWorldData(TagCompound tag)
        {
            tag["FIREFLYcenterX"] = fireflyCenterX;
            tag["FIREFLYcenterY"] = fireflyCenterY;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            fireflyCenterX = tag.GetAsInt("FIREFLYcenterX");
            fireflyCenterY = tag.GetAsInt("FIREFLYcenterY");
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
            if (!OperatingSystem.IsWindows())
            {
                throw new Exception("Windows限定");
            }
            using Stream Img = Everglow.Instance.GetFileStream("Sources/Modules/MythModule/TheFirefly/WorldGeneration/" + Shapepath);
            Bitmap cocoon = new Bitmap(Img);
            for (int y = 0; y < cocoon.Height; y += 1)
            {
                for (int x = 0; x < cocoon.Width; x += 1)
                {
                    Tile tile = Main.tile[x + a, y + b];
                    switch (type)//21是箱子
                    {
                        case 0:
                            if (CheckColor(cocoon.GetPixel(x, y), new Vector4(255, 0, 0, 255)))
                            {
                                if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                {
                                    tile.ClearEverything();
                                }
                            }
                            break;
                        case 1:
                            if (CheckColor(cocoon.GetPixel(x, y), new Vector4(56, 48, 61, 255)))
                            {
                                if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                {
                                    tile.TileType = (ushort)ModContent.TileType<Tiles.DarkCocoon>();
                                    tile.HasTile = true;
                                }
                            }
                            if (CheckColor(cocoon.GetPixel(x, y), new Vector4(0, 0, 255, 255)))
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
                            if (CheckColor(cocoon.GetPixel(x, y), new Vector4(0, 0, 5, 255)))
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
        }
        /// <summary>
        /// 建造流萤之茧
        /// </summary>
        public static void BuildMothCave()
        {
            Point16 AB = CocoonPos();
            int a = AB.X;
            int b = AB.Y;
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            mothLand.fireflyCenterX = a + 140;
            mothLand.fireflyCenterY = b + 140;
            ShapeTile("CocoonKill.bmp", a, b, 0);
            ShapeTile("Cocoon.bmp", a, b, 1);
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
                60,//丛林草方块
                61,//丛林草
                62,//丛林藤
                74,//高大丛林草
                233//丛林花
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
        /// <summary>
        /// 判定颜色是否吻合
        /// </summary>
        /// <param name="c0"></param>
        /// <param name="RGBA"></param>
        /// <returns></returns>
        private static bool CheckColor(System.Drawing.Color c0, Vector4 RGBA)
        {
            Vector4 v0 = new Vector4(c0.R, c0.G, c0.B, c0.A);
            return v0 == RGBA;
        }
    }
}

