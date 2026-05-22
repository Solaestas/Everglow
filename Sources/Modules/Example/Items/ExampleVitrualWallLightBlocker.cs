using Everglow.Commons.EliminateLight;

namespace Everglow.Example.Items;

public class ExampleVitrualWallLightBlocker : ModItem
{
	public override void HoldItem(Player player)
	{
		int range = 6;
		if (Main.mouseLeft)
		{
			Point tilePos = Main.MouseWorld.ToTileCoordinates();
			for (int i = -range; i <= range; i++)
			{
				for (int j = -range; j <= range; j++)
				{
					EliminateLight.AddVirtualWall(tilePos + new Point(i, j));
				}
			}
		}
		base.HoldItem(player);
	}
}