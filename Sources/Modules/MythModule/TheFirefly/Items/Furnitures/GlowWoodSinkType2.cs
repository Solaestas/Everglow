using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Furnitures 
{ 
	public class GlowWoodSinkType2 : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Furnitures.GlowWoodSinkType2>();
			Item.width = 24;
			Item.height = 30;
			Item.value = 3000;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 6);
			recipe.AddIngredient(ItemID.WaterBucket);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
