using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.CyanVine;

public class YggdrasilAmberLaser : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 38;
		Item.height = 44;
		Item.useAnimation = 36;
		Item.useTime = 36;
		Item.knockBack = 4f;
		Item.damage = 13;
		Item.rare = ItemRarityID.Blue;
		Item.UseSound = SoundID.Item1;
		Item.value = 3106;
		Item.autoReuse = false;
		Item.DamageType = DamageClass.Magic;
		Item.channel = true;
		Item.mana = 21;
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
