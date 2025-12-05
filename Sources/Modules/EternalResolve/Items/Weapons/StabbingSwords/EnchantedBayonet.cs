using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.EternalResolve.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
	public class EnchantedBayonet : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.knockBack = 1.45f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 24, 0);
			Item.shoot = ModContent.ProjectileType<EnchantedBayonet_Pro>();
			PowerfulStabDamageFlat = 4f;
			StaminaCost -= 0.1f;
			PowerfulStabProj = ModContent.ProjectileType<EnchantedBayonet_Pro_Stab>();
			base.SetDefaults();
		}
	}
}
