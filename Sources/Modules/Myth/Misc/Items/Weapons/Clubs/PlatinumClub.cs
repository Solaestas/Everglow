namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class PlatinumClub : ClubItem
{
	//TODO:Translate:铂棍棒
	public override void SetDef()
	{
		Item.damage = 11;
		Item.value = 147;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PlatinumClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PlatinumClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.PlatinumBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
