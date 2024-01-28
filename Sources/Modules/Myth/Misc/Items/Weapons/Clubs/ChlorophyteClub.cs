namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class ChlorophyteClub : ClubItem
{
	//TODO:Translate:叶绿棍棒
	public override void SetDef()
	{
		Item.damage = 80;
		Item.value = 9605;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.ChlorophyteClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.ChlorophyteClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.ChlorophyteBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
