using Everglow.Commons.Weapons.Whips;
using Everglow.Food.Projectiles;

namespace Everglow.Food.Items.Weapons;

public class GrilledSquidTentacle : WhipItem
{
	public override void SetDef()
	{
		Item.width = 18;
		Item.height = 18;
		Item.shoot = ModContent.ProjectileType<GrilledSquidTentacle_proj>();
		Item.shootSpeed = 5.04f;
		Item.value = 32000;
		Item.rare = ItemRarityID.Green;
		Item.damage = 25;
		Item.useAnimation = 30;
		Item.useTime = 30;
	}
}
