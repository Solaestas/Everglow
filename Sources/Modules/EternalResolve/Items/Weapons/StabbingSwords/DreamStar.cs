using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class DreamStar : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 33;
			Item.knockBack = 1.5f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 2, 27, 15);
			Item.shoot = ModContent.ProjectileType<DreamStar_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
	}
}