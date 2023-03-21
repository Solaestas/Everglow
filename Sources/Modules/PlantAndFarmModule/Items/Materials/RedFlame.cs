﻿namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Materials
{
    public class RedFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Red Blossom");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "赤锦");
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 36;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
