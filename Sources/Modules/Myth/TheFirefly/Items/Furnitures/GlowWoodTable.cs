using Terraria.GameContent.Creative;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Furnitures
{
	public class GlowWoodTable : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodTable>());
			Item.value = 150;
			Item.maxStack = 99;
			Item.width = 38;
			Item.height = 24;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}