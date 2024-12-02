namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class SilverClub_Item : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 8;
		Item.value = 108;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.SilverClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.SilverClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.SilverBar, 12)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
