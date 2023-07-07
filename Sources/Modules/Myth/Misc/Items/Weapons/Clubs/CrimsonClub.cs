namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CrimsonClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 13;
		Item.value = 174;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrimsonClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.CrimtaneBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
