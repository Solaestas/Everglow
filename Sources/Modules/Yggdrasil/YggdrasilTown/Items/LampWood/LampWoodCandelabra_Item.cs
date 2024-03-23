namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;

public class LampWoodCandelabra_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.LampWoodCandelabra>());
	}
}