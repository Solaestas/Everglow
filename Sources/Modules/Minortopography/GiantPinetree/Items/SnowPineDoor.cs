namespace Everglow.Minortopography.GiantPinetree.Items;

public class SnowPineDoor : ModItem
{
	public override void SetStaticDefaults()
	{
	}

	public override void SetDefaults()
	{
		Item.width = 14;
		Item.height = 28;
		Item.maxStack = 99;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.value = 150;
		Item.createTile = ModContent.TileType<TilesAndWalls.SnowPineDoor>();
	}
}