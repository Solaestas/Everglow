namespace Everglow.Yggdrasil.YggdrasilTown.Items.BossDrops;

public class SquamousShellTrophy : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        // Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.BossDrops.SquamousShellTrophy>());

        Item.width = 32;
        Item.height = 32;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.buyPrice(0, 1);
    }
}