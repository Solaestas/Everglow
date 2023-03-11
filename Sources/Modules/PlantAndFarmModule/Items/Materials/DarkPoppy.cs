﻿namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Materials
{
    public class DarkPoppy : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Poppy");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "暗红帽");
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 40;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
