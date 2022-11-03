using Terraria.DataStructures;
using Terraria.IO;
using Everglow.Sources.Modules.MinortopographyModule.GiantPinetree.TilesAndWalls;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.MinortopographyModule.GiantPinetree
{
    public class GiantPinetree : ModSystem
    {
        private class GiantPinetreeGenPass : GenPass
        {
            public GiantPinetreeGenPass() : base("GiantPinetree", 500)
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                //Todo:翻译：建造巨大的雪松
                Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everlow.Common.WorldSystem.BuildMothCave");
                BuildGiantPinetree();
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) => tasks.Add(new GiantPinetreeGenPass());
        /// <summary>
        /// 建造巨大的雪松
        /// </summary>
        public static void BuildGiantPinetree()
        {
            
            Point16 CenterPoint = RandomPointInSurfaceSnow();
            int X0 = CenterPoint.X;
            int Y0 = CenterPoint.Y - 26;//上移26格

            if (Main.snowBG[2] == 260)//在这种条件下，背景符合这段代码生成的松树
            {

            }
            float Width = Main.rand.NextFloat(24f, 32f);//随机摇宽度
            int j = 0;
            for (int a = -3; a <= 3; a++)
            {
                GenerateRoots(new Point16(X0, Y0), 0, a / 2f);//随机发射树根
            }
            while (Width > 0)
            {
                j--;
                if (j + Y0 >= Main.maxTilesY - 10 || j + Y0 <= 10 || -10 + X0 <= 10 || 10 + X0 >= Main.maxTilesX + 10)//防止超界
                {
                    break;
                }
                for (int i = (int)-Width; i <= (int)Width; i++)
                {
                    Tile tile = Main.tile[i + X0, j + Y0];
                    if (i <= -Width + 4 || i >= Width - 4)
                    {
                        tile.TileType = TileID.PineTree;
                        tile.HasTile = true;
                    }
                    if (i > -Width + 2 && i < Width - 2)
                    {
                        tile.WallType = (ushort)ModContent.WallType<PineLeavesWall>();
                    }
                }
                Width -= (float)(Math.Sin(j * 0.8) * 0.5 + 0.2);//制造松树一层一层的效果
            }
            GenerateRoots(new Point16(X0, Y0), 3.14159f, 3.14159f, false);//反向生成树根，作为树干
        }

        /// <summary>
        /// 建造根系,起始角度=0时向下,根系会在起始位置以起始角度发射,并逐渐转向目标角度,如果天然卷曲,根系会在末尾处再发生一次拐弯
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="startRotation"></param>
        /// <param name="trendRotation"></param>
        /// <param name="naturalCurve"></param>
        public static void GenerateRoots(Point16 Start, float startRotation = 0, float trendRotation = 0, bool naturalCurve = true)
        {
            int X0 = Start.X;
            int Y0 = Start.Y;
            float Width = Main.rand.NextFloat(8f, 10f);//随机摇宽度
            Vector2 IJ = new Vector2(0, 0);
            Vector2 RootVelocity = new Vector2(0, 1).RotatedBy(startRotation);//根系当前速度
            Vector2 RootTrendVelocity = new Vector2(0, 1).RotatedBy(trendRotation);//根系稳定趋势速度
            float omega = Main.rand.NextFloat(-0.2f, 0.2f);//末端旋转的角速度
            if (!naturalCurve)//如果禁止了自然旋转,角速度=0
            {
                omega = 0;
            }
            float StartToRotatedByOmega = Main.rand.NextFloat(1.81f, 3.62f);//算作末端的起始位置，这里用剩余宽度统计
            while (Width > 0)
            {
                for (int a = (int)-Width; a <= (int)Width; a++)
                {
                    Vector2 RootBuildingPosition = IJ + a * (RootVelocity).RotatedBy(MathHelper.PiOver2) * 0.6f;
                    int i = (int)(RootBuildingPosition.X);
                    int j = (int)(RootBuildingPosition.Y);
                    if (j + Y0 >= Main.maxTilesY - 10 || j + Y0 <= 10 || -10 + X0 <= 10 || 10 + X0 >= Main.maxTilesX + 10)//防止超界
                    {
                        break;
                    }
                    Tile tile = Main.tile[i + X0, j + Y0];
                    if (a <= -Width + 4 || a >= Width - 4)
                    {
                        if (tile.WallType != ModContent.WallType<PineWoodWall>())//防止松树块互相重合
                        {
                            tile.TileType = (ushort)ModContent.TileType<PineWood>();
                            tile.HasTile = true;
                        }

                    }
                    else
                    {
                        tile.HasTile = false;
                        tile.LiquidAmount = 0;
                    }
                    if (a > -Width + 2 && a < Width - 2)
                    {
                        tile.WallType = (ushort)ModContent.WallType<PineWoodWall>();
                    }
                }
                IJ += RootVelocity;
                if (Width > StartToRotatedByOmega)//没有收束到末端
                {
                    RootVelocity = RootVelocity * 0.95f + RootTrendVelocity * 0.05f;
                }
                else//已经收束到末端
                {
                    RootVelocity = RootVelocity.RotatedBy(omega * (StartToRotatedByOmega - Width) / StartToRotatedByOmega);
                }
                if (naturalCurve)//只有自然卷曲才会导致以下现象
                {
                    //重力因素也会影响根系,下面判定根系悬空程度
                    int AroundTileCount = 0;//我们判定周围存在方块的数量来推断悬空程度，存在方块越少越悬空
                    for (int b = 0; b < 12; b++)
                    {
                        Vector2 RootBuildingPosition = IJ + 3 * (RootVelocity).RotatedBy(b / 6d * Math.PI);
                        int i = (int)(RootBuildingPosition.X);
                        int j = (int)(RootBuildingPosition.Y);
                        Tile tile = Main.tile[i + X0, j + Y0];
                        if (tile.HasTile || tile.WallType == (ushort)ModContent.WallType<PineWoodWall>()/*这一项是为了防止自己干扰自己*/)
                        {
                            AroundTileCount++;
                        }
                    }
                    if (AroundTileCount < 6)
                    {
                        RootVelocity += new Vector2(0, (6 - AroundTileCount) / 16f);//重力自然下垂
                        RootVelocity = Vector2.Normalize(RootVelocity);//化作单位向量
                        Width += (6 - AroundTileCount) / 50f;//防止下降过程根系过分收束
                    }
                    else if (AroundTileCount > 9)
                    {
                        Width -= (AroundTileCount - 9) / 20f;//周围物块太多，产生阻力，加快收束
                    }
                }
                Width -= 0.1f;
                if(Width < 1.8f)//太细了，破掉吧
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 在雪地表面随机获取一点
        /// </summary>
        /// <returns></returns>
        public static Point16 RandomPointInSurfaceSnow()
        {
            List<Point16> AimPoint = new List<Point16>();
            int Jmin = Main.maxTilesY - 100;
            for (int i = 33; i < Main.maxTilesX - 34; i += 33)
            {
                for (int j = 12; j < Main.maxTilesY - 100; j += 6)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile.TileType == TileID.SnowBlock && tile.HasTile)
                    {
                        AimPoint.Add(new Point16(i, j));
                        if(j < Jmin)
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
                if(point.Y <= Jmin + 30)
                {
                    newAimPoint.Add(point);
                }
            }
            return newAimPoint[Main.rand.Next(newAimPoint.Count)];
        }
    }
}

