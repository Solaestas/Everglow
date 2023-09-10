using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Pickaxes
{
    public class AquamarinePickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("海蓝宝石镐");
			// base.Tooltip.SetDefault("");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石镐");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "");
		}
		public override void SetDefaults()
		{
			base.Item.damage = 111;
			base.Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			base.Item.width = 52;
			base.Item.height = 52;
			base.Item.useTime = 4;
			base.Item.useAnimation = 10;
			base.Item.useTurn = true;
			base.Item.pick = 235;
			base.Item.useStyle = 1;
			base.Item.knockBack = 9f;
			base.Item.value = 80000;
			base.Item.UseSound = SoundID.Item1;
			base.Item.autoReuse = true;
			base.Item.tileBoost += 4;
            base.Item.rare = 11;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(3) == 0)
			{
				int num = Main.rand.Next(3);
				if (num == 0)
				{
					num = 59;
				}
				else if (num == 1)
				{
					num = 59;
				}
				else
				{
					num = 59;
				}
				Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, num, 0f, 0f, 0, default(Color), 1f);
			}
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "Aquamarine", 7);
            recipe.AddIngredient(null, "RedCoral", 1);
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
