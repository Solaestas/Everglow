namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class PierWithSlabs : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.PierWithSlabs>());
		Item.width = 26;
		Item.height = 26;
		Item.value = 2046;
	}
}
