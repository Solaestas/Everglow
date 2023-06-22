namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CorruptClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 12;
		Item.value = 169;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CorruptClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.DemoniteBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
