using Terraria.ID;

namespace Everglow.Yggdrasil.YggdrasilTown.Items
{
	public class CyanVineBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Cyan Bar");
			//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "青缎钢锭");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
			Item.rare = ItemRarityID.White;
			Item.scale = 1f;
			Item.createTile = TileID.Dirt;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.value = 1600;
			Item.createTile = ModContent.TileType<Tiles.CyanVine.CyanVineBar>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<CyanVineOre>(), 3)
				.AddTile(TileID.Furnaces)
				.Register();
		}
	}
}
