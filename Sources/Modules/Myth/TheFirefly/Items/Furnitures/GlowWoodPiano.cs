using Everglow.Myth.TheFirefly.Items;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodPiano : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodPiano>());
        Item.width = 38;
        Item.height = 24;
        Item.value = 2000;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 15);
        recipe.AddIngredient(ItemID.Bone, 4);
        recipe.AddIngredient(ItemID.Book, 1);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}