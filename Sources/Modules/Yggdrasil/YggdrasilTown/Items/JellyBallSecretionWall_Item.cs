namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class JellyBallSecretionWall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.JellyBallSecretionWall>());
		Item.width = 24;
		Item.height = 24;
	}

	public override void AddRecipes()
	{
		CreateRecipe(4)
			.AddIngredient(ModContent.ItemType<JellyBallCube>(), 1)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}