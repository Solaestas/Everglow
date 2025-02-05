using Everglow.Yggdrasil.YggdrasilTown.Walls.TwilightForest;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.TwilightForest;

public class GreenRelicWall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<GreenRelicWall>());
		Item.width = 24;
		Item.height = 24;
	}

	public override void AddRecipes()
	{
		CreateRecipe(4)
			.AddIngredient(ModContent.ItemType<GreenRelicBrick_Item>(), 1)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}