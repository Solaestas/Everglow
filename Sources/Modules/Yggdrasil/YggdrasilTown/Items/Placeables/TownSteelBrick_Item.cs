namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class TownSteelBrick_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TownSteelBrick>());
		Item.width = 16;
		Item.height = 16;
	}
}