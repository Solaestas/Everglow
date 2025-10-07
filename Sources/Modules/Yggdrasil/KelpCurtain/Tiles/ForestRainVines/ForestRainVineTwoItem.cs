namespace Everglow.Yggdrasil.KelpCurtain.Tiles.ForestRainVines;

public class ForestRainVineTwoItem : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 24;
		Item.value = 40;
		Item.DefaultToPlaceableTile(ModContent.TileType<ForestRainVineTwoTile>());
	}

	public override void HoldItem(Player player)
	{
		Main.placementPreview = true;
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.BlueLight, 15)
			.AddIngredient(ItemID.Wire, 5)
			.AddTile(TileID.WorkBenches)
			.Register();
		CreateRecipe()
			.AddIngredient(ItemID.BlueLights)
			.AddIngredient(ItemID.Wire, 2)
			.AddTile(TileID.WorkBenches)
			.Register();
	}

	public override bool? UseItem(Player player)
	{
		return base.UseItem(player);
	}

	public override bool AltFunctionUse(Player player)
	{
		return true;
	}
}