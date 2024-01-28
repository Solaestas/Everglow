namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CrimsonClub : ClubItem
{
	//TODO:Translate:血金棍棒
	public override void SetDef()
	{
		Item.damage = 13;
		Item.value = 174;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrimsonClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrimsonClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.CrimtaneBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
