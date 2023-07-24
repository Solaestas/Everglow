namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class PalmWoodClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 7;
		Item.value = 72;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.PalmWoodClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.PalmWood, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
