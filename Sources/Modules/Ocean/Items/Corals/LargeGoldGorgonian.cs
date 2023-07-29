using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.Items.Corals
{
    public class LargeGoldGorgonian : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("大金柳珊瑚");
            // Tooltip.SetDefault("珊瑚礁很美丽，不要让它受伤");
        }
        public override void SetDefaults()
        {
            base.Item.width = 78;
            base.Item.height = 92;
            base.Item.rare = 2;
            base.Item.scale = 1f;
            base.Item.createTile = base.Mod.Find<ModTile>("大金柳珊瑚").Type;
            base.Item.useStyle = 1;
            base.Item.useTurn = true;
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.autoReuse = true;
            base.Item.consumable = true;
            base.Item.maxStack = 999;
            base.Item.value = 3000;
        }
    }
}
