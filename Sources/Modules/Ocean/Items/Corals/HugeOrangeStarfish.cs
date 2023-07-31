using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.Items.Corals
{
    public class HugeOrangeStarfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("´ó³ÈÉ«º£ÐÇ");
        }
        public override void SetDefaults()
        {
            base.Item.width = 16;
            base.Item.height = 16;
            base.Item.rare = 2;
            base.Item.scale = 1f;
            base.Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.´ó³ÈÉ«º£ÐÇ>();
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
