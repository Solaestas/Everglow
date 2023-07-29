using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.Items.Corals
{
    public class SharkBone : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("鲨鱼骸骨");
            // Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            base.Item.width = 48;
            base.Item.height = 14;
            base.Item.rare = 0;
            base.Item.scale = 1f;
            base.Item.createTile = base.Mod.Find<ModTile>("鲨鱼骸骨").Type;
            base.Item.useStyle = 1;
            base.Item.useTurn = true;
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.autoReuse = true;
            base.Item.consumable = true;
            base.Item.maxStack = 999;
            base.Item.value = 400;
        }
        public override void AddRecipes()
        {
        }
    }
}
