using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class FluorescentLeafStaff : ModItem
{
	//TODO:Translate:流萤荧光素树叶魔杖,消耗流萤木,摆放流萤荧光素树叶
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}
	public override void SetDefaults()
	{
		Item.width = 48;
		Item.height = 48;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.tileWand = ModContent.ItemType<GlowWood>();
		Item.value = 2000;
		Item.createTile = ModContent.TileType<Tiles.LifeFluorescentTreeLeaf>();
	}
}