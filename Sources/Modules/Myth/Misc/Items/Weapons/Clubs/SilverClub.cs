namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class SilverClub : ClubItem
{
	//TODO:Translate:银棍棒
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
			.AddIngredient(ItemID.SilverBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
