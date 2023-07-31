using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
    public class AquamarineHook : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("海蓝宝石钩");
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石钩");
		}
		public override void SetDefaults()
		{
			base.Item.width = 40;
			base.Item.height = 40;
			base.Item.value = Item.sellPrice(0, 4, 0, 0);
			base.Item.rare = 7;
			base.Item.noUseGraphic = true;
			base.Item.useStyle = 5;
			base.Item.shootSpeed = 30f;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.AquamarineHookPro>();
			base.Item.UseSound = SoundID.Item1;
			base.Item.useAnimation = 20;
			base.Item.useTime = 20;
			base.Item.noMelee = false;
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "Aquamarine", 7);
            recipe.AddIngredient(null, "RedCoral", 1); 
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
	}
}
