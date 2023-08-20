namespace Everglow.Minortopography.GiantPinetree.Items;
public class IcedSpear : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 50;
		Item.height = 54;
		Item.useAnimation = 16;
		Item.useTime = 16;
		Item.knockBack = 3f;
		Item.damage = 10;
		Item.rare = ItemRarityID.White;
		Item.UseSound = SoundID.Item1;
		Item.value = 36;
		Item.autoReuse = false;
		Item.DamageType = DamageClass.Ranged;
		Item.consumable= true;
		Item.maxStack = 9999;
		Item.noMelee = true;
		Item.noUseGraphic = true;


		Item.shoot = ModContent.ProjectileType<Projectiles.IcedSpear>();
	}
	public override void AddRecipes()
	{
		CreateRecipe(20)
			.AddIngredient(ItemID.BorealWood, 2)
			.AddTile(ItemID.IceBlock)
			.Register();
	}
}
