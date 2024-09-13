namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;

public class LampWood_Wood : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.LampWood_Wood_Tile>());
	}
}
