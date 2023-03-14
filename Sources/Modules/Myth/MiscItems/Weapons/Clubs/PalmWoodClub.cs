namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
	public class PalmWoodClub : ClubItem
	{
		public override void SetDef()
		{
			Item.damage = 7;
			Item.value = 72;
			ProjType = ModContent.ProjectileType<Projectiles.PalmWoodClub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PalmWood, 18)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
