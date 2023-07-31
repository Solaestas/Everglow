using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class CopperBayonet : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			staminaCost += 0.3f;
			Item.damage = 3;
			Item.knockBack = 1f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 12, 0);
			Item.shoot = ModContent.ProjectileType<CopperStabbingSword_Pro>();
			StabMulDamage = 4f;
			PowerfulStabProj = ModContent.ProjectileType<CopperStabbingSword_Pro_Stab>();
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.CopperBar, 17).
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
    }
}