using Terraria.GameContent.Creative;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Furnitures
{
	public class GlowWoodLamp : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 2000;
			Item.createTile = ModContent.TileType<Tiles.Furnitures.GlowWoodLamp>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 3);
			recipe.AddIngredient(ItemID.Torch, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}