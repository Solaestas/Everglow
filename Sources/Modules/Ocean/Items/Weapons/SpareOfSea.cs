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

namespace MythMod.Items.Weapons
{
    public class SpareOfSea : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("");
			base.Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "碧海长矛");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "让海潮伴我来守护你");
		}
		public override void SetDefaults()
		{
			base.item.width = 58;
            base.item.height = 58;
			base.item.damage = 140;
			base.item.melee = true;
            base.item.crit = 8;
			base.item.noMelee = true;
			base.item.useTurn = true;
			base.item.noUseGraphic = true;
			base.item.useAnimation = 19;
			base.item.useStyle = 5;
			base.item.useTime = 76;
			base.item.knockBack = 7.5f;
			base.item.UseSound = SoundID.Item1;
			base.item.autoReuse = true;
			base.item.maxStack = 1;
			base.item.value = 50000;
			base.item.rare = 8;
            base.item.shoot = base.mod.ProjectileType("AzureOceanSpear");
			base.item.shootSpeed = 12f;
		}
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "OceanBlueBar", 12);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX / 1.2f, speedY / 1.2f, mod.ProjectileType("AzureOceanSpear2"), damage, knockBack, player.whoAmI);
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("AzureOceanSpear"), damage, knockBack, player.whoAmI);
            return false;
        }
	}
}
