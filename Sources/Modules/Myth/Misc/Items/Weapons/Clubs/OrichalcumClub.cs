namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class OrichalcumClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 60;
		Item.value = 2717;
		Item.rare = ItemRarityID.LightRed;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.OrichalcumClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.OrichalcumClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.OrichalcumBar, 14)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
