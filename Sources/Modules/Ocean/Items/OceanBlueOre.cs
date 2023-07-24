using Terraria.Localization;
namespace MythMod.OceanMod.Items.Ores
{
    public class OceanBlueOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Current Ore");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "沧流矿");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 14;
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
            Item.value = 3200;
            Item.createTile = ModContent.TileType<Tiles.OceanBlueOre>();
        }
        /*public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<FeatherMagic>()
                .AddIngredient(1518, 3)
                .AddTile(125)
                .Register();
        }*/
    }
}
