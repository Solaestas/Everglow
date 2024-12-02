namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class LeadClub_Item : ClubItem
{
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
			.AddIngredient(ItemID.LeadBar, 12)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
