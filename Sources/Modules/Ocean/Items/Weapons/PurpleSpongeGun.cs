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
    public class PurpleSpongeGun : ModItem
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("紫色海绵炮");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "紫色海绵炮");
		}
		public override void SetDefaults()
		{
			base.Item.damage = 108;
			base.Item.DamageType = DamageClass.Magic;
            base.Item.mana = 12; 
            base.Item.rare = 3;
			base.Item.width = 70;
			base.Item.height = 28;
			base.Item.useTime = 12;
			base.Item.useAnimation = 12;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 3.75f;
			base.Item.value = 60000;
			base.Item.UseSound = SoundID.Item95;
			base.Item.autoReuse = true;
            base.Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.HighPowerBubble>();
            base.Item.shootSpeed = 20f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            for(int h = 0;h < 5;h++)
            {
                Vector2 v = new Vector2(speedX, speedY).RotatedBy(Main.rand.NextFloat(-0.25f,0.25f)) * Main.rand.NextFloat(0.65f,1.37f);
                int num = Projectile.NewProjectile(position.X + speedX + v.X, position.Y - 12f + speedY + v.Y, v.X, v.Y, type, damage, knockBack, player.whoAmI, 0f, 0f);
            }
            for (int h = 0; h < 3; h++)
            {
                Vector2 v = new Vector2(speedX, speedY).RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f)) * Main.rand.NextFloat(0.15f, 0.4f);
                int num = Projectile.NewProjectile(position.X + speedX + v.X, position.Y + speedY + v.Y + 5f, v.X, v.Y, ModContent.ProjectileType<Everglow.Ocean.Projectiles.PurpleSponge>(), damage * 2, knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
		}
		public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12.0f, 0);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "PurpleSpongeChannel", 30);
            recipe.AddIngredient(null, "VoidBubble", 15);
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
