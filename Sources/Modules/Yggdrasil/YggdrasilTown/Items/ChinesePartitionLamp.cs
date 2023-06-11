namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class ChinesePartitionLamp : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.ChinesePartitionLamp>());
		Item.width = 18;
		Item.height = 38;
		Item.value = 1000;
	}
}
