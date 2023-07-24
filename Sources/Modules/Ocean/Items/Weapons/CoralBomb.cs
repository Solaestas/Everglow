using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Collections.Generic;
using System.IO;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Achievements;
namespace MythMod.Items.Weapons.OceanWeapons
{
    public class CoralBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("");
			Item.staff[base.item.type] = true;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "柳珊瑚水雷");
		}
		public override void SetDefaults()
		{
			base.item.damage = 200;
			base.item.width = 88;
			base.item.height = 86;
			base.item.useTime = 36;
			base.item.useAnimation = 36;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 1.5f;
			base.item.value = 3000;
			base.item.rare = 11;
            base.item.noUseGraphic = true;
            base.item.UseSound = SoundID.Item60;
			base.item.autoReuse = true;
			base.item.shootSpeed = 6f;
            base.item.shoot = base.mod.ProjectileType("GorgonianWaterBomb");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GorgonianPiece", 30); //需要一个材料
            recipe.AddIngredient(null, "BladeScale", 8); //需要一个材料
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1); //制作一个武器
            recipe.AddRecipe();
        }
    }
}
