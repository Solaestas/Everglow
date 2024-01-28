namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class ShadewoodClub : ClubItem
{
	//TODO:Translate:暗影木棍棒
	public override void SetDef()
	{
		Item.damage = 7;
		Item.value = 80;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.ShadewoodClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.ShadewoodClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Shadewood, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
