namespace Everglow.Myth.TheTusk.Items;

public class TuskStatusIII : ModItem
{
	//TODO:Translate:被猩红色苔藓侵染的石碑 其三
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Crimson Statue III");
	}

	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.maxStack = 999;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.createTile = ModContent.TileType<Tiles.StrangeTuskStone3>();
		Item.placeStyle = 0;
	}
}
