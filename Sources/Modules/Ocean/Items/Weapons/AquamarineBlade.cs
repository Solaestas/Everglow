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
    public class AquamarineBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            // DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石之刃");
        }
        private int num = 0;
        private bool k = true;
        public override void SetDefaults()
        {

            Item.damage = 175;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 62;
            Item.height = 56;
            Item.useTime = 25;
            Item.rare = 11;
            Item.useAnimation = 25;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 7;
            Item.value = 12000;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.AquamarineBlade>();
            Item.shootSpeed = 4f;

        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 59, 0f, 0f, 0,  default(Color), 0.6f);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "Aquamarine", 11);
            recipe.AddIngredient(null, "RedCoral", 1);
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
