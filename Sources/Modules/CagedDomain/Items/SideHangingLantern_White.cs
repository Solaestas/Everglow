namespace Everglow.CagedDomain.Items;

public class SideHangingLantern_White : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SideHangingLantern_White>());
		Item.width = 22;
		Item.height = 20;
		Item.value = 1000;
	}
}
