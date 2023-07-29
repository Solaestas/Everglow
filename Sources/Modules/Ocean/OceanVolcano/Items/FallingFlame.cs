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
using Terraria.Graphics.Shaders;

namespace Everglow.Ocean.OceanVolcano.Items
{
    public class FallingFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "天火");
            // Tooltip.SetDefault("");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.width = 34;
            Item.height = 76;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.damage = 350;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.crit = 4;
            Item.value = 30000;
            Item.scale = 1f;
            Item.rare = 11;
            Item.useStyle = 5;
            Item.knockBack = 1;
            Item.shoot = 1;
            Item.useAmmo = 40;
            Item.noMelee = true;
            Item.shootSpeed = 20f;
            Item.reuseDelay = 14;
            Item.glowMask = GetGlowMask;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12.0f, 0.0f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 v = new Vector2(speedX, speedY);
            int num = Main.rand.Next(2, 6);
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0f, 0f);
            float X = Main.rand.NextFloat(-250, 250);
            float Y = Main.rand.NextFloat(-250, 250);
            Vector2 v2 = (new Vector2(Main.screenPosition.X + Main.mouseX + Main.rand.NextFloat(-24, 24), Main.screenPosition.Y + Main.mouseY + Main.rand.NextFloat(-24, 24)) - new Vector2((float)position.X + X, (float)position.Y - 1000f + Y));
            v2 = v2 / v2.Length() * 13f;
            if (Main.rand.Next(2) == 0)
            {
                int o = Projectile.NewProjectile((float)position.X + X, (float)position.Y - 1000f + Y, v2.X, v2.Y, 686, (int)damage * 10, (float)knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[o].hostile = false;
                Main.projectile[o].friendly = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "LavaCrystal", 20);
            modRecipe.AddIngredient(null, "LavaStone", 20);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
    }
}
  