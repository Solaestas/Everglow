using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using System;

namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
    public class RedCoralBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            // // DisplayName.AddTranslation(GameCulture.Chinese, "红珊瑚剑");
        }
        private int num = 0;
        private bool k = true;
        public override void SetDefaults()
        {
            Item.damage = 218;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 78;
            Item.height = 100;
            Item.useTime = 6;
            Item.rare = 11;
            Item.useAnimation = 30;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 7;
            Item.value = 140000;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.RedCoral>();
            Item.shootSpeed = 3f;

        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            num += 1;
            Vector2 pc = player.position + new Vector2(player.width, player.height) / 2;
            Vector2 v = new Vector2(velocity.X, velocity.Y);
            v = v.RotatedBy(Math.PI * (2 - (num % 5)) / 5f * -player.direction);
            Projectile.NewProjectile(null, pc.X, pc.Y, v.X, v.Y, ModContent.ProjectileType<Everglow.Ocean.Projectiles.RedCoral>(), damage / 2, knockback, player.whoAmI,2);
            if(num >= 4)
            {
                k = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "RedCoral", 8);
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
