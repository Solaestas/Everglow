namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class GoldClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 10;
		Item.value = 142;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.GoldClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.GoldBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
