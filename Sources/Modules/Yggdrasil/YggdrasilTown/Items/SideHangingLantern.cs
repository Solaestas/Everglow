namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class SideHangingLantern : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SideHangingLantern>());
		Item.width = 22;
		Item.height = 20;
		Item.value = 1000;
	}
}
