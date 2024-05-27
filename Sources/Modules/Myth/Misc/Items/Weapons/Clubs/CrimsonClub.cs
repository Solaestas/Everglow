namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class CrimsonClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 13;
		Item.value = 174;
		Item.rare = ItemRarityID.Blue;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrimsonClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.CrimsonClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.CrimtaneBar, 12)
			.AddIngredient(ItemID.TissueSample, 4)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
