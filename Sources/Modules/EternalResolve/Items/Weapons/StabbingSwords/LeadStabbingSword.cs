using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class LeadStabbingSword : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 4;
			Item.knockBack = 1.45f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 24, 0);
			Item.shoot = ModContent.ProjectileType<LeadStabbingSword_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.LeadBar, 17).
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
	}
}
