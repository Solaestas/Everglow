using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria;
namespace MythMod.Items.Gems
{
    public class Aquamarine : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石");
            Tooltip.SetDefault("非常好看的宝石");
        }
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 44;
            item.rare = 24;
            item.scale = 1f;
            item.value = 2000;
            item.maxStack = 999;
            item.useTime = 14;
        }
    }
}
