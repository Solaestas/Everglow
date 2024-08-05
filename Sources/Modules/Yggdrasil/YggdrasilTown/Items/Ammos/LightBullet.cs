namespace Everglow.Yggdrasil.YggdrasilTown.Items.Ammos;

internal class LightBullet : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;

		Item.DamageType = DamageClass.Ranged;
		Item.damage = 7;
		Item.knockBack = 4;

		Item.value = Item.buyPrice(copper: 3);

		Item.ammo = AmmoID.Bullet;
		Item.maxStack = Item.CommonMaxStack;
		Item.consumable = true;

		Item.shoot = ModContent.ProjectileType<Projectiles.LightBullet>();
	}
}