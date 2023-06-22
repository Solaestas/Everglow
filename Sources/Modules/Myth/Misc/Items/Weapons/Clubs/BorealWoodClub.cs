namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class BorealWoodClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 6;
		Item.value = 54;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.BorealWoodClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.BorealWood, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
