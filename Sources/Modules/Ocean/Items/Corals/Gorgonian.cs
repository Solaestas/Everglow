using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.Items.Corals//制作是mod名字
{
    public class Gorgonian : ModItem//材料是物品名称
    {
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("柳珊瑚");
            // Tooltip.SetDefault("珊瑚礁很美丽，不要让它受伤");//教程是物品介绍
        }
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
        public override void SetDefaults()
        {
            base.Item.width = 40;//宽
            base.Item.height = 24;//高
            base.Item.rare = 2;//品质
            base.Item.scale = 1f;//大小
            base.Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.柳珊瑚>();
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
