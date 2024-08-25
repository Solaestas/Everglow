using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.Auburn;

internal class FeatheredStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 50;
		Item.height = 54;

		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.useAnimation = 35;
		Item.useTime = 35;
		Item.UseSound = SoundID.Item20;
		Item.autoReuse = true;
		Item.noMelee = true;

		Item.damage = 12;
		Item.DamageType = DamageClass.Magic;
		Item.crit = 4;
		Item.knockBack = 3.25f;
		Item.mana = 6;

		Item.shoot = ModContent.ProjectileType<Projectiles.FeatheredStaff>();
		Item.shootSpeed = 10;

		Item.SetShopValues(
			ItemRarityColor.Green2,
			Item.buyPrice(silver: 20));
	}

	public override bool? UseItem(Player player)
	{
		int projectileNumber = 3;

		for (int i = 1; i <= projectileNumber; i++)
		{
			Vector2 shootDirection = Vector2.Normalize(Main.MouseWorld - player.position) * Item.shootSpeed;
			Projectile.NewProjectile(
				player.GetSource_ItemUse(Item),
				player.Center,
				shootDirection.RotatedByRandom(MathHelper.ToRadians(15)),
				Item.shoot,
				Item.damage,
				Item.knockBack,
				player.whoAmI);
		}
		return true;
	}
}