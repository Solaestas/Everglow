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

namespace Everglow.Ocean.Items.Weapons
{
    public class SpareOfSea : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("");
			// base.Tooltip.SetDefault("");
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "碧海长矛");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "让海潮伴我来守护你");
		}
		public override void SetDefaults()
		{
			base.Item.width = 58;
            base.Item.height = 58;
			base.Item.damage = 140;
			base.Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            base.Item.crit = 8;
			base.Item.noMelee = true;
			base.Item.useTurn = true;
			base.Item.noUseGraphic = true;
			base.Item.useAnimation = 19;
			base.Item.useStyle = 5;
			base.Item.useTime = 76;
			base.Item.knockBack = 7.5f;
			base.Item.UseSound = SoundID.Item1;
			base.Item.autoReuse = true;
			base.Item.maxStack = 1;
			base.Item.value = 50000;
			base.Item.rare = 8;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureOceanSpear>();
			base.Item.shootSpeed = 12f;
		}
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "OceanBlueBar", 12);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX / 1.2f, speedY / 1.2f, ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureOceanSpear2>(), damage, knockBack, player.whoAmI);
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<Everglow.Ocean.Projectiles.AzureOceanSpear>(), damage, knockBack, player.whoAmI);
            return false;
        }
	}
}
