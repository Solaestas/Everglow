namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class MysteriousTablet : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 32;
		Item.height = 32;

		Item.DamageType = DamageClass.Default;
		Item.damage = 18;
		Item.knockBack = 1.0f;
		Item.crit = 50;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = Item.useTime = 36;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = false;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.rare = ItemRarityID.White;
		Item.value = 0;

		//Item.consumable = true;
		//Item.maxStack = 9999;

		Item.shoot = ModContent.ProjectileType<Projectiles.MysteriousTablet>();
		Item.shootSpeed = 12;
	}
}