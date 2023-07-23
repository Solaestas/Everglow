namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CopperClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 6;
		Item.value = 65;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CopperClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.CopperBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
