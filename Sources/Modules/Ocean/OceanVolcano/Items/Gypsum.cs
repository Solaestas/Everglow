using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria;
namespace MythMod.Items.Volcano
{
    public class Gypsum : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "石膏");
        }
        public override void SetDefaults()
        {
            base.item.width = 40;
            base.item.height = 40;
            base.item.rare = 8;
            base.item.scale = 1f;
            base.item.createTile = base.mod.TileType("Gypsum");
            base.item.useStyle = 1;
            base.item.useTurn = true;
            base.item.useAnimation = 15;
            base.item.useTime = 10;
            base.item.autoReuse = true;
            base.item.consumable = true;
            base.item.width = 16;
            base.item.height = 16;
            base.item.maxStack = 999;
            base.item.value = 4000;
        }
    }
}
