using Terraria.GameContent.Creative;

namespace Everglow.Commons.ItemAbstracts.Furniture;

/// <summary>
/// 床物品模板
/// </summary>
public abstract class BedItem : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 20;
		Item.value = 2000;
		Item.maxStack = Item.CommonMaxStack;
		Item.useAnimation = 14;
	}
}
