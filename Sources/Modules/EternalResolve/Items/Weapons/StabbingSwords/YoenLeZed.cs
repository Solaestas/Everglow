using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class YoenLeZed : StabbingSwordItem
	{
		//TODO:翻译
		//攻击附带雷电，水中雷电伤害翻三倍且更容易传导
		//比考场忘带身份证还刺激一点
		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.knockBack = 1.08f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 90, 0);
			Item.shoot = ModContent.ProjectileType<YoenLeZed_Pro>();
			StabMulDamage = 4f;
			PowerfulStabProj = ModContent.ProjectileType<YoenLeZed_Pro_Stab>();
			base.SetDefaults();
		}
	}
}
