namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class PalmWoodClub : ClubItem
{
	//TODO:Translate:棕榈木棍棒
	public override void SetDef()
	{
		Item.damage = 7;
		Item.value = 72;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PalmWoodClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PalmWoodClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.PalmWood, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
