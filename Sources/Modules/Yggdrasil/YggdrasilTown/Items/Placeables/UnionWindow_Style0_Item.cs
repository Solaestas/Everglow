namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class UnionWindow_Style0_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.UnionWindow_Style0>());
		Item.width = 16;
		Item.height = 16;
	}
}