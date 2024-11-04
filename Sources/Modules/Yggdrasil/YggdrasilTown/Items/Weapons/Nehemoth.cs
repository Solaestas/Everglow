using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

// TODO: Replace sprite
public class Nehemoth : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 62;
		Item.height = 32;
		Item.scale = 0.75f;

		Item.damage = 36;
		Item.DamageType = DamageClass.Ranged;
		Item.knockBack = 5f;
		Item.crit = 15;
		Item.noMelee = true;

		Item.useTime = 90;
		Item.useAnimation = 90;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item41;
		Item.autoReuse = false;

		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 10);

		Item.shootSpeed = 16f;
		Item.shoot = ModContent.ProjectileType<NehemothBullet>();
		Item.useAmmo = AmmoID.Bullet;
	}

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		if (type == ProjectileID.Bullet)
		{
			type = ModContent.ProjectileType<NehemothBullet>();
		}
	}
}