using Everglow.Food.Tiles;

namespace Everglow.Food.Items.Cookers;

public class CleaverRack_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<CleaverRack>());
		Item.width = 30;
		Item.height = 26;
		Item.value = 1000;
	}
}