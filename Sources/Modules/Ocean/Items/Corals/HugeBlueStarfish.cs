using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.Items.Corals
{
    public class HugeBlueStarfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("´óÀ¶É«º£ÐÇ");
        }
        public override void SetDefaults()
        {
            base.Item.width = 16;
            base.Item.height = 16;
            base.Item.rare = 2;
            base.Item.scale = 1f;
            base.Item.createTile = base.Mod.Find<ModTile>("´óÀ¶É«º£ÐÇ").Type;
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
