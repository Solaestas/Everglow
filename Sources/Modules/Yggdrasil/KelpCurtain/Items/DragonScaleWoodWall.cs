namespace Everglow.Yggdrasil.KelpCurtain.Items
{
	public class DragonScaleWoodWall : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Walls.DragonScaleWoodWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<DragonScaleWood>(), 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
