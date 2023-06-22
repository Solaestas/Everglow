namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class IronClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 7;
		Item.value = 85;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.IronClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.IronBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
