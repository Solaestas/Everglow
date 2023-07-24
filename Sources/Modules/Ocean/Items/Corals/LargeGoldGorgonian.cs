using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace MythMod.Items.Corals
{
    public class LargeGoldGorgonian : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("大金柳珊瑚");
            Tooltip.SetDefault("珊瑚礁很美丽，不要让它受伤");
        }
        public override void SetDefaults()
        {
            base.item.width = 78;
            base.item.height = 92;
            base.item.rare = 2;
            base.item.scale = 1f;
            base.item.createTile = base.mod.TileType("大金柳珊瑚");
            base.item.useStyle = 1;
            base.item.useTurn = true;
            base.item.useAnimation = 15;
            base.item.useTime = 10;
            base.item.autoReuse = true;
            base.item.consumable = true;
            base.item.maxStack = 999;
            base.item.value = 3000;
        }
    }
}
