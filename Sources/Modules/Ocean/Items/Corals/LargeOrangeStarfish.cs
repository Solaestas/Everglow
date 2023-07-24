using Terraria.Localization;
namespace MythMod.OceanMod.Items
{
    public class LargeOrangeStarfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Large Orange Starfish");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "大橙色海星");
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 42;
            Item.rare = 0;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<OceanMod.Tiles.LargeOrangeStarfish>();
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 3000;
        }
    }
}
