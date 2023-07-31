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
namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
    public class CoralBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("");
			Item.staff[base.Item.type] = true;
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "柳珊瑚水雷");
		}
		public override void SetDefaults()
		{
			base.Item.damage = 200;
			base.Item.width = 88;
			base.Item.height = 86;
			base.Item.useTime = 36;
			base.Item.useAnimation = 36;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 1.5f;
			base.Item.value = 3000;
			base.Item.rare = 11;
            base.Item.noUseGraphic = true;
            base.Item.UseSound = SoundID.Item60;
			base.Item.autoReuse = true;
			base.Item.shootSpeed = 6f;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.GorgonianWaterBomb>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);//制作一个武器
            recipe.AddIngredient(null, "GorgonianPiece", 30); //需要一个材料
            recipe.AddIngredient(null, "BladeScale", 8); //需要一个材料
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
