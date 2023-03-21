using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.Items.Weapons;

public class DustOfCorrupt : ModItem
{
	public override void SetDefaults()
	{
		Item.damage = 13;
		Item.DamageType = DamageClass.Magic;
		Item.mana = 4;
		Item.width = 60;
		Item.height = 60;
		Item.useTime = 7;
		Item.useAnimation = 7;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.knockBack = 2.5f;
		Item.value = Item.sellPrice(0, 0, 20, 0);
		Item.rare = ItemRarityID.Green;
		Item.UseSound = SoundID.Item20;
		Item.autoReuse = true;
		Item.shoot = ModContent.ProjectileType<Projectiles.CorruptDust>();
		Item.shootSpeed = 17f;
	}

	public override void SetStaticDefaults()
	{
		Item.staff[Item.type] = true;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * 48, velocity, type, damage, knockback, player.whoAmI, 0, player.GetCritChance(DamageClass.Magic));
		return false;
	}
}