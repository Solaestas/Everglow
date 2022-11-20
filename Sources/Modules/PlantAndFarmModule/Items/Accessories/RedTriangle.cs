﻿namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Accessories
{
    public class RedTriangle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Three-pointed Red Blossom");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "三尖红狐");
            //Tooltip.SetDefault("Increases crit damage by 18%\n'It is an aggressive shape in Terrarian culture'");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "暴击伤害增加18%\n'在泰拉文化中,这是一种具有攻击性的形状'");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 26;
            Item.value = 3567;
            Item.accessory = true;
            Item.rare = 3;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //MythPlayer.AddCritDamage = 0.18f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.WindMoveSeed>(), 15)
                .AddIngredient(ModContent.ItemType<Materials.RedFlame>(), 24)
                .AddTile(304)
                .Register();
        }
    }
}
