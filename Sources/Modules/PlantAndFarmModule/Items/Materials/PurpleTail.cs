﻿namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Materials
{
    public class PurpleTail : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Purple Reed");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "紫风草");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
