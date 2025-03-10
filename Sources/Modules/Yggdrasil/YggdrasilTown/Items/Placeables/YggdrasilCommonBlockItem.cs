using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class YggdrasilCommonBlockItem : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<YggdrasilCommonBlock>());
		Item.width = 16;
		Item.height = 16;
	}
}