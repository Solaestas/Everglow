namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CactusClub : ClubItem
{
	//TODO:Translate:仙人掌棍棒
	public override void SetDef()
	{
		Item.damage = 7;
		Item.value = 68;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CactusClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CactusClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Cactus, 18)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
