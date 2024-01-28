namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class TinClub : ClubItem
{
	//TODO:Translate:锡棍棒
	public override void SetDef()
	{
		Item.damage = 6;
		Item.value = 70;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.TinClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.TinClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.TinBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
