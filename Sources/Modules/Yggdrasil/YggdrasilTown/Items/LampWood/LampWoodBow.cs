namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;

// TODO: Replace sprite
public class LampWoodBow : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 64;
		Item.height = 78;

		Item.damage = 8;
		Item.DamageType = DamageClass.Ranged;
		Item.knockBack = 0;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = 29;
		Item.useAnimation = 25;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = false;
		Item.noUseGraphic = false;
		Item.noMelee = true;
		Item.channel = false;

		Item.shoot = ProjectileID.WoodenArrowFriendly;
		Item.shootSpeed = 12f;
		Item.useAmmo = AmmoID.Arrow;

		Item.rare = ItemRarityID.White;
		Item.value = Item.sellPrice(silver: 1);
	}
}