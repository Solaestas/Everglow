using Everglow.Commons.EliminateLight;

namespace Everglow.Example.Items;

public class ExampleVitrualWallLightBlocker : ModItem
{
	public override void HoldItem(Player player)
	{
		int range = 48;
		if (Main.mouseLeft)
		{
			//Point tilePos = Main.MouseWorld.ToTileCoordinates();
			//for (int i = -range; i <= range; i++)
			//{
			//	for (int j = -range; j <= range; j++)
			//	{
			//		EliminateLight.AddVirtualWall(tilePos + new Point(i, j));
			//	}
			//}
			List<Vector2> polygon = new List<Vector2>();
			for(int i = 0;i < 10;i++)
			{
				float mulRange = 1f;
				if(i % 2 == 0)
				{
					mulRange = 0.5f;
				}
				polygon.Add(Main.MouseWorld + new Vector2(0, -range * 16 * mulRange).RotatedBy(i / 10f * MathHelper.TwoPi + Main.GlobalTimeWrappedHourly));
			}
			EliminateLight.AddVirtualWall_Polygon(polygon);
		}
		base.HoldItem(player);
	}
}