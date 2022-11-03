using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader.IO;
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
            int Y0 = CenterPoint.Y + 10;
            float Width = Main.rand.NextFloat(8f,10f);
            int j = 0;
            while(Width > 0)
            {
                j--;
                if(j + Y0 >= Main.maxTilesY - 10 || j + Y0 <= 10 || -10 + X0 <= 10 || 10 + X0 >= Main.maxTilesX + 10)
                {
                    break;
                }
                for (int i = (int)-Width; i <= (int)Width; i++)
                {
                    Tile tile = Main.tile[i + X0, j + Y0];
                    if(i <= -Width + 2 || i >= Width - 2)
                    {
                        tile.TileType = TileID.PineTree;
                        tile.HasTile = true;
                    }
                    if (i > -Width + 1 && i < Width - 1)
                    {
                        tile.WallType = WallID.LivingWood;
                    }
                }
                Width -= 0.08f;
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

