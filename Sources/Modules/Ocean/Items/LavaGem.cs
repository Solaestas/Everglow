using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
namespace Everglow.Ocean.Items
{
    public class LavaGem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            // DisplayName.SetDefault("LavaGem");
            // DisplayName.AddTranslation(GameCulture.Chinese, "熔岩结核");
        }
        public override void SetDefaults()
        {
            //base.item.createTile = base.mod.TileType("橄榄石矿");
            base.Item.useStyle = 1;
            base.Item.useTurn = true;
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.autoReuse = true;
            base.Item.consumable = true;
            Item.width = 30;//宽
            Item.height = 20;//高
            Item.rare = 11;//品质
            Item.scale = 1f;//大小
            Item.value = 9000;
            Item.maxStack = 999;
        }
    }
}
