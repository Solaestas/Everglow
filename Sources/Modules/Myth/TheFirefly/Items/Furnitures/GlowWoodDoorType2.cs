using Everglow.Myth.TheFirefly.Items;
using Everglow.Myth.TheFirefly.Tiles.Furnitures;

namespace Everglow.Myth.TheFirefly.Items.Furnitures
{
	public class GlowWoodDoorType2 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 28;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<GlowWoodDoorClosedType2>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 6);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}