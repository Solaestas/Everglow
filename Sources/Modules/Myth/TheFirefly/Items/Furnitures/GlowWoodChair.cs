using Terraria.GameContent.Creative;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Furnitures
{
	public class GlowWoodChair : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodChair>());
			Item.value = 150;
			Item.maxStack = 99;
			Item.width = 12;
			Item.height = 30;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}