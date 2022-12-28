using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheTusk.Tiles;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.Graphics.Effects;

namespace Everglow.Sources.Modules.MythModule.TheTusk.WorldGeneration
{
    public class TuskGen : ModSystem
    {
        public override void PostUpdateEverything()
        {
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
        /// <summary>
        /// 判定是否开启地形
        /// </summary>
        /// <returns></returns>
        public static bool TuskLandActive()
        {
            TuskGen tuskGen = ModContent.GetInstance<TuskGen>();
            Vector2 TuskBiomeCenter = new Vector2(tuskGen.tuskCenterX, tuskGen.tuskCenterY) * 16;
            Vector2 v0 = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f - TuskBiomeCenter;
            return v0.Length() < 1000;
        }
        internal float TuskS = 0;
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (TuskLandActive())
            {
                if (TuskS < 1)
                {
                    TuskS += 0.01f;
                }
                else
                {
                    TuskS = 1f;
                }


                if (!SkyManager.Instance["TuskSky"].IsActive())
                {
                    SkyManager.Instance.Activate("TuskSky");
                }
            }
            else
            {
                if (TuskS > 0)
                {
                    TuskS -= 0.01f;
                }
                else
                {
                    TuskS = 0;
                }
                if (SkyManager.Instance["TuskSky"].IsActive())
                {
                    SkyManager.Instance.Deactivate("TuskSky");
                }
            }
            tileColor *= (1 - TuskS * 0.6f);
            tileColor.G = (byte)(tileColor.G * (1 - TuskS * 0.6f));
            tileColor.B = (byte)(tileColor.B * (1 - TuskS * 0.6f));
            backgroundColor *= (1 - TuskS * 0.6f);
            backgroundColor.A = 255;
            base.ModifySunLightColor(ref tileColor, ref backgroundColor);
        }
        internal class WorldTuskLandGenPass : GenPass
        {
            public WorldTuskLandGenPass() : base("TuskLand", 500)//TODO:给大地安装血肉之颌
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everglow.Common.WorldSystem.BuildWorldTuskTable");
                BuildTuskLand();
            }
        }


        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) => tasks.Add(new WorldTuskLandGenPass());

        /// <summary>
        /// 地形中心坐标
        /// </summary>
        public int tuskCenterX = 400;

        public int tuskCenterY = 300;

        public override void SaveWorldData(TagCompound tag)
        {
            tag["TUSKcenterX"] = tuskCenterX;
            tag["TUSKcenterY"] = tuskCenterY;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            tuskCenterX = tag.GetAsInt("TUSKcenterX");
            tuskCenterY = tag.GetAsInt("TUSKcenterY");
        }
        public static void ShapeTile(string Shapepath, int a, int b, int type)
        {
            var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Sources/Modules/MythModule/TheTusk/WorldGeneration/" + Shapepath);
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
                                if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.ClearEverything();
                                    }
                                }
                                break;

                            case 1:
                                int Ty = 0;
                                int c = 0;
                                Vector2[] Plc = new Vector2[30];
                                Vector2[] Plc2 = new Vector2[60];
                                if (pixel.R == 158 && pixel.G == 26 && pixel.B == 37)
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.TileType = (ushort)ModContent.TileType<TuskFlesh>();
                                        tile.HasTile = true;
                                    }
                                }
                                if (pixel.R == 91 && pixel.G == 27 && pixel.B == 52)
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.TileType = (ushort)TileID.BoneBlock;
                                        tile.HasTile = true;
                                    }
                                }
                                if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.TileType = (ushort)ModContent.TileType<AbTuskFlesh>();
                                        tile.HasTile = true;
                                    }
                                }
                                break;

                            case 2:
                                if (pixel.R == 96 && pixel.G == 8 && pixel.B == 14)
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        tile.WallType = (ushort)ModContent.WallType<Walls.BloodyStoneWall>();
                                    }
                                }
                                break;
                        }
                    }
                }
            });
        }
        public static void BuildTuskLand()
        {
            Point abPos = GetTuskLandPosition();
            int a = abPos.X;
            int b = abPos.Y;
            Main.statusText = "CocoonStart";
            ShapeTile("BloodPlat.bmp", a, b, 1);
            Main.statusText = "TuskWallStart";
            ShapeTile("BloodPlatWall.bmp", a, b, 2);
            SmoothTuskTile(a, b, 160, 80);
            TuskGen tuskGen = ModContent.GetInstance<TuskGen>();
            tuskGen.tuskCenterX = a + 80;
            tuskGen.tuskCenterY = b + 10;
        }
        public static Point GetTuskLandPosition()
        {
            int a = (int)(Main.maxTilesX * 0.3);
            int b = (int)(Main.maxTilesY * 0.1);
            while (!CanPlaceTusk(new Point(a, b)))
            {
                a = (int)(Main.maxTilesX * Main.rand.NextFloat(0.1f, 0.88f));
                b = (int)(Main.maxTilesY * Main.rand.NextFloat(0.11f,0.31f));
            }
            return new Point(a, b);
        }
        public static bool CanPlaceTusk(Point position)
        {
            if(position.X < 20 || position.Y < 20)
            {
                return false;
            }
            if (position.X + 160 > Main.maxTilesX - 20 || position.Y + 80 > Main.maxTilesY - 20)
            {
                return false;
            }
            for (int x = 0;x < 161;x++)
            {
                for (int y = 0; y < 81; y++)
                {
                    if(Main.tile[x + position.X, y + position.Y].HasTile)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private static void SmoothTuskTile(int a, int b, int width = 256, int height = 512)
        {
            for (int y = 0; y < width; y += 1)
            {
                for (int x = 0; x < height; x += 1)
                {
                    if (Main.tile[x + a, y + b].TileType == (ushort)ModContent.TileType<Tiles.TuskFlesh>())
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
    
        public static void RandomUpdate(int i,int j,int Type)
        {
            if(Main.tile[i,j].TileType != Type || !Main.tile[i, j].HasTile)
            {
                return;
            } 
        }    
    }
}
