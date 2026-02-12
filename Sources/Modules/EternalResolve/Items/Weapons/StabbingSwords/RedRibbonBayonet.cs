using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.EternalResolve.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
	public class RedRibbonBayonet : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 66;
			Item.knockBack = 2.7f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 4, 72, 0);
			Item.shoot = ModContent.ProjectileType<RedRibbonBayonet_Pro>();
			PowerfulStabDamageFlat = 4f;
			PowerfulStabProj = ModContent.ProjectileType<RedRibbonBayonet_Pro_Stab>();
			base.SetDefaults();
		}
	}
}