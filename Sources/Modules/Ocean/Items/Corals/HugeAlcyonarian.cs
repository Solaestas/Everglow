using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace MythMod.Items.Corals//????????????mod????????
{
    public class HugeAlcyonarian : ModItem//??????????????????????????
    {
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("巨大海鸡冠");
            Tooltip.SetDefault("");//????????????????????????
        }
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
        public override void SetDefaults()
        {
            base.item.width = 62;//????
            base.item.height = 44;//????
            base.item.rare = 2;//??????
            base.item.scale = 1f;//????С
            base.item.createTile = base.mod.TileType("巨大海鸡冠");
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
