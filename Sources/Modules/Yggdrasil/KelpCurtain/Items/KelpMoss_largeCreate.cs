using Everglow.Yggdrasil.KelpCurtain.Tiles;
namespace Everglow.Yggdrasil.KelpCurtain.Items;

public class KelpMoss_largeCreate : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<KelpMoss_large_tile>());
		Item.width = 16;
		Item.height = 16;
	}
}
