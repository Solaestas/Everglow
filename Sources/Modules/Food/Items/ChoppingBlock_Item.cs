using Everglow.Food.Tiles;

namespace Everglow.Food.Items;

public class ChoppingBlock_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<ChoppingBlock>());
		Item.width = 32;
		Item.height = 40;
		Item.value = 1000;
	}

	public override void HoldItem(Player player)
	{
		var point = Main.MouseWorld.ToTileCoordinates();
		var tile = Main.tile[point];
		var tileBelow = Main.tile[point + new Point(0, 1)];
		if(tileBelow.HasTile)
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<ChoppingBlock>());
			Item.width = 32;
			Item.height = 40;
			Item.value = 1000;
		}
		else if(tile.wall != 0)
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<ChoppingBlock_Hang>());
			Item.width = 32;
			Item.height = 40;
			Item.value = 1000;
		}
	}
}