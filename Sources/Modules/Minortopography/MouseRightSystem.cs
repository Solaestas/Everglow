namespace Everglow.Minortopography;

/// <summary>
/// 这个System的作用是为了方便测试,右键戳一下就会执行一些代码。//TODO:正式PR的时候记得完全删除
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
	//    if (iteration > 50)//万一发散就完了
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

