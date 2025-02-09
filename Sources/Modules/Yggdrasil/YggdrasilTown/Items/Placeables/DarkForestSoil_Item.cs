namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class DarkForestSoil_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.DarkForestSoil>());
	}
}