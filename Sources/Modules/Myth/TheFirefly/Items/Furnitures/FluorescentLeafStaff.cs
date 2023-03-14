using Everglow.Myth.TheFirefly.Items;
using Everglow.Myth.TheFirefly.Tiles;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class FluorescentLeafStaff : ModItem
{
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

		Item.useAmmo = ModContent.ItemType<GlowWood>();
		Item.value = 2000;
		Item.createTile = ModContent.TileType<LifeFluorescentTreeLeaf>();
	}
}