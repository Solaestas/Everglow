namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class OrichalcumClub : ClubItem
{
	//TODO:Translate:山铜棍棒
	public override void SetDef()
	{
		Item.damage = 60;
		Item.value = 2717;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.OrichalcumClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.OrichalcumClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.OrichalcumBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
