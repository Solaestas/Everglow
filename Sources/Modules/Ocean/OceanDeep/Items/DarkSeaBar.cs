using Terraria.Localization;
namespace Everglow.Ocean.OceanDeep.Items
{
    public class DarkSeaBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Abyssal Bar");
            // DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "深焚锭");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
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
            Item.value = 12800;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Ores.DarkSeaOre>(), 3)
                .AddTile(17)
                .Register();
        }
    }
}
