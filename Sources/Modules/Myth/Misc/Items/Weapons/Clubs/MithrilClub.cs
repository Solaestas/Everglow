namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class MithrilClub : ClubItem
{
	//TODO:Translate:秘银棍棒
	public override void SetDef()
	{
		Item.damage = 56;
		Item.value = 2682;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.MithrilClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.MithrilClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.MythrilBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
