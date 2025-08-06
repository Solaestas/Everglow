using Terraria.GameContent.Creative;

namespace Everglow.Commons.ItemAbstracts.Furniture;

/// <summary>
/// 烛台物品模板
/// </summary>
public abstract class CandelabraItem : ModItem
{
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
