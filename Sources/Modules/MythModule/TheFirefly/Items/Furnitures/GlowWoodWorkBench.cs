using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Furnitures
{
	public class GlowWoodWorkBench : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.createTile = ModContent.TileType<Tiles.Furnitures.GlowWoodWorkBench>(); // This sets the id of the tile that this item should place when used.
			Item.width = 28; // The item texture's width
			Item.height = 14; // The item texture's height
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.value = 150;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 10);
			recipe.Register();
		}
	}
}
