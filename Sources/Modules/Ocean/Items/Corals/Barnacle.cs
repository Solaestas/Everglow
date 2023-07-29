using Terraria.Localization;
namespace Everglow.Ocean.Items
{
    public class Barnacle : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Barnacle");
            // DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "藤壶");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 14;
            Item.rare = 0;
            Item.scale = 1f;
            Item.createTile = 0;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 400;
        }
        public override void AddRecipes()
        {
        }
        public override void UpdateInventory(Player player)
        {
            int ty = (int)((Math.Sin(Main.mouseX) * 200) % 4);
            if (ty == 0)
            {
                Item.createTile = ModContent.TileType<Tiles.SparseBarnacle1>();
            }
            if (ty == 1)
            {
                Item.createTile = ModContent.TileType<Tiles.SparseBarnacle2>();
            }
            if (ty == 2)
            {
                Item.createTile = ModContent.TileType<Tiles.SparseBarnacle3>();
            }
            if (ty == 3)
            {
                Item.createTile = ModContent.TileType<Tiles.SparseBarnacle4>();
            }
            base.UpdateInventory(player);
        }
    }
}
