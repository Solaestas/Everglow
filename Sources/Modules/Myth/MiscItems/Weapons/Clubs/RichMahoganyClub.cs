namespace Everglow.Myth.MiscItems.Weapons.Clubs
{
	public class RichMahoganyClub : ClubItem
	{
		public override void SetDef()
		{
			Item.damage = 6;
			Item.value = 64;
			ProjType = ModContent.ProjectileType<Projectiles.RichMahoganyClub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.RichMahogany, 18)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
