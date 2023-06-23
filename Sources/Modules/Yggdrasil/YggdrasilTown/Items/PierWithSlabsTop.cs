namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class PierWithSlabsTop : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.PierWithSlabsTop>());
		Item.width =50;
		Item.height = 34;
		Item.value = 7086;
	}
}
