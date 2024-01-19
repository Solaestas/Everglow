namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class MeteorClub : ClubItem
{
	public override void SetDef()
	{

		Item.damage = 16;
		Item.value = 576;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.MeteorClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.MeteorClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.MeteoriteBar, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
