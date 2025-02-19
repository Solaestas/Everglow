using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.LightSeeker;

public class BowOfEnlightment : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 64;
		Item.height = 78;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useAnimation = 29;
		Item.useTime = 29;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = false;
		Item.noUseGraphic = false;
		Item.noMelee = true;
		Item.channel = true;

		Item.damage = 10;
		Item.DamageType = DamageClass.Ranged;
		Item.crit = 4;
		Item.knockBack = 0f;

		Item.shoot = ProjectileID.WoodenArrowFriendly;
		Item.shootSpeed = 12f;
		Item.useAmmo = AmmoID.Arrow;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(silver: 5);
	}

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		if (type == ProjectileID.WoodenArrowFriendly)
		{
			type = ModContent.ProjectileType<LightArrow>();
		}
	}
}