using Everglow.Yggdrasil.YggdrasilTown.Walls.TwilightForest;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Walls;

public class TwilightEucalyptusWoodWall_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableWall(ModContent.WallType<TwilightEucalyptusWoodWall>());
        Item.width = 24;
        Item.height = 24;
    }

    public override void AddRecipes()
    {
        CreateRecipe(4)
            .AddIngredient(ModContent.ItemType<TwilightEucalyptusWood_Item>(), 1)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}