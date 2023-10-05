namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class DarkForestSoil : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DarkForestSoil>());
	}
}
