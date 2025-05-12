using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class SteamBoiler_Item : ModItem
{
	public int State = 0;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<SteamBoiler>());
		Item.width = 36;
		Item.height = 28;
	}
}