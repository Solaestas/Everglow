using Everglow.Commons.Weapons.Slingshots;

namespace Everglow.Myth.Misc.Items.Weapons.Slingshots;

public class GelSlingshot : SlingshotItem
{
	public override void SetDef()
	{
		Item.damage = 12;
		Item.crit = 4;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Ranged.Slingshots.GelSlingshot>();

		Item.rare = ItemRarityID.Green;
		Item.value = Item.sellPrice(0, 1, 0, 0);
	}
}