using Everglow.Yggdrasil.Common.Tiles;

namespace Everglow.Yggdrasil.Common.Items;

public class YggdrasilBlackRock_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<YggdrasilBlackRock>());
		Item.width = 16;
		Item.height = 16;
	}
}