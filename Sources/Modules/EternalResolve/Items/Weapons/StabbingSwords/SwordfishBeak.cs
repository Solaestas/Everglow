using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class SwordfishBeak : StabbingSwordItem
	{
		//TODO:长喙剑鱼\n对于水里的敌人造成伤害下降40%，对于干燥的敌人伤害提高40%
		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.knockBack = 1.22f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 2, 14, 0);
			Item.shoot = ModContent.ProjectileType<SwordfishBeak_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Swordfish)
				.AddTile(TileID.Sawmill)
				.Register();
		}
	}
}
