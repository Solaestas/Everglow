using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using System;

namespace MythMod.Items.Weapons.OceanWeapons
{
    public class RedCoralBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            DisplayName.AddTranslation(GameCulture.Chinese, "红珊瑚剑");
        }
        private int num = 0;
        private bool k = true;
        public override void SetDefaults()
        {
            item.damage = 218;
            item.melee = true;
            item.width = 78;
            item.height = 100;
            item.useTime = 6;
            item.rare = 11;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.knockBack = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 7;
            item.value = 140000;
            item.scale = 1f;
            item.shoot = mod.ProjectileType("RedCoral");
            item.shootSpeed = 3f;

        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            num += 1;
            Vector2 pc = player.position + new Vector2(player.width, player.height) / 2;
            Vector2 v = new Vector2(speedX, speedY);
            v = v.RotatedBy(Math.PI * (2 - (num % 5)) / 5f * -player.direction);
            Projectile.NewProjectile(pc.X, pc.Y, v.X, v.Y, mod.ProjectileType("RedCoral"), damage / 2, knockBack, player.whoAmI,2);
            if(num >= 4)
            {
                k = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "RedCoral", 8);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
