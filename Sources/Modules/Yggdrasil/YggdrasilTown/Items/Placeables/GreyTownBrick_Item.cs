namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class GreyTownBrick_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.GreyTownBrick>());
		Item.width = 16;
		Item.height = 16;
	}
}