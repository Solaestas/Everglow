using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class TungstenStabbingSword : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.knockBack = 1.74f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 48, 0);
			Item.shoot = ModContent.ProjectileType<TungstenStabbingSword_Pro>();
			StabMulDamage = 4f;
			PowerfulStabProj = ModContent.ProjectileType<TungstenStabbingSword_Pro_Stab>();
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.TungstenBar, 17).
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
	}
}