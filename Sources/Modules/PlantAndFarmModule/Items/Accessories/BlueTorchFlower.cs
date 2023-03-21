﻿namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Accessories
{
    public class BlueTorchFlower : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Jade Blue Icy Poker");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "翠蓝冰炬");
            //Tooltip.SetDefault("Increases max mana by 20\nstrike an enemy recovers 2 mana, this effect has a 0.2s CD\n'A varietas of red hot poker'");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "增加20魔力上限\n命中敌人回复2魔力,有0.2秒冷却\n'火炬花的一个变种'");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 36;
            Item.value = 3244;
            Item.accessory = true;
            Item.rare = 3;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.WindMoveSeed>(), 15)
                .AddIngredient(ModContent.ItemType<Materials.BlueFreeze>(), 24)
                .AddTile(304)
                .Register();
        }
    }
}
