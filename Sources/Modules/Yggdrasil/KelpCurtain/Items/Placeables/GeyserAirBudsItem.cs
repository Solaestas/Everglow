using Everglow.Yggdrasil.KelpCurtain.Tiles.GeyserAirBuds;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class GeyserAirBudsItem : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.maxStack = Item.CommonMaxStack;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 15;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.value = Item.sellPrice(0, 0, 1, 0);
		Item.rare = ItemRarityID.White;
		Item.createTile = ModContent.TileType<GeyserAirBudsPlatform>();

		Item.DefaultToPlaceableTile(ModContent.TileType<GeyserAirBudsPlatform>());
	}

	public override void HoldItem(Player player)
	{
		Main.placementPreview = true;
	}
}