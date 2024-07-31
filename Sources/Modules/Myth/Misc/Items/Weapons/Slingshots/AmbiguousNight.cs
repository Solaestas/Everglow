using Everglow.Commons.Weapons.Slingshots;

namespace Everglow.Myth.Misc.Items.Weapons.Slingshots;

public class AmbiguousNight : SlingshotItem
{
	public override void SetDef()
	{
		Item.damage = 54;
		Item.crit = 8;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Ranged.Slingshots.AmbiguousNight>();
		Item.width = 40;
		Item.height = 32;
		Item.rare = ItemRarityID.Pink;
		Item.value = Item.sellPrice(0, 2, 0, 0);
	}
}