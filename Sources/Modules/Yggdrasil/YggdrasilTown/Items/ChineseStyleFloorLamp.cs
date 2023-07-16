namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class ChineseStyleFloorLamp : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.ChineseStyleFloorLamp>());
		Item.width = 18;
		Item.height = 38;
		Item.value = 1000;
	}
}
