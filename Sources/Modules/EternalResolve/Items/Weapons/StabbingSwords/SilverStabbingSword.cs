using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class SilverStabbingSword : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.knockBack = 1.54f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.shoot = ModContent.ProjectileType<SilverStabbingSword_Pro>();
			StabMulDamage = 4f;
			PowerfulStabProj = ModContent.ProjectileType<SilverStabbingSword_Pro_Stab>();
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.SilverBar, 17).
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
	}
}