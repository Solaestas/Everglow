using Terraria.ID;

namespace Everglow.Sources.Modules.YggdrasilModule.KelpCurtain.Items
{
    public class DragonScaleWood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dragon Scale Wood");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "龙鳞木");
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.White;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.DragonScaleWood>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 1000;
        }
    }
}
