namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class StoneDragonScaleWood : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.StoneScaleWood>());
		Item.width = 38;
		Item.height = 30;
	}
}
