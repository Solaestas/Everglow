using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheTusk.Tiles;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;

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
            return v0.Length() < 2000;
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
                                if (pixel.R == 155 && pixel.G == 66 && pixel.B == 183)
                                {
                                    if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                    {
                                        Main.tile[x + a, y + b].TileType = (ushort)ModContent.TileType<Tiles.TuskFlesh>();
                                        ((Tile)Main.tile[x + a, y + b]).HasTile = true;
                                        if (Main.netMode == 1)
                                        {
                                            NetMessage.SendTileSquare(Main.myPlayer, x + a, y + b);
                                        }
                                        for (int h = 0; h < 200; h++)
                                        {
                                            Main.tile[x + a, y + b + h].TileType = (ushort)ModContent.TileType<Tiles.TuskFlesh>();
                                            ((Tile)Main.tile[x + a, y + b + h]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, x + a, y + b + h);
                                            }
                                            Main.tile[x + a + 1, y + b + h].TileType = (ushort)ModContent.TileType<Tiles.TuskFlesh>();
                                            ((Tile)Main.tile[x + a + 1, y + b + h]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, x + a + 1, y + b + h);
                                            }
                                            if (((Tile)Main.tile[x + a, y + b + h + 12]).HasTile && Main.tile[x + a, y + b + h + 12].TileType != (ushort)ModContent.TileType<Tiles.TuskFlesh>() && Main.tile[x + a, y + b + h + 12].TileType != 208)
                                            {
                                                Plc[c] = new Vector2(x + a, y + b + h + 3);
                                                c++;
                                                Ty = y + h;
                                                break;
                                            }
                                        }
                                    }
                                }
                                for (int k = 0; k < 30; k += 1)
                                {
                                    if (Plc[k] == Vector2.Zero)
                                    {
                                        break;
                                    }
                                    Plc2[k * 2] = Plc[k];
                                }
                                for (int k = 0; k < 29; k += 1)
                                {
                                    if (Plc[k + 1] == Vector2.Zero)
                                    {
                                        break;
                                    }
                                    Plc2[k * 2 + 1] = Plc[k] * 0.5f + Plc[k + 1] * 0.5f;
                                }

                                for (int k = 3; k < 58; k += 2)
                                {
                                    if (Plc2[k] == Vector2.Zero)
                                    {
                                        break;
                                    }
                                    for (int l = 0; l < 30; l += 1)
                                    {
                                        float t = l / 30f;
                                        Vector2 v = Plc2[k - 2] * t + Plc2[k] * (1 - t);
                                        ushort type = (ushort)ModContent.TileType<Tiles.TuskFlesh>();
                                        int WALL = 0;
                                        int MaxC = 30;

                                        if (k >= 7 && k <= 27)
                                        {
                                            WALL = 1;
                                        }

                                        for (int h = 1; h < 21; h += 1)
                                        {
                                            if ((int)v.Y - 3 + h > 20 && (int)v.Y - 3 + h < Main.maxTilesY && (int)v.X > 20 && (int)v.X < Main.maxTilesX)
                                            {
                                                if (k < 7)
                                                {
                                                    MaxC = (int)v.X - a - 20;
                                                }
                                                if (k > 27)
                                                {
                                                    MaxC = a + 135 - (int)v.X;
                                                }
                                                if (MaxC > 30)
                                                {
                                                    MaxC = 30;
                                                }
                                                if (h <= MaxC)
                                                {
                                                    Main.tile[(int)v.X, (int)v.Y - 3 + h].ClearEverything();
                                                    if (Main.netMode == 1)
                                                    {
                                                        NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y - 3 + h);
                                                    }
                                                }

                                                if (WALL == 1)
                                                {
                                                    Main.tile[(int)v.X, (int)v.Y - 3 + h].WallType = (ushort)ModContent.WallType<Walls.BloodyStoneWall>();
                                                    if (Main.netMode == 1)
                                                    {
                                                        NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y - 3 + h);
                                                    }
                                                }
                                                else
                                                {
                                                    if ((int)v.X % 6 >= 4)
                                                    {
                                                        Main.tile[(int)v.X, (int)v.Y - 3 + h].WallType = (ushort)ModContent.WallType<Walls.BloodyStoneWall>();
                                                        if (Main.netMode == 1)
                                                        {
                                                            NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y - 3 + h);
                                                        }
                                                    }
                                                }

                                                if (h >= 18)
                                                {
                                                    Main.tile[(int)v.X, (int)v.Y - 3 + h].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                                    ((Tile)Main.tile[(int)v.X, (int)v.Y - 3 + h]).HasTile = true;
                                                    if (Main.netMode == 1)
                                                    {
                                                        NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y - 3 + h);
                                                    }
                                                }
                                            }
                                        }
                                        if ((int)v.Y - 3 > 20 && (int)v.Y - 3 < Main.maxTilesY && (int)v.X > 20 && (int)v.X < Main.maxTilesX)
                                        {
                                            Main.tile[(int)v.X, (int)v.Y - 2].TileType = type;
                                            ((Tile)Main.tile[(int)v.X, (int)v.Y - 2]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y - 2);
                                            }
                                            Main.tile[(int)v.X, (int)v.Y - 3].TileType = type;
                                            ((Tile)Main.tile[(int)v.X, (int)v.Y - 3]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y - 3);
                                            }
                                            Main.tile[(int)v.X, (int)v.Y - 4].TileType = type;
                                            ((Tile)Main.tile[(int)v.X, (int)v.Y - 4]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y - 4);
                                            }
                                            Main.tile[(int)v.X, (int)v.Y - 5].TileType = type;
                                            ((Tile)Main.tile[(int)v.X, (int)v.Y - 5]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y - 5);
                                            }
                                        }
                                        if (Math.Abs((int)v.X - a - 70) <= 8)
                                        {
                                            Ty = (int)v.Y - 4;
                                        }
                                    }
                                }
                                for (int k = 3; k < 58; k += 2)
                                {
                                    if (Plc2[k] == Vector2.Zero)
                                    {
                                        break;
                                    }
                                    if (k == 7)
                                    {
                                        Vector2 v = Plc2[k] - new Vector2(0, 3);
                                        for (int za = -2; za < 3; za++)
                                        {
                                            if ((int)v.Y + 18 > 20 && (int)v.Y + 18 < Main.maxTilesY && (int)v.X + za > 20 && (int)v.X + za < Main.maxTilesX)
                                            {
                                                Main.tile[(int)v.X + za, (int)v.Y + 17].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                                ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 17]).HasTile = true;
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 17);
                                                }
                                                Main.tile[(int)v.X + za, (int)v.Y + 18].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                                ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 18]).HasTile = true;
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 18);
                                                }
                                                Main.tile[(int)v.X + za, (int)v.Y + 19].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                                ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 19]).HasTile = true;
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 19);
                                                }
                                            }
                                        }
                                        for (int ia = 0; ia < 7; ia++)
                                        {
                                            if ((int)v.Y + 10 + ia > 20 && (int)v.Y + 10 + ia < Main.maxTilesY && (int)v.X > 20 && (int)v.X < Main.maxTilesX)
                                            {
                                                ((Tile)Main.tile[(int)v.X, (int)v.Y + 10 + ia]).HasTile = true;
                                                Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileType = (ushort)ModContent.TileType<Tiles.StrangeTuskStone>();
                                                short num0 = 0;
                                                Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileFrameX = num0;
                                                Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileFrameY = (short)(ia * 18);
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y + 10 + ia);
                                                }
                                            }
                                        }
                                    }
                                    if (k == 13)
                                    {
                                        Vector2 v = Plc2[k] - new Vector2(0, 3);
                                        for (int za = -2; za < 3; za++)
                                        {
                                            if ((int)v.Y + 18 > 20 && (int)v.Y + 18 < Main.maxTilesY && (int)v.X > 20 && (int)v.X < Main.maxTilesX)
                                            {
                                                Main.tile[(int)v.X + za, (int)v.Y + 17].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                                ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 17]).HasTile = true;
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 17);
                                                }
                                                Main.tile[(int)v.X + za, (int)v.Y + 18].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                                ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 18]).HasTile = true;
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 18);
                                                }
                                                Main.tile[(int)v.X + za, (int)v.Y + 19].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                                ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 19]).HasTile = true;
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 19);
                                                }
                                            }
                                        }
                                        for (int ia = 0; ia < 7; ia++)
                                        {
                                            ((Tile)Main.tile[(int)v.X, (int)v.Y + 10 + ia]).HasTile = true;
                                            Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileType = (ushort)ModContent.TileType<Tiles.StrangeTuskStone>();
                                            short num0 = 192;
                                            Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileFrameX = num0;
                                            Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileFrameY = (short)(ia * 18);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y + 10 + ia);
                                            }
                                        }
                                    }
                                    if (k == 21)
                                    {
                                        Vector2 v = Plc2[k] - new Vector2(0, 3);
                                        for (int za = -2; za < 3; za++)
                                        {
                                            Main.tile[(int)v.X + za, (int)v.Y + 17].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                            ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 17]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 17);
                                            }
                                            Main.tile[(int)v.X + za, (int)v.Y + 18].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                            ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 18]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 18);
                                            }
                                            Main.tile[(int)v.X + za, (int)v.Y + 19].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                            ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 19]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 19);
                                            }
                                        }
                                        for (int ia = 0; ia < 7; ia++)
                                        {
                                            ((Tile)Main.tile[(int)v.X, (int)v.Y + 10 + ia]).HasTile = true;
                                            Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileType = (ushort)ModContent.TileType<Tiles.StrangeTuskStone>();
                                            short num0 = 64;
                                            Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileFrameX = num0;
                                            Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileFrameY = (short)(ia * 18);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y + 10 + ia);
                                            }
                                        }
                                    }
                                    if (k == 27)
                                    {
                                        Vector2 v = Plc2[k] - new Vector2(0, 3);
                                        for (int za = -2; za < 3; za++)
                                        {
                                            Main.tile[(int)v.X + za, (int)v.Y + 17].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                            ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 17]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 17);
                                            }
                                            Main.tile[(int)v.X + za, (int)v.Y + 18].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                            ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 18]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 18);
                                            }
                                            Main.tile[(int)v.X + za, (int)v.Y + 19].TileType = (ushort)ModContent.TileType<Tiles.BloodMossStone>();
                                            ((Tile)Main.tile[(int)v.X + za, (int)v.Y + 19]).HasTile = true;
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X + za, (int)v.Y + 19);
                                            }
                                        }
                                        for (int ia = 0; ia < 7; ia++)
                                        {
                                            ((Tile)Main.tile[(int)v.X, (int)v.Y + 10 + ia]).HasTile = true;
                                            Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileType = (ushort)ModContent.TileType<Tiles.StrangeTuskStone>();
                                            short num0 = 128;
                                            Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileFrameX = num0;
                                            Main.tile[(int)v.X, (int)v.Y + 10 + ia].TileFrameY = (short)(ia * 18);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendTileSquare(Main.myPlayer, (int)v.X, (int)v.Y + 10 + ia);
                                            }
                                        }
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

            int a;
            int b;

            for (int h0 = 50; h0 < Main.maxTilesY - 400; h0 += 1)
            {
                int xk = 0;
                for (int w0 = 20; w0 < Main.maxTilesX - 240; w0 += 10)
                {
                    if (Main.tile[w0, h0].TileType == 203)
                    {
                        h0 = Main.maxTilesY - 200;
                        xk = 2;
                        break;
                    }
                    if (Main.tile[w0, h0].TileType == 116)
                    {
                        h0 = Main.maxTilesY - 200;
                        xk = 2;
                        break;
                    }
                }
                if (xk > 1)
                {
                    break;
                }
            }
            a = (int)(Main.maxTilesX * 0.3);

            b = (int)(Main.maxTilesY * 0.1);
            if (Main.maxTilesX > 6000)
            {
                b = (int)(Main.maxTilesY * 0.16);
            }
            Main.statusText = "BloodPlatKillStart";
            ShapeTile("BloodPlatKill.bmp", a, b, 0);
            Main.statusText = "CocoonStart";
            ShapeTile("BloodPlat.bmp", a, b, 1);
            Main.statusText = "TuskWallStart";
            ShapeTile("BloodPlatWall.bmp", a, b, 2);
            SmoothMothTile(a, b,160,80);
            TuskGen tuskGen = ModContent.GetInstance<TuskGen>();
            tuskGen.tuskCenterX = a + 80;
            tuskGen.tuskCenterY = b + 40;
        }

        /// <summary>
        /// 获取一个出生地附近的平坦地面
        /// </summary>
        /// <returns></returns>
        
        private static void SmoothMothTile(int a, int b, int width = 256, int height = 512)
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
