namespace Everglow.Myth.MiscItems.Weapons.Clubs;

public class PalladiumClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 44;
		Item.value = 2074;
		ProjType = ModContent.ProjectileType<Projectiles.PalladiumClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.PalladiumBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
