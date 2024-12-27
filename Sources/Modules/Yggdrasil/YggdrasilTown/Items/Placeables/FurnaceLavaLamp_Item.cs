namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FurnaceLavaLamp_Item : ModItem
{
	public int State = 0;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceLavaLamp_H>());
		Item.width = 16;
		Item.height = 16;
	}

	public override void HoldItem(Player player)
	{
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			State += 1;
			State %= 2;
		}
		if (State == 0)
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceLavaLamp_H>());
		}
		else if (State == 1)
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceLavaLamp_V>());
		}
	}
}