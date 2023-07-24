using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Weapons.OceanWeapons
{
    public class AquamarineStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海蓝宝石法杖");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石法杖");
		}
		public override void SetDefaults()
		{
			base.item.damage = 240;
			base.item.magic = true;
			base.item.mana = 16;
			base.item.width = 30;
			base.item.height = 30;
			base.item.useTime = 26;
			base.item.useAnimation = 26;
			base.item.useStyle = 5;
			Item.staff[base.item.type] = true;
			base.item.noMelee = true;
			base.item.knockBack = 5f;
			base.item.value = Item.sellPrice(0, 8, 0, 0);
            base.item.rare = 11;
            base.item.UseSound = SoundID.Item43;
			base.item.autoReuse = true;
            base.item.shoot = base.mod.ProjectileType("AquamarineStaffPro");
			base.item.shootSpeed = 10.8f;
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
