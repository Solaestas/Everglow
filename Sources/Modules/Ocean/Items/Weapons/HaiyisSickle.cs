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
    public class HaiyisSickle : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "粉红伊人");
            // Tooltip.SetDefault("射出正二十面体结晶");
        }
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 52;
            Item.height = 62;
            Item.useTime = 24;
            Item.rare = 11;
            Item.useAnimation = 12;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 22;
            Item.value = 10000;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.HaiyisSickle>();
            Item.shootSpeed = 8f;
        }
        private int a = 0;
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(2436, 7);
            recipe.AddIngredient(2438, 7);
            recipe.AddIngredient(3741, 12);
            recipe.AddIngredient(Mod.Find<ModItem>("VoidBubble").Type, 15);
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
