using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace MythMod.Items.Corals
{
    public class HugeBlueStarfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("大蓝色海星");
        }
        public override void SetDefaults()
        {
            base.item.width = 16;
            base.item.height = 16;
            base.item.rare = 2;
            base.item.scale = 1f;
            base.item.createTile = base.mod.TileType("大蓝色海星");
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
