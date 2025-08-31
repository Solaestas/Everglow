using Terraria.GameContent.Creative;

namespace Everglow.Commons.Templates.Furniture;

/// <summary>
/// 烛台物品模板
/// </summary>
public abstract class CandelabraItem : ModItem
{
    public override string LocalizationCategory => Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.value = 1500;
        Item.maxStack = Item.CommonMaxStack;
        Item.useAnimation = 14;
    }
}
