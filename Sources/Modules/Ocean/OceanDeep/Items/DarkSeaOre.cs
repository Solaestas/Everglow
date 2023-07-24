using Terraria.Localization;
namespace MythMod.OceanMod.Items.Ores
{
    public class DarkSeaOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Ore");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "深焚石");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 18;
            Item.rare = 9;
            Item.scale = 1f;
            Item.createTile = 0;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 3600;
            Item.createTile = ModContent.TileType<Tiles.DarkSeaOre>();
        }
    }
}
