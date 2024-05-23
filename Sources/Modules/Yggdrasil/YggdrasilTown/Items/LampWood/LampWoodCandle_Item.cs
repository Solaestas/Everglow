namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;

public class LampWoodCandle_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.LampWoodCandle>());
	}
}