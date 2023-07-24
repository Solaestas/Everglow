using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
namespace MythMod.Items.Weapons.OceanWeapons
{
    public class OlivineSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("");
			base.Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石长枪");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "");
		}
		public override void SetDefaults()
		{
			base.item.width = 64;
            base.item.height = 62;
			base.item.damage = 288;
			base.item.melee = true;
            base.item.crit = 8;
			base.item.noMelee = true;
			base.item.useTurn = true;
			base.item.noUseGraphic = true;
			base.item.useAnimation = 19;
			base.item.useStyle = 5;
			base.item.useTime = 38;
			base.item.knockBack = 7.5f;
			base.item.UseSound = SoundID.Item1;
			base.item.autoReuse = true;
			base.item.maxStack = 1;
			base.item.value = 14000;
			base.item.rare = 11;
            base.item.shoot = base.mod.ProjectileType("橄榄石长枪");
			base.item.shootSpeed = 12f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX / 1.2f, speedY / 1.2f, mod.ProjectileType("橄榄石长枪2"), damage, knockBack, player.whoAmI);
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("橄榄石长枪"), damage, knockBack, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Olivine", 8);
            recipe.AddIngredient(null, "GoldGorgonianBranch", 12);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void PostUpdate()
        {
            Lighting.AddLight((int)((base.item.position.X + (float)(base.item.width / 5f)) / 16f), (int)((base.item.position.Y + (float)(base.item.height / 1.1f)) / 16f), 0.25f, 0.65f, 0.0f);
        }
	}
}
