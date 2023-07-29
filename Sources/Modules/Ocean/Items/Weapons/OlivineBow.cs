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
    public class OlivineBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            // DisplayName.AddTranslation(GameCulture.Chinese, "éÏé­Ê¯¹­");
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.width = 38;
            Item.height = 82;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.damage = 285;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.crit = 7;
            Item.value = 16000;
            Item.scale = 1f;
            Item.rare = 11;
            Item.useStyle = 5;
            Item.knockBack = 1;
            Item.useAmmo = 40;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.OlivineArrow>();
            Item.shootSpeed = 13f;
            Item.reuseDelay = 14;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return !(player.itemAnimation < Item.useAnimation - 2);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(1));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<Everglow.Ocean.Projectiles.OlivineArrow>(), damage, knockBack, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "Olivine", 8);
            recipe.requiredTile[0] = 412;
            recipe.AddIngredient(null, "GoldGorgonianBranch", 15);
            recipe.Register();
        }
    }
}
  