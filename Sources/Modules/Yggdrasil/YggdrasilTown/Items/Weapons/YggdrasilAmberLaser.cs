using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class YggdrasilAmberLaser : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 38;
		Item.height = 44;

		Item.DamageType = DamageClass.Magic;
		Item.damage = 13;
		Item.knockBack = 4f;
		Item.mana = 21;

		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(silver: 31, copper: 6);

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 36;
		Item.useTime = 36;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = false;
		Item.channel = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.shoot = ModContent.ProjectileType<YggdrasilAmberLaser_proj>();
		Item.shootSpeed = 12;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		return base.Shoot(player, source, position, velocity, type, damage, knockback);
	}
}