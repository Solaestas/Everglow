using Everglow.Yggdrasil.YggdrasilTown.Walls;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class HeatProofPlatingWall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<HeatProofPlatingWall>());
		Item.width = 24;
		Item.height = 24;
	}
}