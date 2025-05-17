using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class Nehemoth : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 154;
		Item.height = 40;
		Item.scale = 0.8f;

		Item.damage = 36;
		Item.DamageType = DamageClass.Ranged;
		Item.knockBack = 5f;
		Item.crit = 15;
		Item.noMelee = true;

		Item.useTime = Item.useAnimation = 90;
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

	public override Vector2? HoldoutOffset() => new Vector2(-45, 0);
}