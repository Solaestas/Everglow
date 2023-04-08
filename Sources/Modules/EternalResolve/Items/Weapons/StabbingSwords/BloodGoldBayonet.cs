using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class BloodGoldBayonet : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.knockBack = 1.61f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 1, 2, 0);
			Item.shoot = ModContent.ProjectileType<BloodGoldBayonet_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.CrimtaneBar, 17).
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
    }
}