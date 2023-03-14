using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Items.BossDrop
{
	public class BloodyTuskTrophy : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("The Tusk Trophy");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "鲜血獠牙纪念章");
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			//Item.createTile = ModContent.TileType<Tiles.BossDrop.TuskRelic>();
			Item.placeStyle = 2;
		}

	}
}
