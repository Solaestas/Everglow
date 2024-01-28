namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CorruptClub : ClubItem
{
	//TODO:Translate:魔金棍棒
	public override void SetDef()
	{
		Item.damage = 12;
		Item.value = 169;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CorruptClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CorruptClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.DemoniteBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
