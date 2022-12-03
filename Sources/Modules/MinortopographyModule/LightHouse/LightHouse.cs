using Everglow.Sources.Modules.MinortopographyModule.GiantPinetree.TilesAndWalls;
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
                //progress.Message = ("正在为[外海]添加废弃的灯塔……"); //先暂时主世界海边生成
                //BuildLightHouse();
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
            int X0 = CenterPoint.X;
            int Y0 = CenterPoint.Y + Main.rand.Next(15, 25);
            float Width = 5;

            for (int i = (int)-Width; i <= (int)Width; i++)
            {
                if (i <= -Width + 7 || i >= Width - 7)
                {
                    QuickBuild(X0, Y0, "LightHouseDemo.mapio");
                }
            }
        }

        public static Point16 RandomPointInSea()
        {
            //目标取点
            List<Point16> AimPoint = new List<Point16>();

            int Jmin = Main.maxTilesY - 100;
            for (int i = 33; i < Main.maxTilesX - 34; i += 33)
            {
                for (int j = 12; j < Main.maxTilesY - 100; j += 6)
                {
                    Tile tile = Main.tile[i, j];
                    //生成灯塔，选择棕榈树作为获取点随机取一点导致越界
                    if (tile.TileType == TileID.PalmWood && tile.HasTile)
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
            return newAimPoint[Main.rand.Next(newAimPoint.Count)];
        }
    }
}