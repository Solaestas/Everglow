using Everglow.Commons.Templates.Weapons.Yoyos;
using Everglow.Myth.LanternMoon.Projectiles.Weapons;

namespace Everglow.Myth.LanternMoon.Items.Weapons;

/// <summary>
/// Mark target with a lantern label.
/// Do at least 500 damage to a labeled target will remove the label and trigger an explosion.
/// </summary>
public class LanternYoyo : YoyoItem
{
	public override void SetCustomDefaults()
	{
		Item.damage = 52;
		Item.rare = ItemRarityID.Lime;
		Item.value = 15000;
		Item.shoot = ModContent.ProjectileType<LanternYoyoProjectile>();
	}
}