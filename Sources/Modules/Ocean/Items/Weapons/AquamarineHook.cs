using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Weapons.OceanWeapons
{
    public class AquamarineHook : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海蓝宝石钩");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石钩");
		}
		public override void SetDefaults()
		{
			base.item.width = 40;
			base.item.height = 40;
			base.item.value = Item.sellPrice(0, 4, 0, 0);
			base.item.rare = 7;
			base.item.noUseGraphic = true;
			base.item.useStyle = 5;
			base.item.shootSpeed = 30f;
            base.item.shoot = base.mod.ProjectileType("AquamarineHookPro");
			base.item.UseSound = SoundID.Item1;
			base.item.useAnimation = 20;
			base.item.useTime = 20;
			base.item.noMelee = false;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Aquamarine", 7);
            recipe.AddIngredient(null, "RedCoral", 1); 
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}
