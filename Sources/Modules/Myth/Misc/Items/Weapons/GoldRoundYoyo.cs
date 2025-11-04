using Everglow.Commons.Templates.Weapons.Yoyos;

namespace Everglow.Myth.Misc.Items.Weapons;

public class GoldRoundYoyo : YoyoItem
{
	public override void SetCustomDefaults()
	{
		Item.damage = 136;
		Item.value = Item.sellPrice(0, 5, 0, 0);
		Item.rare = ItemRarityID.Yellow;
		Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.Melee.GoldRoundYoyo>();
	}
}