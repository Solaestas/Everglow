namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class ChlorophyteClub_Item : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 80;
		Item.value = 9605;
		Item.rare = ItemRarityID.Lime;
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
