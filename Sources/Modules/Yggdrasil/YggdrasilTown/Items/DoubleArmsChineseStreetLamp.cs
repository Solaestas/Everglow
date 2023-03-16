namespace Everglow.Yggdrasil.YggdrasilTown.Items
{
	public class DoubleArmsChineseStreetLamp : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Lantern Post");
			//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "灯笼柱");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.rare = ItemRarityID.White;
			Item.scale = 1f;
			Item.createTile = ModContent.TileType<Tiles.DoubleArmsChineseStreetLamp>();
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
