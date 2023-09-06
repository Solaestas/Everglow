using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class GoldenStabbingSword : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.knockBack = 2.7f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 72, 0);
			Item.shoot = ModContent.ProjectileType<GoldenStabbingSword_Pro>();
			StabMulDamage = 4f;
			PowerfulStabProj = ModContent.ProjectileType<GoldenStabbingSword_Pro_Stab>();
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.GoldBar, 17).
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
	}
}