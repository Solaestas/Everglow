using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
    public class OlivineSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("");
			// base.Tooltip.SetDefault("");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石长枪");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "");
		}
		public override void SetDefaults()
		{
			base.Item.width = 64;
            base.Item.height = 62;
			base.Item.damage = 288;
			base.Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            base.Item.crit = 8;
			base.Item.noMelee = true;
			base.Item.useTurn = true;
			base.Item.noUseGraphic = true;
			base.Item.useAnimation = 19;
			base.Item.useStyle = 5;
			base.Item.useTime = 38;
			base.Item.knockBack = 7.5f;
			base.Item.UseSound = SoundID.Item1;
			base.Item.autoReuse = true;
			base.Item.maxStack = 1;
			base.Item.value = 14000;
			base.Item.rare = 11;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.Weapons.Other.橄榄石长枪>();
			base.Item.shootSpeed = 12f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(null, position.X, position.Y, velocity.X / 1.2f, velocity.Y / 1.2f, ModContent.ProjectileType<Everglow.Ocean.Projectiles.Weapons.Other.橄榄石长枪2>(), damage, knockback, player.whoAmI);
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            velocity.X = perturbedSpeed.X;
            velocity.Y = perturbedSpeed.Y;
            Projectile.NewProjectile(null, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Everglow.Ocean.Projectiles.Weapons.Other.橄榄石长枪>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "Olivine", 8);
            recipe.AddIngredient(null, "GoldGorgonianBranch", 12);
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
        public override void PostUpdate()
        {
            Lighting.AddLight((int)((base.Item.position.X + (float)(base.Item.width / 5f)) / 16f), (int)((base.Item.position.Y + (float)(base.Item.height / 1.1f)) / 16f), 0.25f, 0.65f, 0.0f);
        }
	}
}
