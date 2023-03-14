namespace Everglow.Myth.MiscItems.Weapons.Clubs
{
	public class WoodenClub : ClubItem
	{
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Wood, 18)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
		public override void SetDef()
		{
			ProjType = ModContent.ProjectileType<Projectiles.WoodenClub>();
		}
	}
}
