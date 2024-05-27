using Everglow.CagedDomain.Tiles;

namespace Everglow.CagedDomain.Items;

public class LapisLazuliDome_mini_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<LapisLazuliDome_mini>());
		Item.width = 36;
		Item.height = 14;
	}
}