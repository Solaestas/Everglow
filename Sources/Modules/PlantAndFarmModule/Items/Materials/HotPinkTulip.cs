﻿namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Materials
{
    public class HotPinkTulip : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pink Tulip");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "粉酒杯花");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
