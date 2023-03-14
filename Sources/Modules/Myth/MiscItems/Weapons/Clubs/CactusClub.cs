namespace Everglow.Myth.MiscItems.Weapons.Clubs;

public class CactusClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 7;
		Item.value = 68;
		ProjType = ModContent.ProjectileType<Projectiles.CactusClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Cactus, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
