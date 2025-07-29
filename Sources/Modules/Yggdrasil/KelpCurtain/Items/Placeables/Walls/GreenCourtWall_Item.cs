using Everglow.Yggdrasil.KelpCurtain.Walls;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables.Walls;

public class GreenCourtWall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<GreenCourtWall>());
		Item.width = 24;
		Item.height = 24;
	}

	public override void AddRecipes()
	{
		CreateRecipe(4)
			.AddIngredient(ModContent.ItemType<GreenCourtBrick_Item>(), 1)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}