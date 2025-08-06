using Terraria.GameContent.Creative;

namespace Everglow.Commons.ItemAbstracts.Furniture;

/// <summary>
/// 平台物品模板
/// </summary>
public abstract class PlatformItem : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 8;
		Item.height = 10;
		Item.value = 0;
		Item.maxStack = Item.CommonMaxStack;
		Item.useAnimation = 14;
	}
}
