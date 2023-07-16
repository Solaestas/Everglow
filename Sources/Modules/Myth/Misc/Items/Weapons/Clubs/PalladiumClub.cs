namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class PalladiumClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 44;
		Item.value = 2074;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PalladiumClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.PalladiumBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
