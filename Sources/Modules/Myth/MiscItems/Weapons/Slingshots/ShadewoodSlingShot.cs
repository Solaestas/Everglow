namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
	public class ShadewoodSlingShot : SlingshotItem
	{
		public override void SetDef()
		{
			ProjType = ModContent.ProjectileType<Projectiles.ShadewoodSlingShot>();
			Item.damage = 9;
			Item.useTime = 22;
			Item.useAnimation = 22;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Cobweb, 14)
				.AddIngredient(ItemID.Shadewood, 7)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}