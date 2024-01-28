using Terraria.Localization;

namespace Everglow.Myth.TheTusk.Items.BossDrop;
//TODO:Translate:鲜血獠牙圣物
public class TuskRelic : ModItem
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("The Tusk Relic");
			}
	public override void SetDefaults()
	{
		Item.width = 38;
		Item.height = 50;
		Item.useAnimation = 20;
		Item.master = true;
		Item.useTime = 20;
		Item.maxStack = 99;
		Item.rare = ItemRarityID.White;
		Item.value = Item.sellPrice(0, 1, 0, 0);
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.createTile = ModContent.TileType<Tiles.BossDrop.TuskRelic>();
	}

}
