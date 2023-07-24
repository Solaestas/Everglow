using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Weapons.OceanWeapons
{
    public class OlivineAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("橄榄石斧");
			base.Tooltip.SetDefault("brush!");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石斧");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "");
		}
		public override void SetDefaults()
		{
			base.item.damage = 176;
			base.item.melee = true;
			base.item.width = 66;
			base.item.height = 62;
			base.item.useTime = 18;
			base.item.useAnimation = 18;
			base.item.useTurn = true;
			base.item.axe = 49;	
			base.item.useStyle = 1;
			base.item.knockBack = 9f;
			base.item.value = 90000;
			base.item.UseSound = SoundID.Item1;
			base.item.autoReuse = true;
            base.item.rare = 11;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height / 4, mod.DustType("Olivine"), 0f, 0f, 0, default(Color), 1f);
			Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height / 2, mod.DustType("Olivine"), 0f, 0f, 0, default(Color), 1.8f);
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
            recipe.AddIngredient(null, "Olivine", 7);
            recipe.AddIngredient(null, "GoldGorgonianBranch", 12);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
