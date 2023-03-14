namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
	public class RichMahoganySlingshot : SlingshotItem
	{
		public override void SetDef()
		{
			ProjType = ModContent.ProjectileType<Projectiles.RichMahoganySlingshot>();
			Item.damage = 7;
			Item.useTime = 24;
			Item.useAnimation = 24;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Cobweb, 14)
				.AddIngredient(ItemID.RichMahogany, 7)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}