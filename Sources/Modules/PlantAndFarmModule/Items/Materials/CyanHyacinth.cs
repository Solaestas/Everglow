﻿namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Materials
{
    public class CyanHyacinth : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cyan Hyacinth");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "天青盏");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
