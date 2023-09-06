using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class MechanicMosquito : StabbingSwordItem
	{
		//TODO:翻译：械蚊\n召唤三个会吸血的机械蚊子协助攻击,蚊子每次攻击会获得1点吸血，最多累积10点。使用结束之后，治疗吸血累积的点数。
		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.knockBack = 1.14f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 8, 48, 0);
			Item.shoot = ModContent.ProjectileType<MechanicMosquito_Pro>();
			StabMulDamage = 4f;
			staminaCost = 0.65f;//机械剑省力，很合理（）
			PowerfulStabProj = ModContent.ProjectileType<MechanicMosquito_Pro_Stab>();
			base.SetDefaults();
		}
	}
}
