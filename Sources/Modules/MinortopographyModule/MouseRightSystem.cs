using Everglow.Sources.Modules.MinortopographyModule.GiantPinetree.TilesAndWalls;

namespace Everglow.Sources.Modules.MinortopographyModule
{
    /// <summary>
    /// ���System��������Ϊ�˷������,�Ҽ���һ�¾ͻ�ִ��һЩ���롣//TODO:��ʽPR��ʱ��ǵ���ȫɾ��
    /// </summary>
    public class MouseRightSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            if (Main.mouseRight && Main.mouseRightRelease)
            {
                //int X = (int)(Main.MouseWorld.X / 16);
                //int Y = (int)(Main.MouseWorld.Y / 16);
                //GiantPinetree.GiantPinetree.BuildGiantPinetree();
                //placeBranch(X, Y, 0, 120, new Vector2(0, -1));
            }
        }
        //public void placeBranch(int i, int j, int iteration, float strength, Vector2 direction)
        //{
        //    if (iteration > 50)//��һ��ɢ������
        //        return;
        //    for (int x = 0; x < strength; x++)
        //    {
        //        int ABSXStr = Math.Min((int)((strength - x) * 0.16f), 8);

        //        for (int y = -ABSXStr; y < ABSXStr + 1; y++)
        //        {
        //            Vector2 normalizedDirection = Utils.SafeNormalize(direction, new Vector2(0, -1));
        //            Vector2 VnormalizedDirection = normalizedDirection.RotatedBy(Math.PI / 2d);
        //            int a = (int)(i + normalizedDirection.X * x + VnormalizedDirection.X * y);
        //            int b = (int)(j + normalizedDirection.Y * x + VnormalizedDirection.Y * y);
        //            var tile = Main.tile[a, b];
        //            tile.TileType = TileID.PineTree;
        //            tile.HasTile = true;
        //            if (strength - x > 1)
        //            {
        //                tile.WallType = (ushort)ModContent.WallType<PineLeavesWall>();
        //            }
        //            if(y == 0)
        //            {
        //                if (x % 6 == 1)
        //                {
        //                    placeBranch(a, b, iteration + 1, (strength - x) * 0.3f, normalizedDirection.RotatedBy(Math.PI * 0.3));
        //                }
        //                if (x % 6 == 4)
        //                {
        //                    placeBranch(a, b, iteration + 1, (strength - x) * 0.3f, normalizedDirection.RotatedBy(-Math.PI * 0.3));
        //                }
        //            }
        //        }
        //    }
        //}
    }
}

