using Terraria.Localization;

namespace Everglow.Myth.TheTusk.Items;

public class TuskFlesh : ModItem
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Living Tissue");
			}

	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.maxStack = 999;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.createTile = ModContent.TileType<Tiles.TuskFlesh>();
	}
}
