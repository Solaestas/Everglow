using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
    public class AquamarineStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("海蓝宝石法杖");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石法杖");
		}
		public override void SetDefaults()
		{
			base.Item.damage = 240;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 16;
			base.Item.width = 30;
			base.Item.height = 30;
			base.Item.useTime = 26;
			base.Item.useAnimation = 26;
			base.Item.useStyle = 5;
			Item.staff[base.Item.type] = true;
			base.Item.noMelee = true;
			base.Item.knockBack = 5f;
			base.Item.value = Item.sellPrice(0, 8, 0, 0);
            base.Item.rare = 11;
            base.Item.UseSound = SoundID.Item43;
			base.Item.autoReuse = true;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.Weapons.Other.AquamarineStaffPro>();
			base.Item.shootSpeed = 10.8f;
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
