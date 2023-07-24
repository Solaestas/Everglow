using Terraria.Localization;
namespace MythMod.OceanMod.Items
{
    public class LargeBlueStarfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Large Blue Starfish");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "大蓝色海星");
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 42;
            Item.rare = 1;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<OceanMod.Tiles.LargeBlueStarfish>();
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
