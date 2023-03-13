using Terraria.ID;

namespace Everglow.Sources.Modules.YggdrasilModule.KelpCurtain.Items
{
    public class YggdrasilDirt : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemUseStyleID.Swing;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.YggdrasilDirt>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 100;
        }
    }
}
