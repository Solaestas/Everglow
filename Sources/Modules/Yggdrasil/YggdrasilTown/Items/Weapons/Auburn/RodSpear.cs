using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.Auburn;

internal class RodSpear : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 50;
		Item.height = 54;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 45;
		Item.useTime = 45;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.noUseGraphic = true;
		Item.noMelee = true;

		Item.damage = 18;
		Item.DamageType = DamageClass.Melee;
		Item.crit = 4;
		Item.knockBack = 4f;

		Item.SetShopValues(
			ItemRarityColor.Green2,
			Item.buyPrice(silver: 20));

		Item.shoot = ModContent.ProjectileType<Projectiles.RodSpear>();
		Item.shootSpeed = 20;
	}

	public override void AddRecipes()
	{
		CreateRecipe(8)
			.AddIngredient(ItemID.BorealWood, 2)
			.AddIngredient(ItemID.IceBlock, 1)
			.AddTile(TileID.IceBlock)
			.Register();
	}
}