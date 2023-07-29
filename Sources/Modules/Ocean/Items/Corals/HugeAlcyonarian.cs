using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.Items.Corals//????????????mod????????
{
    public class HugeAlcyonarian : ModItem//??????????????????????????
    {
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("巨大海鸡冠");
            // Tooltip.SetDefault("");//????????????????????????
        }
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
        public override void SetDefaults()
        {
            base.Item.width = 62;//????
            base.Item.height = 44;//????
            base.Item.rare = 2;//??????
            base.Item.scale = 1f;//????С
            base.Item.createTile = base.Mod.Find<ModTile>("巨大海鸡冠").Type;
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
