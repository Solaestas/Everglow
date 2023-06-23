namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CobaltClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 41;
		Item.value = 2005;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CobaltClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.CobaltBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
