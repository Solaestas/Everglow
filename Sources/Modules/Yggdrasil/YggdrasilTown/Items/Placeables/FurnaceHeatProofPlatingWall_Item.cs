using Everglow.Yggdrasil.YggdrasilTown.Walls;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FurnaceHeatProofPlatingWall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<FurnaceHeatProofPlatingWall>());
		Item.width = 24;
		Item.height = 24;
	}
}