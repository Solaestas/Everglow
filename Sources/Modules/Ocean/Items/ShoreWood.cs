using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria;
namespace Everglow.Ocean.Items.Shore
{
    public class ShoreWood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //Tooltip.SetDefault("它长满了海藻");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "滨岸木");
        }
        public override void SetDefaults()
        {
            base.Item.createTile = base.Mod.Find<ModTile>("ShoreWood").Type;
            base.Item.useStyle = 1;
			base.Item.useTurn = true;
            base.Item.useAnimation = 15;
			base.Item.useTime = 10;
            base.Item.autoReuse = true;
			base.Item.consumable = true;
            Item.width = 24;
            Item.height = 22;
            Item.rare = 3;
            Item.scale = 1f;
            Item.value = 0;
            Item.maxStack = 999;
            Item.useTime = 14;
        }
    }
}
