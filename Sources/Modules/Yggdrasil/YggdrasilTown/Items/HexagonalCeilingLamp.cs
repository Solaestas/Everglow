namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class HexagonalCeilingLamp : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.HexagonalCeilingLamp>());
		Item.width = 22;
		Item.height = 26;
		Item.value = 1000;
	}
}