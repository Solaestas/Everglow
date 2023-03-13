using Terraria.ID;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items
{
    public class ShoreMud : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cyan Ore");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "青缎矿");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.White;
            Item.scale = 1f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 0;
            Item.createTile = ModContent.TileType<Tiles.DarkMud>();
        }
    }
}
