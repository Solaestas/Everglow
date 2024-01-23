namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class PearlwoodClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 9;
		Item.value = 111;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PearlwoodClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PearlwoodClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Pearlwood, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
