namespace Everglow.Yggdrasil.Common.Tiles;

public class YggdrasilWorldPylon_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<YggdrasilWorldPylon>());
	}
}