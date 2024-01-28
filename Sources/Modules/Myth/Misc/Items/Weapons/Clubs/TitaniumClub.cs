namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class TitaniumClub : ClubItem
{
	//TODO:Translate:钛棍棒
	public override void SetDef()
	{
		Item.damage = 80;
		Item.value = 3751;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.TitaniumClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.TitaniumClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.TitaniumBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
