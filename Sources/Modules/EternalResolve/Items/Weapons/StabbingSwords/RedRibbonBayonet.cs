using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class RedRibbonBayonet : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 66;
			Item.knockBack = 2.7f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 72, 0);
			Item.shoot = ModContent.ProjectileType<RedRibbonBayonet_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
	}
}