using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class HolyBayonet : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 58;
			Item.knockBack = 2.7f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 0, 72, 0);
			Item.shoot = ModContent.ProjectileType<HolyBayonet_Pro>();
			StabMulDamage = 4f;
			PowerfulStabProj = ModContent.ProjectileType<HolyBayonet_Pro_Stab>();
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.HallowedBar, 10).
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
	}
}