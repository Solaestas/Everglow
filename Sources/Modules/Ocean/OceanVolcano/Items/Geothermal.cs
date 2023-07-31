using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
namespace Everglow.Ocean.OceanVolcano.Items
{
    public class Geothermal : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "地炎");
            // Tooltip.SetDefault("灼热导致它真实伤害远远高于面板");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.damage = 480;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 62;
            Item.height = 62;
            Item.useTime = 48;
            Item.rare = 11;
            Item.useAnimation = 12;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 22;
            Item.value = 10000;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.火山剑气>();
            Item.shootSpeed = 8f;
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "LavaCrystal", 20);
            modRecipe.AddIngredient(null, "LavaStone", 20);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        private int a = 0;
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(24, 900, false);
        }
    }
}
