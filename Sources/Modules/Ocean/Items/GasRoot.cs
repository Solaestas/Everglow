using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria;
namespace MythMod.Items.Shore
{
    public class GasRoot : ModItem
    {
        public override void SetStaticDefaults()
        {
            //Tooltip.SetDefault("它长满了海藻");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "气根");
        }
        public override void SetDefaults()
        {
            base.item.createTile = base.mod.TileType("GasRoot");
            base.item.useStyle = 1;
			base.item.useTurn = true;
            base.item.useAnimation = 15;
			base.item.useTime = 10;
            base.item.autoReuse = true;
			base.item.consumable = true;
            item.width = 24;
            item.height = 22;
            item.rare = 3;
            item.scale = 1f;
            item.value = 0;
            item.maxStack = 999;
            item.useTime = 14;
        }
    }
}
