using Terraria.GameContent.Creative;

namespace Everglow.Commons.ItemAbstracts.Furniture;

/// <summary>
/// 蜡烛物品模板
/// </summary>
public abstract class CandleItem : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 8;
        Item.height = 18;
        Item.value = 300;
        Item.maxStack = Item.CommonMaxStack;
        Item.useAnimation = 14;
    }
}
