using Everglow.Commons.Templates.Weapons.Yoyos;
using Everglow.Food.Projectiles;

namespace Everglow.Food.Items.Weapons;

public class CheeseYoyo : YoyoItem
{
	public override void SetCustomDefaults()
	{
		Item.damage = 24;
		Item.value = Item.sellPrice(0, 0, 2, 0);
		Item.rare = ItemRarityID.Green;
		Item.shoot = ModContent.ProjectileType<CheeseYoyo_proj>();
	}
}