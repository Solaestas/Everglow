using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class MachanicMosquito : StabbingSwordItem
	{
		//TODO:翻译：械蚊\n召唤三个会吸血的机械蚊子协助攻击,蚊子每次攻击会获得1点吸血，最多累积10点。使用结束之后，治疗吸血累积的点数。
		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.knockBack = 1.14f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 1, 14, 0);
			Item.shoot = ModContent.ProjectileType<MachanicMosquito_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
	}
}
