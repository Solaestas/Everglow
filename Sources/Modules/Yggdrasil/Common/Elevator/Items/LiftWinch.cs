namespace Everglow.Yggdrasil.Common.Elevator.Items
{
	public class LiftWinch : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 16;
			Item.createTile = ModContent.TileType<Tiles.Winch>();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.value = 1000;
		}
	}
}
