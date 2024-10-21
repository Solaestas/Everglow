using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.LightSeeker;

internal class LightBeamStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 34;
		Item.height = 40;

		Item.damage = 12;
		Item.DamageType = DamageClass.Magic;
		Item.crit = 4;
		Item.knockBack = 3.5f;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = 36;
		Item.useAnimation = 36;
		Item.UseSound = SoundID.Item12;
		Item.autoReuse = true;
		Item.noUseGraphic = true;
		Item.mana = 5;

		Item.SetShopValues(
			ItemRarityColor.Green2,
			Item.buyPrice(silver: 8));

		Item.shoot = ModContent.ProjectileType<LightBeamStaff_proj>();
		Item.shootSpeed = 15f;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		Projectile.NewProjectile(Item.GetSource_FromAI(), position, velocity, type, damage, knockback, player.whoAmI);
		return false;
	}
}