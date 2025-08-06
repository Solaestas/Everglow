namespace Everglow.Myth.TheTusk.Items.BossDrops;

public class TuskRelic : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

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
        Item.createTile = ModContent.TileType<Tiles.BossDrops.TuskRelic>();
    }
}