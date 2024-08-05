namespace Everglow.Yggdrasil.YggdrasilTown.Items.Ammos;

internal class LightArrow : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 99;
	}

	public override void SetDefaults()
	{
		Item.width = 18;
		Item.height = 52;

		Item.damage = 6;
		Item.DamageType = DamageClass.Ranged;

		Item.maxStack = Item.CommonMaxStack;
		Item.consumable = true;
		Item.knockBack = 4f;

		Item.rare = ItemRarityID.Blue;
		Item.value = Item.sellPrice(copper: 3);

		Item.shoot = ModContent.ProjectileType<Projectiles.LightArrow>();

		Item.ammo = AmmoID.Arrow;
	}
}