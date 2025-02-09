using Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furniture.TwilightForest;

public class RustBronzeTreasureChest_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<RustBronzeTreasureChest>());
		Item.width = 32;
		Item.height = 26;
		Item.value = 4110;
	}
}