using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Melee;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.TwilightForest;

public class DivineAscend : MeleeItem_3D
{
	public override void SetCustomDefaults()
	{
		Item.damage = 27;
		Item.knockBack = 3;

		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(gold: 3);

		Item.shoot = ModContent.ProjectileType<DivineAscendProj>();
	}
}