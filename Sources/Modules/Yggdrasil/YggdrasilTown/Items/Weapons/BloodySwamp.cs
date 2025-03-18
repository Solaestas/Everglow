using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Weapons;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class BloodySwamp : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.staff[Type] = true;
	}

	public override void SetDefaults()
	{
		Item.width = 48;
		Item.height = 48;

		Item.DamageType = DamageClass.Magic;
		Item.damage = 12;
		Item.knockBack = 0.5f;
		Item.mana = 6;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item20;
		Item.useTime = Item.useAnimation = 21;
		Item.noMelee = true;
		Item.autoReuse = false;
		Item.rare = ItemRarityID.Green;
		Item.value = 14400;

		Item.shoot = ModContent.ProjectileType<BloodySwamp_shoot>();
		Item.shootSpeed = 8;
	}

	public override bool AltFunctionUse(Player player) => true;

	public override bool? UseItem(Player player) => base.UseItem(player);

	public override bool CanUseItem(Player player)
	{
		return base.CanUseItem(player);
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.altFunctionUse != 2)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
		}
		else
		{
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<BloodySwamp_shoot_area>(), damage, knockback, player.whoAmI);
		}
		return false;
	}
}