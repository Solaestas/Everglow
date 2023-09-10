using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.Items
{
    public class Aquamarine : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石");
            // Tooltip.SetDefault("非常好看的宝石");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 44;
            Item.rare = 24;
            Item.scale = 1f;
            Item.value = 2000;
            Item.maxStack = 999;
            Item.useTime = 14;
        }
    }
}
