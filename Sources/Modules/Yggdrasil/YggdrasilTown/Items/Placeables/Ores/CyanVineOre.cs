namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Ores;

public class CyanVineOre : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.CyanVine.CyanVineStone>());
		Item.width = 22;
		Item.height = 22;
		Item.value = 400;
	}
}