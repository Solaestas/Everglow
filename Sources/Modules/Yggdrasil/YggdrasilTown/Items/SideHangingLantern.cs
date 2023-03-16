namespace Everglow.Yggdrasil.YggdrasilTown.Items
{
	public class SideHangingLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 18;
			Item.rare = ItemRarityID.White;
			Item.scale = 1f;
			Item.createTile = ModContent.TileType<Tiles.SideHangingLantern>();
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
}
