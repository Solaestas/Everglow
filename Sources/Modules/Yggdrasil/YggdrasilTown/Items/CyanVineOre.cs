using Everglow.Yggdrasil.YggdrasilTown.Tiles.CyanVine;
using Terraria.ID;

namespace Everglow.Yggdrasil.YggdrasilTown.Items
{
	public class CyanVineOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Cyan Ore");
			//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "青缎矿");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 20;
			Item.rare = ItemRarityID.White;
			Item.scale = 1f;
			Item.createTile = TileID.Dirt;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.value = 400;
			Item.createTile = ModContent.TileType<CyanVineStone>();
		}
	}
}
