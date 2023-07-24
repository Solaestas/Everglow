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
    public class OlivineBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            DisplayName.AddTranslation(GameCulture.Chinese, "éÏé­Ê¯¹­");
        }
        public override void SetDefaults()
        {
            item.ranged = true;
            item.width = 38;
            item.height = 82;
            item.useTime = 8;
            item.useAnimation = 8;
            item.damage = 285;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.crit = 7;
            item.value = 16000;
            item.scale = 1f;
            item.rare = 11;
            item.useStyle = 5;
            item.knockBack = 1;
            item.useAmmo = 40;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("OlivineArrow");
            item.shootSpeed = 13f;
            item.reuseDelay = 14;
        }
        public override bool ConsumeAmmo(Player player)
        {
            return !(player.itemAnimation < item.useAnimation - 2);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(1));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("OlivineArrow"), damage, knockBack, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Olivine", 8);
            recipe.requiredTile[0] = 412;
            recipe.AddIngredient(null, "GoldGorgonianBranch", 15);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
  