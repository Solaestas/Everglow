using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class PrisonFireBayonet : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.knockBack = 2.34f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 1, 12, 0);
			Item.shoot = ModContent.ProjectileType<PrisonFireBayonet_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.HellstoneBar, 17).
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
	}
}