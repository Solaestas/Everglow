using Terraria.GameContent.Creative;

namespace Everglow.Commons.ItemAbstracts.Furniture;

/// <summary>
/// 灯笼物品模板
/// </summary>
public abstract class LanternItem : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 12;
		Item.height = 28;
		Item.value = 150;
		Item.maxStack = Item.CommonMaxStack;
		Item.useAnimation = 14;
	}
}
