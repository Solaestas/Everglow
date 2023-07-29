using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.OceanVolcano.Items
{
    public class Gypsum : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "石膏");
        }
        public override void SetDefaults()
        {
            base.Item.width = 40;
            base.Item.height = 40;
            base.Item.rare = 8;
            base.Item.scale = 1f;
            base.Item.createTile = base.Mod.Find<ModTile>("Gypsum").Type;
            base.Item.useStyle = 1;
            base.Item.useTurn = true;
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.autoReuse = true;
            base.Item.consumable = true;
            base.Item.width = 16;
            base.Item.height = 16;
            base.Item.maxStack = 999;
            base.Item.value = 4000;
        }
    }
}
