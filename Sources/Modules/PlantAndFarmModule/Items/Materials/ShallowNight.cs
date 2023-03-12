﻿namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Materials
{
    public class ShallowNight : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heavenly Bloom");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "琼霄花");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 38;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
