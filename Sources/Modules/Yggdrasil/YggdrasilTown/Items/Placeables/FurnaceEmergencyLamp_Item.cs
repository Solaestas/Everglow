namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FurnaceEmergencyLamp_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceEmergencyLamp>());
		Item.width = 16;
		Item.height = 16;
	}
}