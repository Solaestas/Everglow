namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class UnionOfficeDoor_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.UnionOfficeDoorClosed>());
		Item.width = 16;
		Item.height = 16;
	}
}