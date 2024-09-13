namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class TinClub_Item : ClubItem
{
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
			.AddIngredient(ItemID.TinBar, 8)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
