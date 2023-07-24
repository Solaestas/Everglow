using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Pickaxes
{
    public class AquamarinePickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海蓝宝石镐");
			base.Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石镐");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "");
		}
		public override void SetDefaults()
		{
			base.item.damage = 111;
			base.item.melee = true;
			base.item.width = 52;
			base.item.height = 52;
			base.item.useTime = 4;
			base.item.useAnimation = 10;
			base.item.useTurn = true;
			base.item.pick = 235;
			base.item.useStyle = 1;
			base.item.knockBack = 9f;
			base.item.value = 80000;
			base.item.UseSound = SoundID.Item1;
			base.item.autoReuse = true;
			base.item.tileBoost += 4;
            base.item.rare = 11;
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Aquamarine", 7);
            recipe.AddIngredient(null, "RedCoral", 1);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
