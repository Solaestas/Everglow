using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class NightfireStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 22;
		Item.height = 22;

		Item.DamageType = DamageClass.Magic;
		Item.damage = 12;
		Item.knockBack = 0.5f;
		Item.mana = 8;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.UseSound = SoundID.Item20;
		Item.useTime = Item.useAnimation = 32;
		Item.noMelee = true;
		Item.autoReuse = false;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(gold: 1);

		Item.shoot = ModContent.ProjectileType<NightfireStaff_Projectile>();
		Item.shootSpeed = 8;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		Projectile.NewProjectile(source, Main.MouseWorld - 4 * velocity + new Vector2(0, 20).RotatedBy(velocity.ToRotation() - MathHelper.Pi), velocity, type, damage, knockback, player.whoAmI, velocity.ToRotation(), 1);
		Projectile.NewProjectile(source, Main.MouseWorld - 4 * velocity - new Vector2(0, 20).RotatedBy(velocity.ToRotation() - MathHelper.Pi), velocity, type, damage, knockback, player.whoAmI, velocity.ToRotation(), -1);
		return false;
	}
}