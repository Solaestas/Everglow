using Everglow.Yggdrasil.Common.Walls;

namespace Everglow.Yggdrasil.Common.Items;

public class YggdrasilGrayRockWall_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<YggdrasilGrayRockWall>());
		Item.width = 24;
		Item.height = 24;
	}

	public override void AddRecipes()
	{
		CreateRecipe(4)
			.AddIngredient(ModContent.ItemType<YggdrasilGrayRock_Item>(), 1)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}