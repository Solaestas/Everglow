namespace Everglow.Yggdrasil.YggdrasilTown.Items.CyanVine;

public class CyanVineBar : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.CyanVine.CyanVineBar>());
		Item.width = 30;
		Item.height = 24;
		Item.value = 1600;
	}
	public override void AddRecipes()
	{
		CreateRecipe(1)
			.AddIngredient(ModContent.ItemType<CyanVineOre>(), 4)
			.AddTile(TileID.Furnaces)
			.Register();
	}
}
