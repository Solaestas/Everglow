using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Weapons.OceanWeapons
{
    public class CoralBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("花鹿角珊瑚刃");
        }
        private int num = 0;
        private bool k = true;
        public override void SetDefaults()
        {

            item.damage = 210;
            item.melee = true;
            item.width = 64;
            item.height = 80;
            item.useTime = 4;
            item.rare = 11;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.knockBack = 5.0f ;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 9;
            item.value = 10000;
            item.scale = 1f;
            item.shoot = mod.ProjectileType("鹿角珊瑚");
            item.shootSpeed = 6f;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (hitbox.Width == 76 && k)
            {
                num = 0;
                k = false;
            }
            /*player.statLifeMax2 = hitbox.Width;*/
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            num += 1;
            Vector2 pc = player.position + new Vector2(player.width, player.height) / 2;
            Vector2 v = new Vector2(speedX, speedY);
            v = v.RotatedBy(Math.PI * (2 - (num % 5)) / 5f * -player.direction);
            //顶上的不要改，不然会鬼畜
            int num3 = Projectile.NewProjectile(pc.X, pc.Y, v.X, v.Y, mod.ProjectileType("鹿角珊瑚"), damage, knockBack, player.whoAmI);//195是特效代码，这里指发射物会发射到鼠标位置，34是伤害，10是击退
            Main.projectile[num3].ai[0] = 3;
            if (num >= 4)
            {
                k = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Acropora", 8); //需要一个材料
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1); //制作一个武器
            recipe.AddRecipe();
        }
    }
}
