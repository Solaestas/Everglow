using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
    public class CoralBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("花鹿角珊瑚刃");
        }
        private int num = 0;
        private bool k = true;
        public override void SetDefaults()
        {

            Item.damage = 210;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 64;
            Item.height = 80;
            Item.useTime = 4;
            Item.rare = 11;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 5.0f ;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 9;
            Item.value = 10000;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.Weapons.Other.鹿角珊瑚>();
            Item.shootSpeed = 6f;
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            num += 1;
            Vector2 pc = player.position + new Vector2(player.width, player.height) / 2;
            Vector2 v = velocity;
            v = v.RotatedBy(Math.PI * (2 - (num % 5)) / 5f * -player.direction);
            //顶上的不要改，不然会鬼畜
            int num3 = Projectile.NewProjectile(null, pc.X, pc.Y, v.X, v.Y, ModContent.ProjectileType<Everglow.Ocean.Projectiles.Weapons.Other.鹿角珊瑚>(), damage, (int)knockback, player.whoAmI);//195是特效代码，这里指发射物会发射到鼠标位置，34是伤害，10是击退
            Main.projectile[num3].ai[0] = 3;
            if (num >= 4)
            {
                k = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);//制作一个武器
            recipe.AddIngredient(null, "Acropora", 8); //需要一个材料
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
