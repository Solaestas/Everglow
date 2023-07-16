namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class SilverClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 8;
		Item.value = 108;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.SilverClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.SilverBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
