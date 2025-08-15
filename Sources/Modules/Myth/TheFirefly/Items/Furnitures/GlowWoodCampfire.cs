using Everglow.Myth.TheFirefly.Items;
using Everglow.Myth.TheFirefly.Tiles.Furnitures;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodCampfire : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodCampfire>());
        Item.width = 32;
        Item.height = 34;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 10);
        recipe.AddIngredient(ModContent.ItemType<GlowWoodTorch>(), 5);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}