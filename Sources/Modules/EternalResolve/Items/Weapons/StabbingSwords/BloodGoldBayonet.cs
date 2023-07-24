using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class BloodGoldBayonet : StabbingSwordItem
	{
		//TODO:翻译
		//命中敌人后有1/25的概率吸血,吸血量为造成伤害的30%
		//命中的敌人未死之前,你的生命回复+2
		//据不完全统计，多数吸血鬼并不会吸取史莱姆汁
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