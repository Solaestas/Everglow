using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Weapons.OceanWeapons
{
    public class PurpleSpongeGun : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("紫色海绵炮");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "紫色海绵炮");
		}
		public override void SetDefaults()
		{
			base.item.damage = 108;
			base.item.magic = true;
            base.item.mana = 12; 
            base.item.rare = 3;
			base.item.width = 70;
			base.item.height = 28;
			base.item.useTime = 12;
			base.item.useAnimation = 12;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 3.75f;
			base.item.value = 60000;
			base.item.UseSound = SoundID.Item95;
			base.item.autoReuse = true;
            base.item.shoot = mod.ProjectileType("HighPowerBubble");
            base.item.shootSpeed = 20f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            for(int h = 0;h < 5;h++)
            {
                Vector2 v = new Vector2(speedX, speedY).RotatedBy(Main.rand.NextFloat(-0.25f,0.25f)) * Main.rand.NextFloat(0.65f,1.37f);
                int num = Projectile.NewProjectile(position.X + speedX + v.X, position.Y - 12f + speedY + v.Y, v.X, v.Y, type, damage, knockBack, player.whoAmI, 0f, 0f);
            }
            for (int h = 0; h < 3; h++)
            {
                Vector2 v = new Vector2(speedX, speedY).RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f)) * Main.rand.NextFloat(0.15f, 0.4f);
                int num = Projectile.NewProjectile(position.X + speedX + v.X, position.Y + speedY + v.Y + 5f, v.X, v.Y, mod.ProjectileType("PurpleSponge"), damage * 2, knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
		}
		public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12.0f, 0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PurpleSpongeChannel", 30);
            recipe.AddIngredient(null, "VoidBubble", 15);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
