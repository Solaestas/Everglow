using Terraria.GameContent.Creative;

namespace Everglow.Commons.ItemAbstracts.Furniture;

/// <summary>
/// 门物品模板
/// </summary>
public abstract class DoorItem : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 14;
		Item.height = 28;
		Item.value = 200;
		Item.maxStack = Item.CommonMaxStack;
		Item.useAnimation = 14;
	}
}