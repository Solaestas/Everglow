namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
	public class CopperClub : ClubItem
	{
		public override void SetDef()
		{
			Item.damage = 6;
			Item.value = 65;
			ProjType = ModContent.ProjectileType<Projectiles.CopperClub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CopperBar, 18)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
