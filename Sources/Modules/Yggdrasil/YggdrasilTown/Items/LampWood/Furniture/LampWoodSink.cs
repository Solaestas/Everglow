namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood.Furniture;

// TODO: Replace sprite
public class LampWoodSink : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.Furniture.LampWoodSink>());
	}
}