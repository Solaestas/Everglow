using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class TinStabbingSword : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 3;
			Item.knockBack = 1.24f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 16, 0);
			Item.shoot = ModContent.ProjectileType<TinStabbingSword_Pro>();
			StabMulDamage = 4f;
			PowerfulStabProj = ModContent.ProjectileType<TinStabbingSword_Pro_Stab>();
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.TinBar, 17).
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
    }
}