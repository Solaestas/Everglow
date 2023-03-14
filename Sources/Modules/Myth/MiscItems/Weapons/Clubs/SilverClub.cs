namespace Everglow.Myth.MiscItems.Weapons.Clubs
{
	public class SilverClub : ClubItem
	{
		public override void SetDef()
		{
			Item.damage = 8;
			Item.value = 108;
			ProjType = ModContent.ProjectileType<Projectiles.SilverClub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SilverBar, 18)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
