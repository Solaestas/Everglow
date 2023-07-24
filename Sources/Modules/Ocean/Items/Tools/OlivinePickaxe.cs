using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Pickaxes
{
    public class OlivinePickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("橄榄石镐");
			base.Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石镐");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "");
		}
		public override void SetDefaults()
		{
			base.item.damage = 121;
			base.item.melee = true;
			base.item.width = 40;
			base.item.height = 42;
			base.item.useTime = 4;
			base.item.useAnimation = 9;
			base.item.useTurn = true;
			base.item.pick = 290;
			base.item.useStyle = 1;
			base.item.knockBack = 9f;
			base.item.value = 90000;
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
					num = mod.DustType("Olivine");
				}
				else if (num == 1)
				{
					num = mod.DustType("Olivine");
				}
				else
				{
					num = mod.DustType("Olivine");
				}
				Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, num, 0f, 0f, 0, default(Color), 1f);
			}
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Olivine", 10);
            recipe.AddIngredient(null, "GoldGorgonianBranch", 15);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
