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
    public class HaiyisSickle : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.AddTranslation(GameCulture.Chinese, "粉红伊人");
            Tooltip.SetDefault("射出正二十面体结晶");
        }
        public override void SetDefaults()
        {
            item.damage = 40;
            item.melee = true;
            item.width = 52;
            item.height = 62;
            item.useTime = 24;
            item.rare = 11;
            item.useAnimation = 12;
            item.useStyle = 1;
            item.knockBack = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 22;
            item.value = 10000;
            item.scale = 1f;
            item.shoot = mod.ProjectileType("HaiyisSickle");
            item.shootSpeed = 8f;
        }
        private int a = 0;
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(2436, 7);
            recipe.AddIngredient(2438, 7);
            recipe.AddIngredient(3741, 12);
            recipe.AddIngredient(mod.ItemType("VoidBubble"), 15);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
