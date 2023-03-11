﻿namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Materials
{
    public class WhiteStar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Silk Star");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "白锦星");
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 36;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
