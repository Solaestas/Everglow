using Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.MinortopographyModule.LightHouse
{



    //TODO 目前生成世界有一点问题，我会自己解决这个问题的。

    /* Fork：Minortopography/GenPass
     * 
     * Everglow Dev Team
     * //MapIO
     * Ling Write 2022-12-3 12:40
     */
    public class LightHouse : ModSystem
    {
        private class LightHouseGenPass : GenPass
        {

            public LightHouseGenPass() : base("LightHouse", 500)
            {
            }


            //将东西写入WordGen里面并生效
            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                //Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everlow.Common.WorldSystem.BuildLightHouse");
                progress.Message = ("正在为[外海]添加废弃的灯塔……"); //先暂时主世界海边生成
                BuildLightHouse();
            }
        }

        //MapIO
        public static void QuickBuild(int x, int y, string Path)
        {
            MapIO mapIO = new MapIO(x, y);

            mapIO.Read(Everglow.Instance.GetFileStream("Sources/Modules/MinortopographyModule/MapIO/" + Path));

            var it = mapIO.GetEnumerator();
            while (it.MoveNext())
            {
                WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
                WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) => tasks.Add(new LightHouseGenPass());

        /// <summary>
        /// 生成废弃灯塔
        /// </summary>
        public static void BuildLightHouse()
        {
            //获取目标点

            Point16 CenterPoint = RandomPointInSea();
            int X0 = CenterPoint.X/(int)4.2;
            int Y0 = CenterPoint.Y-45;
            float Width = 5;

            for (int i = (int)-Width; i <= (int)Width; i++)
            {
                if (i <= -Width + 7 || i >= Width - 7)
                {
                    QuickBuild(X0, Y0, "LightHouseDemo.mapio");
                }
            }
        }

        //在世界上的
        public static bool InWorld(int x, int y, int fluff = 0)
        {
            if (x < fluff || x >= Main.maxTilesX - fluff || y < fluff || y >= Main.maxTilesY - fluff)
                return false;

            return true;
        }

        //灯塔与其他地形的校正

        public static Point16 RandomPointInSea()
        {
            List<Point16> AimPoint = new List<Point16>();
            int Jmin = Main.maxTilesY - 30;
            for (int i = 33; i < Main.maxTilesX - 34; i += 33)
            {
                for (int j = 12; j < Main.maxTilesY - 100; j += 6)
                {
                    Tile tile = Main.tile[i, j];
                    if (!LookNoLightHouse(i,j) && tile.HasTile)
                    {
                        AimPoint.Add(new Point16(i, j));
                        if (j < Jmin)
                        {
                            Jmin = j;
                        }
                        break;
                    }
                }
            }
            List<Point16> newAimPoint = new List<Point16>();
            foreach (Point16 point in AimPoint)
            {
                if (point.Y <= Jmin + 30)
                {
                    newAimPoint.Add(point);
                }
            }
            return newAimPoint[WorldGen.genRand.Next(0)];
        }

        public static bool LookNoLightHouse(int i, int y)
        {
            int CheckPoint = y;
            if (!InWorld(i, y))
                return false;

            while (TileID.Sets.TreeSapling[Main.tile[i, CheckPoint].TileType])
            {
                CheckPoint++;
                if (Main.tile[i, CheckPoint] == null)
                    return false;
            }

            Tile tile = Main.tile[i, CheckPoint];
            Tile tile2 = Main.tile[i, CheckPoint - 1];
            tile.Clone();
            tile.IsHalfBlock = true;
            if (!tile.HasTile || tile.IsHalfBlock || tile.Slope != 0)
                return false;

            if (tile2.WallType != 0 || tile2.LiquidType != 0)
                return false;

            if (tile.TileType != TileID.ShellPile && tile.TileType != 234 && tile.TileType != TileID.ShellPile && tile.TileType != 112 && !TileLoader.CanGrowModPalmTree(tile.TileType))
                return false;

            if (!EmptyTileCheck(i, i, CheckPoint - 2, CheckPoint - 1, 20))
                return false;

            if (!EmptyTileCheck(i - 1, i + 1, CheckPoint - 30, CheckPoint - 3, 20))
                return false;

            int testpointone = WorldGen.genRand.Next(10, 21);
            int testpointtwo = WorldGen.genRand.Next(-8, 9);
            testpointtwo *= 2;
            short reload = 0;
            for (int j = 0; j < testpointone; j++)
            {
                tile = Main.tile[i, CheckPoint - 1 - j];
                if (j == 0)
                {
                    tile.HasTile = true;
                    tile.TileType = TileID.ShellPile;
                    tile.TileFrameX = 66;
                    tile.TileFrameY = 0;
                    continue;
                }

                if (j == testpointone - 1)
                {
                    tile.HasTile = true;
                    tile.TileType = TileID.ShellPile;
                    tile.TileFrameX = (short)(22 * WorldGen.genRand.Next(4, 7));
                    tile.TileFrameY = reload;
                    continue;
                }

                if (reload != testpointtwo)
                {
                    float shortpoint = (float)j / (float)testpointone;
                    bool flag = false;
                    if (!(shortpoint < 0.25f) && ((shortpoint < 0.5f && WorldGen.genRand.Next(13) == 0) || (shortpoint < 0.7f && WorldGen.genRand.Next(9) == 0) || !(shortpoint < 0.95f) || WorldGen.genRand.Next(5) != 0 || true))
                    {
                        //无标记点的时候获取
                        short NoSignPoint = (short)Math.Sign(testpointtwo);
                        reload = (short)(reload + (short)(NoSignPoint * 2));
                    }
                }

                tile.HasTile = true;
                tile.TileType = TileID.ShellPile;
                tile.TileFrameX = (short)(22 * WorldGen.genRand.Next(0, 3));
                tile.TileFrameY = reload;
            }

            RangeFrame(i - 2, CheckPoint - testpointone - 1, i + 2, CheckPoint + 1);
            NetMessage.SendTileSquare(-1, i, CheckPoint - testpointone, 1, testpointone);
            return true;
        }

        //获取范围帧以重定向
        public static void RangeFrame(int startX, int startY, int endX, int endY)
        {
            //末点核查
            int ENDX = endX + 1;
            int ENDY = endY + 1;
            for (int i = startX - 1; i < ENDX + 1; i++)
            {
                for (int j = startY - 1; j < ENDY + 1; j++)
                {
                    //含有的就直接调用，毕竟谁也不想自己搓轮子坐牢（
                    WorldGen.TileFrame(i, j);
                    Framing.WallFrame(i, j);
                }
            }
        }


        //检查空白地块
        public static bool EmptyTileCheck(int startX, int endX, int startY, int endY, int ignoreID = -1)
        {
            if (startX < 0)
                return false;

            if (endX >= Main.maxTilesX)
                return false;

            if (startY < 0)
                return false;

            if (endY >= Main.maxTilesY)
                return false;

            bool flag = false;
            if (ignoreID != -1 && TileID.Sets.CommonSapling[ignoreID])
                flag = true;

            for (int i = startX; i < endX + 1; i++)
            {
                for (int j = startY; j < endY + 1; j++)
                {
                    //万象说过，用HasTile == false检查
                    //而在TML源代码中，是active()
                    //internal bool active() => HasTile;
                    if (!Main.tile[i, j].HasTile == false)
                        continue;

                    switch (ignoreID)
                    {
                        case -1:
                            return false;
                        case 11:
                            if (Main.tile[i, j].TileType == 11)
                                continue;
                            return false;
                        case 71:
                            if (Main.tile[i, j].TileType == 71)
                                continue;
                            return false;
                    }

                    if (flag)
                    {
                        if (TileID.Sets.CommonSapling[Main.tile[i, j].TileType])
                            break;

                        /*
						switch (Main.tile[i, j].type) {
							case 3:
							case 24:
							case 32:
							case 61:
							case 62:
							case 69:
							case 71:
							case 73:
							case 74:
							case 82:
							case 83:
							case 84:
							case 110:
							case 113:
							case 201:
							case 233:
							case 352:
							case 485:
							case 529:
							case 530:
								continue;
						}
						*/

                        if (TileID.Sets.IgnoredByGrowingSaplings[Main.tile[i, j].TileType])
                            continue;

                        return false;
                    }
                }
            }

            return true;
        }
    }
}