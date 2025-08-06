using Terraria.GameContent.Creative;

namespace Everglow.Commons.ItemAbstracts.Furniture;

/// <summary>
/// 工作台物品模板
/// </summary>
public abstract class WorkBenchItem : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 14;
		Item.value = 150;
		Item.maxStack = Item.CommonMaxStack;
		Item.useAnimation = 14;
	}
}
