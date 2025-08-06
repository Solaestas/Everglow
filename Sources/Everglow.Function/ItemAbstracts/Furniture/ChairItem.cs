using Terraria.GameContent.Creative;

namespace Everglow.Commons.ItemAbstracts.Furniture;

/// <summary>
/// 椅物品模板
/// </summary>
public abstract class ChairItem : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 12;
		Item.value = 150;
		Item.maxStack = Item.CommonMaxStack;
		Item.useAnimation = 14;
	}
}
