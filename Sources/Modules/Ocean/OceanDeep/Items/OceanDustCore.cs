using Everglow.Ocean.NPCs;
using Terraria.GameContent.Generation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Everglow.Ocean.Items.UnderSea
{
    public class OceanDustCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("海因子");
            // Tooltip.SetDefault("");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item refItem = new Item();
            Item.width = refItem.width;
            Item.height = refItem.height;
            Item.maxStack = 999;
            Item.value = 2000;
            Item.rare = 8;
        }
    }
}