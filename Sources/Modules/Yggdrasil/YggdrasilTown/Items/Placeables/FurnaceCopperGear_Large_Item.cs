namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FurnaceCopperGear_Large_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceCopperGear_Large>());
		Item.width = 16;
		Item.height = 16;
	}

	public override void HoldItem(Player player)
	{
		Item.placeStyle = WorldGen.genRand.Next(4);
	}
}