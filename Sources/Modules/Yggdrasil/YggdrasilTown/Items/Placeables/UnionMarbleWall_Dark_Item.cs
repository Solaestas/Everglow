using Everglow.Yggdrasil.YggdrasilTown.Walls;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class UnionMarbleWall_Dark_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<UnionMarbleWall_Dark>());
		Item.width = 24;
		Item.height = 24;
	}
}