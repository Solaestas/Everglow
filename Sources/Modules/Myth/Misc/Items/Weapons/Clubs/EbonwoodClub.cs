namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class EbonwoodClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 7;
		Item.value = 75;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.EbonwoodClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.EbonwoodClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Ebonwood, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
