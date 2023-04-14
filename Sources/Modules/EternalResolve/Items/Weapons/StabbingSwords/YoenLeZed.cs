using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class YoenLeZed : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.knockBack = 1.08f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 90, 0);
			Item.shoot = ModContent.ProjectileType<YoenLeZed_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
	}
}
