namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class PlatinumClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 11;
		Item.value = 147;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PlatinumClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.PlatinumBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
