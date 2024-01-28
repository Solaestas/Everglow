namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class GoldClub : ClubItem
{
	//TODO:Translate:金棍棒
	public override void SetDef()
	{
		Item.damage = 10;
		Item.value = 142;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.GoldClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.GoldClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.GoldBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
