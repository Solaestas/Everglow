using Terraria.GameContent.Creative;

namespace Everglow.Minortopography.GiantPinetree.Items;

public class SnowPineWoodStaff : ModItem
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
		Item.tileWand = ItemID.BorealWood;
		Item.value = 2000;
		Item.createTile = ModContent.TileType<TilesAndWalls.PineWood>();
	}
}