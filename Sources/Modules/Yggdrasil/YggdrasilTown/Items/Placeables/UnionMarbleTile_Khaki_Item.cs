namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class UnionMarbleTile_Khaki_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.UnionMarbleTile_Khaki>());
		Item.width = 16;
		Item.height = 16;
	}
}