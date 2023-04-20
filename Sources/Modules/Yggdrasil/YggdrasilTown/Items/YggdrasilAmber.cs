namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class YggdrasilAmber : ModItem
{
	public override void SetStaticDefaults()
	{
	}
	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 22;
		Item.rare = ItemRarityID.White;
		Item.scale = 1f;
		Item.createTile = ModContent.TileType<Tiles.StoneScaleWood>();
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTurn = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.autoReuse = true;
		Item.consumable = true;
		Item.width = 16;
		Item.height = 16;
		Item.maxStack = 999;
		Item.value = 1000;
	}
}
