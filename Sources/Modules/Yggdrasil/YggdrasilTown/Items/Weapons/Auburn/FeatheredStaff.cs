using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Magic;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.Auburn;

public class FeatheredStaff : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.MagicWeapons;

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

		Item.shoot = ModContent.ProjectileType<FeatheredStaff_staff>();
		Item.shootSpeed = 10;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(silver: 20);
	}

	public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;
}