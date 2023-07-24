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
    public class AquamarineBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石之刃");
        }
        private int num = 0;
        private bool k = true;
        public override void SetDefaults()
        {

            item.damage = 175;
            item.melee = true;
            item.width = 62;
            item.height = 56;
            item.useTime = 25;
            item.rare = 11;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.knockBack = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 7;
            item.value = 12000;
            item.scale = 1f;
            item.shoot = mod.ProjectileType("AquamarineBlade");
            item.shootSpeed = 4f;

        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 59, 0f, 0f, 0,  default(Color), 0.6f);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Aquamarine", 11);
            recipe.AddIngredient(null, "RedCoral", 1);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
