namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class BorealWoodClub : ClubItem
{
	//TODO:Translate:针叶木棍棒
	public override void SetDef()
	{
		Item.damage = 6;
		Item.value = 54;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.BorealWoodClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.BorealWoodClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.BorealWood, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
