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
            // DisplayName.SetDefault("0…7¨®0†80…90‡70…00†20„50ˆ40‡5");
        }
        public override void SetDefaults()
        {
            base.Item.width = 16;
            base.Item.height = 16;
            base.Item.rare = 2;
            base.Item.scale = 1f;
            base.Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.Coral.大蓝色海星>();
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
