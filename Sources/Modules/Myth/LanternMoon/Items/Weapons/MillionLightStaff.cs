using Everglow.Myth.LanternMoon.Projectiles.Weapons;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Items.Weapons;

/// <summary>
/// Mark target with a lantern label.
/// Do at least 500 damage to a labeled target will remove the label and trigger an explosion.
/// </summary>
public class MillionLightStaff : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.MagicWeapons;

	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.staff[Type] = true;
		Item.width = 50;
		Item.height = 50;
		Item.useAnimation = 16;
		Item.useTime = 16;
		Item.knockBack = 1.5f;
		Item.damage = 90;
		Item.rare = ItemRarityID.Lime;
		Item.autoReuse = true;
		Item.UseSound = SoundID.Item1;
		Item.DamageType = DamageClass.Magic;
		Item.mana = 13;
		Item.noMelee = true;
		Item.shootSpeed = 14f;
		Item.shoot = ModContent.ProjectileType<MillionLightStaff_Proj>();
		Item.value = 15000;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		for (int k = 0; k < 4; k++)
		{
			Vector2 pos = player.Center + new Vector2(Main.rand.NextFloat(-300, 300), -1000);
			Vector2 vel = Main.MouseWorld - pos;
			vel = vel.NormalizeSafe() * Main.rand.NextFloat(22f, 30f);
			vel = vel.RotatedByRandom(0.14f);
			var p0 = Projectile.NewProjectileDirect(source, pos, vel, type, damage, knockback, player.whoAmI);
		}
		return false;
	}
}