namespace Everglow.Myth.MiscItems.Weapons.Clubs;

public class TitaniumClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 80;
		Item.value = 3751;
		ProjType = ModContent.ProjectileType<Projectiles.TitaniumClub>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.TitaniumBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
