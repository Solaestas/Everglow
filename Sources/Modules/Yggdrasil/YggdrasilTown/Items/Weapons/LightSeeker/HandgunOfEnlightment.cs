using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.LightSeeker;

internal class HandgunOfEnlightment : ModItem
{
	public override void SetDefaults()
	{
		// Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

		// Common Properties
		Item.width = 62; // Hitbox width of the item.
		Item.height = 32; // Hitbox height of the item.
		Item.scale = 0.75f;

		// Use Properties
		Item.useTime = 25; // The item's use time in ticks (60 ticks == 1 second.)
		Item.useAnimation = 25; // The length of the item's use animation in ticks (60 ticks == 1 second.)
		Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
		Item.autoReuse = false; // Whether or not you can hold click to automatically use it again.

		// The sound that this item plays when used.
		Item.UseSound = SoundID.Item41;

		// Weapon Properties
		Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
		Item.damage = 15; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
		Item.knockBack = 1f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
		Item.noMelee = true; // So the item's animation doesn't do damage.

		// Gun Properties
		Item.shoot = ModContent.ProjectileType<LightBullet>(); // For some reason, all the guns in the vanilla source have this.
		Item.shootSpeed = 16f; // The speed of the projectile (measured in pixels per frame.)
		Item.useAmmo = AmmoID.Bullet; // The "ammo Id" of the ammo item that this weapon uses. Ammo IDs are magic numbers that usually correspond to the item id of one item that most commonly represent the ammo type.

		// Shop Properties
		Item.SetShopValues(
			Terraria.Enums.ItemRarityColor.Green2,
			Item.sellPrice(silver: 3));
	}

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		if (type == ProjectileID.Bullet)
		{
			type = ModContent.ProjectileType<LightBullet>();
		}
	}
}