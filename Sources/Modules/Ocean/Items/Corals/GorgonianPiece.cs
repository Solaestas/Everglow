using Terraria.Localization;

namespace MythMod.OceanMod.Items
{
    public class GorgonianPiece : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gorgonian Piece");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "柳珊瑚片");
        }
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 28;
            Item.maxStack = 999;
            Item.rare = 0;
            Item.value = Item.sellPrice(0, 0, 3, 0);
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            //Item.createTile = ModContent.TileType<OceanMod.Tiles.LargeGorgonian>();
        }
    }
}
