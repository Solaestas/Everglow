namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class UnionWallLamp_Style0_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.UnionWallLamp_Style0>());
		Item.width = 22;
		Item.height = 20;
		Item.value = 1000;
	}
}