namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class TinClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 6;
		Item.value = 70;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.TinClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.TinBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
