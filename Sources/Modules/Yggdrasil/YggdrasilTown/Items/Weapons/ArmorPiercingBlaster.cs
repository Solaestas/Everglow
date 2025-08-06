using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Weapons;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class ArmorPiercingBlaster : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 30;

		Item.DamageType = DamageClass.Ranged;
		Item.damage = 26;
		Item.knockBack = 1.0f;
		Item.crit = 4;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = Item.useTime = 36;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = false;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.rare = ItemRarityID.Blue;
		Item.value = 0;

		Item.shoot = ModContent.ProjectileType<ArmorPiercingBlasterProj>();
		Item.shootSpeed = 6;
	}
}