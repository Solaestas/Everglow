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
    public class AquamarineMiniGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("");
			base.Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石迷你机枪");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "这东西太贵重了,只好做成这个尺寸了\n 66%不消耗弹药");
		}
		public override void SetDefaults()
		{
			base.item.damage = 200;
			base.item.width = 62;
			base.item.height = 44;
			base.item.useTime = 5;
			base.item.useAnimation = 5;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.ranged = true;
			base.item.knockBack = 1f;
			base.item.value = 100000;
			base.item.rare = 11;
			base.item.UseSound = SoundID.Item31;
			base.item.autoReuse = true;
            base.item.shoot = 14;
			base.item.shootSpeed = 20f;
			base.item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            Projectile.NewProjectile(position.X, position.Y + Main.rand.Next(-1, 2) * 6f, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
            if((int)(Main.time / 5f) % 5 == 0)
            {
                int k = Projectile.NewProjectile(position.X, position.Y + Main.rand.Next(-2, 2), speedX, speedY, 257, damage * 5, knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[k].friendly = true;
                Main.projectile[k].hostile = false;
            }
            return false;
		}
		public override bool ConsumeAmmo(Player player)
		{
			return Main.rand.Next(0, 100) > 66;
		}
		public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16.0f, 0.0f);
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
