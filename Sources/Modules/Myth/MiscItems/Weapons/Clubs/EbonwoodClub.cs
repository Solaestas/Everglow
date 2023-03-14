namespace Everglow.Myth.MiscItems.Weapons.Clubs
{
	public class EbonwoodClub : ClubItem
	{
		public override void SetDef()
		{
			Item.damage = 7;
			Item.value = 75;
			ProjType = ModContent.ProjectileType<Projectiles.EbonwoodClub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Ebonwood, 18)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
