using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class BlueRibbonBayonet : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 66;
			Item.knockBack = 2.7f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 72, 0);
			Item.shoot = ModContent.ProjectileType<BlueRibbonBayonet_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
		//public override void AddRecipes()
		//{
		//	CreateRecipe().
		//		AddIngredient(ItemID.GoldBar, 17).
		//		AddTile(TileID.Anvils).
		//		Register();
		//	base.AddRecipes();
		//}
	}
}