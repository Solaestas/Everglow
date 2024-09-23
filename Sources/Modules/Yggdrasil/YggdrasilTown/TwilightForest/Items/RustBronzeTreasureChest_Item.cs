using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items;

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