namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class HexagonalCeilingLamp : ModItem
{
	public override void SetStaticDefaults()
	{
	}
	public override void SetDefaults()
	{
		Item.width = 22;
		Item.height = 26;
		Item.rare = ItemRarityID.White;
		Item.scale = 1f;
		Item.createTile = ModContent.TileType<Tiles.HexagonalCeilingLamp>();
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTurn = true;
		Item.useAnimation = 15;
		Item.useTime = 15;
		Item.autoReuse = true;
		Item.consumable = true;
		Item.maxStack = 999;
		Item.value = 1000;
	}
}
