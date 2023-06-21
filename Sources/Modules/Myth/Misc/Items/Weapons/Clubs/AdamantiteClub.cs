namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class AdamantiteClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 74;
		Item.value = 3690;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.AdamantiteClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.AdamantiteBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
