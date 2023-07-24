using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
namespace MythMod.Items.Gems
{
    public class LavaGem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            DisplayName.SetDefault("LavaGem");
            DisplayName.AddTranslation(GameCulture.Chinese, "熔岩结核");
        }
        public override void SetDefaults()
        {
            //base.item.createTile = base.mod.TileType("橄榄石矿");
            base.item.useStyle = 1;
            base.item.useTurn = true;
            base.item.useAnimation = 15;
            base.item.useTime = 10;
            base.item.autoReuse = true;
            base.item.consumable = true;
            item.width = 30;//宽
            item.height = 20;//高
            item.rare = 11;//品质
            item.scale = 1f;//大小
            item.value = 9000;
            item.maxStack = 999;
        }
    }
}
