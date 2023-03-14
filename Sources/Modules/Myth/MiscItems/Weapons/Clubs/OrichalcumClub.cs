namespace Everglow.Myth.MiscItems.Weapons.Clubs
{
	public class OrichalcumClub : ClubItem
	{
		public override void SetDef()
		{
			Item.damage = 60;
			Item.value = 2717;
			ProjType = ModContent.ProjectileType<Projectiles.OrichalcumClub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.OrichalcumBar, 18)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
