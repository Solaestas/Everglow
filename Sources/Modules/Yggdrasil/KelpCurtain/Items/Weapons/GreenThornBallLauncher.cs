using Everglow.Yggdrasil.KelpCurtain.Projectiles.Ranged;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons;

public class GreenThornBallLauncher : ModItem
{
	public override string Texture => ModAsset.GlowstickLauncher_Mod;

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 28;

		Item.DamageType = DamageClass.Ranged;
		Item.damage = 25;
		Item.knockBack = 5.5f;
		Item.crit = 10;

		Item.useTime = Item.useAnimation = 35;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.UseSound = SoundID.Item108;
		Item.autoReuse = true;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(silver: 80);

		Item.useAmmo = AmmoID.Bullet;
		Item.shoot = ProjectileID.Bullet;
		Item.shootSpeed = 8f;
	}

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		type = ModContent.ProjectileType<GreenThornLauncher_Proj>();
	}
}