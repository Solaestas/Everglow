﻿namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Materials
{
    public class SilverClock : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nine Petals");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "九瓣银");
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 32;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
