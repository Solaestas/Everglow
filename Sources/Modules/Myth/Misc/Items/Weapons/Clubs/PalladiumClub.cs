namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class PalladiumClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 44;
		Item.value = 2074;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PalladiumClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PalladiumClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.PalladiumBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
