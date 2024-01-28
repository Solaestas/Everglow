namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class WoodenClub : ClubItem
{
	//TODO:Translate:木棍棒
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Wood, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
	public override void SetDef()
	{
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.WoodenClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.WoodenClub_smash>();
	}
}
