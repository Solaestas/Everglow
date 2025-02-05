using Everglow.Yggdrasil.YggdrasilTown.Walls.TwilightForest;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.TwilightForest;

public class TwilightEucalyptusWoodWall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<TwilightEucalyptusWoodWall>());
		Item.width = 24;
		Item.height = 24;
	}

	public override void AddRecipes()
	{
		CreateRecipe(4)
			.AddIngredient(ModContent.ItemType<TwilightEucalyptusWood_Item>(), 1)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}