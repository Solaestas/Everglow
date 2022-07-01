using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Furnitures
{
	public class GlowWoodDoorType2 : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("This is a modded door.");

		}

		public override void SetDefaults() {
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
			Item.createTile = ModContent.TileType<Tiles.Furnitures.GlowWoodDoorClosedType2>();
		}
	}
}