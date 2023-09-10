using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
namespace Everglow.Ocean.Items
{
    public class Olivine : ModItem//材料是物品名称
    {
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            // DisplayName.SetDefault("Olivine");
            // // DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石");
        }
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
        public override void SetDefaults()
        {
            base.Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.橄榄石矿>();
            base.Item.useStyle = 1;
            base.Item.useTurn = true;
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.autoReuse = true;
            base.Item.consumable = true;
            Item.width = 30;//宽
            Item.height = 20;//高
            Item.rare = 11;//品质
            Item.scale = 1f;//大小
            Item.value = 9000;
            Item.maxStack = 999;
        }
    }
}
