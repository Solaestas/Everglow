using Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;

public class LampWoodWall_item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.LampWood_Wood_Wall>());
		Item.width = 24;
		Item.height = 24;
	}
	public override void AddRecipes()
	{
		CreateRecipe(4)
			.AddIngredient(ModContent.ItemType<LampWood_Wood>(), 1)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
