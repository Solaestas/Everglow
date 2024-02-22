namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class MythrilClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 56;
		Item.value = 2682;
		Item.rare = ItemRarityID.LightRed;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.MythrilClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.MythrilClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.MythrilBar, 14)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
