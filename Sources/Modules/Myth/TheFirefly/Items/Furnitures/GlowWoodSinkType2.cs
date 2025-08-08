using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodSinkType2 : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodSinkType2>());
        Item.width = 30;
        Item.height = 34;
        Item.value = 2000;
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