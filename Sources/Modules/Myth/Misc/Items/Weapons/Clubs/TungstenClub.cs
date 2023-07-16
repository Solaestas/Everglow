namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class TungstenClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 9;
		Item.value = 112;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.TungstenClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.TungstenBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
