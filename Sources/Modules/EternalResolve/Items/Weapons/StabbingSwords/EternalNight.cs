using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class EternalNight : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.knockBack = 2;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 1, 88, 48);
			Item.shoot = ModContent.ProjectileType<EternalNight_Pro>();
			StabMulDamage = 4f;
			PowerfulStabProj = ModContent.ProjectileType<EternalNight_Pro_Stab>();
			base.SetDefaults();
		}
    }
}