namespace Everglow.Myth.MiscItems.Weapons.Clubs
{
	public class AdamantiteClub : ClubItem
	{
		public override void SetDef()
		{
			Item.damage = 74;
			Item.value = 3690;
			ProjType = ModContent.ProjectileType<Projectiles.AdamantiteClub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.AdamantiteBar, 18)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
