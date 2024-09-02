using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Walls;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items;

public class GreenRelicWall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<GreenRelicWall>());
		Item.width = 24;
		Item.height = 24;
	}
}