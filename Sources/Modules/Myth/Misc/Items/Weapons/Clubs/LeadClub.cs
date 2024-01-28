namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class LeadClub : ClubItem
{
	//TODO:Translate:铅棍棒
	public override void SetDef()
	{
		Item.damage = 7;
		Item.value = 88;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.LeadClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.LeadClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.LeadBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
