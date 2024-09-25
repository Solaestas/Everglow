namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood.Furniture;

// TODO: Replace sprite
public class LampWoodToilet : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.Furniture.LampWoodToilet>());
	}
}