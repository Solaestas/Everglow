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
    public class AquamarineSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石剑");
        }
        private int num = 0;
        private bool k = true;
        public override void SetDefaults()
        {

            item.damage = 154;
            item.melee = true;
            item.width = 62;
            item.height = 56;
            item.useTime = 8;
            item.rare = 11;
            item.useAnimation = 40;
            item.useStyle = 1;
            item.knockBack = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 7;
            item.value = 10000;
            item.scale = 1f;
            item.shoot = mod.ProjectileType("OceanBlue");
            item.shootSpeed = 15f;

        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 59, 0f, 0f, 0,  default(Color), 0.6f);
            //string key = hitbox.Width.ToString();
            //Color messageColor = Color.Purple;
            //Main.NewText(Language.GetTextValue(key), messageColor);
            if (hitbox.Width == 124 && k)
            {
                num = 0;
                k = false;
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            num += 1;
            Vector2 pc = player.position + new Vector2(player.width, player.height) / 2;
            Vector2 v = new Vector2(speedX, speedY);
            v = v.RotatedBy(Math.PI * (2 - (num % 5)) / 5f * -player.direction);
            Projectile.NewProjectile(pc.X, pc.Y, v.X, v.Y, mod.ProjectileType("OceanBlue"), damage, knockBack, player.whoAmI);
            if(num >= 4)
            {
                k = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Aquamarine", 7);
            recipe.AddIngredient(null, "RedCoral", 1);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
