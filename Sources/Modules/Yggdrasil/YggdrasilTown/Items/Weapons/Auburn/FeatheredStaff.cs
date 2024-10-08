using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.Auburn;

internal class FeatheredStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 50;
		Item.height = 54;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 35;
		Item.useTime = 35;
		Item.UseSound = SoundID.Item20;
		Item.autoReuse = true;
		Item.channel = true;

		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.damage = 12;
		Item.DamageType = DamageClass.Magic;
		Item.crit = 4;
		Item.knockBack = 3.25f;
		Item.mana = 6;

		Item.shoot = ModContent.ProjectileType<Projectiles.FeatheredStaff_staff>();
		Item.shootSpeed = 10;

		Item.SetShopValues(
			ItemRarityColor.Green2,
			Item.buyPrice(silver: 20));
	}

	public override bool CanUseItem(Player player)
	{
		return player.ownedProjectileCounts[Item.shoot] == 0;
	}

	public override bool? UseItem(Player player)
	{
		return base.UseItem(player);
	}
}