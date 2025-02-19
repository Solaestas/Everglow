using Everglow.Yggdrasil.YggdrasilTown.Walls;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class UnionGoldAcanthusWall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<UnionGoldAcanthusWall>());
		Item.width = 24;
		Item.height = 24;
	}
}