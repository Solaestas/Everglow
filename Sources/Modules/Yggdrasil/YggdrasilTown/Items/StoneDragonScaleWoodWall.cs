namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class StoneDragonScaleWoodWall : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.StoneDragonScaleWoodWall>());
		Item.width = 24;
		Item.height = 24;
	}
	public override void AddRecipes()
	{
		CreateRecipe(4)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 1)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
