namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CopperClub : ClubItem
{
	//TODO:Translate:铜棍棒
	public override void SetDef()
	{
		Item.damage = 6;
		Item.value = 65;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CopperClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CopperClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.CopperBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
